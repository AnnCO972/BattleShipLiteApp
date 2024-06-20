using BattleShipLiteLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipLiteLibrary
{
    public static class GameLogic
    {
        public static void InitializeGrid(PlayerInfoModel model)
        {
            List<string> letters = new List<string>
            {
                "A",
                "B",
                "C",
                "D",
                "E",
                "F",
                "G",
                "H",
                "I",
                "J"
            };

            List<int> numbers = new List<int>
            {
                0,
                1,
                2,
                3,
                4,
                5,
                6,
                7,
                8,
                9
            };

            foreach(string letter in letters)
            {
                foreach(int number in numbers)
                {
                    AddGridSpot(model, letter, number);
                }
            }
        }

        private static void AddGridSpot(PlayerInfoModel model, string letter, int number)
        {
            GridSpotModel spot = new GridSpotModel
            {
                SpotLetter = letter,
                SpotNumber = number,
                Status = GridSpotStatus.Empty
            };

            model.ShotGrid.Add(spot);
        }

        public static bool PlaceShip(PlayerInfoModel model, string startCoordinate,int i, bool horizontal)
        {
            bool output = false;
            (string row, int column) = SplitShotIntoRowAndColumn(startCoordinate);

            bool isValidLocation = ValidateGridLocation(model, row, column);
            bool isSpotOpen = ValidateShipLocation(model,row,column);
            if (isValidLocation && isSpotOpen)
            {
                if (horizontal == false) //vertical
                {
                    List<string> rows = new List<string>();
                    rows.Add(row);
                    for (int j = 1; j< model.Fleet.Ships[i].Size; j++)
                    {
                        char newRow = (char)((int)char.Parse(row) + j);

                        isSpotOpen = ValidateShipLocation(model, newRow.ToString(), column);
                        if (isSpotOpen) { rows.Add(newRow.ToString()); }
                        else { return output; }
                    }
                        

                        foreach (string Row in rows)
                        {
                        model.Fleet.Ships[i].Location.Add(new GridSpotModel
                            {
                                SpotLetter = Row,
                                SpotNumber = column,
                                Status = GridSpotStatus.Ship
                            });
                        }
                        output = true;
                }
                else
                {
                    
                    List<int> columns = new List<int>();
                    columns.Add(column);
                    for( int j = 1; j< model.Fleet.Ships[i].Size;j++)
                    {
                        isSpotOpen = ValidateShipLocation(model, row, column + j);
                        if (isSpotOpen) { columns.Add(column+j); }
                        else { return output; }
                    }

                    foreach (int Column in columns)
                    {
                        model.Fleet.Ships[i].Location.Add(new GridSpotModel
                        {
                            SpotLetter = row,
                            SpotNumber = Column,
                            Status = GridSpotStatus.Ship
                        });
                    }
                    output = true;
                }
            }
            return output;

        }

        private static bool ValidateShipLocation(PlayerInfoModel model, string row, int column)
        {
            bool isValidLocation = true;
            foreach(var ship in model.Fleet.Ships)
            {
                foreach(var location in ship.Location)
                {
                    if(location.SpotLetter == row.ToUpper() && location.SpotNumber == column)
                    {
                        isValidLocation = false; 
                        break;
                    }
                }
            }
            return isValidLocation;
        }

        private static bool ValidateGridLocation(PlayerInfoModel model, string row, int column)
        {
            bool isSpotOpen = false;

            foreach(var ship in model.ShotGrid)
            {
                if(ship.SpotLetter == row.ToUpper() && ship.SpotNumber == column)
                {
                    isSpotOpen = true;
                    break;
                }
            }
            return isSpotOpen;
        }

        public static bool PlayerStillActive(PlayerInfoModel player)
        {
            bool isActive = false;
            foreach (var ship in player.Fleet.Ships)
            {
                if(ship.Status != ShipStatus.Sunk)
                {
                    isActive = true;
                }
            }
            return isActive;
        }

        public static int GetShotCount(PlayerInfoModel player)
        {
            int shotCount = 0;
            foreach(var shot in player.ShotGrid)
            {
                if(shot.Status != GridSpotStatus.Empty)
                {
                    shotCount += 1;
                }
            }
            return shotCount;
        }

        public static (string row, int column) SplitShotIntoRowAndColumn(string shot)
        {
            string row = "";
            int column = 0;
            if (shot.Length !=2)
            {
                throw new ArgumentException("This was an invalid shot type.", "shot");
            }
            char[] shotArray = shot.ToArray();
            row = shotArray[0].ToString();
            column = int.Parse(shotArray[1].ToString());

            return (row, column);

        }

        public static bool ValidateShot(PlayerInfoModel player, string row, int column)
        {
            bool isValiShot = false;

            foreach (var gridSpot in player.ShotGrid)
            {
                if (gridSpot.SpotLetter == row.ToUpper() && gridSpot.SpotNumber == column)
                {
                    if(gridSpot.Status == GridSpotStatus.Empty)
                    {
                        isValiShot = true;
                        break;
                    }
                    break;
                }
            }
            return isValiShot;
        }

        public static (bool isAhit,bool shipSunked) IdentifyShotResult(PlayerInfoModel opponent, string row, int column)
        {
            bool isAHit = false;
            bool shipSunked = false;

            foreach (var ship in opponent.Fleet.Ships)
            {
                foreach (var location in ship.Location)
                {
                    if(location.SpotLetter == row.ToUpper() && location.SpotNumber == column)
                    {
                        isAHit = true;
                        location.Status = GridSpotStatus.Hit;
                        shipSunked = IdentifyShipStatus(ship);
                        break;
                    }
                }
            }
            return (isAHit, shipSunked);
        }

        private static bool IdentifyShipStatus(ShipModel ship)
        {
            bool hasSunk = true;
            foreach(var location in ship.Location) 
            {
                if(location.Status != GridSpotStatus.Hit)
                {
                    hasSunk = false;
                    break;
                }
            }
            if (hasSunk) { ship.Status = ShipStatus.Sunk; }
            return hasSunk;
        }

        public static void MarkShotResult(PlayerInfoModel player, string row, int column, bool isAHit)
        {
            foreach (var gridSpot in player.ShotGrid)
            {
                if (gridSpot.SpotLetter == row.ToUpper() && gridSpot.SpotNumber == column)
                {
                    if(isAHit)
                    {
                        gridSpot.Status = GridSpotStatus.Hit;
                        break;
                    }
                    else
                    {
                        gridSpot.Status = GridSpotStatus.Miss;
                    }
                }
            }
        }
    }
}

