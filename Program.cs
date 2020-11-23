using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace v20201119GraphTutorial
{
    // 2020-11-19 https://docs.microsoft.com/en-us/graph/tutorials/dotnet-core
    // Port from https://github.com/microsoftgraph/msgraph-training-dotnet-core
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(".NET Core Graph Tutorial\n");
            var appConfig = LoadAppSettings();

            if (appConfig == null)
            {
                Console.WriteLine("Missing or invalid appsettings.json...exiting");
                return;
            }

            var appId = appConfig["appId"];
            var scopesString = appConfig["scopes"];
            var scopes = scopesString.Split(';');

            // Initialize the auth provider with values from appsettings.json
            var authProvider = new DeviceCodeAuthProvider(appId, scopes);

            // Request a token to sign in the user
            var accessToken = authProvider.GetAccessToken().Result;

            // Initialize Graph client
            GraphHelper.Initialize(authProvider);

            // Get signed in user
            var user = GraphHelper.GetMeAsync().Result;
            Console.WriteLine($"Welcome {user.DisplayName}!\n");

            int choice = -1;

            while (choice != 0)
            {
                Console.WriteLine("Please choose one of the following options:");
                Console.WriteLine("0. Exit");
                Console.WriteLine("1. Display access token");
                Console.WriteLine("2. View this week's calendar");
                Console.WriteLine("3. Add an event");

                try
                {
                    choice = int.Parse(Console.ReadLine());
                }
                catch (System.FormatException)
                {
                    // Set to invalid value
                    choice = -1;
                }

                switch (choice)
                {
                    case 0:
                        // Exit the program
                        Console.WriteLine("Goodbye...");
                        break;
                    case 1:
                        // Display access token
                        Console.WriteLine($"Access token: {accessToken}\n");
                        break;
                    case 2:
                        // List the calendar
                        break;
                    case 3:
                        // Create a new event
                        break;
                    default:
                        Console.WriteLine("Invalid choice! Please try again.");
                        break;
                }
            }
        }

        static IConfigurationRoot LoadAppSettings()
        {
            var appConfig = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

            // Check for required settings
            if (string.IsNullOrEmpty(appConfig["appId"]) ||
                string.IsNullOrEmpty(appConfig["scopes"]))
            {
                return null;
            }

            return appConfig;
        }
    }
}