using CollegeManagement.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeManagement.Data.Config
{
    public class AcademicSessionConfig : IEntityTypeConfiguration<AcademicSession>
    {
        public void Configure(EntityTypeBuilder<AcademicSession> builder)
        {
            builder.ToTable("AcademicSessions");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.StartYear).IsRequired();
            builder.Property(x => x.EndYear).IsRequired();
            builder.Property(x => x.Term).IsRequired();
            builder.Property(x => x.IsCurrentYear).IsRequired();
            builder.Property(x => x.Duration).IsRequired();

        }
    }
}
