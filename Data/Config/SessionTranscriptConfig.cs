using CollegeManagement.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeManagement.Data.Config
{
    public class SessionTranscriptConfig : IEntityTypeConfiguration<SessionTranscript>
    {
        public void Configure(EntityTypeBuilder<SessionTranscript> builder)
        {
            builder.ToTable("SessionTranscripts");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.StudentId).IsRequired();
            builder.Property(x => x.AcademicSessionId).IsRequired();
            builder.Property(x => x.GPA).IsRequired().HasColumnType("decimal(5, 2)");
            builder.Property(x => x.CGPA).IsRequired().HasColumnType("decimal(5, 2)");
            builder.Property(x => x.TotalCreditsEarned).IsRequired();
            builder.Property(x => x.CreditsAttempted).IsRequired();
            builder.Property(x => x.CalculatedAt).IsRequired();

            builder.HasOne(x => x.Student)
                .WithMany(x => x.SessionTranscripts)
                .HasForeignKey(x => x.StudentId)
                .HasConstraintName("FK_SessionTranscripts_Students")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.AcademicSession)
                .WithMany(x => x.SessionTranscripts)
                .HasForeignKey(x => x.AcademicSessionId)
                .HasConstraintName("FK_SessionTranscripts_AcademicSessions")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
