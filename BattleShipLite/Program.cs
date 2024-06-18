using BattleShipLiteLibrary;
using BattleShipLiteLibrary.Models;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipLite
{
    internal class Program
    {
        static void Main(string[] args)
        {
            WelcomeMessage();

            PlayerInfoModel activePlayer = CreatePlayer("Player 1");
            PlayerInfoModel opponent = CreatePlayer("Player 2");
            PlayerInfoModel winner = null;

            do
            {
                //Display grid from activePlayer on where they fired
                DisplayShotGrid(activePlayer);

                //Ask activePlayer  for a shot
                //Determine if valid shot
                //Determine shot results
                RecordPlayerShot(activePlayer, opponent);

                //Determine if game should continue 
                bool doesGameContinue = GameLogic.PlayerStillActive(opponent);

                // if over set activePlayer as winner 
                //else swap position (activePlayer to opponent )
                if(doesGameContinue == true)
                {
                    //Use tuple to swap players position
                    (activePlayer, opponent) = (opponent, activePlayer);
                }
                else
                {
                    winner = activePlayer;
                }

            } while (winner == null);

            IdentifyWinner(winner);
            Console.ReadLine();
        }

        private static void IdentifyWinner(PlayerInfoModel winner)
        {
            Console.WriteLine($"congratulations to {winner.UsersName} for winning!");
            Console.WriteLine($"{winner.UsersName} took {GameLogic.GetShotCount(winner) } shots");
        }

        private static void RecordPlayerShot(PlayerInfoModel activePlayer, PlayerInfoModel opponent)
        {
            //Ask for a shot (we ask for B2)
            //Determine row and column - split it appart
            //Determine if taht is a valid shot 
            //Go back to begining if not valid shot
            bool isValidShot = false;
            string row = "";
            int column = 0;
            do
            {
                string shot = AskForShot(activePlayer);
                try
                {
                    (row, column) = GameLogic.SplitShotIntoRowAndColumn(shot);
                    isValidShot = GameLogic.ValidateShot(activePlayer, row, column);
                }
                catch (Exception ex)
                {
                    isValidShot=false;
                }

                if(isValidShot == false)
                {
                    Console.WriteLine("Invaid shot location. Please try again");
                }
            } while (isValidShot == false);

            //determine results
            (bool isAHit, bool shipSunk) = GameLogic.IdentifyShotResult(opponent, row, column);
            
            //record results
            GameLogic.MarkShotResult(activePlayer, row, column, isAHit );

            DisplayShotResults(row, column, isAHit, shipSunk);
        }

        private static void DisplayShotResults(string row, int column, bool isAHit, bool shipSunk)
        {
            if (isAHit)
            {
                Console.WriteLine($"{row}{column} is a Hit!"); 
                if( shipSunk ) Console.WriteLine($"A ship has sunk !");
            }
            else
            {
                Console.WriteLine($"{row}{column} is a miss.");
            }
            Console.WriteLine();
        }

        private static string AskForShot(PlayerInfoModel player)
        {
            Console.Write($"{player.UsersName }, please enter your shot selection: ");
            string output = Console.ReadLine();
            
            return output;
        }

        private static void DisplayShotGrid(PlayerInfoModel activePlayer)
        {
            string currentRow = activePlayer.ShotGrid[0].SpotLetter;

            foreach (var gridSpot in activePlayer.ShotGrid)
            {
                if(gridSpot.SpotLetter != currentRow)
                {
                    Console.WriteLine();
                    currentRow = gridSpot.SpotLetter;
                }


                if(gridSpot.Status == GridSpotStatus.Empty)
                {
                    Console.Write($" {gridSpot.SpotLetter}{gridSpot.SpotNumber} ");
                }
                else if (gridSpot.Status == GridSpotStatus.Hit)
                {
                    Console.Write(" X  ");
                }
                else if (gridSpot.Status == GridSpotStatus.Miss)
                {
                    Console.Write(" O  ");
                }
                else
                {
                    Console.Write(" ?  ");
                }               
            }
            Console.WriteLine();
            Console.WriteLine();
        }

        private static void WelcomeMessage()
        {
            Console.WriteLine("Welcome to Battleship Lite");
            Console.WriteLine("created by Anne-Claire OUBLIE");
            Console.WriteLine();
        }

        private static PlayerInfoModel CreatePlayer(string playerTitle)
        {
            
            PlayerInfoModel output = new PlayerInfoModel();
            Console.WriteLine($"Player information for {playerTitle}");
            //Ask user for name
            output.UsersName = AskForUsersName();

            //Load up the shot grid
            GameLogic.InitializeGrid(output);

            //Ask the user for their 5 ship placements
            List<ShipModel> Fleet = new List<ShipModel>();
            Fleet.Add(new ShipModel
            {
                ShipName = "Destroyer",
                Size = 2
            });
            Fleet.Add(new ShipModel
            {
                ShipName = "Submarine",
                Size = 3
            });
            Fleet.Add(new ShipModel
            {
                ShipName = "Cruiser",
                Size = 3
            });
            Fleet.Add(new ShipModel
            {
                ShipName = "BattleShip",
                Size = 4
            });
            Fleet.Add(new ShipModel
            {
                ShipName = "Aircraft",
                Size = 5
            });
            output.Fleet = Fleet;
            PlaceShips(output);

            //Clear 
            Console.Clear();

            return output;
        }

        private static string AskForUsersName()
        {
            Console.Write("What is your name: ");
            string output = Console.ReadLine();

            return output;
        }

        private static void PlaceShips(PlayerInfoModel model)
        {
            int i = 0;
            do
            {
                bool orientation = false;
                Console.Write($"Where do you want to place the ship {model.Fleet[i].ShipName} of size {model.Fleet[i].Size}: ");
                string location = Console.ReadLine();
                Console.Write("Want to place is horizontaly ? (Y/N)");
                string answer = Console.ReadLine();
                if(answer == "Y")
                {
                    orientation = true;
                }


                bool isValidLocation = false;
                try
                {
                    isValidLocation = GameLogic.PlaceShip(model, location, i, orientation);
                    i++;
                }
                catch (Exception ex)
                {

                    Console.WriteLine("Error: " + ex.Message);
                }

                if (isValidLocation == false)
                {
                    Console.WriteLine("That was not a valid location. Please try again");
                    i--;
                }

            } while (i < 5);
        }
    }
}
