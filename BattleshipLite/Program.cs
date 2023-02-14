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

            PlayerInfoModel player = CreatePlayer("Player 1");
            PlayerInfoModel opponent = CreatePlayer("Player 2");
            PlayerInfoModel winner = null;

            do
            {
                DisplayShotGrid(player);
                RecordPlayerShot(player, opponent);
                bool isOpponentActive = PlayerStillActive(opponent);

                if (isOpponentActive)
                {
                    (player, opponent) = (opponent, player);
                }
                else
                {
                    winner = player;
                }
            } while (winner == null);

            IdentifyWinner(winner);

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

            PlayerInfoModel player = new PlayerInfoModel();

            player.PlayerName = AskForPlayerName();
            InitializeGrid(player);
            PlaceShips(player);

            Console.Clear();

            return player;
        }

        private static string AskForPlayerName()
        {
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();

            return name;
        }

        private static void PlaceShips(PlayerInfoModel player)
        {
            do
            {
                Console.Write($"Where do you want to place ship {player.ShipLocations.Count + 1}? ");
                string location = Console.ReadLine();

                bool isShipPlaced = PlaceShip(player, location);
                if (!isShipPlaced)
                {
                    Console.WriteLine("That is not a valid location. Please try again.");
                }
            } while (player.ShipLocations.Count < 5);
        }

        private static void DisplayShotGrid(PlayerInfoModel player)
        {
            Console.WriteLine($"{player.PlayerName}'s Shot Grid");
            Console.WriteLine();

            string currentRow = player.ShotGrid[0].SpotLetter;

            foreach (GridSpotModel gridSpot in player.ShotGrid)
            {
                if (gridSpot.SpotLetter != currentRow)
                {
                    Console.WriteLine();
                    currentRow = gridSpot.SpotLetter;
                }

                switch (gridSpot.Status)
                {
                    case GridSpotStatus.Empty:
                        Console.Write($" {gridSpot.SpotLetter}{gridSpot.SpotNumber} ");
                        break;
                    case GridSpotStatus.Hit:
                        Console.Write(" 💥 ");
                        break;
                    case GridSpotStatus.Miss:
                        Console.Write(" 🌊 ");
                        break;
                    default:
                        Console.Write(" ? ");
                        break;
                }
            }

            Console.WriteLine();
        }

        private static void RecordPlayerShot(PlayerInfoModel player, PlayerInfoModel opponent)
        {
            bool isValidShot;
            string row;
            int column;

            do
            {
                string shot = AskForShot();
                (row, column) = SplitShotIntoRowAndColumn(shot);
                isValidShot = ValidateShot(player, row, column);

                if (!isValidShot)
                {
                    Console.WriteLine("That is not a valid location. Please try again.");
                }
            } while (!isValidShot);

            bool isHit = IdentifyShotResult(opponent, row, column);
            MarkShotResult(player, row, column, isHit);
        }

        private static string AskForShot()
        {
            Console.Write("Enter the location you want to shoot: ");
            return Console.ReadLine();
        }

        private static void IdentifyWinner(PlayerInfoModel winner)
        {
            Console.WriteLine($"Congratulations {winner.PlayerName}! You won the game!");
            Console.WriteLine($"{winner.PlayerName} took {GetShotCount(winner)} shots to sink all of the enemy ships.");
        }
    }
}