using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ExDesign.Scripts;

namespace ExDesign.Pages.Opening
{
    /// <summary>
    /// NewProjectPage.xaml etkileşim mantığı
    /// </summary>
    public partial class NewProjectPage : Page
    {
        
        int unitIndex = 11;
        WallType wallType = WallType.ConcreteRectangleWall;
        public NewProjectPage()
        {
            InitializeComponent();
        }

        private void StartProjectButton_Click(object sender, RoutedEventArgs e)
        {
            StaticVariables.wallType = wallType;
            StaticVariables.UnitIndex = unitIndex;
            ProgramWindow mainWindow = new ProgramWindow();
            mainWindow.FreshStart();
            WpfUtils.OpenWindow(mainWindow);
            
        }

        private void concreteRectangleWall_bttn_Checked(object sender, RoutedEventArgs e)
        {
            wallType = WallType.ConcreteRectangleWall;
        }

        private void concretePileWall_bttn_Checked(object sender, RoutedEventArgs e)
        {
            wallType = WallType.ConcretePileWall;
        }

        private void steelSheetWall_bttn_Checked(object sender, RoutedEventArgs e)
        {
            wallType = WallType.SteelSheetWall;
        }

        private void UnitCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            unitIndex = UnitCombobox.SelectedIndex;
            
        }
    }
}
