using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InstagramProject.Models
{
    public class AccountManager
    {
        private readonly InstagramContext _context;

        public AccountManager(InstagramContext context)
        {
            _context = context;
        }

        public void CreateAccount()
        {
            string userName = GetUserName();
            Console.WriteLine($"Username '{userName}' accepted!");

            string email = GetEmail();
            Console.WriteLine($"Email '{email}' accepted!");

            string password = GetPassword();
            Console.WriteLine("Password accepted!");

            User newUser = new User
            {
                UserName = userName,
                Email = email,
                Password = password
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            Console.WriteLine("User account created successfully!");
        }
        public void ChangeUserDetail(string fieldType)
        {
            // När inloggingen är klar så kommer vi få ändra här så att användaren
            // man är inloggad på skickas hit, istället för att skriva in UserId
            // var mest så vi det att det funkar
            Console.WriteLine("Enter your user ID:");
            int userId = Convert.ToInt32(Console.ReadLine());

            User user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                Console.WriteLine("User not found.");
                return;
            }
            switch (fieldType)
            {

                case "username":
                    user.UserName = GetUserName();
                    break;
                case "password":
                    user.Password = GetPassword();
                    break;
                case "email":
                    user.Email = GetEmail();
                    break;
            }

            _context.SaveChanges();
            Console.WriteLine($"{fieldType} updated successfully!");
        }

        public string GetUserName()
        {
            return ValidateNotEmptyAndUnique(
                "Enter a username:",
                "Username cannot be empty. Please try again.",
                "Username already exists. Please choose another one.",
                user => user.UserName!
            );
        }

        public string GetEmail()
        {
            return ValidateNotEmptyAndUnique(
                "Enter a email:",
                "Email cannot be empty. Please try again.",
                "Email already exists. Please choose another one.",
                user => user.Email!
            );
        }

        public string GetPassword()
        {
            while (true)
            {
                Console.Write("Enter a password: ");
                string password = Console.ReadLine()!;

                if (!ValidatePasswordStrength(password))
                {
                    Console.WriteLine("Please enter a stronger password.");
                }
                else
                {
                    Console.WriteLine("Password is strong enough.");
                return password;
                }
            }
        }
        public string ValidateNotEmptyAndUnique(string inputPrompt, string emptyErrorMessage, string duplicateErrorMessage, Func<User, string> fieldSelector)
        {
            while (true)
            {
                Console.WriteLine(inputPrompt);
                string userInput = Console.ReadLine()!;

                if (string.IsNullOrEmpty(userInput))
                {
                    Console.WriteLine(emptyErrorMessage);
                }
                else
                {
                    bool exists = _context.Users
                                          .Select(fieldSelector)
                                          .ToList()
                                          .Contains(userInput);

                    if (exists)
                    {
                        Console.WriteLine(duplicateErrorMessage);
                    }
                    else
                    {
                        return userInput;
                    }
                }
            }
        }

        public string ValidateNotEmpty(string inputPrompt, string emptyErrorMessage)
        {
            while (true)
            {
                Console.WriteLine(inputPrompt);
                string userInput = Console.ReadLine()!;

                if (string.IsNullOrEmpty(userInput))
                {
                    Console.WriteLine(emptyErrorMessage);
                }
                else
                {
                    return userInput;
                }
            }
        }
        public bool ValidatePasswordStrength(string password)
        {
            List<string> errors = new List<string>();

            if (password.Length < 8)
            {
                errors.Add("Password must be at least 8 characters long.");
            }
            if (!password.Any(char.IsUpper))
            {
                errors.Add("Password must contain at least one uppercase letter.");
            }
            if (!password.Any(char.IsLower))
            {
                errors.Add("Password must contain at least one lowercase letter.");
            }
            if (!password.Any(char.IsDigit))
            {
                errors.Add("Password must contain at least one digit.");
            }

            if (errors.Count > 0)
            {
                foreach (var error in errors)
                {
                    Console.WriteLine(error);
                }
                return false;
            }

            return true;
        }
    }
}
