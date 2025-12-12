using CollegeManagement.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeManagement.Data.Config
{
    public class BookReviewConfig : IEntityTypeConfiguration<BookReview>
    {
        public void Configure(EntityTypeBuilder<BookReview> builder)
        {
            builder.ToTable("BookReviews");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            //builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Title).HasMaxLength(200);
            builder.Property(x => x.Comment).IsRequired();
            builder.Property(x => x.BookId).IsRequired();
            builder.Property(x => x.StudentId).IsRequired();

            builder.HasOne(x => x.Book)
                .WithMany(x => x.BookReviews)
                .HasForeignKey(x => x.BookId)
                .HasConstraintName("FK_BookReview_Books")
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Student)
                .WithMany(x => x.BookReviews)
                .HasForeignKey(x => x.StudentId)
                .HasConstraintName("FK_BookReview_Student")
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(x => new { x.StudentId, x.BookId }).IsUnique();

            builder.HasOne(x => x.ApprovedByUser)
                .WithMany(x => x.BookReviews)
                .HasForeignKey(x => x.ApprovedByUserId)
                .HasConstraintName("FK_BookReview_User")
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
