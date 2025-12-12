using CollegeManagement.Data.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeManagement.Data.Config
{
    public class ParentConfig : IEntityTypeConfiguration<Parent>
    {
        public void Configure(EntityTypeBuilder<Parent> builder)
        {
            builder.ToTable("Parents");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.RelationshipToChild).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.ModifiedDate).IsRequired();

            builder.HasOne(p => p.User)
                .WithOne(p => p.Parent)
                .HasForeignKey<Parent>(p => p.UserId)
                .HasConstraintName("Fk_Parent_User")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.School)
                .WithMany(s => s.Parents)
                .HasForeignKey(p => p.SchoolId)
                .HasConstraintName("FK_Parent_School")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
