using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using TodoMVC.Tests.SqlServer;

namespace TodoMVC.Tests
{
    public class ReportingFixture
    {
        private static readonly Lazy<ChannelFactory<ReportingService2010SoapChannel>> channelFactory = new Lazy<ChannelFactory<ReportingService2010SoapChannel>>(CreateChannelFactory);

        public ChannelFactory<ReportingService2010SoapChannel> ChannelFactory
        {
            get { return channelFactory.Value; }
        }

        public ReportingFixture()
        {
            CleanFolderStructure();
            CreateFoundation();
        }

        public void CleanFolderStructure()
        {
            using (var channel = ChannelFactory.CreateChannel())
            {
                var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
                // Look for a folder called "TodoMVC.Tests + Machine" - "Standard Reports"
                // Delete it
                
                var result = channel.FindItems(new FindItemsRequest
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
                                                                                          Values = new[] {assemblyName}
                                                                                      }
                                                                              }
                                                   });
                result.Items.ToList().ForEach(x => channel.DeleteItem(new DeleteItemRequest { ItemPath = x.Path }));
            }
        }

        public void CreateFoundation()
        {
            using (var channel = ChannelFactory.CreateChannel())
            {
                var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;

                // Base Folders
                var integrationTestsFolder = channel.CreateFolder(new CreateFolderRequest { Folder = assemblyName, Parent = "/" });
                var dataSourcesFolder = channel.CreateFolder(new CreateFolderRequest { Folder = "Data Sources", Parent = integrationTestsFolder.ItemInfo.Path });
                channel.CreateFolder(new CreateFolderRequest { Folder = "Standard Reports", Parent = integrationTestsFolder.ItemInfo.Path });

                // Sample Data Source
                channel.CreateDataSource(new CreateDataSourceRequest
                                {
                                    DataSource = "Sample Data Source",
                                    Definition = new DataSourceDefinition
                                                    {
                                                        ConnectString = String.Empty,
                                                        Extension = "XML",
                                                        CredentialRetrieval = CredentialRetrievalEnum.Integrated,
                                                    },
                                    Parent = dataSourcesFolder.ItemInfo.Path
                                });
            }
        }

        public void LoadData(string embeddedResourceName, string itemName, string path)
        {
            if (Assembly.GetExecutingAssembly().GetManifestResourceInfo(embeddedResourceName) == null)
                throw new ArgumentOutOfRangeException("embeddedResourceName",
                                                      "Unable to find embedded resource with specified name.");
            using (var channel = ChannelFactory.CreateChannel())
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(embeddedResourceName))
            {
                string itemType = null;
                switch(Path.GetExtension(embeddedResourceName).ToLower())
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
                                                  Parent = path,
                                                  Overwrite = true,
                                                  Definition = stream.ReadToEnd()
                                              });
            }
        }

        private static ChannelFactory<ReportingService2010SoapChannel> CreateChannelFactory()
        {
            var result = new ChannelFactory<ReportingService2010SoapChannel>("*");
            return result;
        }
    }
}
