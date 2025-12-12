using CollegeManagement.Data.Enums;
using CollegeManagement.Data.Identity;
using System.ComponentModel.DataAnnotations;

namespace CollegeManagement.Data.Model
{
    public class Question
    {
        public int Id { get; set; }
        public int CourseId { get; set; }  // Which course it belongs to
        public int CreatedByStaffId { get; set; } // Who created this question
        public int? ApprovedById { get; set; }  // SchoolAdmin or SuperAdmin

        public string Title { get; set; }
        public string Description { get; set; }
        //public char Answer { get; set; }
        public AnswerOption Answers { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; } = ApprovalStatus.Pending; // Approval workflow


        public string? CreatedDateOnString { get; set; }
        public string? ModifiedDateOnString { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public virtual Staff? Staff { get; set; }
        public virtual Course? Course { get; set; }
        public virtual User? ApprovedBy { get; set; }

        public virtual ICollection<ExamQuestion> ExamQuestions { get; set; } = new List<ExamQuestion>();
    }

}
