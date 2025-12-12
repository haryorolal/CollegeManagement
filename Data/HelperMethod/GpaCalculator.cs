using CollegeManagement.Data.Model;

namespace CollegeManagement.Data.HelperMethod
{
    public class GpaCalculator
    {
        public static decimal CalculateGPA(IEnumerable<AcademicRecord> records)
        {
            var totalWeightedPoints = records.Sum(r => r.WeightedPoint);
            var totalCreditUnits = records.Sum(r => r.CreditUnit);

            return totalCreditUnits > 0 ? Math.Round(totalWeightedPoints / totalCreditUnits, 2) : 0.0m;
        }
    }
}
