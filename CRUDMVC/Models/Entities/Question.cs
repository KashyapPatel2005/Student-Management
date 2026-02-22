using System.ComponentModel.DataAnnotations;

public class Question
{
    public int Id { get; set; }

    [Required]
    public int TestId { get; set; }

    [Required(ErrorMessage = "Question text is required")]
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
    [EnumDataType(typeof(AnswerOption))]
    public AnswerOption CorrectAnswer { get; set; }

    public Test Test { get; set; }
}
