namespace CollegeManagement.Data.IServices
{
    public interface IUploadExcel<T>
    {
        Task<List<T>> ImportExcelAsync(IFormFile file);
    }
}
