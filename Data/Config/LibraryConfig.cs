using CollegeManagement.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeManagement.Data.Config
{
    public class LibraryConfig : IEntityTypeConfiguration<Library>
    {
        public void Configure(EntityTypeBuilder<Library> builder)
        {
            builder.ToTable("Libraries");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            //builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Description);

            builder.HasOne(x => x.School)
                   .WithMany(x => x.Libraries)
                   .HasForeignKey(x => x.SchoolId)
                   .HasConstraintName("FK_Libraries_Schools")
                   .OnDelete(DeleteBehavior.Cascade);

           
        }
    }
}
