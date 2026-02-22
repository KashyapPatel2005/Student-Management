using System.ComponentModel.DataAnnotations;

public class AddMultipleQuestionsViewModel
{
    public int TestId { get; set; }

    public List<QuestionInputModel> Questions { get; set; } = new();
}

public class QuestionInputModel
{
    [Required]
    public string Text { get; set; }

    [Required]
    public string OptionA { get; set; }

    [Required]
    public string OptionB { get; set; }

    [Required]
    public string OptionC { get; set; }

    [Required]
    public string OptionD { get; set; }

    [Required]
    public AnswerOption CorrectAnswer { get; set; }
}
