using BattleshipLiteLibrary.Models;
using System;
using System.Collections.Generic;

namespace BattleshipLiteLibrary
{
    public static class GameLogic
    {
        public static void InitializeGrid(PlayerInfoModel player)
        {
            var letters = new List<string> { "A", "B", "C", "D", "E" };
            var numbers = new List<int> { 1, 2, 3, 4, 5 };

            foreach (var letter in letters)
            {
                foreach (var number in numbers)
                {
                    AddGridSpot(player, letter, number);
                }
            }
        }

        private static void AddGridSpot(PlayerInfoModel player, string letter, int number)
        {
            var spot = new GridSpotModel
            {
                SpotLetter = letter,
                SpotNumber = number,
                Status = GridSpotStatus.Empty
            };

            player.ShotGrid.Add(spot);
        }

        public static bool PlaceShip(PlayerInfoModel player, string location)
        {
            throw new NotImplementedException();
        }
    }
}