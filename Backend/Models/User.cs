﻿using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class User : BaseModel
    {
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        public string? PasswordHash { get; set; }
        [Required]
        public string? Email { get; set; }

        public int IsActive { get; set; }

        [ForeignKey(nameof(FamilyId))]
        public Guid FamilyId { get; set; }

        public User() { }

        public User(Guid id, string firstName, string lastName, string email, string plainPassword, Guid familyId, int isActive)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PasswordHash = HashPassword(plainPassword);
            FamilyId = familyId;
            IsActive = isActive;
        }

        public bool VerifyPassword(string plainPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword, PasswordHash);
        }

        // Private method to hash the password
        private static string HashPassword(string plainPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(plainPassword);
        }
    }
}
