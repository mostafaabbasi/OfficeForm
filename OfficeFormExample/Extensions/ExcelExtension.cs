using OfficeOpenXml;

namespace OfficeFormExample.Extensions;

public static class ExcelExtension
{
    public static List<T> GetList<T>(this IFormFile file)
    {
        using var stream = new MemoryStream();
        file.CopyTo(stream);
        stream.Position = 0;
        using var package = new ExcelPackage(stream);
        var sheet = package.Workbook.Worksheets.First();

        List<T> list = new List<T>();
        var columnInfo = Enumerable.Range(1, sheet.Dimension.Columns)
            .AsEnumerable()
            .Select(n =>
                new
                {
                    Index = n,
                    ColumnName = sheet.Cells[1, n].Value.ToString()
                }
            );

        for (int row = 2; row <= sheet.Dimension.Rows; row++)
        {
            T obj = (T)Activator.CreateInstance(typeof(T));
            foreach (var prop in typeof(T).GetProperties())
            {
                int col = columnInfo.SingleOrDefault(c => c.ColumnName == prop.Name).Index;
                var val = sheet.Cells[row, col].Value;
                var propType = prop.PropertyType;

                if (propType.IsEnum)
                {
                    var enumValue = Enum.Parse(propType, val.ToString(), true);
                    prop.SetValue(obj, enumValue);
                }
                else if (propType == typeof(string))
                {
                    prop.SetValue(obj, val?.ToString()?.Trim());
                }
                else
                {
                    prop.SetValue(obj, Convert.ChangeType(val, propType));
                }
            }

            list.Add(obj);
        }

        return list;
    }
}