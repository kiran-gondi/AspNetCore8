using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using CompareAttribute = System.ComponentModel.DataAnnotations.CompareAttribute;

namespace CitiesManager.Core.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Person Name can't be blank")]
        public string PersonName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress(ErrorMessage = "Email should be in a proper email address format")]
        [Remote(action: "IsEmailAlreadyRegistered", controller:"Account", ErrorMessage ="Email is already in use")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "PhoneNumber can't be blank")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "PhoneNumber should contains digits only")]
        //[Remote(action: "IsEmailAlreadyRegister", controller: "Account", ErrorMessage = "Email is already in use")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password can't be blank")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "ConfirmPassword can't be blank")]
        [Compare("Password", ErrorMessage = "Password and confirm password do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
