using SoilPro.Scripts;
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

namespace SoilPro.Pages.Inputs
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
        }
        public void GetWallProperties()
        {
            excavationheight.Text = StaticVariables.view3DPage.GetexcavationHeight().ToString();
            excavation_Z.Text = StaticVariables.view3DPage.GetexcavationZ().ToString();
            excavation_X1.Text = StaticVariables.view3DPage.GetexcavationX1().ToString();
            excavation_X2.Text = StaticVariables.view3DPage.GetexcavationX2().ToString();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            view3d_main.Content = StaticVariables.view3DPage;
            sideview_main.Content = StaticVariables.SideviewPage;
            GetWallProperties();
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
            StaticVariables.view3DPage.ChangeExcavationType();
            
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
            StaticVariables.view3DPage.ChangeExcavationType();
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
            StaticVariables.view3DPage.ChangeExcavationType();
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

        private void excavation_Z_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

        private void excavation_Z_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
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

        private void excavation_X1_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

        private void excavation_X1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
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

        private void excavation_X2_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

        private void excavation_X2_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
    }
}
