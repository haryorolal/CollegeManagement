using CollegeManagement.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeManagement.Data.Config
{
    public class AssessmentConfig : IEntityTypeConfiguration<Assessment>
    {
        public void Configure(EntityTypeBuilder<Assessment> builder)
        {
            builder.ToTable("Assessments");
            builder.HasKey(a => a.Id);
            builder.Property(x => x.Score).IsRequired().HasPrecision(6, 2);
            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.StudentId).IsRequired();
            builder.Property(x => x.ExamId).IsRequired();
            builder.Property(x => x.TakenAt).IsRequired();

            builder.HasOne(x => x.Student)
                    .WithMany(x => x.Assessments)
                    .HasForeignKey(x => x.StudentId)
                    .HasConstraintName("FK_Assessments_Student")
                    .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Exam)
                    .WithMany(x => x.Assessments)
                    .HasForeignKey(x => x.ExamId)
                    .HasConstraintName("FK_Assessments_Exam")
                    .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
