using CollegeManagement.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeManagement.Data.Config
{
    public class CourseLevelConfig : IEntityTypeConfiguration<CourseLevel>
    {
        public void Configure(EntityTypeBuilder<CourseLevel> builder)
        {
            builder.ToTable("CourseLevels");
            builder.HasKey(cl => cl.Id);
            builder.Property(cl => cl.LevelName).IsRequired();

            builder.HasData(new List<CourseLevel>()
            {
                new CourseLevel() { Id = 1, LevelName = 100 },
                new CourseLevel() { Id = 2, LevelName = 200 },
                new CourseLevel() { Id = 3, LevelName = 300 },
                new CourseLevel() { Id = 4, LevelName = 400 },
                new CourseLevel() { Id = 5, LevelName = 500 },
                new CourseLevel() { Id = 6, LevelName = 600 }
            });
        }
    }
}
