using System.ComponentModel.DataAnnotations;


namespace MariageApp.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
      

        public string Username { get; set; }

        [StringLength(8,MinimumLength =4,ErrorMessage ="pas mois de 4 et pas plus de 8")]
         [Required]
        public string Password { get; set; }
    }
}