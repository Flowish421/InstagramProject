using System;
using System.Linq;
using InstagramProject.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp
{
    class Program
    {
        static void Main()
        {
            // Begär användarinmatning
            Console.Write("Ange användarnamn: ");
            string username = Console.ReadLine();
            Console.Write("Ange lösenord: ");
            string password = Console.ReadLine();

            // Testa anslutningen till databasen innan vi fortsätter
            if (TestDatabaseConnection())
            {
                // Försök att autentisera användaren
                bool isAuthenticated = AuthenticateUser(username, password);

                if (isAuthenticated)
                {
                    Console.WriteLine("Inloggning lyckades!");
                }
                else
                {
                    Console.WriteLine("Fel användarnamn eller lösenord.");
                }
            }
            else
            {
                Console.WriteLine("Kunde inte ansluta till databasen.");
            }

            Console.ReadLine();
        }

        // Metod för att testa anslutning till databasen utan att skriva ut användarna
        private static bool TestDatabaseConnection()
        {
            try
            {
                using (var context = new InstagramContext())
                {
                    // Försök att hämta alla användare för att se om databasen är tillgänglig
                    var users = context.Users.Take(1).ToList(); // Ta bara 1 användare för att testa anslutningen

                    return true;  // Om vi kan hämta en användare, anslutning är okej
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fel vid anslutning till databasen: {ex.Message}");
                return false;
            }
        }

        // Metod för att autentisera användaren via Entity Framework
        private static bool AuthenticateUser(string username, string password)
        {
            using (var context = new InstagramContext())  // Använd din InstagramContext
            {
                // Använd ToLower() för att säkerställa att jämförelsen är okänslig för stora och små bokstäver
                var user = context.Users
                    .Where(u => u.UserName.ToLower() == username.ToLower() && u.Password == password)
                    .FirstOrDefault();  // Hämta den första matchande användaren

                // Om användaren hittas, autentisering är lyckad
                return user != null;
            }
        }
    }
}
