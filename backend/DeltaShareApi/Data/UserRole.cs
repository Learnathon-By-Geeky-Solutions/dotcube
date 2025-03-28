﻿using System.ComponentModel.DataAnnotations;

namespace DeltaShareApi.Data;

public class UserRole
{
    public Guid Id { get; set; }
    [EmailAddress]
    public string Email { get; set; } = default!;
    public string Role { get; set; } = default!;
}
