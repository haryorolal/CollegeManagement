using CollegeManagement.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeManagement.Data.Config
{
    public class LibraryCardConfig : IEntityTypeConfiguration<LibraryCard>
    {
        public void Configure(EntityTypeBuilder<LibraryCard> builder)
        {
            builder.ToTable("LibraryCards");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.CardNumber).IsRequired();
            builder.Property(x => x.LibraryId).IsRequired();

            builder.HasOne(x => x.Library)
                .WithMany(x => x.LibraryCards)
                .HasForeignKey(x => x.LibraryId)
                .HasConstraintName("FK_LibraryCards_Library")
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Student)
                .WithMany(x => x.LibraryCards)
                .HasForeignKey(x => x.StudentId)
                .HasConstraintName("FK_LibraryCards_Student")
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(x => new { x.StudentId, x.LibraryId }).IsUnique();


        }
    }
}
