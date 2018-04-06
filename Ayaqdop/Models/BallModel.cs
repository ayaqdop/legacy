using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using Ayaqdop.Auxiliary;

namespace Ayaqdop.Models
{
    public sealed class BallModel : INotifyPropertyChanged
    {
        private static BallModel instance = null;
        private static readonly object padlock = new object();
        private BallModel()
        {
            PossibleBallMoves = new List<Position>();
            ballPosition = GlobalSettings.BALL_CENTER_LEFT;
        }

        public static BallModel Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new BallModel();
                    }
                    return instance;
                }
            }
        }

        Position ballPosition;
        public Position CurrentPosition
        {
            get
            {
                return ballPosition;
            }
            set
            {
                ballPosition = value;
                BallRow = ballPosition.Row;
                BallColumn = ballPosition.Column;
            }
        }

        public int BallRow
        {
            get
            {
                return ballPosition.Row;
            }
            set
            {
                ballPosition.Row = value;
                OnPropertyChanged("BallRow");
            }
        }

        public int BallColumn
        {
            get
            {
                return ballPosition.Column;
            }
            set
            {
                ballPosition.Column = value;
                OnPropertyChanged("BallColumn");
            }
        }


        public List<Position> PossibleBallMoves { get; private set; }


        public void ClearPossibilitiesList()
        {
            PossibleBallMoves.Clear();
        }

        public void CalculatePossibleBallMoves(Position player)
        {
            List<Position> playerPositions = new List<Position>();
            foreach (PlayerModel pl in GlobalSettings.AllPlayers)
                playerPositions.Add(pl.CurrentPosition);

            int plCol = player.Column;
            int plRow = player.Row;

            List<Position> moveLimits = GlobalSettings.BOUNDARIES;

            if (GlobalSettings.PENALTY_AREA_LEFT.Contains(CurrentPosition))
            {
                foreach (Position pos in GlobalSettings.GOAL_LEFT)
                    moveLimits.Remove(pos);
            }
            else if (GlobalSettings.PENALTY_AREA_RIGHT.Contains(CurrentPosition))
            {
                foreach (Position pos in GlobalSettings.GOAL_RIGHT)
                    moveLimits.Remove(pos);
            }

            if ((Math.Abs(plCol - BallColumn) <= 1) && (Math.Abs(plRow - BallRow) <= 1))
            {
                if (BallColumn == plCol)
                {
                    if (BallRow > plRow)
                    {
                        for (int row = BallRow + 1; row < GlobalSettings.TOTAL_NUMBER_OF_ROWS; row++)
                        {
                            Position pos = new Position(row, BallColumn);
                            if (playerPositions.Contains(pos) || moveLimits.Contains(pos))
                                break;
                            PossibleBallMoves.Add(pos);
                        }                            
                    }
                    else
                    {
                        for (int row = BallRow - 1; row >= 0; row--)
                        {
                            Position pos = new Position(row, BallColumn);
                            if (playerPositions.Contains(pos) || moveLimits.Contains(pos))
                                break;
                            PossibleBallMoves.Add(pos);
                        }
                    }
                }
                else if (BallRow == plRow)
                {
                    if (BallColumn > plCol)
                    {
                        for (int column = BallColumn + 1; column < GlobalSettings.TOTAL_NUMBER_OF_COLUMNS; column++)
                        {
                            Position pos = new Position(BallRow, column);
                            if (playerPositions.Contains(pos) || moveLimits.Contains(pos))
                                break;
                            PossibleBallMoves.Add(pos);
                        }
                    }
                    else
                    {
                        for (int column = BallColumn - 1; column >= 0; column--)
                        {
                            Position pos = new Position(BallRow, column);
                            if (playerPositions.Contains(pos) || moveLimits.Contains(pos))
                                break;
                            PossibleBallMoves.Add(pos);
                        }
                    }
                }
                else
                {
                    int tCol = BallColumn;
                    int tRow = BallRow;
                    if (BallColumn > plCol && BallRow > plRow)
                    {
                        for (int i = (BallRow > BallColumn) ? BallRow : BallColumn; i < GlobalSettings.TOTAL_NUMBER_OF_ROWS || i < GlobalSettings.TOTAL_NUMBER_OF_COLUMNS; i++)
                        {
                            Position pos = new Position(tRow++, tCol++);
                            if (playerPositions.Contains(pos) || moveLimits.Contains(pos))
                                break;
                            PossibleBallMoves.Add(pos);
                        }
                    }
                    else if (BallColumn < plCol && BallRow < plRow)
                    {
                        for (int i = (BallRow > BallColumn) ? BallRow : BallColumn; i >= 0; i--)
                        {
                            Position pos = new Position(tRow--, tCol--);
                            if (playerPositions.Contains(pos) || moveLimits.Contains(pos))
                                break;
                            PossibleBallMoves.Add(pos);
                        } 
                    }
                    else if (BallColumn < plCol && BallRow > plRow)
                    {
                        for (int i = ((GlobalSettings.TOTAL_NUMBER_OF_ROWS - BallRow) < (GlobalSettings.TOTAL_NUMBER_OF_COLUMNS - BallColumn)) ?
                            (GlobalSettings.TOTAL_NUMBER_OF_ROWS - BallRow) : (GlobalSettings.TOTAL_NUMBER_OF_COLUMNS - BallColumn); i > 0; i--)
                        {
                            Position pos = new Position(tRow++, tCol--);
                            if (playerPositions.Contains(pos) || moveLimits.Contains(pos))
                                break;
                            PossibleBallMoves.Add(pos);
                        }
                    }
                    else
                    {
                        for (int i = (BallRow > (GlobalSettings.TOTAL_NUMBER_OF_ROWS - BallColumn)) ?
                            BallRow : (GlobalSettings.TOTAL_NUMBER_OF_COLUMNS - BallColumn); i > 0; i--)
                        {
                            Position pos = new Position(tRow--, tCol++);
                            if (playerPositions.Contains(pos) || moveLimits.Contains(pos))
                                break;
                            PossibleBallMoves.Add(pos);
                        } 
                    }
                }
            }
        }

        

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }        
    }
}
