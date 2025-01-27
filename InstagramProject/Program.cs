using System;
using Microsoft.Data.SqlClient;  // Correct namespace for SqlClient

class Program
{
    static void Main()
    {
        // Ask the user for their username and password
        Console.Write("Enter Username: ");
        string username = Console.ReadLine();

        Console.Write("Enter Password: ");
        string password = Console.ReadLine();

        // Call the function to check credentials
        bool isAuthenticated = AuthenticateUser(username, password);

        // Display message based on the result
        if (isAuthenticated)
        {
            Console.WriteLine("Login successful!");
        }
        else
        {
            Console.WriteLine("Error: No such account or incorrect password.");
        }
    }

    static bool AuthenticateUser(string username, string password)
    {
        // Define your connection string (replace with actual connection details)
        string connectionString = "Server=MSI;Database=Instagram;Integrated Security=True;TrustServerCertificate=True;";


        // SQL query to check if the username and password match
        string query = "SELECT COUNT(1) FROM Users WHERE Username = @Username AND Password = @Password";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Password", password);  // In production, use hashed passwords

            connection.Open();
            int userCount = (int)command.ExecuteScalar();

            // Return true if user exists, false otherwise
            return userCount > 0;
        }
    }
}
