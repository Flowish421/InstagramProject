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
        private readonly DisplayInstagramMenu _displayInstagramMenu;

        public PostManagement(InstagramContext context, User user, DisplayInstagramMenu displayInstagramMenu)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context), "Database context cannot be null.");
            _currentUser = user ?? throw new ArgumentNullException(nameof(user), "User cannot be null.");
            _displayInstagramMenu = displayInstagramMenu ?? throw new ArgumentNullException(nameof(displayInstagramMenu), "DisplayInstagramMenu cannot be null.");
        }


        //Create Post
        public void CreatePost()
        {
            var image = AnsiConsole.Ask<string>("Enter the [green]image URL[/]:");
            var caption = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter the [green]caption[/]:")
                    .Validate(text =>
                    {
                        return text.Length > 250 ? ValidationResult.Error("[red]Caption cannot be longer than 250 characters![/]") : ValidationResult.Success();
                    })
            );

            using (var context = new InstagramContext())
            {
                var post = new Post
                {
                    Image = image,
                    Caption = caption,
                    UserId = _currentUser.UserId,
                    Timestamp = DateTime.Now
                };
                context.Posts.Add(post);
                context.SaveChanges();
                AnsiConsole.MarkupLine("[bold green]Post created successfully![/]");
            }
        }

        //View Posts
        public void ViewPosts(string username)
        {
            using (var context = new InstagramContext())
            {
                List<Post> posts;

                if (username == "all")
                {
                    posts = context.Posts
                        .Include(p => p.User)
                        .Include(p => p.Likes)
                        .Include(p => p.Comments)
                        .Include(p => p.Shares)
                        .ToList();
                }
                else
                {
                    var user = context.Users
                        .Include(u => u.Posts)
                        .ThenInclude(p => p.Likes)
                        .Include(u => u.Posts)
                        .ThenInclude(p => p.Comments)
                        .Include(u => u.Posts)
                        .ThenInclude(p => p.Shares)
                        .FirstOrDefault(u => u.UserName == username);

                    if (user == null)
                    {
                        AnsiConsole.MarkupLine("[bold red]User not found.[/]");
                        _displayInstagramMenu.DisplayPostMenu();
                        
                        return;
                    }

                    posts = user.Posts.ToList();
                }

                if (posts.Any())
                {
                    int currentIndex = 0;
                    while (true)
                    {
                        var post = posts[currentIndex];
                        var user = post.User;

                        DisplayPost(post, user, currentIndex, posts.Count);

                        var navigationMenu = new SelectionPrompt<string>()
                            .Title("[bold yellow]------ Navigation ------[/]")
                            .AddChoices("Next", "Previous", "Comment", "Share", "Exit");

                        // Check if the user has already liked the post
                        var userLike = post.Likes.FirstOrDefault(l => l.UserId == _currentUser.UserId);
                        if (userLike != null)
                        {
                            navigationMenu.AddChoice("Unlike");
                        }
                        else
                        {
                            navigationMenu.AddChoice("Like");
                        }

                        string navigationChoice = AnsiConsole.Prompt(navigationMenu);

                        switch (navigationChoice)
                        {
                            case "Next":
                                currentIndex = (currentIndex + 1) % posts.Count;
                                break;
                            case "Previous":
                                currentIndex = (currentIndex - 1 + posts.Count) % posts.Count;
                                break;
                            case "Like":
                                LikePost(post);
                                break;
                            case "Unlike":
                                UnlikePost(post, userLike);
                                break;
                            case "Comment":
                                CommentOnPost(post);
                                break;
                            case "Share":
                                SharePost(post);
                                break;
                            case "Exit":
                                _displayInstagramMenu.DisplayPostMenu();
                                return;
                        }

                        // Reload the post to get updated data
                        post = context.Posts
                            .Include(p => p.User)
                            .Include(p => p.Likes)
                            .Include(p => p.Comments)
                            .Include(p => p.Shares)
                            .FirstOrDefault(p => p.PostId == post.PostId);

                        user = post.User;
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine("[bold red]No posts available.[/]");
                    _displayInstagramMenu.DisplayPostMenu();
                }
            }
        }
        private void DisplayPost(Post post, User user, int currentIndex, int totalPosts)
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine($"[bold yellow]Post {currentIndex + 1}/{totalPosts}[/]");
            AnsiConsole.MarkupLine($"[bold yellow]{post.Image}[/]");
            AnsiConsole.MarkupLine($"{post.Caption}");
            AnsiConsole.MarkupLine($"[italic]Posted by {user?.UserName} on {post.Timestamp}[/]");
            AnsiConsole.MarkupLine("");
            AnsiConsole.MarkupLine($"[bold yellow]Likes: {post.Likes.Count}, Comments: {post.Comments.Count}, Shares: {post.Shares.Count}[/]");
        }


        //Like, Unlike, Comment, Share Methods
        private void LikePost(Post post)
        {
            using (var context = new InstagramContext())
            {
                var like = new Like
                {
                    PostId = post.PostId,
                    UserId = _currentUser.UserId,
                    Timestamp = DateTime.Now
                };

                context.Likes.Add(like);
                context.SaveChanges();
                AnsiConsole.MarkupLine("[bold green]Post liked![/]");
            }
        }
        private void UnlikePost(Post post, Like userLike)
        {
            using (var context = new InstagramContext())
            {
                context.Likes.Remove(userLike);
                context.SaveChanges();
                AnsiConsole.MarkupLine("[bold green]Post unliked![/]");
            }
        }
        private void CommentOnPost(Post post)
        {
            using (var context = new InstagramContext())
            {
                var comments = context.Comments
                    .Include(c => c.User)
                    .Where(c => c.PostId == post.PostId)
                    .ToList();

                if (comments.Any())
                {
                    AnsiConsole.MarkupLine("[bold yellow]Comments:[/]");
                    foreach (var comment in comments)
                    {
                        AnsiConsole.MarkupLine($"[bold]{comment.User?.UserName}[/] commented: \"{comment.CommentText}\" on {comment.Timestamp}");
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine("[bold red]No comments available.[/]");
                }

                var commentText = AnsiConsole.Ask<string>("Enter your [green]comment[/]:");

                var newComment = new Comment
                {
                    PostId = post.PostId,
                    UserId = _currentUser.UserId,
                    CommentText = commentText,
                    Timestamp = DateTime.Now
                };

                context.Comments.Add(newComment);
                context.SaveChanges();
                AnsiConsole.MarkupLine("[bold green]Comment added![/]");
            }
        }
        private void SharePost(Post post)
        {
            using (var context = new InstagramContext())
            {
                var share = new Share
                {
                    PostId = post.PostId,
                    UserId = _currentUser.UserId,
                    Timestamp = DateTime.Now
                };

                context.Shares.Add(share);
                context.SaveChanges();
                AnsiConsole.MarkupLine("[bold green]Post shared![/]");
            }
        }
    }
}