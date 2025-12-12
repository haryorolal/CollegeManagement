using CollegeManagement.Data.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CollegeManagement.Data.Config
{
    public class ExamConfig : IEntityTypeConfiguration<Exam>
    {
        public void Configure(EntityTypeBuilder<Exam> builder)
        {
            builder.ToTable("Exams");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.IsExamStarted).IsRequired();
            builder.Property(x => x.IsExamCompleted).IsRequired();
            builder.Property(x => x.DepartmentId).IsRequired();
            builder.Property(x => x.ExamStartTime).IsRequired(false);
            builder.Property(x => x.ExamEndTime).IsRequired(false);
            builder.Ignore(x => x.RemainingTime);

            builder.HasOne(x => x.Department)
                .WithMany(x => x.Exams)
                .HasForeignKey(x => x.DepartmentId)
                .HasConstraintName("FK_Exams_Department")
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
