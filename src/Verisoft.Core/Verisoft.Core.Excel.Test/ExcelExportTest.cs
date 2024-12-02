
using System.Drawing;

namespace Verisoft.Core.Excel.Test;

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
            new ExcelExport<TestDataRecord>(data).Export(fileStream);
        }

        [Test]
        [Explicit]
        public void ShouldCreateFileWithSetValues()
        {
            List<TestDataRecord> data = [new TestDataRecord("Text", 5), new TestDataRecord("Text2", 4), new TestDataRecord("Text", 200)];

            using var fileStream = new FileStream("./test.xlsx", FileMode.Create, FileAccess.Write);
            new ExcelExport<TestDataRecord>(data).Header(x => x.Background(Color.Aqua))
                .Columns(columns =>
                {
                    columns.Add(x => x.IntValue).Header("Počet").Width(20);
                    columns.Add(x => x.StringValue).Header("Jmeno").Width(60);
                }).Export(fileStream);
        }

        private record TestDataRecord(string StringValue, int IntValue);
    }
}
