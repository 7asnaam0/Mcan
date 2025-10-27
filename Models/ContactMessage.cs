using System;
using System.ComponentModel.DataAnnotations;

namespace Models;

public class ContactMessage
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Message { get; set; }
}
