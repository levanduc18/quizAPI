namespace QuizAPI
{
    public class Question
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public virtual ICollection<Answer>? AnswerArray { get; set; }
    }
}
