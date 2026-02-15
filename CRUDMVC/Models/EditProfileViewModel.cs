using System.ComponentModel.DataAnnotations;

public class EditProfileViewModel
{
    [Required]
    public string UserName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
