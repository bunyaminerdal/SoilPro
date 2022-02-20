using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using ExDesign.Datas;
using ExDesign.Scripts;

namespace ExDesign.Windows
{
    /// <summary>
    /// SheetPileWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class SheetPileWindow : Window
    {
        private ObservableCollection<SheetData> tempSheetDataList = new ObservableCollection<SheetData>();
        private ComboBox comboBox;
        public char separator = ',';
        public SheetPileWindow()
        {
            InitializeComponent();
        }
        public void SelectSheet(ComboBox combo)
        {
            comboBox = combo;
            sheetList.SelectedIndex = combo.SelectedIndex;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var sheet in Sheet.SheetDataList)
            {
                tempSheetDataList.Add((SheetData)sheet.Clone());
            }
            sheetList.ItemsSource = tempSheetDataList;
            sheetList.DisplayMemberPath = "Name";
            sheetList.SelectionMode = SelectionMode.Single;
            sheetarea_unit.Content = StaticVariables.areaUnit+"/m";
            sheetheight_unit.Content = StaticVariables.dimensionUnit;
            sheetInertia_unit.Content = StaticVariables.inertiaUnit + "/m";
            sheetlength_unit.Content = StaticVariables.dimensionUnit;
            sheetThickness_unit.Content = StaticVariables.dimensionUnit;
            wely_unit.Content = StaticVariables.volumeUnit + "/m";
            wply_unit.Content = StaticVariables.volumeUnit + "/m";

        }
        private void sheetList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sheetList.SelectedItem != null)
            {

                sheetname.IsEnabled = !((SheetData)sheetList.SelectedItem).isDefault;
                sheetarea.IsEnabled = !((SheetData)sheetList.SelectedItem).isDefault;
                sheetheight.IsEnabled = !((SheetData)sheetList.SelectedItem).isDefault;
                sheetInertia.IsEnabled = !((SheetData)sheetList.SelectedItem).isDefault;
                sheetlength.IsEnabled = !((SheetData)sheetList.SelectedItem).isDefault;
                sheetThickness.IsEnabled = !((SheetData)sheetList.SelectedItem).isDefault;
                wely.IsEnabled = !((SheetData)sheetList.SelectedItem).isDefault;
                wply.IsEnabled = !((SheetData)sheetList.SelectedItem).isDefault;

                sheetname.Text = ((SheetData)sheetList.SelectedItem).Name;
                sheetheight.Text =WpfUtils.ChangeDecimalOptions( WpfUtils.GetDimension(((SheetData)sheetList.SelectedItem).Height));
                sheetlength.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetDimension(((SheetData)sheetList.SelectedItem).Length));
                sheetThickness.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetDimension(((SheetData)sheetList.SelectedItem).Thickness));
                sheetarea.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetArea(((SheetData)sheetList.SelectedItem).Area));
                sheetInertia.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetInertia(((SheetData)sheetList.SelectedItem).Inertia));
                wely.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetVolume(((SheetData)sheetList.SelectedItem).Wely));
                wply.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetVolume(((SheetData)sheetList.SelectedItem).Wply));
            }
        }
        private void addnewpile_button_Click(object sender, RoutedEventArgs e)
        {
            tempSheetDataList.Add(new SheetData() { isDefault = false,type =0, Name = "AU14", Length = 0.75, Height = 0.204, Thickness = 0.01, Inertia = 0.0002868, Area = 0.0132, Wely = 0.001405, Wply = 0.001663 });
            sheetList.SelectedIndex = tempSheetDataList.Count - 1;

        }

        private void deletepile_button_Click(object sender, RoutedEventArgs e)
        {
            if (!((SheetData)sheetList.SelectedItem).isDefault)
            {
                tempSheetDataList.RemoveAt(sheetList.SelectedIndex);
                sheetList.SelectedIndex = 0;
            }
        }

        private void save_close_button_Click(object sender, RoutedEventArgs e)
        {
            SaveChanges();
            comboBox.SelectedIndex = 0;
            comboBox.SelectedIndex = sheetList.SelectedIndex;
            this.Close();
        }

        private void save_button_Click(object sender, RoutedEventArgs e)
        {
            SaveChanges();
        }

        private void cancel_button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void SaveChanges()
        {
            Sheet.SheetDataList.Clear();
            foreach (var sheet in tempSheetDataList)
            {

                Sheet.SheetDataList.Add(sheet);
            }
            Sheet.SheetSave();
        }

        private void sheetname_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            tempSheetDataList[sheetList.SelectedIndex].Name = textBox.Text;
            sheetList.Items.Refresh();
        }

        private void sheetheight_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                tempSheetDataList[sheetList.SelectedIndex].Height = WpfUtils.GetValueDimension(result);
            }
        }

        private void sheetheight_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

        private void sheetheight_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void sheetlength_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                tempSheetDataList[sheetList.SelectedIndex].Length = WpfUtils.GetValueDimension(result);
            }
        }

        private void sheetThickness_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                tempSheetDataList[sheetList.SelectedIndex].Thickness = WpfUtils.GetValueDimension(result);
            }
        }

        private void sheetarea_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                tempSheetDataList[sheetList.SelectedIndex].Area = WpfUtils.GetValueArea(result);
            }
        }

        private void sheetInertia_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                tempSheetDataList[sheetList.SelectedIndex].Inertia = WpfUtils.GetValueInertia(result);
            }
        }

        private void wely_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                tempSheetDataList[sheetList.SelectedIndex].Wely = WpfUtils.GetValueVolume(result);
            }
        }

        private void wply_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                tempSheetDataList[sheetList.SelectedIndex].Wply = WpfUtils.GetValueVolume(result);
            }
        }
    }
}
 