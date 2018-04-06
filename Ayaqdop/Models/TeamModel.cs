using Ayaqdop.Auxiliary;
using System.Collections.Generic;
using System.Windows.Media;

namespace Ayaqdop.Models
{
    public class TeamModel
    {
        public List<PlayerModel> TeamPlayers
        {
            get;
            private set;
        }

        public int MovesLeft { get; set; }
        public int GoalsScored { get; set; }

        private Position[] formationStarting, formationWaiting;
        private bool onLeftSide;
        

        public TeamModel(Position[] formation, bool leftSide, string color, bool starting)
        {
            onLeftSide = leftSide;
            formationStarting = new Position[GlobalSettings.NUMBER_OF_PLAYERS_IN_A_TEAM];
            formationWaiting = new Position[GlobalSettings.NUMBER_OF_PLAYERS_IN_A_TEAM];

            if (onLeftSide)
            {
                formationStarting = formation;    
                for (int i = 0; i < GlobalSettings.NUMBER_OF_PLAYERS_IN_A_TEAM; i++)
                {
                    if (formation[i].Column == 12 || formation[i].Column == 13)
                    {
                        formationWaiting[i].Column = formation[i].Column;
                        if (formation[i].Row < 8)
                            formationWaiting[i].Row = formation[i].Row - 1;
                        else
                            formationWaiting[i].Row = formation[i].Row + 1;
                    }
                    else
                        formationWaiting[i] = formation[i];
                }
            }
            else
            {
                for (int i = 0; i < GlobalSettings.NUMBER_OF_PLAYERS_IN_A_TEAM; i++)
                {
                    formationStarting[i].Column = 25 - formation[i].Column;
                    formationStarting[i].Row = formation[i].Row;

                    formationWaiting[i].Column = 25 - formation[i].Column;

                    if (formation[i].Column == 12 || formation[i].Column == 13)
                    {                        
                        if (formation[i].Row < 8)
                            formationWaiting[i].Row = formation[i].Row - 1;
                        else
                            formationWaiting[i].Row = formation[i].Row + 1;
                    }
                    else
                    {
                        formationWaiting[i].Row = formation[i].Row;
                    }
                    


                }
            }

            GoalsScored = 0;
            MovesLeft = GlobalSettings.NUMBER_OF_MOVES_TEAM;
            TeamPlayers = new List<PlayerModel>();
            var converter = new BrushConverter();

            if (starting)
            {
                for (int i = 0; i < GlobalSettings.NUMBER_OF_PLAYERS_IN_A_TEAM; i++)
                {
                    Brush brush = converter.ConvertFromString(color) as Brush;
                    PlayerModel model = new PlayerModel(i + 1, brush);
                    model.CurrentPosition = formationStarting[i];
                    TeamPlayers.Add(model);
                }
            }
            else
            {
                for (int i = 0; i < GlobalSettings.NUMBER_OF_PLAYERS_IN_A_TEAM; i++)
                {
                    Brush brush = converter.ConvertFromString(color) as Brush;
                    PlayerModel model = new PlayerModel(i + 1, brush);
                    model.CurrentPosition = formationWaiting[i];                   
                    TeamPlayers.Add(model);
                }
            }
            
        }

        public void AfterGoal(bool scored)
        {            
            if (scored)
            {
                GoalsScored++;
                MovesLeft = 0;
                for (int i = 0; i < GlobalSettings.NUMBER_OF_PLAYERS_IN_A_TEAM; i++)
                {
                    TeamPlayers[i].CurrentPosition = formationWaiting[i];
                    TeamPlayers[i].MovesLeft = 3;
                }
            }
            else
            {
                MovesLeft = GlobalSettings.NUMBER_OF_MOVES_TEAM;
                for (int i = 0; i < GlobalSettings.NUMBER_OF_PLAYERS_IN_A_TEAM; i++)
                {
                    TeamPlayers[i].CurrentPosition = formationStarting[i];
                    TeamPlayers[i].MovesLeft = 3;
                }
            }
        }
        

        

    }
}
