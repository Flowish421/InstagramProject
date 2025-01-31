using Microsoft.EntityFrameworkCore;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace InstagramProject.Models
{
    public class PostManagement
    {
        private readonly InstagramContext _context;
        private readonly User _currentUser;

        public PostManagement(InstagramContext context, User user)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context), "Database context cannot be null.");
            _currentUser = user ?? throw new ArgumentNullException(nameof(user), "User cannot be null.");
        }

        // Method to create a post with user input
        public void CreatePostFromUserInput()
        {
            // Prompt user for the image URL with Spectre's text input
            string imageUrl = AnsiConsole.Prompt(
                new TextPrompt<string>("[bold yellow]   Enter the picture URL:[/]")
                    .PromptStyle("cyan")
                    .Validate(url => !string.IsNullOrWhiteSpace(url) ? ValidationResult.Success() : ValidationResult.Error("[red]URL cannot be empty![/]"))
            );

            // Prompt user for the caption with validation (max 250 chars)
            string caption = AnsiConsole.Prompt(
                new TextPrompt<string>("[bold yellow]   Enter the caption (max 250 characters):[/]")
                    .PromptStyle("cyan")
                    .Validate(text =>
                        string.IsNullOrWhiteSpace(text) ? ValidationResult.Error("[red]Caption cannot be empty![/]") :
                        text.Length > 250 ? ValidationResult.Error("[red]Caption is too long! (Max 250 characters)[/]") :
                        ValidationResult.Success()
                    )
            );

            // Create and save post
            var newPost = new Post
            {
                Image = imageUrl,
                Caption = caption,
                Timestamp = DateTime.Now,
                UserId = _currentUser.UserId
            };

            // Display confirmation message
            AnsiConsole.MarkupLine("[bold green]   Post details collected successfully![/]");

            // Call CreatePost method to save post
            CreatePost(newPost);
        }



        // Method to create and save a new post
        public void CreatePost(Post newPost)
        {
            try
            {
                _context.Posts.Add(newPost);
                _context.SaveChanges();

                // Success message with a styled confirmation box
                AnsiConsole.MarkupLine("\n[bold green]   Post successfully created![/]");

                var panel = new Panel($"[bold cyan]Post ID:[/] [yellow]{newPost.PostId}[/]\n" +
                                      $"[bold cyan]Image:[/] [green]{newPost.Image}[/]\n" +
                                      $"[bold cyan]Caption:[/] [blue]{newPost.Caption}[/]\n" +
                                      $"[bold cyan]Timestamp:[/] [magenta]{newPost.Timestamp}[/]")
                {
                    Border = BoxBorder.Rounded,
                    Header = new PanelHeader("[bold yellow]   New Post Details[/]"),
                    Padding = new Padding(2, 1, 2, 1)
                };

                AnsiConsole.Write(panel);
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[bold red]   Error creating post:[/] {ex.Message}");
            }
        }

        // Method to display all posts
        public void DisplayAllPosts()
        {
            try
            {
                var posts = _context.Posts.ToList();

                if (posts.Count == 0)
                {
                    AnsiConsole.MarkupLine("[bold yellow]   No posts found in the database.[/]");
                    return;
                }

                // Create a table
                var table = new Table();
                table.Border(TableBorder.Rounded);
                table.AddColumn("[cyan]Post ID[/]");
                table.AddColumn("[green]Image[/]");
                table.AddColumn("[blue]Caption[/]");
                table.AddColumn("[magenta]Timestamp[/]");

                // Add posts to the table
                foreach (var post in posts)
                {
                    table.AddRow(
                        $"[cyan]{post.PostId}[/]",
                        $"[green]{post.Image}[/]",
                        $"[blue]{post.Caption}[/]",
                        $"[magenta]{post.Timestamp}[/]"
                    );
                }

                // Display the table
                AnsiConsole.Write(table);
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[bold red]   Error retrieving posts:[/] {ex.Message}");
            }
        }


        public void SearchForUserPosts()
        {
            try
            {
                // Fetch all users
                var users = _context.Users.ToList();

                if (users.Count == 0)
                {
                    Console.WriteLine(" No users found in the database.");
                    return;
                }

                // Display all users' names and IDs
                Console.WriteLine("\n List of Users:");
                foreach (var user in users)
                {
                    Console.WriteLine($"[{user.UserId}] {user.UserName}");
                }

                // Prompt user to select a user by username or ID
                Console.Write("\nEnter the username or ID to search for posts: ");
                string userInput = Console.ReadLine()?.Trim();

                // Validate input
                if (string.IsNullOrEmpty(userInput))
                {
                    Console.WriteLine(" Input cannot be empty.");
                    return;
                }

                // Try to parse user input as ID, if possible
                int userId;
                User selectedUser = null;

                if (int.TryParse(userInput, out userId))
                {
                    // Search user by ID
                    selectedUser = _context.Users.FirstOrDefault(u => u.UserId == userId);
                }
                else
                {
                    // Search user by username
                    selectedUser = _context.Users.FirstOrDefault(u => u.UserName.Equals(userInput, StringComparison.OrdinalIgnoreCase));
                }

                if (selectedUser == null)
                {
                    Console.WriteLine($" No user found with the provided input: {userInput}");
                    return;
                }

                // Fetch posts by the selected user ID
                var posts = _context.Posts.Where(p => p.UserId == selectedUser.UserId).ToList();

                if (posts.Count == 0)
                {
                    Console.WriteLine($" No posts found for user: {selectedUser.UserName}");
                    return;
                }

                // Display posts by the selected user
                Console.WriteLine($"\n Posts by {selectedUser.UserName}:");
                foreach (var post in posts)
                {
                    Console.WriteLine($"[{post.PostId}] {post.Image} - {post.Caption} ({post.Timestamp})");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error retrieving posts: {ex.Message}");
            }
        }

    }
}