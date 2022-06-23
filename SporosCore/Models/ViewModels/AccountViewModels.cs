using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SporosCore.Models.ViewModels
{
    public class LoginViewModel
    {

        [Required]
        [Display(Name = "email")]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }
    }
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="Введите адрес электронной почты")]
        [EmailAddress]
        [Display(Name = "Адрес электронной почты")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Введите пароль")]
        [StringLength(100, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage ="Введите подтверждение пароля")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароль и его подтверждение не совпадают.")]
        public string ConfirmPassword { get; set; }

        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Patronymic { get; set; }
        public string CompanyName { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
    }
    public class ProfileViewModel
    {
        [Required(ErrorMessage = "Введите адрес электронной почты")]
        [EmailAddress]
        [Display(Name = "Адрес электронной почты")]
        public string Email { get; set; }

        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Patronymic { get; set; }
        public string CompanyName { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public List<Address> Addresses = new List<Address>();
        public string City { get; set; }
        public string Address { get; set; }
        public List<Orders> Orders = new List<Orders>();
        public List<OrderItems> OrderItems = new List<OrderItems>();
        public List<Items> Items = new List<Items>();
        public bool AddressAdd { get; set; }
        public int AddressId { get; set; }
    }
}
