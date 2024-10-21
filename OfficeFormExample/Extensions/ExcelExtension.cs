using OfficeOpenXml;

namespace OfficeFormExample.Extensions;

public static class ExcelExtension
{
    // public static List<T> GetList<T>(this IFormFile file)
    // {
    //     using var stream = new MemoryStream();
    //     file.CopyTo(stream);
    //     stream.Position = 0;
    //     using var package = new ExcelPackage(stream);
    //     var sheet = package.Workbook.Worksheets.First();
    //
    //     List<T> list = new List<T>();
    //     var columnInfo = Enumerable.Range(1, sheet.Dimension.Columns)
    //         .AsEnumerable()
    //         .Select(n =>
    //             new
    //             {
    //                 Index = n,
    //                 ColumnName = sheet.Cells[1, n].Value.ToString()
    //             }
    //         );
    //
    //     for (int row = 2; row <= sheet.Dimension.Rows; row++)
    //     {
    //         T obj = (T)Activator.CreateInstance(typeof(T));
    //         foreach (var prop in typeof(T).GetProperties())
    //         {
    //             int col = columnInfo.SingleOrDefault(c => c.ColumnName == prop.Name).Index;
    //             var val = sheet.Cells[row, col].Value;
    //             var propType = prop.PropertyType;
    //
    //             if (propType.IsEnum)
    //             {
    //                 var enumValue = Enum.Parse(propType, val.ToString(), true);
    //                 prop.SetValue(obj, enumValue);
    //             }
    //             else if (propType == typeof(string) && val is DateTime time)
    //             {
    //                 prop.SetValue(obj, time.ToString("yyyy-MM-dd"));
    //             }
    //             else if (propType == typeof(string))
    //             {
    //                 DateTime parsedDate;
    //                 if (DateTime.TryParse(val?.ToString(), out parsedDate))
    //                 {
    //                     prop.SetValue(obj, parsedDate.ToString("yyyy-MM-dd"));
    //                 }
    //                 else
    //                 {
    //                     prop.SetValue(obj, val?.ToString()?.Trim());
    //                 }
    //             }
    //             else
    //             {
    //                 prop.SetValue(obj, Convert.ChangeType(val, propType));
    //             }
    //         }
    //
    //         list.Add(obj);
    //     }
    //
    //     return list;
    // }
    
    public static List<T> GetList<T>(this IFormFile file)
{
    using var stream = new MemoryStream();
    file.CopyTo(stream);
    stream.Position = 0;
    using var package = new ExcelPackage(stream);
    var sheet = package.Workbook.Worksheets.First();

    List<T> list = new List<T>();
    var columnInfo = Enumerable.Range(1, sheet.Dimension.Columns)
        .Select(n =>
            new
            {
                Index = n,
                ColumnName = sheet.Cells[1, n].Value?.ToString()
            }
        )
        .ToList();

    for (int row = 2; row <= sheet.Dimension.Rows; row++)
    {
        T obj = (T)Activator.CreateInstance(typeof(T));
        foreach (var prop in typeof(T).GetProperties())
        {
            var column = columnInfo.SingleOrDefault(c => c.ColumnName == prop.Name);
            if (column == null) continue;

            var cellValue = sheet.Cells[row, column.Index].Value;
            var propType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

            try
            {
                if (propType == typeof(DateTime) || propType == typeof(DateTime?))
                {
                    switch (cellValue)
                    {
                        case double doubleValue:
                        {
                            DateTime dateValue = DateTime.FromOADate(doubleValue);
                            prop.SetValue(obj, dateValue);
                            break;
                        }
                        case string stringValue:
                        {
                            if (DateTime.TryParse(stringValue, out DateTime dateValue))
                            {
                                prop.SetValue(obj, dateValue);
                            }

                            break;
                        }
                    }
                }
                else if (propType.IsEnum)
                {
                    var enumValue = Enum.Parse(propType, cellValue.ToString(), true);
                    prop.SetValue(obj, enumValue);
                }
                else if (propType == typeof(string))
                {
                    switch (cellValue)
                    {
                        case DateTime dateTimeValue:
                            prop.SetValue(obj, dateTimeValue.ToString("yyyy-MM-dd"));
                            break;
                        case double doubleValue:
                        {
                            DateTime dateValue = DateTime.FromOADate(doubleValue);
                            prop.SetValue(obj, dateValue.ToString("yyyy-MM-dd"));
                            break;
                        }
                        default:
                        {
                            DateTime parsedDate;
                            if (DateTime.TryParse(cellValue?.ToString(), out parsedDate))
                            {
                                prop.SetValue(obj, parsedDate.ToString("yyyy-MM-dd"));
                            }
                            else
                            {
                                prop.SetValue(obj, cellValue?.ToString()?.Trim());
                            }

                            break;
                        }
                    }
                }
                else
                {
                    if (cellValue != null)
                    {
                        var convertedValue = Convert.ChangeType(cellValue, propType);
                        prop.SetValue(obj, convertedValue);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting property '{prop.Name}' for row {row}: {ex.Message}");
            }
        }

        list.Add(obj);
    }

    return list;
}

}