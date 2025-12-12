using CollegeManagement.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeManagement.Data.Config
{
    public class MatricNumberConfig : IEntityTypeConfiguration<MatricNumber>
    {
        public void Configure(EntityTypeBuilder<MatricNumber> builder)
        {
            builder.ToTable("MatricNumbers");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Number).IsRequired();

            // Keep the relationship configuration here
            builder.HasOne(x => x.Student)
                .WithOne(x => x.MatricNumber)
                .HasForeignKey<MatricNumber>(x => x.StudentId)
                .HasConstraintName("FK_MatricNumbers_Students")
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(x => x.Number).IsUnique();
        }
    }
}
