﻿using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.UserAggregate.User;
public class Login
{
    [Required]
    public string Username { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    public bool RememberMe { get; set; } = false;
}