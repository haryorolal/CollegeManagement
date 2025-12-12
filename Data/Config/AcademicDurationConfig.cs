using CollegeManagement.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeManagement.Data.Config
{
    public class AcademicDurationConfig : IEntityTypeConfiguration<AcademicDuration>
    {
        public void Configure(EntityTypeBuilder<AcademicDuration> builder)
        {
            builder.ToTable("AcademicDurations");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.DepartmentId).IsRequired();
            builder.Property(x => x.TotalYears).IsRequired();

            builder.HasOne(x => x.Department)
                .WithMany(x => x.AcademicDurations)
                .HasForeignKey(x => x.DepartmentId)
                .HasConstraintName("FK_AcademicDuration_Deparment")
                .OnDelete(DeleteBehavior.Restrict);
           
        }
    }
}
