﻿using Microsoft.EntityFrameworkCore;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InstagramProject.Models
{
    public class AccountManager
    {
        private readonly InstagramContext _context;
        private User _loggedInUser;
        DisplayInstagramMenu instagramMenu;

        public AccountManager(InstagramContext context)
        {
            _context = context;
        }

        public void LoginMenu()
        {
            var loginMenu = new SelectionPrompt<string>()
                .Title("[bold yellow]------ Login Menu ------[/]")
                .AddChoices("Login", "Create User", "Exit");

            string loginChoice = AnsiConsole.Prompt(loginMenu);

            switch (loginChoice)
            {
                case "Login":
                    HandleLogin();
                    break;
                case "Create User":
                    CreateAccount();
                    break;
                case "Exit":
                    AnsiConsole.MarkupLine("[bold red]Exit...[/]");
                    Environment.Exit(0);
                    break;
            }
        }


        private void HandleLoginExtra()
        {
            //Ahmed kör sin kod här.
        }

        private void HandleLogin()
        {
            var username = AnsiConsole.Ask<string>("Enter your [green]username[/]:");
            var password = AnsiConsole.Ask<string>("Enter your [green]password[/]:");

            var user = _context.Users.FirstOrDefault(user => user.UserName == username && user.Password == password);

            if (user != null)
            {
                _loggedInUser = user;
                AnsiConsole.MarkupLine("[bold green]Login successful![/]");

                //Här skapar jag DisplayInstragramMenu objektet och sen skickar in all nödvändig data vidare till den objektet
                instagramMenu = new DisplayInstagramMenu(_loggedInUser, _context, this);
                //Sen kör jag metoden för att visa användarmenyn för användaren när den är inloggad.
                instagramMenu.DisplayUserMenu();
            }
            else
            {
                AnsiConsole.MarkupLine("[bold red]Invalid username or password.[/]");
            }
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
        public void ChangeUserDetail(string fieldType, string newValue)
        {
            if (_loggedInUser != null)
            {
                var user = _context.Users.FirstOrDefault(u => u.UserId == _loggedInUser.UserId);
                if (user != null)
                {
                    switch (fieldType)
                    {
                        case "Username":
                            user.UserName = newValue;
                            break;
                        case "Password":
                            user.Password = newValue;
                            break;
                        case "Email":
                            user.Email = newValue;
                            break;
                    }
                    _context.SaveChanges();
                    AnsiConsole.MarkupLine($"[bold green]{fieldType} updated successfully![/]");
                }
            }
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
