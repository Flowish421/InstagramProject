using InstagramProject.Models;
using Microsoft.EntityFrameworkCore;

class Program
{
    static void Main(string[] args)
    {
        using (var context = new InstagramContext()) // ✅ Use ONE instance for all services
        {
            context.Database.EnsureCreated(); // ✅ Ensure the database is set up

            AccountManager accountManager = new AccountManager(context);
            accountManager.LoginMenu();

            //// 🔹 Ensure a user exists in the database
            //var currentUser = context.Users.FirstOrDefault();
            //if (currentUser == null)
            //{
            //    currentUser = new User
            //    {
            //        UserName = "JohnDoe",
            //        Password = "1234",
            //        Email = "john@example.com"
            //    };
            //    context.Users.Add(currentUser);
            //    context.SaveChanges();
            //}

            //// 🔹 Initialize AccountManager and PostManagement properly
            //AccountManager accountManager = new AccountManager(context);
            //PostManagement postManagement = new PostManagement(context, currentUser);

            //// ✅ Pass PostManagement to DisplayInstagramMenu
            //DisplayInstagramMenu instagramMenu = new DisplayInstagramMenu(accountManager, postManagement);

            //// 🔹 Run Instagram Menu
            //instagramMenu.DisplaySettingsMenu();
            //instagramMenu.DisplayUserMenu();

            //// 🔹 Create and Show Posts
            //postManagement.CreatePostFromUserInput();
            //postManagement.DisplayAllPosts();
        }
    }
}
