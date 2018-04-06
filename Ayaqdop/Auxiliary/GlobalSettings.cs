using Ayaqdop.Models;
using System.Collections.Generic;

namespace Ayaqdop.Auxiliary
{
    public static class GlobalSettings
    {
        public const int TOTAL_NUMBER_OF_ROWS = 18;
        public const int TOTAL_NUMBER_OF_COLUMNS = 26;
        public const int NUMBER_OF_PLAYERS_IN_A_TEAM = 11;
        public const int NUMBER_OF_MOVES_TEAM = 5;
        public const int NUMBER_OF_MOVES_PLAYER = 3;

        public static Position BALL_CENTER_LEFT = new Position(8, 12);
        public static Position BALL_CENTER_RIGHT = new Position(9, 13);

        private static List<Position> penaltyLeft, penaltyRight, goalLeft, goalRight, boundaries;
        public static List<Position> PENALTY_AREA_LEFT
        {
            get
            {
                if (penaltyLeft == null)
                {
                    penaltyLeft = new List<Position>();
                    for (int row = 3; row <= 14; row++)
                    {
                        for (int col = 1; col <= 6; col++)
                        {
                            penaltyLeft.Add(new Position(row, col)); 
                        }
                    }
                }
                return penaltyLeft;
            }
        }

        public static List<Position> PENALTY_AREA_RIGHT
        {
            get
            {
                if (penaltyRight == null)
                {
                    penaltyRight = new List<Position>();
                    for (int row = 3; row <= 14; row++)
                    {
                        for (int col = 19; col <= 24; col++)
                        {
                            penaltyRight.Add(new Position(row, col));
                        }
                    }
                }
                return penaltyRight;
            }
        }

        public static List<Position> GOAL_LEFT
        {
            get
            {
                if (goalLeft == null)
                {
                    goalLeft = new List<Position>();
                    for (int row = 3; row <= 14; row++)
                    {
                        goalLeft.Add(new Position(row, 0));
                    }
                }
                return goalLeft;
            }
        }

        public static List<Position> GOAL_RIGHT
        {
            get
            {
                if (goalRight == null)
                {
                    goalRight = new List<Position>();
                    for (int row = 3; row <= 14; row++)
                    {
                        goalRight.Add(new Position(row, 25));
                    }
                }
                return goalRight;
            }
        }

        public static List<Position> BOUNDARIES
        {
            get
            {
                if (boundaries == null)
                {
                    boundaries = new List<Position>();
                    for (int row = 0; row < 18; row++)
                    {
                        BOUNDARIES.Add(new Position(row, 0));
                        BOUNDARIES.Add(new Position(row, 25));
                    }

                    for (int column = 1; column < 25; column++)
                    {
                        BOUNDARIES.Add(new Position(0, column));
                        BOUNDARIES.Add(new Position(17, column));
                    }
                }
                return boundaries;
            }
        }



        public static List<PlayerModel> AllPlayers = new List<PlayerModel>(); 

        public static Position[] FOUR_TWO_FOUR = new Position[] {                           new Position(8, 1), 
                                                            new Position(4, 6), new Position(6, 4), new Position(11, 4), new Position(13, 6),
                                                                                    new Position(6, 9), new Position(11, 9),
                                                            new Position(3, 12), new Position(7, 12), new Position(10, 12), new Position(14, 12),};


        public static Position[] FOUR_FOUR_TWO = new Position[] {                         new Position(8, 1), 
                                                            new Position(4, 6), new Position(6, 5), new Position(11, 5), new Position(13, 6),
                                                            new Position(2, 9), new Position(6, 10), new Position(11, 10), new Position(15, 9),
                                                                            new Position(7, 12), new Position(10, 12), };


        

    }
}
