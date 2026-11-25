using System.ComponentModel.DataAnnotations;

namespace UdalykhE_LR5.Models
{
    public class DatabaseSettings
    {
        [Required(ErrorMessage = "Database name cannot be empty or null.")]
        [MinLength(4, ErrorMessage = "Database name must contain at least 4 characters.")]
        public string DatabaseName { get; set; } = string.Empty;
    }
}