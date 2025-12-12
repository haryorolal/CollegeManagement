using CollegeManagement.Data.Enums;

namespace CollegeManagement.Data.HelperMethod
{
    public static class GradingPolicy
    {
        public static GradeLetter GetGrade(decimal score)
        {
            if (score >= 70) return GradeLetter.A;
            if (score >= 60) return GradeLetter.B;
            if (score >= 50) return GradeLetter.C;
            if (score >= 45) return GradeLetter.D;
            return GradeLetter.F;
        }

        public static bool isPassed(GradeLetter grade)
        {
            return grade != GradeLetter.F;
        }

        public static decimal GetGradePoint(GradeLetter grade)
        {
            return grade switch
            {
                GradeLetter.A => 5.0m,
                GradeLetter.B => 4.0m,
                GradeLetter.C => 3.0m,
                GradeLetter.D => 2.0m,
                GradeLetter.F => 0.0m,
                _ => 0.0m,
            };
        }
    }
}
