using System.ComponentModel.DataAnnotations;

namespace ModelValidationsExample.Models
{
    public class Person
    {
        [Required]
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public double? Price { get; set; }

        public override string ToString()
        {
            return $"Person Object - Person name: {PersonName}, Email: {Email}, " +
                $"Phone: {Phone}, Password: {Password}, " +
                $"ConfirmPassword: {ConfirmPassword}, Price: {Price}";
        }
    }
}
