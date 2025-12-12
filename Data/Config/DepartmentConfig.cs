using CollegeManagement.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeManagement.Data.Config
{
    public class DepartmentConfig : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Departments");
            builder.HasKey(d => d.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(d => d.Name).IsRequired().HasMaxLength(100);
            builder.Property(d => d.Description).HasMaxLength(500);
            builder.Property(d => d.DepartmentCode).IsRequired();
            builder.Property(d => d.FacultyId).IsRequired();
            builder.Property(d => d.SchoolId).IsRequired();

            builder.HasOne(d => d.Faculty)
                   .WithMany(f => f.Departments)
                   .HasForeignKey(d => d.FacultyId)
                   .HasConstraintName("FK_Departments_Faculties")
                   .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.School)
                .WithMany(f => f.Departments)
                .HasForeignKey(d => d.SchoolId)
                .HasConstraintName("FK_Departments_Schools")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
