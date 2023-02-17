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

            string name;
            do
            {
                name = AskForPlayerName();
                if (name.Length == 0)
                {
                    Console.WriteLine("You must enter a name.");
                }
            } while (name.Length == 0);

            player.PlayerName = name;
            InitializeGrid(player);
            DisplayShotGrid(player);
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

                bool isShipPlaced = false;

                try
                {
                    isShipPlaced = PlaceShip(player, location);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                if (!isShipPlaced)
                {
                    Console.WriteLine("That is not a valid location. Please try again.");
                }
            } while (player.ShipLocations.Count < 5);
        }

        private static void DisplayShotGrid(PlayerInfoModel player)
        {
            Console.WriteLine();
            Console.WriteLine($"{player.PlayerName}'s shot grid");
            Console.WriteLine();

            string currentRow = player.ShotGrid[0].SpotLetter;
            int maxColumns = (int)Math.Sqrt(player.ShotGrid.Count);
            bool firstColumn = true;

            foreach (GridSpotModel gridSpot in player.ShotGrid)
            {
                if (firstColumn)
                {
                    Console.Write("  ");
                    for (int i = 1; i <= maxColumns; i++)
                    {
                        Console.Write($" {i} ");
                    }
                    firstColumn = false;
                    Console.WriteLine();
                    Console.Write($"{gridSpot.SpotLetter} ");
                }

                if (gridSpot.SpotLetter != currentRow)
                {
                    currentRow = gridSpot.SpotLetter;
                    Console.WriteLine();
                    Console.Write($"{gridSpot.SpotLetter} ");
                }

                switch (gridSpot.Status)
                {
                    case GridSpotStatus.Empty:
                        Console.Write(" ~ ");
                        break;
                    case GridSpotStatus.Hit:
                        Console.Write(" X ");
                        break;
                    case GridSpotStatus.Miss:
                        Console.Write(" O ");
                        break;
                    default:
                        Console.Write(" ? ");
                        break;
                }
            }


            Console.WriteLine();
            Console.WriteLine();
        }

        private static void RecordPlayerShot(PlayerInfoModel player, PlayerInfoModel opponent)
        {
            bool isValidShot;
            string row = "";
            int column = 0;

            do
            {
                string shot = AskForShot(player);
                Console.WriteLine();

                try
                {
                    (row, column) = SplitShotIntoRowAndColumn(shot);
                    isValidShot = ValidateShot(player, row, column);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    isValidShot = false;
                }

                if (!isValidShot)
                {
                    Console.WriteLine("That is not a valid location. Please try again.");
                }
            } while (!isValidShot);

            bool isHit = IdentifyShotResult(opponent, row, column);
            MarkShotResult(player, row, column, isHit);

            DisplayShotResult(row, column, isHit);
        }

        private static void DisplayShotResult(string row, int column, bool isHit)
        {
            Console.WriteLine($"{row}{column} is a {(isHit ? "hit" : "miss")}.");
        }

        private static string AskForShot(PlayerInfoModel player)
        {
            Console.Write($"{player.PlayerName}, your turn to shoot: ");
            return Console.ReadLine();
        }

        private static void IdentifyWinner(PlayerInfoModel winner)
        {
            Console.WriteLine($"Congratulations {winner.PlayerName}! You won the game!");
            Console.WriteLine($"{winner.PlayerName} took {GetShotCount(winner)} shots to sink all of the enemy ships.");
        }
    }
}