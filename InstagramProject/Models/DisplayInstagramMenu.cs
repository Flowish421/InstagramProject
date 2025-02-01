using Spectre.Console;
using System;

namespace InstagramProject.Models
{
    public class DisplayInstagramMenu
    {
        private readonly AccountManager _accountManager;
        private readonly InstagramContext _context;
        private User _currentUser;
        private PostManagement _postManagement;

        public DisplayInstagramMenu(User currentUser, InstagramContext context, AccountManager accountManager)
        {
            _currentUser = currentUser;
            _context = context;
            _accountManager = accountManager;
            _postManagement = new PostManagement(_context, _currentUser, this);
        }
        public void DisplayUserMenu()
        {
            var menu = new SelectionPrompt<string>()
                .Title("[bold yellow]------ User Menu ------[/]")
                .AddChoices("Create Post", "Show Posts", "Settings", "Exit");

            string choice = AnsiConsole.Prompt(menu);

            switch (choice)
            {
                case "Create Post":
                    _postManagement.CreatePost();
                    break;
                case "Show Posts":
                    HandlePostMenu();
                    break;
                case "Settings":
                    HandleSettingsMenu();
                    break;
                case "Exit":
                    AnsiConsole.MarkupLine("[bold red]Exit...[/]");
                    Environment.Exit(0);
                    break;
            }
        }

        public void DisplayPostMenu()
        {
            var postMenu = new SelectionPrompt<string>()
                .Title("[bold yellow]------ Post Menu ------[/]")
                .AddChoices("Show Your Posts", "Show All Posts", "Search for User's Posts", "Back to Main Menu");

            string choice = AnsiConsole.Prompt(postMenu);

            switch (choice)
            {
                case "Show Your Posts":
                   _postManagement.ViewPosts(_currentUser.UserName!);
                    break;
                case "Show All Posts":
                    _postManagement.ViewPosts("all");
                    break;
                case "Search for User's Posts":
                    var username = AnsiConsole.Ask<string>("Enter the [green]username[/] to search:");
                    _postManagement.ViewPosts(username);
                    break;
                case "Back to Main Menu":
                    DisplayUserMenu();
                    break;
            }
        }

        public void DisplayInteractionPostMenu()
        {
            var interactionMenu = new SelectionPrompt<string>()
                .Title("[bold yellow]------ Interaction Post Menu ------[/]")
                .AddChoices("Show Next Post", "Show Previous Post", "Like", "Share", "Comment / See All Comments", "Delete Post", "Back to Post Menu");

            string choice = AnsiConsole.Prompt(interactionMenu);

            switch (choice)
            {
                case "Show Next Post":
                    // Show next post
                    break;
                case "Show Previous Post":
                    // Show previous post
                    break;
                case "Like":
                    // Like post
                    break;
                case "Share":
                    // Share post
                    break;
                case "Comment / See All Comments":
                    // See or comment
                    break;
                case "Delete Post":
                    // Delete post if it's user's own post
                    break;
                case "Back to Post Menu":
                    HandlePostMenu();
                    break;
            }
        }

        public void DisplaySettingsMenu()
        {
            var settingsMenu = new SelectionPrompt<string>()
                .Title("[bold yellow]------ Settings Menu ------[/]")
                .AddChoices("Change Username", "Change Password", "Change Email", "Back to Main Menu");

            string choice = AnsiConsole.Prompt(settingsMenu);

            switch (choice)
            {
                case "Change Username":
                    var newUsername = AnsiConsole.Ask<string>("Enter your [green]new username[/]:");
                    _accountManager.ChangeUserDetail("Username", newUsername);
                    break;
                case "Change Password":
                    var newPassword = AnsiConsole.Ask<string>("Enter your [green]new password[/]:");
                    _accountManager.ChangeUserDetail("Password", newPassword);
                    break;
                case "Change Email":
                    var newEmail = AnsiConsole.Ask<string>("Enter your [green]new email[/]:");
                    _accountManager.ChangeUserDetail("Email", newEmail);
                    break;
                case "Back to Main Menu":
                    DisplayUserMenu();
                    break;
            }
        }

        public void RunSystem()
        {
            bool running = true;

            while (running)
            {
                DisplayUserMenu();

                // To ask the user whether they want to continue:
                var continueChoice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title("Would you like to continue?")
                    .AddChoices("Yes", "No"));

                if (continueChoice == "No")
                {
                    running = false;
                }
            }
        }

        private void HandlePostMenu()
        {
            bool inPostMenu = true;

            while (inPostMenu)
            {
                DisplayPostMenu();
            }
        }

        private void HandleSettingsMenu()
        {
            bool inSettingsMenu = true;

            while (inSettingsMenu)
            {
                DisplaySettingsMenu();
            }
        }
    }
}

