using CollegeManagement.Data.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CollegeManagement.Data.Config
{
    public class SchoolUserConfig : IEntityTypeConfiguration<SchoolUser>
    {
        public void Configure(EntityTypeBuilder<SchoolUser> builder)
        {
            builder.ToTable("SchoolUsers");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.SchoolId).IsRequired();
            builder.Property(x => x.Role).IsRequired();
            
            builder.HasOne(x => x.User)
                .WithMany(x => x.SchoolUsers)
                .HasForeignKey(x => x.UserId)
                .HasConstraintName("FK_SchoolUsers_Users")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.School)
                .WithMany(x => x.SchoolUsers)
                .HasForeignKey(x => x.SchoolId)
                .HasConstraintName("FK_SchoolUser_Schools")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
