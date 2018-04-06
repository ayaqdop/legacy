using Ayaqdop.Auxiliary;
using Ayaqdop.Views;
using System.ComponentModel;
using System.Windows.Media;

namespace Ayaqdop.Models
{
    public class PlayerModel : INotifyPropertyChanged
    {
        Position playerPosition = new Position(6, 12);
        public Position CurrentPosition
        {
            get
            {
                return playerPosition;
            }
            set
            {
                playerPosition = value;
                PlayerRow = playerPosition.Row;
                PlayerColumn = playerPosition.Column;
            }
        }

        public int PlayerRow
        {
            get
            {
                return playerPosition.Row;
            }
            set
            {
                playerPosition.Row = value;
                OnPropertyChanged("PlayerRow");
            }
        }

        public int PlayerColumn
        {
            get
            {
                return playerPosition.Column;
            }
            set
            {
                playerPosition.Column = value;
                OnPropertyChanged("PlayerColumn");
            }
        }

        
        public int ShirtNumber { get; set; }

        public int MovesLeft { get; set; }

        public Brush ShirtColor { get; set; }

        public PlayerModel (int shirtNumber, Brush color)
        {
            ShirtNumber = shirtNumber;
            MovesLeft = GlobalSettings.NUMBER_OF_MOVES_PLAYER;
            ShirtColor = color;
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
