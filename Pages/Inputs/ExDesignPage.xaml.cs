using ExDesign.Scripts;
using System;
using System.Collections.Generic;
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

namespace ExDesign.Pages.Inputs
{
    /// <summary>
    /// ExDesignPage.xaml etkileşim mantığı
    /// </summary>
    public partial class ExDesignPage : Page
    {
        

        public char separator = ',';
        public ExDesignPage()
        {
            InitializeComponent();
        }
        private void UnitChange()
        {
            excavationheight_unit.Content = StaticVariables.CurrentUnit.ToString().Split('_')[1];
            excavation_Z_unit.Content = StaticVariables.CurrentUnit.ToString().Split('_')[1];
            excavation_X1_unit.Content = StaticVariables.CurrentUnit.ToString().Split('_')[1];
            excavation_X2_unit.Content = StaticVariables.CurrentUnit.ToString().Split('_')[1];
            excavationheight.Text =WpfUtils.ChangeDecimalOptions( WpfUtils.GetDimension(StaticVariables.viewModel.GetexcavationHeight()));
            excavation_Z.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetDimension(StaticVariables.viewModel.GetexcavationZ()));
            excavation_X1.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetDimension(StaticVariables.viewModel.GetexcavationX1()));
            excavation_X2.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetDimension(StaticVariables.viewModel.GetexcavationX2()));

            surface_A1_unit.Content = StaticVariables.CurrentUnit.ToString().Split('_')[1];
            surface_A2_unit.Content = StaticVariables.CurrentUnit.ToString().Split('_')[1];
            surface_B_unit.Content = StaticVariables.CurrentUnit.ToString().Split('_')[1];
            surfaceslope.Text = StaticVariables.viewModel.GetSurfaceBeta().ToString();
            surface_B.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetDimension(StaticVariables.viewModel.GetSurfaceB()));
            surface_A1.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetDimension(StaticVariables.viewModel.GetSurfaceA1()));
            surface_A2.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetDimension(StaticVariables.viewModel.GetSurfaceA2()));

            gw_h1_unit.Content = StaticVariables.CurrentUnit.ToString().Split("_")[1];
            gw_h2_unit.Content = StaticVariables.CurrentUnit.ToString().Split("_")[1];
            gw_h1.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetDimension(StaticVariables.viewModel.GetGroundWaterH1()));
            gw_h2.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetDimension(StaticVariables.viewModel.GetGroundWaterH2()));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            view3d_main.Content = StaticVariables.view3DPage;
            sideview_main.Content = StaticVariables.SideviewPage;
            switch (WpfUtils.GetExcavationType(StaticVariables.viewModel.ExcavationTypeIndex))
            {
                case ExcavationType.none:
                    exsitetype1_button.IsChecked = true;
                    break;
                case ExcavationType.type1:
                    exsitetype2_button.IsChecked = true;
                    break;
                case ExcavationType.type2:
                    exsitetype3_button.IsChecked= true;
                    break;
                default:
                    exsitetype1_button.IsChecked = true;
                    break;
            }
            switch (WpfUtils.GetGroundSurfaceType(StaticVariables.viewModel.GroundSurfaceTypeIndex))
            {
                case GroundSurfaceType.flat:
                    surfacetype_button.IsChecked = true;
                    break;
                case GroundSurfaceType.type1:
                    surfacetype1_button.IsChecked = true;
                    break;
                case GroundSurfaceType.type2:
                    surfacetype2_button.IsChecked = true;
                    break;
                case GroundSurfaceType.type3:
                    surfacetype3_button.IsChecked = true;
                    break;
                default:
                    surfacetype_button.IsChecked = true;
                    break;
            }
            switch (WpfUtils.GetGroundWaterType(StaticVariables.viewModel.WaterTypeIndex))
            {
                case GroundWaterType.none:
                    groundwatertype_button.IsChecked = true;
                    break;
                case GroundWaterType.type1:
                    groundwatertype1_button.IsChecked = true;
                    break;
                case GroundWaterType.type2:
                    groundwatertype2_button.IsChecked = true;
                    break;
                case GroundWaterType.type3:
                    groundwatertype3_button.IsChecked = true;
                    break;
                default:
                    groundwatertype_button.IsChecked = true;
                    break;
            }
            UnitChange();
            StaticEvents.UnitChangeEvent += UnitChange;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            view3d_main.Content = null;
            sideview_main.Content = null;
            
        }

        

        private void exsitetype1_button_Checked(object sender, RoutedEventArgs e)
        {
            if(X1_dock_panel !=null)
            {
                X1_dock_panel.Visibility = Visibility.Hidden;
                X2_dock_panel.Visibility = Visibility.Hidden;
                Z_dock_panel.Visibility = Visibility.Hidden;
            }
            StaticVariables.viewModel.ExcavationTypeIndex =WpfUtils.GetExcavationTypeIndex( ExcavationType.none);
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();
            
        }

        private void exsitetype2_button_Checked(object sender, RoutedEventArgs e)
        {
            if (X1_dock_panel != null)
            {
                X1_dock_panel.Visibility = Visibility.Visible;
                X2_dock_panel.Visibility = Visibility.Visible;
                Z_dock_panel.Visibility = Visibility.Visible;
            }
            StaticVariables.viewModel.ExcavationTypeIndex = WpfUtils.GetExcavationTypeIndex(ExcavationType.type1);
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();

        }

        private void exsitetype3_button_Checked(object sender, RoutedEventArgs e)
        {
            StaticVariables.viewModel.ExcavationTypeIndex = WpfUtils.GetExcavationTypeIndex(ExcavationType.type2);

            if (WpfUtils.CheckExHeight(StaticVariables.viewModel.GetexcavationHeight())==false)
            {
                StaticVariables.viewModel.ChangeexcavationZ(WpfUtils.GetValueDimension((StaticVariables.viewModel.wall_h - StaticVariables.viewModel.excavationHeight) / 2));
                 excavation_Z.Text =   WpfUtils.ChangeDecimalOptions( WpfUtils.GetDimension(StaticVariables.viewModel.GetexcavationZ()));
            }
            if (X1_dock_panel != null)
            {
                X1_dock_panel.Visibility = Visibility.Visible;
                X2_dock_panel.Visibility = Visibility.Visible;
                Z_dock_panel.Visibility = Visibility.Visible;
            }
            
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();

        }
              

        private void excavationheight_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

        private void excavationheight_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                sideview_main.Focus();
                e.Handled = true;
            }
            if (e.Key == Key.Enter)
            {
                sideview_main.Focus();
            }
        }

        
       
        private void excavation_X1_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void excavation_X2_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.ChangeexcavationX2(WpfUtils.GetValueDimension(result));
            }
        }

        private void surfacetype_button_Checked(object sender, RoutedEventArgs e)
        {
            if (B_dock_panel != null)
            {
                B_dock_panel.Visibility = Visibility.Hidden;
                Beta_dock_panel.Visibility = Visibility.Hidden;
                A1_dock_panel.Visibility = Visibility.Hidden;
                A2_dock_panel.Visibility = Visibility.Hidden;
            }
            StaticVariables.viewModel.GroundSurfaceTypeIndex =WpfUtils.GetGroundSurfaceTypeIndex( GroundSurfaceType.flat);
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();

        }

        private void surfacetype1_button_Checked(object sender, RoutedEventArgs e)
        {
            if (B_dock_panel != null)
            {
                B_dock_panel.Visibility = Visibility.Hidden;
                Beta_dock_panel.Visibility = Visibility.Visible;
                A1_dock_panel.Visibility = Visibility.Visible;
                A2_dock_panel.Visibility = Visibility.Hidden;
            }
            StaticVariables.viewModel.GroundSurfaceTypeIndex = WpfUtils.GetGroundSurfaceTypeIndex(GroundSurfaceType.type1);
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();

        }

        private void surfacetype2_button_Checked(object sender, RoutedEventArgs e)
        {
            if (B_dock_panel != null)
            {
                B_dock_panel.Visibility = Visibility.Visible;
                Beta_dock_panel.Visibility = Visibility.Hidden;
                A1_dock_panel.Visibility = Visibility.Visible;
                A2_dock_panel.Visibility = Visibility.Hidden;
            }
            StaticVariables.viewModel.GroundSurfaceTypeIndex = WpfUtils.GetGroundSurfaceTypeIndex(GroundSurfaceType.type2);
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();

        }

        private void surfacetype3_button_Checked(object sender, RoutedEventArgs e)
        {
            if (B_dock_panel != null)
            {
                B_dock_panel.Visibility = Visibility.Visible;
                Beta_dock_panel.Visibility = Visibility.Hidden;
                A1_dock_panel.Visibility = Visibility.Visible;
                A2_dock_panel.Visibility = Visibility.Visible;
            }
            StaticVariables.viewModel.GroundSurfaceTypeIndex = WpfUtils.GetGroundSurfaceTypeIndex(GroundSurfaceType.type3);
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();

        }

        private void surfaceslope_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.ChangeSurfaceBeta(result);
            }
        }

       

        
        private void surface_B_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.ChangeSurfaceB(WpfUtils.GetValueDimension(result));
            }
        }

       

        private void groundwatertype_button_Checked(object sender, RoutedEventArgs e)
        {
            if (gw_h1_dock_panel != null)
            {
                gw_h1_dock_panel.Visibility = Visibility.Hidden;
                gw_h2_dock_panel.Visibility = Visibility.Hidden;
                
            }
            StaticVariables.viewModel.WaterTypeIndex =WpfUtils.GetGroundWaterTypeIndex( GroundWaterType.none);
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();

        }

        private void groundwatertype1_button_Checked(object sender, RoutedEventArgs e)
        {
            if (gw_h1_dock_panel != null)
            {
                gw_h1_dock_panel.Visibility = Visibility.Visible;
                gw_h2_dock_panel.Visibility = Visibility.Visible;

            }
            StaticVariables.viewModel.WaterTypeIndex = WpfUtils.GetGroundWaterTypeIndex(GroundWaterType.type1);
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();

        }

        private void groundwatertype2_button_Checked(object sender, RoutedEventArgs e)
        {
            if (gw_h1_dock_panel != null)
            {
                gw_h1_dock_panel.Visibility = Visibility.Visible;
                gw_h2_dock_panel.Visibility = Visibility.Visible;

            }
            StaticVariables.viewModel.WaterTypeIndex = WpfUtils.GetGroundWaterTypeIndex(GroundWaterType.type2);
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();

        }

        private void groundwatertype3_button_Checked(object sender, RoutedEventArgs e)
        {
            if (gw_h1_dock_panel != null)
            {
                gw_h1_dock_panel.Visibility = Visibility.Visible;
                gw_h2_dock_panel.Visibility = Visibility.Visible;

            }
            StaticVariables.viewModel.WaterTypeIndex = WpfUtils.GetGroundWaterTypeIndex(GroundWaterType.type3);
            StaticVariables.view3DPage.Refreshview();
            StaticVariables.SideviewPage.Refreshview();

        }

        private void gw_h1_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.ChangeGroundWaterH1(WpfUtils.GetValueDimension(result));
            }
        }

        

        private void gw_h2_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.viewModel.ChangeGroundWaterH2(WpfUtils.GetValueDimension(result));
            }
        }

        private void excavationheight_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (double.TryParse(textBox.Text, out double result))
            {
                if (WpfUtils.CheckExHeight(WpfUtils.GetValueDimension(result)))
                {
                    StaticVariables.viewModel.ChangeexcavationHeight(WpfUtils.GetValueDimension(result));

                }
                else
                {
                    textBox.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetDimension(StaticVariables.viewModel.GetexcavationHeight()));
                }
            }
        }

        private void excavation_Z_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (double.TryParse(textBox.Text, out double result))
            {
                if (WpfUtils.CheckExZ(WpfUtils.GetValueDimension(result)))
                {
                    StaticVariables.viewModel.ChangeexcavationZ(WpfUtils.GetValueDimension(result));

                }
                else
                {
                    textBox.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetDimension( StaticVariables.viewModel.GetexcavationZ()));
                }
            }
        }

        private void excavation_X1_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                if (WpfUtils.CheckFrontLength(WpfUtils.GetValueDimension(result) + StaticVariables.viewModel.GetexcavationX2() + 1))
                {
                    StaticVariables.viewModel.ChangeexcavationX1(WpfUtils.GetValueDimension(result));
                }
                else
                {
                    textBox.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetDimension(StaticVariables.viewModel.GetexcavationX1()));
                }
            }
        }

        private void excavation_X2_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                if (WpfUtils.CheckFrontLength(WpfUtils.GetValueDimension(result) + StaticVariables.viewModel.GetexcavationX1() + 1))
                {
                    StaticVariables.viewModel.ChangeexcavationX2(WpfUtils.GetValueDimension(result));
                }
                else
                {
                    textBox.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetDimension(StaticVariables.viewModel.GetexcavationX2()));
                }
            }
        }

        private void surface_A1_LostFocus(object sender, RoutedEventArgs e)
        {
            
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                if (WpfUtils.CheckBackLength(WpfUtils.GetValueDimension(result) + StaticVariables.viewModel.GetSurfaceA2() + 1))
                {
                    StaticVariables.viewModel.ChangeSurfaceA1(WpfUtils.GetValueDimension(result));
                }
                else
                {
                    textBox.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetDimension(StaticVariables.viewModel.GetSurfaceA1()));
                }
            }
        }

        private void surface_A2_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                if (WpfUtils.CheckBackLength(WpfUtils.GetValueDimension(result) + StaticVariables.viewModel.GetSurfaceA1() + 1))
                {
                    StaticVariables.viewModel.ChangeSurfaceA2(WpfUtils.GetValueDimension(result));
                }
                else
                {
                    textBox.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetDimension(StaticVariables.viewModel.GetSurfaceA2()));
                }
            }
        }
    }
}
