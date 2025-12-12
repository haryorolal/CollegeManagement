using CollegeManagement.Data.Enums;
using CollegeManagement.Data.Model;

namespace CollegeManagement.Models
{
    public class QuestionDTO
    {
        public int Id { get; set; }
        public int CourseId { get; set; }  // Which course it belongs to
        public int CreatedByStaffId { get; set; } // Who created this question
        public int? ApprovedById { get; set; }  // SchoolAdmin or SuperAdmin

        public string Title { get; set; }
        public string Description { get; set; }
        public char Answers { get; set; }    //public AnswerOption Answers { get; set; }
        public char ApprovalStatus { get; set; }  //public ApprovalStatus ApprovalStatus { get; set; } // Approval workflow
    }
}
