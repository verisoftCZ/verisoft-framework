Pokud potřebuji exportovat základní data do excelu je možné využít `Verisoft.Core.Excel`


Příklad generování excelu:
``` csharp
//record TestDataRecord
private record TestDataRecord(string StringValue, int IntValue);


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
```

Výsledný excel:
![image.png](/docs/.attachments/image-528ee3df-9c75-4206-b31e-edee1d0e7263.png)

Pokud nejsou konfigurovány header/columns tak se používají defaultní hodnoty.