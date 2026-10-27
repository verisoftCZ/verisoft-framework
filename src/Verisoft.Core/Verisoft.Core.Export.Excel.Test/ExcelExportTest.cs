
using System.Drawing;
using Verisoft.Core.Export.Builders;
using Verisoft.Core.Export.Settings;

namespace Verisoft.Core.Export.Excel.Test;

public class ExcelExportTest
{

    public class Export
    {
        [Test]
        [Explicit]
        public void ShouldCreateFileWithDefaultValues()
        {
            List<TestDataRecord> data = [new TestDataRecord("Text", 5), new TestDataRecord("Text2", 4), new TestDataRecord("Text", 200)];

            using var fileStream = new FileStream("./test.xlsx", FileMode.Create, FileAccess.Write);
            var settings = new ExportSettings<TestDataRecord>();
            new ExcelExport<TestDataRecord>(data, settings).Export(fileStream);
        }

        [Test]
        [Explicit]
        public void ShouldCreateFileWithSetValues()
        {
            List<TestDataRecord> data = [new TestDataRecord("Text", 5), new TestDataRecord("Text2", 4), new TestDataRecord("Text", 200)];

            using var fileStream = new FileStream("./test.xlsx", FileMode.Create, FileAccess.Write);
            var settings = new ExportBuilder<TestDataRecord>()
                .Header(x => x.Background(Color.Aqua))
                .Column(x =>
                {
                    x.PropertyExpression(y => y.IntValue);
                    x.Type(typeof(int));
                    x.Header("Počet");
                    x.Width(20);
                })
                .Column(x =>
                {
                    x.PropertyExpression(y => y.StringValue);
                    x.Type(typeof(string));
                    x.Header("Jmeno");
                    x.Width(60);
                })
                .Build();

            new ExcelExport<TestDataRecord>(data, settings).Export(fileStream);
        }

        private record TestDataRecord(string StringValue, int IntValue);
    }
}
