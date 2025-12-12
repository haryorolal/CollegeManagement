using CollegeManagement.Data.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeManagement.Data.Config
{
    public class SchoolAdminConfig : IEntityTypeConfiguration<SchoolAdmin>
    {
        public void Configure(EntityTypeBuilder<SchoolAdmin> builder) {

            builder.ToTable("SchoolAdmins");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.SchoolId).IsRequired();

            builder.HasOne(x => x.User)
                .WithMany(x => x.SchoolAdmins)
                .HasForeignKey(x => x.UserId)
                .HasConstraintName("FK_SchoolAdmin_Users")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.School)
                .WithMany(x => x.SchoolAdmins)
                .HasForeignKey(x => x.SchoolId)
                .HasConstraintName("FK_SchoolAdmin_School")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
