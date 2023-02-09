using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MarsRover.Models
{
    public class GameData : INotifyPropertyChanged
    {
        private string token;
        private string name;
        private Orientation orientation;
        private Coordinate target;
        private Coordinate perseverancePosition;
        private Coordinate ingenuityPosition;
        private int perseveranceBattery;
        private int ingenuityBattery;
        private LowResolutionMap[] lowResolutionMap;
        private Dictionary<long, Cell> highResolutionMap;

        public string Token 
        { 
            get => token; 
            set
            {
                token = value;
                OnPropertyChanged(nameof(Token));
            } 
        }

        public string Name 
        { 
            get => name;
            set
            {
                name = value;
                OnPropertyChanged(nameof(Token));
            }
        }


        public Orientation Orientation 
        { 
            get => orientation;
            set 
            {
                orientation = value;
                OnPropertyChanged(nameof(Orientation)); 
            }
        }

        public Coordinate Target 
        { 
            get => target;
            set 
            {
                target = value;
                OnPropertyChanged(nameof(Target));  
            } 
        }

        public Coordinate PerseverancePosition 
        { 
            get => perseverancePosition;
            set
            {
                perseverancePosition = value;
                OnPropertyChanged(nameof(PerseverancePosition));
            }
        }

        public Coordinate IngenuityPosition
        {
            get => ingenuityPosition;
            set
            {
                ingenuityPosition = value;
                OnPropertyChanged(nameof(IngenuityPosition));
            }
        }

        public int PerseveranceBattery 
        { 
            get => perseveranceBattery;
            set 
            { 
                perseveranceBattery = value; 
                OnPropertyChanged(nameof(PerseveranceBattery)); 
            } 
        }

        public int IngenuityBattery
        {
            get => ingenuityBattery;    
            set
            {
                ingenuityBattery = value;
                OnPropertyChanged(nameof(IngenuityBattery));
            }
        }

        public LowResolutionMap[] LowResolutionMap 
        { 
            get => lowResolutionMap; 
            set
            {
                lowResolutionMap = value;
                OnPropertyChanged(nameof(LowResolutionMap));
            }
        }

        public Dictionary<long, Cell> HighResolutionMap
        {
            get => highResolutionMap;
            set
            {
                highResolutionMap = value;
                OnPropertyChanged(nameof(HighResolutionMap));   
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, e);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
    }
}
