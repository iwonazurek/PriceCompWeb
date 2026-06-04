using System.ComponentModel.DataAnnotations;

namespace PriceCompWeb.Models;

public class AdminLoginViewModel
{
    [Required]
    [EmailAddress]
    [Display(Name = "E-mail")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Haslo")]
    public string Password { get; set; } = string.Empty;

    public string? ReturnUrl { get; set; }
}
