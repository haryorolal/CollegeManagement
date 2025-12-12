using CollegeManagement.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeManagement.Data.Config
{
    public class StudentCoursesConfig : IEntityTypeConfiguration<StudentCourses>
    {
        public void Configure(EntityTypeBuilder<StudentCourses> builder)
        {
            builder.ToTable("StudentCourses");
            builder.HasKey(sc => sc.Id);
            builder.Property(x => x.StudentId).IsRequired();
            builder.Property(x => x.CourseId).IsRequired();

            builder.HasOne(x => x.Student)
                     .WithMany(s => s.StudentCourses)
                     .HasForeignKey(x => x.StudentId)
                     .HasConstraintName("FK_StudentCourses_Student")
                     .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Course)
                    .WithMany(c => c.StudentCourses)
                    .HasForeignKey(x => x.CourseId)
                    .HasConstraintName("FK_StudentCourses_Course")
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
