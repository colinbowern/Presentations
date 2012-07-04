using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using TodoMVC.Tests.SqlServer;
using Xunit;

namespace TodoMVC.Tests.Services
{
    public class ReportingTests : IUseFixture<ReportingFixture>
    {
        private ReportingFixture reporting;

        public void SetFixture(ReportingFixture data)
        {
            reporting = data;
            var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            reporting.LoadData("TodoMVC.Tests.Services.ReportingTests.rsd", "Sample Data Set", String.Format("/{0}/Data Sources", assemblyName));
            reporting.LoadData("TodoMVC.Tests.Services.ReportingTests.rdl", "Sample Report", String.Format("/{0}/Standard Reports", assemblyName));
        }

        [Fact]
        public void SampleReportExists()
        {
            using(var channel = reporting.ChannelFactory.CreateChannel())
            {
                var itemPath = String.Format("/{0}/Standard Reports", Assembly.GetExecutingAssembly().GetName().Name);

                var actual = channel.ListChildren(new ListChildrenRequest { ItemPath = itemPath });

                actual.CatalogItems.Should().Contain(x => x.Name == "Sample Report");
            }
        }
    }
}
