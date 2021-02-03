using System.Windows.Controls;
using ModernWPFcore.Pages;

namespace ModernWPFcore
{
    /// <summary>
    /// Interaction logic for NavigationRootPage.xaml
    /// </summary>
    public partial class NavigationRootPage
    {
        public NavigationRootPage()
        {
            InitializeComponent();
            RootFrame?.Navigate(new InputOutputPage("Lipidyzer"));
        }

        private void MenuList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem menuSelection = (sender as ListBox).SelectedItem as ListBoxItem;
            if (menuSelection == null || menuSelection.Name == "Home" || 
                menuSelection.Name == "NormalizeToQC" || menuSelection.Name == "FillTemplate")
            {
                // These haven't been fully implemented yet
                RootFrame?.Navigate(new HomePage());
            }
            else
            { 
                RootFrame?.Navigate(new InputOutputPage(menuSelection.Name));
            }
        }
    }
}
