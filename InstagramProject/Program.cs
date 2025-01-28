using InstagramProject.Models;
using Microsoft.EntityFrameworkCore;
using System;

class Program
{
    static void Main(string[] args)
    {
        var context = new InstagramContext();
        AccountManager accountManager = new AccountManager(context);
        DisplayInstagramMenu instagramMenu = new DisplayInstagramMenu(accountManager);

        // Skapa ett konto 
        //accountManager.CreateAccount();

        // Kör settingsmenu
        instagramMenu.DisplaySettingsMenu();

        // Kör huvudmenyn
        //instagramMenu.RunSystem();
    }
}
