using System.ComponentModel.DataAnnotations;

public class UserAnswer
{
    public int Id { get; set; }

    [Required]
    public int TestAttemptId { get; set; }

    [Required]
    public int QuestionId { get; set; }

    [Required]
    [EnumDataType(typeof(AnswerOption))]
    public AnswerOption SelectedAnswer { get; set; }
}
