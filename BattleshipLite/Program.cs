using BattleshipLiteLibrary;
using BattleshipLiteLibrary.Models;
using System;
using static BattleshipLiteLibrary.GameLogic;

namespace BattleshipLite
{
    internal class Program
    {
        static void Main(string[] args)
        {
            WelcomeMessage();

            var player1 = CreatePlayer("Player 1");
            var player2 = CreatePlayer("Player 2");

            Console.ReadLine();
        }

        private static void WelcomeMessage()
        {
            Console.WriteLine("Welcome to Battleship Lite!");
            Console.WriteLine();
        }

        private static PlayerInfoModel CreatePlayer(string playerTitle)
        {
            Console.WriteLine($"Setup for {playerTitle}");
            Console.WriteLine();

            var player = new PlayerInfoModel();

            player.PlayerName = AskForPlayerName();

            InitializeGrid(player);

            PlaceShips(player);

            Console.Clear();

            return player;
        }

        private static string AskForPlayerName()
        {
            Console.Write("Enter your name: ");
            var name = Console.ReadLine();

            return name;
        }

        private static void PlaceShips(PlayerInfoModel player)
        {
            do
            {
                Console.Write($"Where do you want to place ship {player.ShipLocations.Count + 1}? ");
                var location = Console.ReadLine();

                var isShipPlaced = GameLogic.PlaceShip(player, location);
                if (!isShipPlaced)
                {
                    Console.WriteLine("That is not a valid location. Please try again.");
                }
            } while (player.ShipLocations.Count < 5);
        }
    }
}
