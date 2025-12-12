using CollegeManagement.Data.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CollegeManagement.Data.Config
{
    public class AttendanceConfig : IEntityTypeConfiguration<Attendance>
    {
        public void Configure(EntityTypeBuilder<Attendance> builder)
        {
            builder.ToTable("Attendances");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.IsActive).IsRequired().HasMaxLength(100);
            builder.Property(x => x.CourseId).IsRequired();

            builder.HasOne(x => x.Course)
                .WithMany(x => x.Attendances)
                .HasForeignKey(x => x.CourseId)
                .HasConstraintName("FK_Attendances_Course")
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Student)
                .WithMany(x => x.Attendances)
                .HasForeignKey(x => x.StudentId)
                .HasConstraintName("FK_Attendances_Student")
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
