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
        }

        private void MenuList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem menuSelection = (sender as ListBox).SelectedItem as ListBoxItem;
            RootFrame?.Navigate(new InputOutputPage(menuSelection.Name));
        }
    }
}
