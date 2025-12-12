using CollegeManagement.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeManagement.Data.Config
{
    public class QuestionConfig : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable("Questions");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Title).HasMaxLength(100);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(1000);
            builder.Property(x => x.CourseId).IsRequired();
            builder.Property(x => x.CreatedByStaffId).IsRequired();
            builder.Property(x => x.ApprovedById).IsRequired(false);
            builder.Property(x => x.ApprovalStatus).IsRequired();
            builder.Property(x => x.Answers).IsRequired();
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.ModifiedDate).IsRequired();


            builder.HasOne(x => x.Course)
                .WithMany(x => x.Questions)
                .HasForeignKey(x => x.CourseId)
                .HasConstraintName("FK_Questions_Course")
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
