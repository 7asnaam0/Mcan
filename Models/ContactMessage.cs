using System;
using System.ComponentModel.DataAnnotations;

namespace Models;

public class ContactMessage
{
    public int Id { get; set; }


    public string Name { get; set; }

    [EmailAddress(ErrorMessage = "Invalid Email address.")]
    public string Email { get; set; }

    [RegularExpression(@"^\d{11}$", ErrorMessage = "Phone number must be exactly 11 digits.")]
    public string PhoneNumber { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
