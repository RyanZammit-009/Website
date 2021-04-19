using QuestionWeb.Model;

namespace QuestionWeb.Model
{
    public class Question
    {
        public int PlayerID { get; set; }
        public string Text { get; set; }
        public bool IsAnswered { get; set; }
        public bool IsCorrect { get; set; }
        public Answer[] Answer { get; set; }
    }
}
