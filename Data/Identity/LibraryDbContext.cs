using CollegeManagement.Data.Config;
using CollegeManagement.Data.Model;
using CollegeManagement.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CollegeManagement.Data.Identity
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        {
            
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Student> Students {  get; set; }
        public DbSet<LibraryCard> LibraryCards { get; set; }
        public DbSet<Library> Libraries { get; set; }
        public DbSet<DepartmentHead> DepartmentHeads { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<FacultyHead> FacultyHeads { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<MatricNumber> MatricNumbers { get; set; }
        public DbSet<AcademicDuration> AcademicDurations { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseLevel> CourseLevels { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<SchoolAdmin> SchoolAdmins { get; set; }
        public DbSet<BookReview> BookReviews { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamQuestion> ExamQuestions { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<StaffCourses> CourseStaffs { get; set; }
        public DbSet<StudentCourses> StudentCourses { get; set; }
        //public DbSet<StudentLibraryCard> StudentLibraries { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePrivilege> RolePrivileges { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<SchoolUser> SchoolUsers { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<StudentParent> StudentParents { get; set; }
        public DbSet<Transcript> Transcripts { get; set; }
        public DbSet<SessionTranscript> SessionTranscripts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BookConfig());
            modelBuilder.ApplyConfiguration(new AuthorConfig());
            modelBuilder.ApplyConfiguration(new StudentConfig());
            modelBuilder.ApplyConfiguration(new DepartmentHeadConfig());
            modelBuilder.ApplyConfiguration(new DepartmentConfig());
            modelBuilder.ApplyConfiguration(new FacultyHeadConfig());
            modelBuilder.ApplyConfiguration(new FacultyConfig());
            modelBuilder.ApplyConfiguration(new LibraryConfig());
            modelBuilder.ApplyConfiguration(new LibraryCardConfig());
            modelBuilder.ApplyConfiguration(new SchoolConfig());
            modelBuilder.ApplyConfiguration(new MatricNumberConfig());
            modelBuilder.ApplyConfiguration(new AcademicDurationConfig());
            modelBuilder.ApplyConfiguration(new CourseConfig());
            modelBuilder.ApplyConfiguration(new CourseLevelConfig());
            modelBuilder.ApplyConfiguration(new StaffCourseConfig());
            modelBuilder.ApplyConfiguration(new StudentCoursesConfig());
            modelBuilder.ApplyConfiguration(new StaffConfig());
            modelBuilder.ApplyConfiguration(new BookReviewConfig());
            modelBuilder.ApplyConfiguration(new AttendanceConfig());
            modelBuilder.ApplyConfiguration(new QuestionConfig());
            modelBuilder.ApplyConfiguration(new ExamConfig());
            modelBuilder.ApplyConfiguration(new ExamQuestionConfig());
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new RoleConfig());
            modelBuilder.ApplyConfiguration(new RolePrivilegeConfig());
            modelBuilder.ApplyConfiguration(new SchoolUserConfig());
            modelBuilder.ApplyConfiguration(new SchoolAdminConfig());
            modelBuilder.ApplyConfiguration(new UserTypeConfig());
            modelBuilder.ApplyConfiguration(new ParentConfig());
            modelBuilder.ApplyConfiguration(new StudentParentConfig());
            modelBuilder.ApplyConfiguration(new SessionTranscriptConfig());
            modelBuilder.ApplyConfiguration(new TranscriptConfig());
            modelBuilder.ApplyConfiguration(new AssessmentConfig());
            modelBuilder.ApplyConfiguration(new AcademicRecordConfig());
            modelBuilder.ApplyConfiguration(new AcademicSessionConfig());
        }
    }
}
