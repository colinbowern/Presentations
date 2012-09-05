using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.IO;
using Microsoft.SqlServer.Management.Smo;
using Bits.Infrastructure.ReportExecutionService;
using Bits.Infrastructure.ReportService;

namespace Bits.IntegrationTests
{
    public class ReportingFixture
    {
        private static readonly string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
        private static readonly Lazy<ChannelFactory<ReportingService2010SoapChannel>> reportServiceChannelFactory = new Lazy<ChannelFactory<ReportingService2010SoapChannel>>(() => new ChannelFactory<ReportingService2010SoapChannel>("*"));
        private static readonly Lazy<ChannelFactory<ReportExecutionServiceSoapChannel>> reportExecutionServiceChannelFactory = new Lazy<ChannelFactory<ReportExecutionServiceSoapChannel>>(() => new ChannelFactory<ReportExecutionServiceSoapChannel>("*"));

        public ChannelFactory<ReportingService2010SoapChannel> ReportServiceChannelFactory
        {
            get
            {
                return reportServiceChannelFactory.Value;
            }
        }

        public ChannelFactory<ReportExecutionServiceSoapChannel> ReportExecutionServiceChannelFactory
        {
            get
            {
                return reportExecutionServiceChannelFactory.Value;
            }
        }

        public ReportingFixture()
        {
            CleanFolderStructure();
            CreateFoundation();
        }

        public void CleanFolderStructure()
        {
            using (var channel = ReportServiceChannelFactory.CreateChannel())
            {
                // Folder
                var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
                var folders = channel.FindItems(new FindItemsRequest
                {
                    Folder = "/",
                    SearchConditions = new[]
                    {
                        new SearchCondition
                        {
                            Condition =
                                ConditionEnum.Equals,
                            ConditionSpecified = true,
                            Name = "Name",
                            Values = new[] { assemblyName }
                        }
                    }
                });
                folders.Items.ToList().ForEach(x => channel.DeleteItem(new DeleteItemRequest { ItemPath = x.Path }));
            }
        }

        public void CreateFoundation()
        {
            using (var channel = ReportServiceChannelFactory.CreateChannel())
            {
                // Base Folders
                var integrationTestsFolder = channel.CreateFolder(new CreateFolderRequest { Folder = assemblyName, Parent = "/" });
                channel.CreateFolder(new CreateFolderRequest { Folder = "Standard Reports", Parent = integrationTestsFolder.ItemInfo.Path });
                var dataSourcesFolder = channel.CreateFolder(new CreateFolderRequest { Folder = "Data Sources", Parent = integrationTestsFolder.ItemInfo.Path });

                // Data Sources
                channel.CreateDataSource(new CreateDataSourceRequest
                {
                    DataSource = "Sample Data Source",
                    Definition = new DataSourceDefinition
                    {
                        ConnectString = String.Empty,
                        Extension = "XML",
                        CredentialRetrieval = CredentialRetrievalEnum.None,
                    },
                    Parent = dataSourcesFolder.ItemInfo.Path
                });

                var connectionString =
                    new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["Database"].ConnectionString);
                if (connectionString.AttachDBFilename.StartsWith("|DataDirectory|", StringComparison.OrdinalIgnoreCase))
                {
                    connectionString.AttachDBFilename = Path.Combine(Environment.CurrentDirectory,
                        connectionString.AttachDBFilename.Substring(
                            "|DataDirectory|".Length));

                    connectionString.Pooling = false;
                }

                // HACK: Need to get the named pipe for the LocalDB instance
                // because Reporting Services does not recognize '(localdb)'
                if (connectionString.DataSource.StartsWith("(localdb)", StringComparison.OrdinalIgnoreCase))
                {
                    var dbServer = new Server(connectionString.DataSource);
                    connectionString.DataSource = String.Format(@"np:\\.\pipe\{0}\tsql\query", dbServer.InstanceName);
                }

                channel.CreateDataSource(new CreateDataSourceRequest
                {
                    DataSource = "Sample Data Source",
                    Definition = new DataSourceDefinition
                    {
                        ConnectString = connectionString.ToString(),
                        Extension = "SQL",
                        CredentialRetrieval = CredentialRetrievalEnum.None,
                    },
                    Parent = dataSourcesFolder.ItemInfo.Path
                });
            }
        }

        public void CreateFolder(string path, string itemName)
        {
            using (var channel = ReportServiceChannelFactory.CreateChannel())
            {
                if (ItemExists(channel, path, itemName))
                    return;

                channel.CreateFolder(new CreateFolderRequest { Folder = itemName, Parent = String.Format("/{0}/{1}", assemblyName, path.TrimStart('/')) });
            }
        }

