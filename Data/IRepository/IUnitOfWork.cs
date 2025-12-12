namespace CollegeManagement.Data.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        //IAttendanceRepository AttendanceRepositoryInterface { get; }
        
        IUserService UserServiceInterface { get; }
        IBookRepository BookRepositoryInterface { get; }
        ICourseRepository CourseRepositoryInterface { get; }
        IDepartmentRepository DepartmentRepositoryInterface { get; }
        IExamRepository ExamRepositoryInterface { get; }
        IFacultyRepository FacultyRepositoryInterface { get; }
        ILibraryCardRepository LibraryCardRepositoryInterface { get; }
        IMatricRepository MatricRepositoryInterface { get; }
        IQuestionRespository QuestionRespositoryInterface { get; }
        IStaffRepository StaffRepositoryInterface { get; }
        IStudentRepository StudentRepositoryInterface { get; }

        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
        int Save();
    }
}
