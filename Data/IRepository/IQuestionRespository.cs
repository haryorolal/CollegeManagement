using CollegeManagement.Data.Model;
using CollegeManagement.Data.Repository;
using CollegeManagement.Models;

namespace CollegeManagement.Data.IRepository
{
    public interface IQuestionRespository : ICollegeRepository<Question>
    {
        //Task<APIResponse> GetAllQuestions();
        //Task<APIResponse> GetQuestionById(int Id);
        //Task<APIResponse> AddQuestion(QuestionDTO dto);
        //Task<APIResponse> UpdateQuestion(QuestionDTO dto);
        //Task<APIResponse> DeleteQuestion(int Id);
    }
}
