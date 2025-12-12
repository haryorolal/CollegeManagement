using AutoMapper;
using CollegeManagement.Data.IRepository;
using CollegeManagement.Data.Identity;
using Microsoft.EntityFrameworkCore.Storage;

namespace CollegeManagement.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryDbContext _libraryDbContext;
        private readonly IMapper _mapper;
        private IDbContextTransaction _transaction;

        //public IAttendanceRepository AttendanceRepositoryInterface { get; private set; }
        public IUserService UserServiceInterface { get; set; }
        public IBookRepository BookRepositoryInterface { get; private set; }
        public ICourseRepository CourseRepositoryInterface { get; private set; }
        public IDepartmentRepository DepartmentRepositoryInterface { get; private set; }

        public IExamRepository ExamRepositoryInterface { get; private set; }
        public IFacultyRepository FacultyRepositoryInterface { get; private set; }

        public ILibraryCardRepository LibraryCardRepositoryInterface { get; private set; }

        public IMatricRepository MatricRepositoryInterface { get; private set; }

        public IQuestionRespository QuestionRespositoryInterface { get; private set; }

        public IStaffRepository StaffRepositoryInterface { get; private set; }

        public IStudentRepository StudentRepositoryInterface { get; private set; }

       

        public UnitOfWork(LibraryDbContext libraryDbContext, IMapper mapper)
        {
            _libraryDbContext = libraryDbContext;
            _mapper = mapper;
            //AttendanceRepositoryInterface = new AttendanceRepository(_libraryDbContext, _mapper);
            UserServiceInterface = new UserService(_libraryDbContext, _mapper);
            BookRepositoryInterface = new BookRepository(_libraryDbContext, _mapper);
            CourseRepositoryInterface = new CourseRepository(_libraryDbContext, _mapper);
            DepartmentRepositoryInterface = new DepartmentRepository(_libraryDbContext, _mapper);
            ExamRepositoryInterface = new ExamRepository(_libraryDbContext, _mapper);
            FacultyRepositoryInterface = new FacultyRepository(_libraryDbContext, _mapper);
            LibraryCardRepositoryInterface = new LibraryCardRepository(_libraryDbContext, _mapper);
            MatricRepositoryInterface = new MatricRepository(_libraryDbContext, _mapper);
            QuestionRespositoryInterface = new QuestionRepository(_libraryDbContext, _mapper);
            StaffRepositoryInterface = new StaffRepository(_libraryDbContext, _mapper);
            StudentRepositoryInterface = new StudentRepository(_libraryDbContext, _mapper);
        }

        public void Dispose()
        {
            _libraryDbContext.Dispose();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _libraryDbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await _transaction.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            await _transaction.RollbackAsync();
        }

        public int Save()
        {
            return _libraryDbContext.SaveChanges();
        }
    }
}
