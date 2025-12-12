using CollegeManagement.Data.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CollegeManagement.Data.Config
{
    public class StaffCourseConfig : IEntityTypeConfiguration<StaffCourses>
    {
        public void Configure(EntityTypeBuilder<StaffCourses> builder)
        {
            builder.ToTable("StaffCourses");
            builder.HasKey(sc => sc.Id);
            builder.Property(x => x.StaffId).IsRequired();
            builder.Property(x => x.CourseId).IsRequired();


            builder.HasOne(sc => sc.Staff)
                   .WithMany(s => s.StaffCourses)
                   .HasForeignKey(sc => sc.StaffId)
                   .HasConstraintName("FK_StaffCourses_Staff")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(sc => sc.Course)
                     .WithMany(c => c.StaffCourses)
                     .HasForeignKey(sc => sc.CourseId)
                     .HasConstraintName("FK_StaffCourses_Course")
                     .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
