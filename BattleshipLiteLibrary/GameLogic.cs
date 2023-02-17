using BattleshipLiteLibrary.Models;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

namespace BattleshipLiteLibrary
{
    public static class GameLogic
    {
        private static readonly Regex ShotPattern = new Regex(@"^[A-Za-z]{1}[0-9]{1}$");

        public static void InitializeGrid(PlayerInfoModel player)
        {
            List<string> letters = new List<string> { "A", "B", "C", "D", "E" };
            List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

            foreach (string letter in letters)
            {
                foreach (int number in numbers)
                {
                    AddGridSpot(player, letter, number);
                }
            }
        }

        private static void AddGridSpot(PlayerInfoModel player, string letter, int number)
        {
            GridSpotModel spot = new GridSpotModel
            {
                SpotLetter = letter,
                SpotNumber = number,
                Status = GridSpotStatus.Empty
            };

            player.ShotGrid.Add(spot);
        }

        public static bool PlaceShip(PlayerInfoModel player, string location)
        {
            bool output = false;
            (string row, int column) = SplitShotIntoRowAndColumn(location);

            bool isValidLocation = ValidateGridLocation(player, row, column);
            bool isSpotOpen = ValidateShipLocation(player, row, column);

            if (isValidLocation && isSpotOpen)
            {
                player.ShipLocations.Add(new GridSpotModel
                {
                    SpotLetter = row,
                    SpotNumber = column,
                    Status = GridSpotStatus.Ship
                });

                output = true;
            }

            return output;
        }

        private static bool ValidateGridLocation(PlayerInfoModel player, string row, int column)
        {
            bool isValid = false;

            foreach (GridSpotModel spot in player.ShotGrid)
            {
                if (spot.SpotLetter == row && spot.SpotNumber == column)
                {
                    isValid = true;
                }
            }

            return isValid;
        }

        private static bool ValidateShipLocation(PlayerInfoModel player, string row, int column)
        {
            bool isValid = true;

            foreach (GridSpotModel ship in player.ShipLocations)
            {
                if (ship.SpotLetter == row && ship.SpotNumber == column)
                {
                    isValid = false;
                }
            }

            return isValid;
        }

        public static bool PlayerStillActive(PlayerInfoModel player)
        {
            bool isActive = false;

            foreach (GridSpotModel ship in player.ShipLocations)
            {
                if (ship.Status != GridSpotStatus.Sunk)
                {
                    isActive = true;
                }
            }

            return isActive;
        }

        public static int GetShotCount(PlayerInfoModel player)
        {
            int shotsTaken = 0;

            foreach (GridSpotModel shot in player.ShotGrid)
            {
                if (shot.Status != GridSpotStatus.Empty)
                {
                    shotsTaken += 1;
                }
            }

            return shotsTaken;
        }

        public static (string row, int column) SplitShotIntoRowAndColumn(string shot)
        {
            if (shot.Length != 2 || !ShotPattern.IsMatch(shot))
            {
                throw new ArgumentException("Shot must be two characters long, e.g. A1", nameof(shot));
            }

            string row = shot.Substring(0, 1).ToUpper();
            int column = int.Parse(shot.Substring(1, 1));

            return (row, column);
        }

        public static bool ValidateShot(PlayerInfoModel player, string row, int column)
        {
            bool isValid = false;

            foreach (GridSpotModel shot in player.ShotGrid)
            {
                if (shot.SpotLetter == row && shot.SpotNumber == column && shot.Status == GridSpotStatus.Empty)
                {
                    isValid = true;
                }
            }

            return isValid;
        }

        public static bool IdentifyShotResult(PlayerInfoModel player, string row, int column)
        {
            bool isHit = false;

            foreach (GridSpotModel ship in player.ShipLocations)
            {
                if (ship.SpotLetter == row && ship.SpotNumber == column)
                {
                    isHit = true;
                    ship.Status = GridSpotStatus.Sunk;
                }
            }

            return isHit;
        }

        public static void MarkShotResult(PlayerInfoModel player, string row, int column, bool isHit)
        {
            foreach (GridSpotModel shot in player.ShotGrid)
            {
                if (shot.SpotLetter == row && shot.SpotNumber == column)
                {
                    shot.Status = isHit ? GridSpotStatus.Hit : GridSpotStatus.Miss;
                }
            }
        }
    }
}