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

        }
    }
}
