using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace ExDesign.Pages.Inputs
{
    /// <summary>
    /// MaterialsPage.xaml etkileşim mantığı
    /// </summary>
    public partial class MaterialsPage : Page
    {
        public char separator = ',';

        public MaterialsPage()
        {
            InitializeComponent();
        }
        public void Set3dView( Views.View3dPage view)
        {
            view3d_main.Content = StaticVariables.view3DPage;
        }
        private void UnitChange()
        {
            concretewall_height_unit.Content = StaticVariables.CurrentUnit.ToString().Split('_')[1];
            concretewall_thickness_unit.Content = StaticVariables.CurrentUnit.ToString().Split('_')[1];
            pilewall_height_unit.Content = StaticVariables.CurrentUnit.ToString().Split('_')[1];
            pile_diameter_unit.Content = StaticVariables.CurrentUnit.ToString().Split('_')[1];
            pile_space_unit.Content = StaticVariables.CurrentUnit.ToString().Split('_')[1];
            beam_height_unit.Content = StaticVariables.CurrentUnit.ToString().Split('_')[1];
            beam_width_unit.Content = StaticVariables.CurrentUnit.ToString().Split('_')[1];

            concretewall_height.Text = WpfUtils.GetDimension(StaticVariables.viewModel.GetWallHeight()).ToString();
            concretewall_thickness.Text = WpfUtils.GetDimension(StaticVariables.viewModel.GetWallThickness()).ToString();
            pilewall_height.Text = WpfUtils.GetDimension(StaticVariables.viewModel.GetWallHeight()).ToString();
            pile_diameter.Text = WpfUtils.GetDimension(StaticVariables.viewModel.GetWallThickness()).ToString();
            pile_space.Text = WpfUtils.GetDimension(StaticVariables.viewModel.GetPileSpace()).ToString();
            beam_height.Text = WpfUtils.GetDimension(StaticVariables.viewModel.GetCapBeamH()).ToString();
            beam_width.Text = WpfUtils.GetDimension(StaticVariables.viewModel.GetCapBeamB()).ToString();
        }
         

        private void concretewall_height_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            
            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.ChangeWallHeight(WpfUtils.GetValueDimension(result));
            }

        }
        
        private void concretewall_height_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {            
            separator = Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);           
            if (separator == ',')
            {
                Regex regex = new Regex("^[,][0-9]+$|^[0-9]*[,]{0,1}[0-9]*$");
                e.Handled = !regex.IsMatch(((TextBox)sender).Text.Insert(((TextBox)sender).SelectionStart, e.Text));
            }
            else
            {
                Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
                e.Handled = !regex.IsMatch(((TextBox)sender).Text.Insert(((TextBox)sender).SelectionStart, e.Text));
            }
            
        }

        private void concretewall_height_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Space)
            {
                e.Handled=true;
            }
            
        }

        private void concretewall_thickness_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.ChangeWallThickness(WpfUtils.GetValueDimension( result));
            }
        }

       
        private void pilewall_height_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.ChangeWallHeight(WpfUtils.GetValueDimension(result));
            }
        }

        private void pile_diameter_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.ChangeWallThickness(WpfUtils.GetValueDimension(result));
            }
        }

        private void pile_space_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.ChangePileSpace(WpfUtils.GetValueDimension(result));
            }
        }
        private void beam_height_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.ChangeCapBeamH(WpfUtils.GetValueDimension(result));
            }
        }

        private void beam_width_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.ChangeCapBeamB(WpfUtils.GetValueDimension(result));
            }
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            view3d_main.Content = StaticVariables.view3DPage;
            sideview_main.Content = StaticVariables.SideviewPage;
            switch (WpfUtils.GetWallType( StaticVariables.viewModel.WallTypeIndex))
            {
                case WallType.ConcreteRectangleWall:
                    rectanglewallgroupbox.Visibility = Visibility.Visible;
                    pilewallgroupbox.Visibility = Visibility.Hidden;
                    capbeamgroupbox.Visibility = Visibility.Hidden;
                    sheetpilewallgroupbox.Visibility = Visibility.Hidden;
                    break;
                case WallType.ConcretePileWall:
                    rectanglewallgroupbox.Visibility = Visibility.Hidden;
                    pilewallgroupbox.Visibility = Visibility.Visible;
                    capbeamgroupbox.Visibility = Visibility.Visible;
                    sheetpilewallgroupbox.Visibility = Visibility.Hidden;
                    break;
                case WallType.SteelSheetWall:
                    rectanglewallgroupbox.Visibility = Visibility.Hidden;
                    pilewallgroupbox.Visibility = Visibility.Hidden;
                    capbeamgroupbox.Visibility = Visibility.Hidden;
                    sheetpilewallgroupbox.Visibility = Visibility.Visible;
                    break;
                default:
                    rectanglewallgroupbox.Visibility = Visibility.Visible;
                    pilewallgroupbox.Visibility = Visibility.Hidden;
                    capbeamgroupbox.Visibility = Visibility.Hidden;
                    sheetpilewallgroupbox.Visibility = Visibility.Hidden;
                    break;
            }            
            Pile.GetPileDiameterDataList(pileDiameterCombobox);
            if(StaticVariables.viewModel.PileIndex>pileDiameterCombobox.Items.Count-1)
            {
                StaticVariables.viewModel.PileIndex = 0;
            }
            pileDiameterCombobox.SelectedIndex = StaticVariables.viewModel.PileIndex;

            Sheet.GetSheetDataList(sheetpileCombobox);
            if (StaticVariables.viewModel.SheetIndex > sheetpileCombobox.Items.Count - 1)
            {
                StaticVariables.viewModel.SheetIndex = 0;
            }
            sheetpileCombobox.SelectedIndex = StaticVariables.viewModel.SheetIndex;
            UnitChange();
            StaticEvents.UnitChangeEvent += UnitChange;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            view3d_main.Content = null;
            sideview_main.Content = null;
        }

        private void pileDiameterCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StaticVariables.viewModel.PileIndex = pileDiameterCombobox.SelectedIndex;
            PileData pileDiameterData = pileDiameterCombobox.SelectedItem as PileData;
            if (pileDiameterData != null) StaticVariables.viewModel.ChangeWallThickness(pileDiameterData.t);
            UnitChange();
        }

        private void piledesignwindow_Click(object sender, RoutedEventArgs e)
        {
            Windows.PileWindow pileWin = new Windows.PileWindow();
            pileWin.SelectPile(pileDiameterCombobox);
            pileWin.ShowDialog();
        }

        private void sheetpileCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StaticVariables.viewModel.SheetIndex = sheetpileCombobox.SelectedIndex;
            SheetData sheetData = sheetpileCombobox.SelectedItem as SheetData;
            if (sheetData != null) StaticVariables.viewModel.ChangeWallThickness(sheetData.Height);
            UnitChange();
        }

        private void sheetpiledesignwindow_Click(object sender, RoutedEventArgs e)
        {
            Windows.SheetPileWindow sheetpileWin = new Windows.SheetPileWindow();
            sheetpileWin.SelectSheet(sheetpileCombobox);
            sheetpileWin.ShowDialog();
        }
    }
   

}
