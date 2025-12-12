using CollegeManagement.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeManagement.Data.Config
{
    public class CourseConfig : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.ToTable("Courses");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.DepartmentId).IsRequired();
            builder.Property(x => x.SchoolId).IsRequired();

            builder.HasOne(x => x.Department)
                .WithMany(x => x.Courses)
                .HasForeignKey(x => x.DepartmentId)
                .HasConstraintName("FK_Courses_Department")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Faculty)
                .WithMany(x => x.Courses)
                .HasForeignKey(x => x.FacultyId)
                .HasConstraintName("FK_Courses_Faculty")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.School)
                    .WithMany(x => x.Courses)
                    .HasForeignKey(x => x.SchoolId)
                    .HasConstraintName("FK_Courses_School")
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CourseLevel)
                .WithMany(x => x.Courses)
                .HasForeignKey(x => x.CourseLevelId)
                .HasConstraintName("FK_Courses_CourseLevel")
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
