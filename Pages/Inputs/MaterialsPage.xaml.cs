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
        
        private void UnitChange()
        {
            concretewall_height_unit.Content = StaticVariables.CurrentUnit.ToString().Split('_')[1];
            concretewall_thickness_unit.Content = StaticVariables.CurrentUnit.ToString().Split('_')[1];
            pilewall_height_unit.Content = StaticVariables.CurrentUnit.ToString().Split('_')[1];
            pile_diameter_unit.Content = StaticVariables.CurrentUnit.ToString().Split('_')[1];
            pile_space_unit.Content = StaticVariables.CurrentUnit.ToString().Split('_')[1];
            beam_height_unit.Content = StaticVariables.CurrentUnit.ToString().Split('_')[1];
            beam_width_unit.Content = StaticVariables.CurrentUnit.ToString().Split('_')[1];

            concretewall_height.Text = WpfUtils.GetDimension(StaticVariables.view3DPage.GetWallHeight()).ToString();
            concretewall_thickness.Text = WpfUtils.GetDimension(StaticVariables.view3DPage.GetWallThickness()).ToString();
            pilewall_height.Text = WpfUtils.GetDimension(StaticVariables.view3DPage.GetWallHeight()).ToString();
            pile_diameter.Text = WpfUtils.GetDimension(StaticVariables.view3DPage.GetWallThickness()).ToString();
            pile_space.Text = WpfUtils.GetDimension(StaticVariables.view3DPage.GetPileSpace()).ToString();
            beam_height.Text = WpfUtils.GetDimension(StaticVariables.view3DPage.GetCapBeamH()).ToString();
            beam_width.Text = WpfUtils.GetDimension(StaticVariables.view3DPage.GetCapBeamB()).ToString();
        }
         

        private void concretewall_height_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            
            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.view3DPage.ChangeWallHeight(WpfUtils.GetValue(result));
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
                StaticVariables.view3DPage.ChangeWallThickness(WpfUtils.GetValue( result));
            }
        }

       
        private void pilewall_height_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.view3DPage.ChangeWallHeight(WpfUtils.GetValue(result));
            }
        }

        private void pile_diameter_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.view3DPage.ChangeWallThickness(WpfUtils.GetValue(result));
            }
        }

        private void pile_space_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.view3DPage.ChangePileSpace(WpfUtils.GetValue(result));
            }
        }
        private void beam_height_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.view3DPage.ChangeCapBeamH(WpfUtils.GetValue(result));
            }
        }

        private void beam_width_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.view3DPage.ChangeCapBeamB(WpfUtils.GetValue(result));
            }
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            view3d_main.Content = StaticVariables.view3DPage;
            sideview_main.Content = StaticVariables.SideviewPage;
            switch (StaticVariables.wallType)
            {
                case WallType.ConcreteRectangleWall:
                    rectanglewallgroupbox.Visibility = Visibility.Visible;
                    pilewallgroupbox.Visibility = Visibility.Hidden;
                    capbeamgroupbox.Visibility = Visibility.Hidden;
                    break;
                case WallType.ConcretePileWall:
                    rectanglewallgroupbox.Visibility = Visibility.Hidden;
                    pilewallgroupbox.Visibility = Visibility.Visible;
                    capbeamgroupbox.Visibility = Visibility.Visible;

                    break;
                case WallType.SteelSheetWall:
                    break;
                default:
                    rectanglewallgroupbox.Visibility = Visibility.Visible;
                    pilewallgroupbox.Visibility = Visibility.Hidden;
                    capbeamgroupbox.Visibility = Visibility.Hidden;

                    break;
            }            
            Pile.GetPileDiameterDataList(pileDiameterCombobox);
            pileDiameterCombobox.SelectedIndex = 1;            
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
            PileData pileDiameterData = pileDiameterCombobox.SelectedItem as PileData;
            if (pileDiameterData != null) StaticVariables.view3DPage.ChangeWallThickness(pileDiameterData.t);
            UnitChange();
        }

        private void piledesignwindow_Click(object sender, RoutedEventArgs e)
        {
            Windows.PileWindow pileWin = new Windows.PileWindow();
            pileWin.SelectPile(pileDiameterCombobox);
            pileWin.ShowDialog();
        }

       
    }
   

}
