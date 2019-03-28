using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace ExpandableList
{
    public partial class MainPage : ContentPage
    {
        private ObservableCollection<FoodGroup> _allGroups;
        private ObservableCollection<FoodGroup> _expandedGroups;

        public MainPage()
        {
            InitializeComponent();
            _allGroups = FoodGroup.All;
            UpdateListContent();
        }

        private void UpdateListContent()
        {
            _expandedGroups = new ObservableCollection<FoodGroup>();
            foreach (FoodGroup group in _allGroups)
            {
                FoodGroup newGroup = new FoodGroup(group.Title, group.ShortName, group.Expanded);

                newGroup.FoodCount = group.Count;
                if (group.Expanded)
                {
                    foreach (Food food in group)
                    {
                        newGroup.Add(food);
                    }
                }
                _expandedGroups.Add(newGroup);
            }
            GroupedView.ItemsSource = _expandedGroups;
        }

        private void HeaderTapped(object sender, EventArgs args)
        {
            int selectedIndex = _expandedGroups.IndexOf(
                ((FoodGroup)((Button)sender).CommandParameter));
            _allGroups[selectedIndex].Expanded = !_allGroups[selectedIndex].Expanded;
            UpdateListContent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ListViewGroupingView());
        }
    }
}
