﻿using System.ComponentModel.DataAnnotations;


namespace asp_net_po_schedule_management_server.Dto.Requests
{
    public sealed class ChangePasswordRequestDto
    {
        [Required(ErrorMessage = "Pole poprzedniego hasła nie może być puste")]
        public string OldPassword { get; set; }
        
        [Required(ErrorMessage = "Pole nowego hasła nie może być puste")]
        [RegularExpression(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$",
            ErrorMessage = "Hasło musi mieć minimum 8 znaków, zawierać co najmniej jedną liczbę, " +
                           "jedną wielką literę oraz jeden znak specjalny.")]
        public string NewPassword { get; set; }
        
        [Required(ErrorMessage = "Pole potwierdzenia nowego hasła nie może być puste")]
        [Compare(nameof(NewPassword), ErrorMessage = "Hasła w obu polach muszą być identyczne.")]
        public string NewPasswordConfirmed { get; set; }
    }
}