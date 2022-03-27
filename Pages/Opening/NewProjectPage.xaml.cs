using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ExDesign.Datas;
using ExDesign.Scripts;
using Microsoft.Win32;

namespace ExDesign.Pages.Opening
{
    /// <summary>
    /// NewProjectPage.xaml etkileşim mantığı
    /// </summary>
    public partial class NewProjectPage : Page
    {
        bool isTakeExistingProject;
        int unitIndex = 11;
        WallType wallType = WallType.ConcreteRectangleWall;
        public NewProjectPage()
        {
            InitializeComponent();
        }

        private void StartProjectButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.RestartModel();
            if (isTakeExistingProject)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Ex-Design files (*.exdb)|*.exdb";
                if (openFileDialog.ShowDialog() == true)
                {
                    ViewModel.OpenModel(openFileDialog.FileName);
                    StaticVariables.viewModel.Path = "Untitled";
                    StaticVariables.viewModel.ProjectName = "Untitled";
                    StaticVariables.viewModel.SaveDate = "0";
                    
                }
            }
            
            StaticVariables.viewModel.UnitIndex= unitIndex;
            StaticVariables.viewModel.WallTypeIndex = WpfUtils.GetWallTypeIndex(wallType);
            if (WpfUtils.GetWallType(StaticVariables.viewModel.WallTypeIndex) == WallType.SteelSheetWall)
            {
                StaticVariables.viewModel.strutDatas = new ObservableCollection<StrutData>();
                foreach (var anchor in StaticVariables.viewModel.anchorDatas)
                {
                    anchor.IsSoldierBeam = false;
                    anchor.SoldierBeamHeight = 0;
                    anchor.SoldierBeamwidth = 0;
                    anchor.IsPassiveAnchor = true;
                    anchor.PreStressForce = 0;
                }
            }
            ProgramWindow mainWindow = new ProgramWindow();            
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

        private void takeexistingProject_Checked(object sender, RoutedEventArgs e)
        {
            if(takeexistingProject.IsChecked==true)
            {
                isTakeExistingProject = true;
            }
            else
            {
                isTakeExistingProject=false;
            }
        }
    }
}
