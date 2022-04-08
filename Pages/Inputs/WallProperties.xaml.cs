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
using ExDesign.Datas;
using ExDesign.Scripts;

namespace ExDesign.Pages.Inputs
{
    /// <summary>
    /// MaterialsPage.xaml etkileşim mantığı
    /// </summary>
    public partial class WallProperties : Page
    {
        public char separator = ',';

        public WallProperties()
        {
            InitializeComponent();
        }

        private void UnitChange()
        {
            concretewall_height_unit.Content = StaticVariables.dimensionUnit;
            concretewall_thickness_unit.Content = StaticVariables.dimensionUnit;
            pilewall_height_unit.Content = StaticVariables.dimensionUnit;
            pile_diameter_unit.Content = StaticVariables.dimensionUnit;
            pile_space_unit.Content = StaticVariables.dimensionUnit;
            beam_height_unit.Content = StaticVariables.dimensionUnit;
            beam_width_unit.Content = StaticVariables.dimensionUnit;
            sheetpilewall_height_unit.Content = StaticVariables.dimensionUnit;
            areaText_unit.Content = StaticVariables.areaUnit+"/m";
            inertiaText_unit.Content = StaticVariables.inertiaUnit+"/m";
            EIText_unit.Content = StaticVariables.EIUnit+"/m";
            EAText_unit.Content = StaticVariables.forceUnit+"/m";

            concretewall_height.Text=WpfUtils.ChangeDecimalOptions( WpfUtils.GetDimension(StaticVariables.viewModel.GetWallHeight()));
            concretewall_thickness.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetDimension(StaticVariables.viewModel.GetWallThickness()));
            pilewall_height.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetDimension(StaticVariables.viewModel.GetWallHeight()));
            pile_diameter.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetDimension(StaticVariables.viewModel.GetWallThickness()));
            pile_space.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetDimension(StaticVariables.viewModel.GetPileSpace()));
            beam_height.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetDimension(StaticVariables.viewModel.GetCapBeamH()));
            beam_width.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetDimension(StaticVariables.viewModel.GetCapBeamB()));
            sheetpilewall_height.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetDimension(StaticVariables.viewModel.GetWallHeight()));
            areaText.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetArea(StaticVariables.viewModel.GetWallArea()));
            inertiaText.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetInertia(StaticVariables.viewModel.GetWallInertia()));
            EIText.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetMomentScaleDimension(StaticVariables.viewModel.GetWallEI()));            
            EAText.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetForce(StaticVariables.viewModel.GetWallEA()));

            topOfWallLevel.Text = StaticVariables.viewModel.GetTopOfWallLevel().ToString();
        }
         private void WallPropertiesChange()
        {
            StaticVariables.viewModel.ChangeWallProperties();
            areaText.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetArea(StaticVariables.viewModel.GetWallArea()));
            inertiaText.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetInertia(StaticVariables.viewModel.GetWallInertia()));
            EIText.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetMomentScaleDimension(StaticVariables.viewModel.GetWallEI()));
            EAText.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetForce(StaticVariables.viewModel.GetWallEA()));
        }

        private void concretewall_height_TextChanged(object sender, TextChangedEventArgs e)
        {
            

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
                sideview_main.Focus();
                e.Handled=true;
            }
            if(e.Key == Key.Enter)
            {
                sideview_main.Focus();
            }
            
        }

        private void concretewall_thickness_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.ChangeWallThickness(WpfUtils.GetValueDimension( result));
            }
            WallPropertiesChange();
        }
       
        private void pilewall_height_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.ChangeWallHeight(WpfUtils.GetValueDimension(result));
            }
        }

        
        private void pile_space_TextChanged(object sender, TextChangedEventArgs e)
        {
            
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
            StaticEvents.UnitChangeEvent -= UnitChange;
        }

        private void pileDiameterCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          if(  pileDiameterCombobox.SelectedIndex>=0) StaticVariables.viewModel.PileIndex = pileDiameterCombobox.SelectedIndex;
            PileData pileDiameterData = pileDiameterCombobox.SelectedItem as PileData;
            if (pileDiameterData != null && WpfUtils.GetWallType(StaticVariables.viewModel.WallTypeIndex) == WallType.ConcretePileWall) StaticVariables.viewModel.ChangeWallThickness(pileDiameterData.t);
            if (!WpfUtils.CheckPileS(StaticVariables.viewModel.GetPileSpace()))
            {
                StaticVariables.viewModel.ChangePileSpace(pileDiameterData.t);
            }
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
           if(sheetpileCombobox.SelectedIndex>=0) StaticVariables.viewModel.SheetIndex = sheetpileCombobox.SelectedIndex;
            SheetData sheetData = sheetpileCombobox.SelectedItem as SheetData;
            if (sheetData != null && WpfUtils.GetWallType(StaticVariables.viewModel.WallTypeIndex) == WallType.SteelSheetWall) StaticVariables.viewModel.ChangeWallThickness(sheetData.Height*2);
            
            UnitChange();
        }

        private void sheetpiledesignwindow_Click(object sender, RoutedEventArgs e)
        {
            Windows.SheetPileWindow sheetpileWin = new Windows.SheetPileWindow();
            sheetpileWin.SelectSheet(sheetpileCombobox);
            sheetpileWin.ShowDialog();
        }

        private void beambottom_Checked(object sender, RoutedEventArgs e)
        {
            StaticVariables.viewModel.isCapBeamBottom = true;
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();
        }

        private void beamtop_Checked(object sender, RoutedEventArgs e)
        {
            StaticVariables.viewModel.isCapBeamBottom = false;
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();
        }

        private void topOfWallLevel_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //allow negatif numbers
            separator = Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            if (separator == ',')
            {
                Regex regex = new Regex("^[-][,][0-9]+$|^[-][0-9]*[,]{0,1}[0-9]*$|^[,][0-9]+$|^[0-9]*[,]{0,1}[0-9]*$");
                e.Handled = !regex.IsMatch(((TextBox)sender).Text.Insert(((TextBox)sender).SelectionStart, e.Text));
            }
            else
            {
                Regex regex = new Regex("^[-][.][0-9]+$|^[-][0-9]*[.]{0,1}[0-9]*$|^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
                e.Handled = !regex.IsMatch(((TextBox)sender).Text.Insert(((TextBox)sender).SelectionStart, e.Text));
            }
        }

        private void topOfWallLevel_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.ChangeTopOfWallLevel(result);
            }
        }

        private void sheetpilewall_height_LostFocus(object sender, RoutedEventArgs e)
        {            
            TextBox textBox = (TextBox)sender;
            if (double.TryParse(textBox.Text, out double result))
            {
                if (WpfUtils.CheckWallH(WpfUtils.GetValueDimension(result)))
                {
                    StaticVariables.viewModel.ChangeWallHeight(WpfUtils.GetValueDimension(result));

                }
                else
                {
                    textBox.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetDimension(StaticVariables.viewModel.GetWallHeight()));
                }
            }
        }

        private void concretewall_height_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (double.TryParse(textBox.Text, out double result))
            {
                if (WpfUtils.CheckWallH(WpfUtils.GetValueDimension(result)))
                {
                    StaticVariables.viewModel.ChangeWallHeight(WpfUtils.GetValueDimension(result));

                }
                else
                {
                    textBox.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetDimension(StaticVariables.viewModel.GetWallHeight()));
                }
            }
        }

        private void pilewall_height_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (double.TryParse(textBox.Text, out double result))
            {
                if (WpfUtils.CheckWallH(WpfUtils.GetValueDimension(result)))
                {
                    StaticVariables.viewModel.ChangeWallHeight(WpfUtils.GetValueDimension(result));

                }
                else
                {
                    textBox.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetDimension(StaticVariables.viewModel.GetWallHeight()));
                }
            }
        }

        private void pile_space_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                if (WpfUtils.CheckPileS(WpfUtils.GetValueDimension(result)))
                {
                    StaticVariables.viewModel.ChangePileSpace(WpfUtils.GetValueDimension(result));

                }
                else
                {
                    textBox.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetDimension(StaticVariables.viewModel.GetPileSpace()));
                }
            }
            WallPropertiesChange();
        }
    }
   

}
