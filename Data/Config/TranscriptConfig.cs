using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using CollegeManagement.Data.Model;

namespace CollegeManagement.Data.Config
{
    public class TranscriptConfig : IEntityTypeConfiguration<Transcript>
    {
        public void Configure(EntityTypeBuilder<Transcript> builder)
        {
            builder.ToTable("Transcripts");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.StudentId).IsRequired();
            builder.Property(x => x.CGPA).IsRequired().HasColumnType("decimal(5, 2)");
            builder.Property(x => x.TotalCreditsEarned).IsRequired();
            builder.Property(x => x.TotalCreditsAttempted).IsRequired();
            builder.Property(x => x.GeneratedAt).IsRequired();

            builder.HasOne(x => x.Student)
                .WithOne(x => x.Transcript)
                .HasForeignKey<Transcript>(x => x.StudentId)
                .HasConstraintName("FK_Transcripts_Student")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
