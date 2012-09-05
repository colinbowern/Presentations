using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bits.Infrastructure.ReportService;
using FluentAssertions;
using Xunit;

namespace Bits.IntegrationTests.Infrastructure
{
    public class ReportingFacts : IUseFixture<ReportingFixture>
    {
        private ReportingFixture reporting;

        public void SetFixture(ReportingFixture data)
        {
            reporting = data;
            reporting.LoadData(GetType(), ReportingFixture.ItemType.DataSet, "Data Sources", "Sample Data Set");
            reporting.LoadData(GetType(), ReportingFixture.ItemType.Report, "Standard Reports", "Sample Report");
        }

        [Fact]
        public void SampleReportExists()
        {
            using (var channel = reporting.ReportServiceChannelFactory.CreateChannel())
            {
                var itemPath = String.Format("/{0}/Standard Reports", Assembly.GetExecutingAssembly().GetName().Name);

                var actual = channel.ListChildren(new ListChildrenRequest { ItemPath = itemPath });

                actual.CatalogItems.Should().Contain(x => x.Name == "Sample Report");
            }
        }
    }
}
