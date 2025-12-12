using CollegeManagement.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeManagement.Data.Config
{
    public class DepartmentHeadConfig : IEntityTypeConfiguration<DepartmentHead>
    {
        public void Configure(EntityTypeBuilder<DepartmentHead> builder)
        {
            builder.ToTable("DepartmentHeads");
            builder.HasKey(dh => dh.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.StaffId).IsRequired();
            builder.Property(x => x.DepartmentId).IsRequired();
            builder.Property(dh => dh.IsCurrent).IsRequired();
            builder.Property(dh => dh.StartYear).IsRequired();

            builder.HasOne(dh => dh.Department)
                .WithMany(d => d.DepartmentHeads)
                .HasForeignKey(dh => dh.DepartmentId)
                .HasConstraintName("FK_DepartmentHeads_Departments");
            
        }
    }
}
