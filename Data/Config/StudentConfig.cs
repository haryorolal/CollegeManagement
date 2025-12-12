using CollegeManagement.Data.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeManagement.Data.Config
{
    public class StudentConfig : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder) {
            builder.ToTable("Students");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn(); //.UseIdentityColumn();
            builder.Property(x => x.FirstName).IsRequired().HasMaxLength(200);
            builder.Property(x => x.LastName).IsRequired().HasMaxLength(200);
            builder.Property(x => x.OtherName).IsRequired().HasMaxLength(200);
            builder.Property(x => x.DateOfBirth).IsRequired();
            builder.Property(x => x.Age).HasComputedColumnSql("DATEDIFF(year, DateOfBirth, GETDATE())");
            builder.Property(x => x.Email).IsRequired();
            builder.Property(x => x.PhoneNUmber).IsRequired();
            builder.Property(x => x.CanBorrowBook).IsRequired();
            builder.Property(x => x.DepartmentId).IsRequired();
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.SchoolId).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.IsDeleted).IsRequired();
            builder.Property(x => x.DurationOfStudyId).IsRequired();

            builder.HasOne(x => x.Department)
                .WithMany(x => x.Students)
                .HasForeignKey(x => x.DepartmentId)
                .HasConstraintName("FK_Students_Department")
                .OnDelete(DeleteBehavior.NoAction);



            builder.HasOne(x => x.AcademicDuration)
                .WithMany(x => x.Students)
                .HasForeignKey(x => x.DurationOfStudyId)
                .HasConstraintName("FK_Students_AcademicDuration")
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.School)
               .WithMany(x => x.Students)
               .HasForeignKey(x => x.SchoolId)
               .HasConstraintName("FK_School_Student")
               .OnDelete(DeleteBehavior.Cascade);


            //builder.HasOne(x => x.LibraryCard)
            //    .WithMany(x => x.Students)
            //    .HasForeignKey(x => x.LibraryCardId)
            //    .HasConstraintName("FK_Students_LibraryCard")
            //    .OnDelete(DeleteBehavior.NoAction);

            //builder.HasMany(x => x.Courses)
            //    .WithMany(x => x.Students)
            //    .UsingEntity<StudentCourses>(
            //        j => j.HasOne(sc => sc.Course)
            //              .WithMany()
            //              .HasForeignKey(x => x.CourseId),
            //        j => j.HasOne(sc => sc.Student)
            //              .WithMany()
            //              .HasForeignKey(x => x.StudentId),
            //        j => {
            //            j.HasKey(sc => new { sc.StudentId, sc.CourseId });
            //            j.ToTable("StudentCourses");
            //        }
            //    );

            //builder.HasMany(x => x.LibraryCard)
            //    .WithMany(x => x.Students)
            //    .UsingEntity<StudentLibraryCard> (
            //        j => j.HasOne(sl => sl.LibraryCard)
            //              .WithMany()
            //              .HasForeignKey(x => x.LibraryCardId),
            //        j => j.HasOne(sl => sl.Student)
            //              .WithMany()
            //              .HasForeignKey(x => x.StudentId),
            //        j =>
            //        {
            //            j.HasKey(sl => new { sl.StudentId, sl.LibraryCardId });
            //            j.ToTable("StudentLibraryCards");
            //        }
            //    );



        }
    }
}
