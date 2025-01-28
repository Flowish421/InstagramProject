using InstagramProject.Models;
using Microsoft.EntityFrameworkCore;
using System;

class Program
{
    static void Main(string[] args)
    {
        var context = new InstagramContext();
        AccountManager accountManager = new AccountManager(context);
        DisplayInstagramMenu instagramMenu = new DisplayInstagramMenu();

        // Skapa ett konto först om användaren vill
        accountManager.CreateAccount();

        // Kör huvudmenyn
        //instagramMenu.RunSystem();
    }
}
