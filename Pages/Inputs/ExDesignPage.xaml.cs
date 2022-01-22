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
            excavationheight.Text = WpfUtils.GetDimension(StaticVariables.view3DPage.GetexcavationHeight()).ToString();
            excavation_Z.Text = WpfUtils.GetDimension(StaticVariables.view3DPage.GetexcavationZ()).ToString();
            excavation_X1.Text = WpfUtils.GetDimension(StaticVariables.view3DPage.GetexcavationX1()).ToString();
            excavation_X2.Text = WpfUtils.GetDimension(StaticVariables.view3DPage.GetexcavationX2()).ToString();

            surface_A1_unit.Content = StaticVariables.CurrentUnit.ToString().Split('_')[1];
            surface_A2_unit.Content = StaticVariables.CurrentUnit.ToString().Split('_')[1];
            surface_B_unit.Content = StaticVariables.CurrentUnit.ToString().Split('_')[1];
            surfaceslope.Text = StaticVariables.view3DPage.GetSurfaceBeta().ToString();
            surface_B.Text = WpfUtils.GetDimension(StaticVariables.view3DPage.GetSurfaceB()).ToString();
            surface_A1.Text = WpfUtils.GetDimension(StaticVariables.view3DPage.GetSurfaceA1()).ToString();
            surface_A2.Text = WpfUtils.GetDimension(StaticVariables.view3DPage.GetSurfaceA2()).ToString();

            gw_h1_unit.Content = StaticVariables.CurrentUnit.ToString().Split("_")[1];
            gw_h2_unit.Content = StaticVariables.CurrentUnit.ToString().Split("_")[1];
            gw_h1.Text = WpfUtils.GetDimension(StaticVariables.view3DPage.GetGroundWaterH1()).ToString();
            gw_h2.Text = WpfUtils.GetDimension(StaticVariables.view3DPage.GetGroundWaterH2()).ToString();

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            view3d_main.Content = StaticVariables.view3DPage;
            sideview_main.Content = StaticVariables.SideviewPage;
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
            StaticVariables.excavationType = ExcavationType.none;
            StaticVariables.view3DPage.Refresh3Dview();
            
        }

        private void exsitetype2_button_Checked(object sender, RoutedEventArgs e)
        {
            if (X1_dock_panel != null)
            {
                X1_dock_panel.Visibility = Visibility.Visible;
                X2_dock_panel.Visibility = Visibility.Visible;
                Z_dock_panel.Visibility = Visibility.Visible;
            }
            StaticVariables.excavationType = ExcavationType.type1;
            StaticVariables.view3DPage.Refresh3Dview();
        }

        private void exsitetype3_button_Checked(object sender, RoutedEventArgs e)
        {
            if (X1_dock_panel != null)
            {
                X1_dock_panel.Visibility = Visibility.Visible;
                X2_dock_panel.Visibility = Visibility.Visible;
                Z_dock_panel.Visibility = Visibility.Visible;
            }
            StaticVariables.excavationType = ExcavationType.type2;
            StaticVariables.view3DPage.Refresh3Dview();
        }

        private void excavationheight_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.view3DPage.ChangeexcavationHeight(WpfUtils.GetValue(result));
            }
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
                e.Handled = true;
            }
        }

        private void excavation_Z_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.view3DPage.ChangeexcavationZ(WpfUtils.GetValue(result));
            }
        }

       
        private void excavation_X1_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.view3DPage.ChangeexcavationX1(WpfUtils.GetValue(result));
            }
        }

        

        

        private void excavation_X2_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.view3DPage.ChangeexcavationX2(WpfUtils.GetValue(result));
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
            StaticVariables.groundSurfaceType = GroundSurfaceType.flat;
            StaticVariables.view3DPage.Refresh3Dview();
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
            StaticVariables.groundSurfaceType = GroundSurfaceType.type1;
            StaticVariables.view3DPage.Refresh3Dview();
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
            StaticVariables.groundSurfaceType = GroundSurfaceType.type2;
            StaticVariables.view3DPage.Refresh3Dview();
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
            StaticVariables.groundSurfaceType = GroundSurfaceType.type3;
            StaticVariables.view3DPage.Refresh3Dview();
        }

        private void surfaceslope_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.view3DPage.ChangeSurfaceBeta(result);
            }
        }

       

        
        private void surface_B_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.view3DPage.ChangeSurfaceB(WpfUtils.GetValue(result));
            }
        }

        

       

        private void surface_A1_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.view3DPage.ChangeSurfaceA1(WpfUtils.GetValue(result));
            }
        }

        

       

        private void surface_A2_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.view3DPage.ChangeSurfaceA2(WpfUtils.GetValue(result));
            }
        }

        

       

        private void groundwatertype_button_Checked(object sender, RoutedEventArgs e)
        {
            if (gw_h1_dock_panel != null)
            {
                gw_h1_dock_panel.Visibility = Visibility.Hidden;
                gw_h2_dock_panel.Visibility = Visibility.Hidden;
                
            }
            StaticVariables.groundWaterType = GroundWaterType.none;
            StaticVariables.view3DPage.Refresh3Dview();
        }

        private void groundwatertype1_button_Checked(object sender, RoutedEventArgs e)
        {
            if (gw_h1_dock_panel != null)
            {
                gw_h1_dock_panel.Visibility = Visibility.Visible;
                gw_h2_dock_panel.Visibility = Visibility.Visible;

            }
            StaticVariables.groundWaterType = GroundWaterType.type1;
            StaticVariables.view3DPage.Refresh3Dview();
        }

        private void groundwatertype2_button_Checked(object sender, RoutedEventArgs e)
        {
            if (gw_h1_dock_panel != null)
            {
                gw_h1_dock_panel.Visibility = Visibility.Visible;
                gw_h2_dock_panel.Visibility = Visibility.Visible;

            }
            StaticVariables.groundWaterType = GroundWaterType.type2;
            StaticVariables.view3DPage.Refresh3Dview();
        }

        private void groundwatertype3_button_Checked(object sender, RoutedEventArgs e)
        {
            if (gw_h1_dock_panel != null)
            {
                gw_h1_dock_panel.Visibility = Visibility.Visible;
                gw_h2_dock_panel.Visibility = Visibility.Visible;

            }
            StaticVariables.groundWaterType = GroundWaterType.type3;
            StaticVariables.view3DPage.Refresh3Dview();
        }

        private void gw_h1_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.view3DPage.ChangeGroundWaterH1(WpfUtils.GetValue(result));
            }
        }

        

        private void gw_h2_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                StaticVariables.view3DPage.ChangeGroundWaterH2(WpfUtils.GetValue(result));
            }
        }

        
    }
}
