﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourcePlanner.DTOs
{
    public class LoginResponseDto
    {
        public string? UserId { get; set; }
        public string? InstitutionId { get; set; }
        public string? UserRole { get; set; }
        public string? AccessToken { get; set; }
        public int ExpiresIn { get; set; }
    }
}
