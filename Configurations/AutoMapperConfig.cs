using AutoMapper;
using CollegeManagement.Data.Identity;
using CollegeManagement.Data.Model;
using CollegeManagement.Data.Models;
using CollegeManagement.Models;

namespace CollegeManagement.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<AttendanceDTO, Attendance>().ReverseMap();
            CreateMap<AuthorDTO, Author>().ReverseMap();
            CreateMap<BookDTO, Book>().ReverseMap();
            CreateMap<AcademicDurationDTO, AcademicDuration>().ReverseMap();
            CreateMap<BookReviewDTO, BookReview>().ReverseMap();
            CreateMap<CourseDTO, Course>().ReverseMap();
            CreateMap<DepartmentDTO, Department>().ReverseMap();
            CreateMap<DepartmentHeadDTO, DepartmentHead>().ReverseMap();
            CreateMap<FacultyDTO, Faculty>().ReverseMap();
            CreateMap<FacultyHeadDTO, FacultyHead>().ReverseMap();
            CreateMap<LibraryCardDTO, LibraryCard>().ReverseMap();
            CreateMap<LibraryDTO, Library>().ReverseMap();
            CreateMap<MatricNumberDTO, MatricNumber>().ReverseMap();
            CreateMap<SchoolDTO, School>().ReverseMap();
            CreateMap<StaffDTO, Staff>().ReverseMap();
            CreateMap<StudentDTO, Student>().ReverseMap();
            CreateMap<CourseLevelDTO, CourseLevel>().ReverseMap();
            CreateMap<ExamDTO, Exam>().ReverseMap();
            CreateMap<QuestionDTO, Question>().ReverseMap();
            CreateMap<StudentLibraryCardDTO, StudentLibraryCard>().ReverseMap();
            CreateMap<StudentCoursesDTO, StudentCourses>().ReverseMap();
            CreateMap<StaffCoursesDTO, StaffCourses>().ReverseMap(); 
            CreateMap<SchoolAdminDTO, SchoolAdmin>().ReverseMap();
            CreateMap<RoleDTO, Role>().ReverseMap();
            CreateMap<RolePrivilegeDTO,  RolePrivilege>().ReverseMap();
            CreateMap<UserDTO, User>().ReverseMap();
            CreateMap<UserResponse, User>().ReverseMap();
            CreateMap<UserTypeDTO, UserType>().ReverseMap();
        }
    }
}
