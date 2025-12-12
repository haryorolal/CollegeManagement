using CollegeManagement.Data.IServices;
using CollegeManagement.Models;
using OfficeOpenXml;
using System.Net;
using System.Reflection;

namespace CollegeManagement.Data.Services
{
    public class UploadExcel<T> : IUploadExcel<T> where T : class, new()
    {
        public UploadExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public async Task<List<T>> ImportExcelAsync(IFormFile file) //where T : class, new()
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file uploaded.");

            var result = new List<T>();

            using (var stream = file.OpenReadStream())
            {
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null || worksheet.Dimension == null)
                        throw new ArgumentException("The uploaded file is empty or invalid.");

                    int rowCount = worksheet.Dimension.Rows;
                    int colCount = worksheet.Dimension.Columns;

                    var headers = new List<string>();
                    for (int col = 0; col < colCount; col++)
                    {
                        headers.Add(worksheet.Cells[1, col + 1].Text);
                        //headers.Add(worksheet.Cells[1, col].Text.Trim());
                    }
                   
                    //build dtos row by row
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var dto = new T();
                        var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                        foreach (var item in props)
                        {
                            var headerIndex = headers.FindIndex(h => string.Equals(h, item.Name, StringComparison.OrdinalIgnoreCase));
                            if (headerIndex >= 0 && headerIndex < colCount)
                            {
                                var cellValue = worksheet.Cells[row, headerIndex + 1].Text;
                                if (!string.IsNullOrEmpty(cellValue))
                                {
                                    try
                                    {
                                        var convertedValue = Convert.ChangeType(cellValue, item.PropertyType);
                                        item.SetValue(dto, convertedValue);
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new Exception($"Error converting value '{cellValue}' to type '{item.PropertyType.Name}' for property '{item.Name}': {ex.Message}");
                                    }
                                }
                            }
                        }
                        result.Add(dto);
                    }
                }

            }
            return result;
        }
    }
}
