using System.ComponentModel.DataAnnotations;

    namespace SmartQuiz_APP.Models
    {
        public class login_signup
        {
            [Key] 
            public int Id { get; set; }

            [Required]
            [StringLength(100)]
            public string First_Name { get; set; }

            [Required]
            [StringLength(100)]
            public string Last_Name { get; set; }

            [Required]
            [StringLength(50)]
            public string Roll_no { get; set; } 

            [Required]
            [EmailAddress]
            [StringLength(100)]
            public string Email { get; set; }

        }
    }