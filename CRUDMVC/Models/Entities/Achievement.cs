using System.ComponentModel.DataAnnotations;

namespace CRUDMVC.Models.Entities
{
    public class Achievement
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime AchievedOn { get; set; } = DateTime.Now;


        public string CertificateFileName { get; set; }
        public string CertificateFilePath { get; set; }

    }
}
