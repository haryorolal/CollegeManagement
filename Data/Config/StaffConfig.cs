using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using CollegeManagement.Data.Identity;

namespace CollegeManagement.Data.Config
{
    public class StaffConfig : IEntityTypeConfiguration<Staff>
    {
        public void Configure(EntityTypeBuilder<Staff> builder)
        {
            builder.ToTable("Staffs");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn(); //.UseIdentityColumn();
            builder.Property(x => x.Designation).IsRequired();
            builder.Property(x => x.StaffNumber).IsRequired();
            builder.Property(x => x.StaffType).IsRequired();
            builder.Property(x => x.FirstName).IsRequired();
            builder.Property(x => x.LastName).IsRequired();
            builder.Property(x => x.Email).IsRequired();
            builder.Property(x => x.PhoneNumber).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.DepartmentId).IsRequired();
            builder.Property(x => x.SchoolId).IsRequired();
            builder.Property(x => x.DateOfEmployement).IsRequired();

            //builder.HasMany(x => x.Courses)
            //       .WithMany(x => x.Staffs)
            //       .UsingEntity<StaffCourses>(
            //    j => j.HasOne(ca => ca.Course)
            //          .WithMany()
            //          .HasForeignKey(ca => ca.CourseId),
            //    j => j.HasOne(ca => ca.Staff)
            //          .WithMany()
            //          .HasForeignKey(ca => ca.StaffId),
            //    j =>j.HasKey(ca => new { ca.StaffId, ca.CourseId })
            //);


            builder.HasOne(x => x.Department)
                .WithMany(x => x.Staffs)
                .HasForeignKey(x => x.DepartmentId)
                .HasConstraintName("FK_Staffs_Departments")
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.School)
                .WithMany(x => x.Staffs)
                .HasForeignKey(x => x.SchoolId)
                .HasConstraintName("FK_Students_School")
                .OnDelete(DeleteBehavior.NoAction);

            //builder.HasOne(x => x.User)
            //   .WithMany(x => x.Staffs)
            //   .HasForeignKey(x => x.UserId)
            //   .HasConstraintName("FK_Staffs_User");
        }
    }
}
