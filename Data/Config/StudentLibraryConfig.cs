using CollegeManagement.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeManagement.Data.Config
{
    public class StudentLibraryConfig : IEntityTypeConfiguration<StudentLibraryCard>
    {
        public void Configure(EntityTypeBuilder<StudentLibraryCard> builder)
        {
            builder.ToTable("StudentLibraries");
            //builder.Property(x => x.HasBorrowedBook).IsRequired();
            //builder.Property(x => x.HasReturnedBook).IsRequired();
            //builder.Property(x => x.IsBookAvailable).IsRequired();
        }
    }
}
