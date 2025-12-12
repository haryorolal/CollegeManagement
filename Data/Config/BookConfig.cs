using CollegeManagement.Data.Models;
using CollegeManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeManagement.Data.Config
{
    public class BookConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Books");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            //builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
            builder.Property(x => x.AuthorId).IsRequired();
            builder.Property(x => x.LibraryId).IsRequired();

            builder.HasOne(n => n.Author)
                .WithMany(x => x.Books)
                .HasForeignKey(x => x.AuthorId)
                .HasConstraintName("FK_Books_Author")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(n => n.Library)
                .WithMany(x => x.Books)
                .HasForeignKey(x => x.LibraryId)
                .HasConstraintName("FK_Books_Library")
                .OnDelete(DeleteBehavior.NoAction);

            
        }
    }
}
