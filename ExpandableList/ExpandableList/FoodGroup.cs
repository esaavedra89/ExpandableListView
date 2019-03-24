
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ExpandableList
{
    public class FoodGroup : ObservableCollection<Food>, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Attributes
        bool _expanded; 
        #endregion

        #region Properties
        public string Title { get; set; }

        public string TitleWithItemCount
        {
            get
            {
                return string.Format("{0} ({1})", Title, FoodCount);
            }
        }

        public string ShortName { get; set; }

        public bool Expanded
        {
            get { return _expanded; }
            set
            {
                if (_expanded != value)
                {
                    _expanded = value;
                    OnPropertyChanged("Expanded");
                    OnPropertyChanged("StateIcon");
                }
            }
        }

        public string StateIcon
        {
            get { return Expanded ? "expanded_blue.png" : "collapsed_blue.png"; }
        }

        public int FoodCount { get; set; }

        public FoodGroup(string title, string shortName, bool expanded = true)
        {
            Title = title;
            ShortName = shortName;
            Expanded = expanded;
        }

        public static ObservableCollection<FoodGroup> All { get; private set; }
        #endregion

        public FoodGroup()
        {
            ObservableCollection<FoodGroup> Groups = new ObservableCollection<FoodGroup>
            {
                new FoodGroup("Carbohidratos", "C")
                {
                    new Food { Name = "Pasta", Description = "Descripcion de la pasta", Icon = "Icon"},
                    new Food { Name = "Potato", Description = "Descripcion de la Potato", Icon = "Icon"},
                    new Food { Name = "Bread", Description = "Descripcion de la Bread", Icon = "Icon"},
                    new Food { Name = "Rice", Description = "Descripcion de la Rice", Icon = "Icon"},
                },
                new FoodGroup("Fruits", "A")
                {
                    new Food { Name = "Orange", Description = "Descripcion de la Orange", Icon = "Icon"},
                    new Food { Name = "Apple", Description = "Descripcion de la Apple", Icon = "Icon"},
                    new Food { Name = "Pear", Description = "Descripcion de la Pear", Icon = "Icon"},
                    new Food { Name = "Banana", Description = "Descripcion de la Banana", Icon = "Icon"},
                },
                new FoodGroup("Vegetables", "D")
                {
                    new Food { Name = "Carrot", Description = "Descripcion de la Carrot", Icon = "Icon"},
                    new Food { Name = "Grean bean", Description = "Descripcion de la Grean bean", Icon = "Icon"},
                    new Food { Name = "Broccoli", Description = "Descripcion de la Broccoli", Icon = "Icon"},
                    new Food { Name = "Peas", Description = "Descripcion de la Peas", Icon = "Icon"},
                },
                new FoodGroup("Dairy", "X")
                {
                    new Food { Name = "Mil", Description = "Descripcion de la Mil", Icon = "Icon"},
                    new Food { Name = "Cheese", Description = "Descripcion de la Cheese", Icon = "Icon"},
                    new Food { Name = "Ice cream", Description = "Descripcion de la Ice cream", Icon = "Icon"},
                },
            };

            All = Groups;
        }
    }
}
