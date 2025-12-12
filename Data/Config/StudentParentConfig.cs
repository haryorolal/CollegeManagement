using CollegeManagement.Data.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CollegeManagement.Data.Config
{
    public class StudentParentConfig : IEntityTypeConfiguration<StudentParent>
    {
        public void Configure(EntityTypeBuilder<StudentParent> builder)
        {
            builder.ToTable("StudentParents");
            builder.HasKey(sp => new { sp.StudentId, sp.ParentId });
            
            builder.HasOne(sp => sp.Student)
                   .WithMany(s => s.StudentParents)
                   .HasForeignKey(sp => sp.StudentId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(sp => sp.Parent)
                   .WithMany(p => p.StudentParents)
                   .HasForeignKey(sp => sp.ParentId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
