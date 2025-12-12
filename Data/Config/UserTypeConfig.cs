using CollegeManagement.Data.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CollegeManagement.Data.Config
{
    public class UserTypeConfig : IEntityTypeConfiguration<UserType>
    {
        public void Configure(EntityTypeBuilder<UserType> builder)
        {
            builder.ToTable("UserTypes");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Code).IsRequired();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Description);

            builder.HasData(new List<UserType>()
            {                
                new UserType { Id = 1, Code = "SDT", Name = "Student", Description = "For Student" },
                new UserType { Id = 2, Code = "STF", Name = "Staff", Description = "For Staffs" },
                new UserType { Id = 3, Code = "DTH", Name = "DepartmentHead", Description = "For Department Head" },
                new UserType { Id = 4, Code = "FTH", Name = "FacultyHead", Description = "For Faculty Head" },
                new UserType { Id = 5, Code = "SDP", Name = "StudentParent", Description = "For Students Parent" },
                new UserType { Id = 6, Code = "ADMIN", Name = "Admin", Description = "For Admin" },
                new UserType { Id = 7, Code = "SADMIN", Name = "SuperAdmin", Description = "For Super Admin" }
            });
        }
    }
}
