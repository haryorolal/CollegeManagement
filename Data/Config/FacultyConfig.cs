using CollegeManagement.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeManagement.Data.Config
{
    public class FacultyConfig : IEntityTypeConfiguration<Faculty>
    {
        public void Configure(EntityTypeBuilder<Faculty> builder)
        {
            builder.ToTable("Faculties");
            builder.HasKey(e => e.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            //builder.Property(e => e.Id).UseIdentityColumn();
            builder.Property(e => e.Name).IsRequired();

            builder.HasOne(e => e.School)
                .WithMany(e => e.Faculties)
                .HasForeignKey(e => e.SchoolId)
                .HasConstraintName("FK_Faculties_School")
                .OnDelete(DeleteBehavior.Cascade); ;

            
        }
    }
}
