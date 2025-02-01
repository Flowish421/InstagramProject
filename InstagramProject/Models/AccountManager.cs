using Microsoft.EntityFrameworkCore;
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
            string newUsername;
            string newEmail;
            string newPassword;

            while (true)
            {

                    newUsername = AnsiConsole.Ask<string>("Enter your [green]new username[/]:");

                    if (CheckDuplicate(newUsername))
                    {
                    }
                    else
                    {
                        break;
                    }
            }

            while (true)
            {
                newEmail = AnsiConsole.Ask<string>("Enter your [green]new email[/]:");

                if (CheckDuplicate(newEmail))
                {
                }
                else
                {
                    break;
                }
            }

            while (true)
            {
                newPassword = AnsiConsole.Ask<string>("Enter your [green]new password[/]:");

                if (ValidatePasswordStrength(newPassword))
                {
                    break;
                }
                else
                {
                    AnsiConsole.MarkupLine("[bold red]Password does not meet strength requirements. Please choose a stronger one.[/]");
                }
            }

            User newUser = new User
            {
                UserName = newUsername,
                Email = newEmail,
                Password = newPassword
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            AnsiConsole.MarkupLine("[bold green]User account created successfully![/]");
            // vill vi att användaren ska loggas in här?
        }

        public void ChangeUserDetail(string fieldType, string newValue)
        {
            if (_loggedInUser != null)
            {
                var user = _loggedInUser;

                switch (fieldType)
                {
                    case "Username":
                        if (CheckDuplicate(newValue))
                        {
                            return;
                        }
                        user.UserName = newValue;
                        break;
                    case "Password":
                        if (ValidatePasswordStrength(newValue))
                        {
                            user.Password = newValue;
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[bold red]Password does not meet strength requirements.[/]");
                            return;
                        }
                        break;
                    case "Email":
                        if (CheckDuplicate(newValue))
                        {
                            return;
                        }
                        user.Email = newValue;
                        break;
                    default:
                        AnsiConsole.MarkupLine("[bold red]Invalid field type.[/]");
                        return;
                }

                _context.SaveChanges();
                AnsiConsole.MarkupLine($"[bold green]{fieldType} updated successfully![/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[bold red]No user is logged in.[/]");
            }
        }

        private bool CheckDuplicate(string newValue)
        {
            if (_context.Users.Any(u => u.UserName == newValue))
            {
                AnsiConsole.MarkupLine("[bold red]Username already exists. Please choose a different username.[/]");
                return true;
            }

            if (_context.Users.Any(u => u.Email == newValue))
            {
                AnsiConsole.MarkupLine("[bold red]Email already exists. Please choose a different email.[/]");
                return true;
            }
            return false;
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
