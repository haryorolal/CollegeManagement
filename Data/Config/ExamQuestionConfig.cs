using CollegeManagement.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace CollegeManagement.Data.Config
{
    public class ExamQuestionConfig : IEntityTypeConfiguration<ExamQuestion>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ExamQuestion> builder)
        {
            builder.ToTable("ExamQuestions");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Marks).IsRequired().HasColumnType("decimal(5, 2)");
            builder.Property(x => x.ExamId).IsRequired();
            builder.Property(x => x.QuestionId).IsRequired();
            builder.Property(x => x.Order).IsRequired();
            //builder.HasKey(eq => new { eq.ExamId, eq.QuestionId });

            // Exam ↔ ExamQuestion
            builder.HasOne(eq => eq.Exam)
                   .WithMany(e => e.ExamQuestions)
                   .HasForeignKey(eq => eq.ExamId)
                   .HasConstraintName("FK_ExamQuestions_Exam")
                   .OnDelete(DeleteBehavior.Cascade);

            // Question ↔ ExamQuestion
            builder.HasOne(eq => eq.Question)
                   .WithMany(q => q.ExamQuestions)
                   .HasForeignKey(eq => eq.QuestionId)
                   .HasConstraintName("FK_ExamQuestions_Question")
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
