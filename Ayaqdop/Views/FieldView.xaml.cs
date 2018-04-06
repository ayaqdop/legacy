using Ayaqdop.Auxiliary;
using Ayaqdop.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Ayaqdop.Views
{
    /// <summary>
    /// Interaktionslogik für FieldView.xaml
    /// </summary>
    public partial class FieldView : UserControl
    {
        PlayerView selectedPlayer;
        BallView ball;
        TeamModel firstTeam, secondTeam;
        TextBlock movesLeft, movesRight, scoreLeft, scoreRight;
        bool movePlayer, moveBall, turnFirstTeam;
        List<Ellipse> possibilityPointers = new List<Ellipse>();
        List<Position> playersNearTheBall = new List<Position>();
        public FieldView()
        {
            InitializeComponent();
            DrawField();

            CreateBall();  
            CreateTeams();            
        }
       

        #region Draw Board
        private void DrawField()
        {
            string alpha = " ABCDEFGHIJKLMNOP ";

            for (int column = 0; column < 8; column++)
            {
                thePitch.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int row = 0; row < 18; row++)
            {
                thePitch.RowDefinitions.Add(new RowDefinition());
                thePitch.ColumnDefinitions.Add(new ColumnDefinition());

                DrawRectangle(row, 0, Brushes.DarkGreen);
                DrawRectangle(row, 25, Brushes.DarkGreen);

                DrawCoordinate(row, 0, alpha[row].ToString());
                DrawCoordinate(row, 25, alpha[row].ToString());                
            }

            for (int column = 1; column < 25; column++)
            {
                DrawRectangle(0, column, Brushes.DarkGreen);
                DrawRectangle(17, column, Brushes.DarkGreen);

                DrawCoordinate(0, column, column.ToString());
                DrawCoordinate(17, column, column.ToString());
            }

            for (int row = 1; row < 17; row++)
            {
                for (int column = 1; column < 25; column++)
                {
                    if ((row + column) % 2 == 0)
                        DrawRectangle(row, column, Brushes.Green);
                    else
                        DrawRectangle(row, column, Brushes.LightGreen);
                }
            }
            DrawLeftMoves();
            DrawScores();
            DrawBorders();
            DrawGoals();
        }
        private void DrawScores()
        {
            scoreLeft = new TextBlock();
            scoreLeft.HorizontalAlignment = HorizontalAlignment.Center;
            scoreLeft.VerticalAlignment = VerticalAlignment.Center;
            scoreLeft.FontSize = 20;
            scoreLeft.Foreground = Brushes.Red;
            scoreLeft.FontWeight = FontWeights.Bold;
            scoreLeft.SetValue(Grid.RowProperty, 0);
            scoreLeft.SetValue(Grid.ColumnProperty, 0);
            thePitch.Children.Add(scoreLeft);

            scoreRight = new TextBlock();
            scoreRight.HorizontalAlignment = HorizontalAlignment.Center;
            scoreRight.VerticalAlignment = VerticalAlignment.Center;
            scoreRight.FontSize = 20;
            scoreRight.Foreground = Brushes.Red;
            scoreRight.FontWeight = FontWeights.Bold;
            scoreRight.SetValue(Grid.RowProperty, 0);
            scoreRight.SetValue(Grid.ColumnProperty, GlobalSettings.TOTAL_NUMBER_OF_COLUMNS);
            thePitch.Children.Add(scoreRight);
        }
        private void DrawLeftMoves()
        {
            TextBlock movesTextLeft = new TextBlock();
            movesTextLeft.HorizontalAlignment = HorizontalAlignment.Center;
            movesTextLeft.VerticalAlignment = VerticalAlignment.Bottom;
            movesTextLeft.Foreground = Brushes.Yellow;
            movesTextLeft.Text = "moves";
            movesTextLeft.SetValue(Grid.RowProperty, GlobalSettings.TOTAL_NUMBER_OF_ROWS);
            movesTextLeft.SetValue(Grid.ColumnProperty, 0);
            thePitch.Children.Add(movesTextLeft);

            TextBlock movesTextRight = new TextBlock();
            movesTextRight.HorizontalAlignment = HorizontalAlignment.Center;
            movesTextRight.VerticalAlignment = VerticalAlignment.Bottom;
            movesTextRight.Foreground = Brushes.Yellow;
            movesTextRight.Text = "moves";
            movesTextRight.SetValue(Grid.RowProperty, GlobalSettings.TOTAL_NUMBER_OF_ROWS);
            movesTextRight.SetValue(Grid.ColumnProperty, GlobalSettings.TOTAL_NUMBER_OF_COLUMNS);
            thePitch.Children.Add(movesTextRight);

            movesLeft = new TextBlock();
            movesLeft.HorizontalAlignment = HorizontalAlignment.Center;
            movesLeft.VerticalAlignment = VerticalAlignment.Center;
            movesLeft.Foreground = Brushes.Yellow;
            movesLeft.FontWeight = FontWeights.Bold;
            movesLeft.SetValue(Grid.RowProperty, GlobalSettings.TOTAL_NUMBER_OF_ROWS);
            movesLeft.SetValue(Grid.ColumnProperty, 0);            
            thePitch.Children.Add(movesLeft);

            movesRight = new TextBlock();
            movesRight.HorizontalAlignment = HorizontalAlignment.Center;
            movesRight.VerticalAlignment = VerticalAlignment.Center;
            movesRight.Foreground = Brushes.Yellow;
            movesRight.SetValue(Grid.RowProperty, GlobalSettings.TOTAL_NUMBER_OF_ROWS);
            movesRight.SetValue(Grid.ColumnProperty, GlobalSettings.TOTAL_NUMBER_OF_COLUMNS);            
            thePitch.Children.Add(movesRight);
        }
        private void DrawCoordinate(int row, int column, string text)
        {
            TextBlock txt = new TextBlock();
            txt.Text = text;
            txt.HorizontalAlignment = HorizontalAlignment.Center;
            txt.VerticalAlignment = VerticalAlignment.Center;
            txt.Foreground = Brushes.White;
            txt.SetValue(Grid.RowProperty, row);
            txt.SetValue(Grid.ColumnProperty, column);
            thePitch.Children.Add(txt);
        }
        private void DrawRectangle(int row, int column, Brush color)
        {
            Rectangle rect = new Rectangle();
            rect.Fill = color;
            rect.PreviewMouseLeftButtonDown += rect_PreviewMouseLeftButtonDown;
            rect.SetValue(Grid.RowProperty, row);
            rect.SetValue(Grid.ColumnProperty, column);
            thePitch.Children.Add(rect);
        }
        private void DrawBorders()
        {
            int brushThickness = 4;

            Border border = new Border();
            border.SetValue(Border.BorderBrushProperty, Brushes.White);
            border.SetValue(Grid.RowProperty, 1);
            border.SetValue(Grid.ColumnProperty, 1);
            border.SetValue(Grid.RowSpanProperty, 16);
            border.SetValue(Grid.ColumnSpanProperty, 24);
            border.SetValue(BorderThicknessProperty, new Thickness(brushThickness));
            thePitch.Children.Add(border);

            Border middleLine = new Border();
            middleLine.SetValue(Border.BorderBrushProperty, Brushes.White);
            middleLine.SetValue(Grid.RowProperty, 1);
            middleLine.SetValue(Grid.ColumnProperty, 13);
            middleLine.SetValue(Grid.RowSpanProperty, 16);
            middleLine.SetValue(Grid.ColumnSpanProperty, 12);
            middleLine.SetValue(BorderThicknessProperty, new Thickness(brushThickness, 0, 0, 0));
            thePitch.Children.Add(middleLine);

            Border penaltyLeft = new Border();
            penaltyLeft.SetValue(Border.BorderBrushProperty, Brushes.White);
            penaltyLeft.SetValue(Grid.RowProperty, 3);
            penaltyLeft.SetValue(Grid.ColumnProperty, 1);
            penaltyLeft.SetValue(Grid.RowSpanProperty, 12);
            penaltyLeft.SetValue(Grid.ColumnSpanProperty, 6);
            penaltyLeft.SetValue(BorderThicknessProperty, new Thickness(0, brushThickness, brushThickness, brushThickness));
            thePitch.Children.Add(penaltyLeft);

            Border penaltyRight = new Border();
            penaltyRight.SetValue(Border.BorderBrushProperty, Brushes.White);
            penaltyRight.SetValue(Grid.RowProperty, 3);
            penaltyRight.SetValue(Grid.ColumnProperty, 19);
            penaltyRight.SetValue(Grid.RowSpanProperty, 12);
            penaltyRight.SetValue(Grid.ColumnSpanProperty, 6);
            penaltyRight.SetValue(BorderThicknessProperty, new Thickness(brushThickness, brushThickness, 0, brushThickness));
            thePitch.Children.Add(penaltyRight);

            Border innerLeft = new Border();
            innerLeft.SetValue(Border.BorderBrushProperty, Brushes.White);
            innerLeft.SetValue(Grid.RowProperty, 5);
            innerLeft.SetValue(Grid.ColumnProperty, 1);
            innerLeft.SetValue(Grid.RowSpanProperty, 8);
            innerLeft.SetValue(Grid.ColumnSpanProperty, 2);
            innerLeft.SetValue(BorderThicknessProperty, new Thickness(0, brushThickness, brushThickness, brushThickness));
            thePitch.Children.Add(innerLeft);

            Border innerRight = new Border();
            innerRight.SetValue(Border.BorderBrushProperty, Brushes.White);
            innerRight.SetValue(Grid.RowProperty, 5);
            innerRight.SetValue(Grid.ColumnProperty, 23);
            innerRight.SetValue(Grid.RowSpanProperty, 8);
            innerRight.SetValue(Grid.ColumnSpanProperty, 2);
            innerRight.SetValue(BorderThicknessProperty, new Thickness(brushThickness, brushThickness, 0, brushThickness));
            thePitch.Children.Add(innerRight);

            Border goalLeft = new Border();
            goalLeft.SetValue(Border.BorderBrushProperty, Brushes.White);
            goalLeft.SetValue(Grid.RowProperty, 6);
            goalLeft.SetValue(Grid.ColumnProperty, 0);
            goalLeft.SetValue(Grid.RowSpanProperty, 6);
            goalLeft.SetValue(BorderThicknessProperty, new Thickness(brushThickness, brushThickness, 0, brushThickness));
            thePitch.Children.Add(goalLeft);

            Border goalRight = new Border();
            goalRight.SetValue(Border.BorderBrushProperty, Brushes.White);
            goalRight.SetValue(Grid.RowProperty, 6);
            goalRight.SetValue(Grid.ColumnProperty, 25);
            goalRight.SetValue(Grid.RowSpanProperty, 6);
            goalRight.SetValue(BorderThicknessProperty, new Thickness(0, brushThickness, brushThickness, brushThickness));
            thePitch.Children.Add(goalRight);

            Border circle = new Border();
            circle.SetValue(Border.BorderBrushProperty, Brushes.White);
            circle.SetValue(Grid.RowProperty, 7);
            circle.SetValue(Grid.ColumnProperty, 11);
            circle.SetValue(Grid.RowSpanProperty, 4);
            circle.SetValue(Grid.ColumnSpanProperty, 4);
            circle.SetValue(Border.CornerRadiusProperty, new CornerRadius(System.Windows.SystemParameters.VirtualScreenHeight));
            circle.SetValue(BorderThicknessProperty, new Thickness(brushThickness));
            thePitch.Children.Add(circle);

            Ellipse midpoint = new Ellipse();
            midpoint.Fill = Brushes.White;
            midpoint.Width = System.Windows.SystemParameters.VirtualScreenHeight / 40;
            midpoint.Height = System.Windows.SystemParameters.VirtualScreenHeight / 40;
            midpoint.SetValue(Grid.RowProperty, 8);
            midpoint.SetValue(Grid.ColumnProperty, 12);
            midpoint.SetValue(Grid.RowSpanProperty, 2);
            midpoint.SetValue(Grid.ColumnSpanProperty, 2);
            thePitch.Children.Add(midpoint);
        }
        private void DrawGoals()
        {
            VisualBrush vb = new VisualBrush();
            vb.TileMode = TileMode.Tile;
            vb.Viewport = new Rect(0, 0, 10, 10);
            vb.ViewportUnits = BrushMappingMode.Absolute;
            vb.Viewbox = new Rect(0, 0, 12, 12);
            vb.ViewboxUnits = BrushMappingMode.Absolute;

            Path path = new Path();
            path.Stroke = Brushes.White;
            path.Fill = Brushes.Transparent;
            path.StrokeThickness = 1;
            path.HorizontalAlignment = HorizontalAlignment.Left;
            path.VerticalAlignment = VerticalAlignment.Center;

            LineGeometry lineGeometry = new LineGeometry();
            lineGeometry.StartPoint = new System.Windows.Point(0, 0);
            lineGeometry.EndPoint = new System.Windows.Point(30, 30);

            path.Data = lineGeometry;
            vb.Visual = path;

            VisualBrush vb2 = new VisualBrush();

            vb2.TileMode = TileMode.Tile;

            vb2.Viewport = new Rect(0, 0, 10, 10);
            vb2.ViewportUnits = BrushMappingMode.Absolute;

            vb2.Viewbox = new Rect(0, 0, 12, 12);
            vb2.ViewboxUnits = BrushMappingMode.Absolute;

            Path path2 = new Path();
            path2.Stroke = Brushes.White;
            path2.Fill = Brushes.Transparent;
            path2.StrokeThickness = 1;
            path2.HorizontalAlignment = HorizontalAlignment.Left;
            path2.VerticalAlignment = VerticalAlignment.Center;

            LineGeometry lineGeometry2 = new LineGeometry();
            lineGeometry2.StartPoint = new System.Windows.Point(10, 0);
            lineGeometry2.EndPoint = new System.Windows.Point(0, 10);

            path2.Data = lineGeometry2;
            vb2.Visual = path2;

            Rectangle rect1 = new Rectangle();
            rect1.Fill = vb;
            rect1.PreviewMouseLeftButtonDown += rect_PreviewMouseLeftButtonDown;
            rect1.SetValue(Grid.RowProperty, 6);
            rect1.SetValue(Grid.RowSpanProperty, 6);
            rect1.SetValue(Grid.ColumnProperty, 0);
            thePitch.Children.Add(rect1);

            Rectangle rect2 = new Rectangle();
            rect2.Fill = vb2;
            rect2.PreviewMouseLeftButtonDown += rect_PreviewMouseLeftButtonDown;
            rect2.SetValue(Grid.RowProperty, 6);
            rect2.SetValue(Grid.RowSpanProperty, 6);
            rect2.SetValue(Grid.ColumnProperty, 0);
            thePitch.Children.Add(rect2);

            Rectangle rect3 = new Rectangle();
            rect3.Fill = vb;
            rect3.PreviewMouseLeftButtonDown += rect_PreviewMouseLeftButtonDown;
            rect3.SetValue(Grid.RowProperty, 6);
            rect3.SetValue(Grid.RowSpanProperty, 6);
            rect3.SetValue(Grid.ColumnProperty, 25);
            thePitch.Children.Add(rect3);

            Rectangle rect4 = new Rectangle();
            rect4.Fill = vb2;
            rect4.PreviewMouseLeftButtonDown += rect_PreviewMouseLeftButtonDown;
            rect4.SetValue(Grid.RowProperty, 6);
            rect4.SetValue(Grid.RowSpanProperty, 6);
            rect4.SetValue(Grid.ColumnProperty, 25);
            thePitch.Children.Add(rect4);
        }
        #endregion

        #region Clicks
        void rect_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (movePlayer)
                MovePlayer();
            if (moveBall)
                MoveBall();
        }      

        private void player_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            selectedPlayer = sender as PlayerView;
            MovePlayer();
        }

        private void ball_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MoveBall();
        }
        #endregion

        #region Help Methods
        private void CreateBall()
        {
            ball = new BallView();
            ball.PreviewMouseLeftButtonDown += ball_PreviewMouseLeftButtonDown;
            ball.DataContext = BallModel.Instance;
            ball.SetBinding(Grid.RowProperty, "BallRow");
            ball.SetBinding(Grid.ColumnProperty, "BallColumn");
            ball.SetValue(Grid.ZIndexProperty, 1);
            thePitch.Children.Add(ball);
        }
        private void CreatePlayer(PlayerModel model)
        {
            PlayerView newPlayer = new PlayerView();
            newPlayer.DataContext = model;
            newPlayer.Nummer.Text = model.ShirtNumber.ToString();
            newPlayer.ShirtColor.Fill = model.ShirtColor;
            newPlayer.PreviewMouseLeftButtonDown += player_PreviewMouseLeftButtonDown;

            Binding b = new Binding("PlayerRow");
            b.Source = model;
            b.Mode = BindingMode.TwoWay;

            Binding b2 = new Binding("PlayerColumn");
            b2.Source = model;
            b2.Mode = BindingMode.TwoWay;

            newPlayer.SetBinding(Grid.RowProperty, b);
            newPlayer.SetBinding(Grid.ColumnProperty, b2);
            newPlayer.SetValue(Grid.ZIndexProperty, 1);

            GlobalSettings.AllPlayers.Add(model);
            thePitch.Children.Add(newPlayer);
        }
        private void CreateTeams()
        {
            firstTeam = new TeamModel(GlobalSettings.FOUR_TWO_FOUR, true, "#00B5CC", true);
            secondTeam = new TeamModel(GlobalSettings.FOUR_FOUR_TWO, false, "#000000", false);

            foreach (PlayerModel plm in firstTeam.TeamPlayers)
                CreatePlayer(plm);

            foreach (PlayerModel plm in secondTeam.TeamPlayers)
                CreatePlayer(plm);

            turnFirstTeam = true;
        }
        private void ChangeTurn()
        {
            if (firstTeam.MovesLeft == 0)
            {
                firstTeam.MovesLeft = 5;
                foreach (PlayerModel p in firstTeam.TeamPlayers)
                    p.MovesLeft = 3;
                turnFirstTeam = false;
            }

            if (secondTeam.MovesLeft == 0)
            {
                secondTeam.MovesLeft = 5;
                foreach (PlayerModel p in secondTeam.TeamPlayers)
                    p.MovesLeft = 3;
                turnFirstTeam = true;
            }

            if (turnFirstTeam)
            {
                movesLeft.Text = firstTeam.MovesLeft.ToString();
                movesRight.Text = "0";
            }
            else
            {
                movesLeft.Text = "0";
                movesRight.Text = secondTeam.MovesLeft.ToString();
            }

            scoreLeft.Text = firstTeam.GoalsScored.ToString();
            scoreRight.Text = secondTeam.GoalsScored.ToString();
        }
        private void AddPossibilityPointer(int row, int column)
        {
            if (row >= 0
                && row <= thePitch.RowDefinitions.Count
                && column >= 0
                && column <= thePitch.ColumnDefinitions.Count)
            {
                Ellipse possible = new Ellipse();
                possible.Width = ball.ActualWidth / 5;
                possible.Height = ball.ActualHeight / 5;
                possible.Fill = Brushes.White;
                possible.PreviewMouseLeftButtonDown += rect_PreviewMouseLeftButtonDown;

                possible.SetValue(Grid.RowProperty, row);
                possible.SetValue(Grid.ColumnProperty, column);
                thePitch.Children.Add(possible);
                possibilityPointers.Add(possible);
            }
        }
        private Position SelectedPosition()
        {
            int selectedRow = 0;
            int selectedColumn = 0;
            if (possibilityPointers != null)
                foreach (Ellipse elem in possibilityPointers)
                    thePitch.Children.Remove(elem);

            var point = Mouse.GetPosition(thePitch);
            selectedRow = 0;
            selectedColumn = 0;


            double accumulatedHeight = 0.0, accumulatedWidth = 0.0;
            foreach (var rowDefinition in thePitch.RowDefinitions)
            {
                accumulatedHeight += rowDefinition.ActualHeight;
                if (accumulatedHeight >= point.Y)
                    break;
                selectedRow++;
            }

            foreach (var columnDefinition in thePitch.ColumnDefinitions)
            {
                accumulatedWidth += columnDefinition.ActualWidth;
                if (accumulatedWidth >= point.X)
                    break;
                selectedColumn++;
            }

            return new Position(selectedRow, selectedColumn);
        }
        #endregion

        private void MovePlayer()
        {
            int selectedRow = SelectedPosition().Row;
            int selectedColumn = SelectedPosition().Column;

            int plCol = (int)selectedPlayer.GetValue(Grid.ColumnProperty);
            int plRow = (int)selectedPlayer.GetValue(Grid.RowProperty);

            PlayerModel pm = selectedPlayer.DataContext as PlayerModel;

            if (plCol == selectedColumn && selectedRow == plRow)
            {
                movePlayer = true;
                moveBall = false;
                possibilityPointers.Clear();

                if (((firstTeam.TeamPlayers.Contains(pm) && turnFirstTeam) || (secondTeam.TeamPlayers.Contains(pm) && !turnFirstTeam)) && pm.MovesLeft > 0)
                {
                    for (int row = -1; row < 2; row++)
                    {
                        for (int column = -1; column < 2; column++)
                        {
                            AddPossibilityPointer(selectedRow + row, selectedColumn + column);
                        }
                    }
                }                
            }
            else
            {
                if (movePlayer
                    && ((Math.Abs(plCol - selectedColumn) == 1) || (plCol == selectedColumn))
                    && ((Math.Abs(plRow - selectedRow) == 1) || (selectedRow == plRow)))
                {
                    if (pm.MovesLeft > 0)
                    {
                        if (firstTeam.TeamPlayers.Contains(pm) && turnFirstTeam && firstTeam.MovesLeft > 0)
                        {
                            selectedPlayer.SetValue(Grid.RowProperty, selectedRow);
                            selectedPlayer.SetValue(Grid.ColumnProperty, selectedColumn);
                            pm.MovesLeft--;
                            firstTeam.MovesLeft--;
                        }
                        else if (secondTeam.TeamPlayers.Contains(pm) && !turnFirstTeam && secondTeam.MovesLeft > 0)
                        {
                            selectedPlayer.SetValue(Grid.RowProperty, selectedRow);
                            selectedPlayer.SetValue(Grid.ColumnProperty, selectedColumn);
                            pm.MovesLeft--;
                            secondTeam.MovesLeft--;
                        }
                    }

                    ChangeTurn();                    
                    
                }
                movePlayer = false;
            }
        }
               
        private void MoveBall()
        {
            int selectedRow = SelectedPosition().Row;
            int selectedColumn = SelectedPosition().Column;

            if (BallModel.Instance.BallColumn == selectedColumn && BallModel.Instance.BallRow == selectedRow)
            {
                moveBall = true;
                movePlayer = false;
                possibilityPointers.Clear();
                BallModel.Instance.ClearPossibilitiesList();
                playersNearTheBall.Clear();

                foreach (UIElement element in thePitch.Children)
                {
                    if (element is PlayerView)
                    {
                        int row = (int)element.GetValue(Grid.RowProperty);
                        int col = (int)element.GetValue(Grid.ColumnProperty);

                        PlayerModel anotherPlayer = ((PlayerView)element).DataContext as PlayerModel;

                        if ((firstTeam.TeamPlayers.Contains(anotherPlayer) && turnFirstTeam) || (secondTeam.TeamPlayers.Contains(anotherPlayer) && !turnFirstTeam))
                        {
                            if ((Math.Abs(col - BallModel.Instance.BallColumn) <= 1)
                            && (Math.Abs(row - BallModel.Instance.BallRow) <= 1))
                                playersNearTheBall.Add(new Position(row, col));
                        }                        
                    }
                }

                foreach (Position pos in playersNearTheBall)
                    BallModel.Instance.CalculatePossibleBallMoves(pos);

                foreach (Position pos in BallModel.Instance.PossibleBallMoves)
                    AddPossibilityPointer(pos.Row, pos.Column);
            }
            else
            {
                bool letMove = false;
                foreach (Position pos in playersNearTheBall)
                {
                    if ((Math.Abs(pos.Column - BallModel.Instance.BallColumn) <= 1)
                        && (Math.Abs(pos.Row - BallModel.Instance.BallRow) <= 1))
                    {
                        letMove = true;
                        break;
                    }                        
                }
                if (moveBall && letMove
                        && BallModel.Instance.PossibleBallMoves.Contains(new Position(selectedRow, selectedColumn)))
                {
                    Position lastMove = new Position(selectedRow, selectedColumn);

                    if (turnFirstTeam)
                    {
                        BallModel.Instance.CurrentPosition = lastMove;
                        firstTeam.MovesLeft--;
                        GoalCheck(lastMove);
                    }
                    else
                    {
                        BallModel.Instance.CurrentPosition = lastMove;
                        secondTeam.MovesLeft--;
                        GoalCheck(lastMove);
                    }
                    ChangeTurn();
                                       
                }
                moveBall = false;
            }

        }

        private void GoalCheck(Position lastMove)
        {
            if (GlobalSettings.GOAL_RIGHT.Contains(lastMove))
            {
                BallModel.Instance.CurrentPosition = GlobalSettings.BALL_CENTER_RIGHT;
                firstTeam.AfterGoal(true);
                secondTeam.AfterGoal(false);
            }
            else if (GlobalSettings.GOAL_LEFT.Contains(lastMove))
            {
                BallModel.Instance.CurrentPosition = GlobalSettings.BALL_CENTER_LEFT;
                firstTeam.AfterGoal(false);
                secondTeam.AfterGoal(true);
            }
        }



    }
}