        public void LinkReport(string sourceItem, string destinationFolder, string itemName, params ItemParameter[] parameters)
        {
            using (var channel = ReportServiceChannelFactory.CreateChannel())
            {
                if (ItemExists(channel, destinationFolder, itemName))
                    return;

                var description =
                    channel.GetProperties(new GetPropertiesRequest
                    {
                        ItemPath =
                            String.Format("/{0}/{1}", assemblyName,
                                sourceItem.TrimStart('/'))
                    }).Values.
                    FirstOrDefault(
                        x => "Description".Equals(x.Name, StringComparison.OrdinalIgnoreCase));

                var properties = description != null ? new[] { description } : null;

                channel.CreateLinkedItem(new CreateLinkedItemRequest
                {
                    Parent =
                        String.Format("/{0}/{1}", assemblyName,
                            destinationFolder.TrimStart('/')),
                    ItemPath = itemName,
                    Link =
                        String.Format("/{0}/{1}", assemblyName,
                            sourceItem.TrimStart('/')),
                    Properties = properties
                });

                if (parameters != null && parameters.Any())
                {
                    channel.SetItemParameters(new SetItemParametersRequest
                    {
                        ItemPath =
                            String.Format("/{0}/{1}/{2}", assemblyName,
                                destinationFolder.TrimStart('/'), itemName),
                        Parameters = parameters
                    });
                }
            }
        }

        public void CreateSnapshot(string itemPath)
        {
            using (var channel = ReportServiceChannelFactory.CreateChannel())
            {
                var fullItemPath = String.Format("/{0}/{1}", assemblyName, itemPath.TrimStart('/'));
                var historyOptions = channel.GetItemHistoryOptions(new GetItemHistoryOptionsRequest { ItemPath = fullItemPath });
                if (!historyOptions.EnableManualSnapshotCreation || !historyOptions.KeepExecutionSnapshots)
                {
                    channel.SetItemHistoryOptions(new SetItemHistoryOptionsRequest
                    {
                        ItemPath = fullItemPath,
                        EnableManualSnapshotCreation = true,
                        KeepExecutionSnapshots = true,
                        Item = new NoSchedule()
                    });
                }
                channel.CreateItemHistorySnapshot(new CreateItemHistorySnapshotRequest { ItemPath = fullItemPath });
            }
        }

        public void LoadData(Type type, ItemType itemType, string path, string itemName)
        {
            switch (itemType)
            {
                case ItemType.DataSet:
                    LoadData(String.Format("{0}.{1}.rsd", type.Namespace, type.Name), path, itemName);
                    break;
                case ItemType.Report:
                    LoadData(String.Format("{0}.{1}.rdl", type.Namespace, type.Name), path, itemName);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public void LoadData(string embeddedResourceName, string path, string itemName)
        {
            if (Assembly.GetExecutingAssembly().GetManifestResourceInfo(embeddedResourceName) == null)
                throw new ArgumentOutOfRangeException("embeddedResourceName", embeddedResourceName, "Unable to find embedded resource with specified name.");

            using (var channel = ReportServiceChannelFactory.CreateChannel())
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(embeddedResourceName))
                {
                    if (ItemExists(channel, path, itemName))
                        return;

                    string itemType;
                    switch (Path.GetExtension(embeddedResourceName).ToLower())
                    {
                        case ".rdl":
                            itemType = "Report";
                            break;
                        case ".rsd":
                            itemType = "DataSet";
                            break;
                        default:
                            throw new InvalidOperationException("Unknown item type");
                    }
                    channel.CreateCatalogItem(new CreateCatalogItemRequest
                    {
                        ItemType = itemType,
                        Name = itemName,
                        Parent = String.Format("/{0}/{1}", Assembly.GetExecutingAssembly().GetName().Name, path.TrimStart('/')),
                        Overwrite = true,
                        Definition = stream.ReadToEnd()
                    });
                }
        }

        public bool ItemExists(ReportingService2010SoapChannel channel, string path, string itemName)
        {
            return channel.FindItems(new FindItemsRequest
            {
                Folder = String.Format("/{0}/{1}", assemblyName, path.TrimStart('/')),
                SearchConditions = new[]
                {
                    new SearchCondition
                    {
                        Condition = ConditionEnum.Equals,
                        ConditionSpecified = true,
                        Name = "Name",
                        Values = new[] { itemName }
                    }
                }
            }).Items.Any();
        }

        public enum ItemType
        {
            Report,
            DataSet
        }
    }
}
