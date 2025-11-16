using System.ComponentModel.DataAnnotations;


namespace SmartQuiz_APP.Models
{
    public class QuizAttempt
    {
        [Key]
        public int Id { get; set; } 


        [Required] 
        public int UserId { get; set; }


        public login_signup User { get; set; } = null!; 

        // Quiz Details
        [Required(ErrorMessage = "Score is required.")]
        [Range(0, 10, ErrorMessage = "Score must be between 0 and 10.")]
        public int Score { get; set; }

        [Required(ErrorMessage = "Total Questions is required.")]
        [Range(1, 10, ErrorMessage = "Total Questions must be between 1 and 10.")] 
        public int TotalQuestions { get; set; }

        [Required]
        public DateTime AttemptDate { get; set; } = DateTime.UtcNow; 
    }
}