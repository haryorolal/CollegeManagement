using CollegeManagement.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeManagement.Data.Config
{
    public class FacultyHeadConfig : IEntityTypeConfiguration<FacultyHead>
    {
        public void Configure(EntityTypeBuilder<FacultyHead> builder)
        {
            builder.ToTable("FacultyHeads");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.StaffId).IsRequired();
            builder.Property(x => x.FacultyId).IsRequired();
            builder.Property(x => x.IsCurrent).IsRequired();
            builder.Property(x => x.StartYear).IsRequired();

            builder.HasOne(x => x.Faculty)
                .WithMany(x => x.FacultyHeads)
                .HasForeignKey(x => x.FacultyId)
                .HasConstraintName("FK_FacultyHeads_Faculty");
            
        }
    }
}
