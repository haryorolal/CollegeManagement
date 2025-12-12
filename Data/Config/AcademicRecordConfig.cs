using CollegeManagement.Data.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CollegeManagement.Data.Config
{
    public class AcademicRecordConfig : IEntityTypeConfiguration<AcademicRecord>
    {
        public void Configure(EntityTypeBuilder<AcademicRecord> builder)
        {
            builder.ToTable("AcademicRecords");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.StudentId).IsRequired();
            builder.Property(x => x.CourseId).IsRequired();
            builder.Property(x => x.AcademicSessionId).IsRequired();

            builder.Property(x => x.TotalScore).IsRequired().HasPrecision(6, 2);
            builder.Property(x => x.Grade).IsRequired();
            builder.Property(x => x.GradePoint).IsRequired().HasPrecision(5, 2);
            builder.Property(x => x.CreditUnit).IsRequired();
            builder.Property(x => x.IsPassed).IsRequired();
            builder.Property(x => x.IsCarriedOver).IsRequired();

            // Course ↔ AcademicRecord
            builder.HasOne(x => x.Course)
                .WithMany(x => x.AcademicRecords)
                .HasForeignKey(x => x.CourseId)
                .HasConstraintName("FK_AcademicRecord_Course")
                .OnDelete(DeleteBehavior.Restrict);

            // Session ↔ AcademicRecord
            builder.HasOne(x => x.AcademicSession)
                .WithMany(x => x.AcademicRecords)
                .HasForeignKey(x => x.AcademicSessionId)
                .HasConstraintName("FK_AcademicRecord_AcademicSession")
                .OnDelete(DeleteBehavior.Restrict);

            // Student ↔ AcademicRecords (One-to-Many)
            builder.HasOne(x => x.Student)
                .WithMany(x => x.AcademicRecords)
                .HasForeignKey(x => x.StudentId)
                .HasConstraintName("FK_AcademicRecord_Student")
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}