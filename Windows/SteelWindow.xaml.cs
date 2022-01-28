using ExDesign.Datas;
using ExDesign.Scripts;
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

namespace ExDesign.Windows
{
    /// <summary>
    /// SteelWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class SteelWindow : Window
    {
        private ObservableCollection<SteelData> tempSteelDataList = new ObservableCollection<SteelData>();
        private ComboBox comboBox;
        public char separator = ',';
        public SteelWindow()
        {
            InitializeComponent();
        }
        public void SelectSteel(ComboBox combo)
        {
            comboBox = combo;
            SteelList.SelectedIndex = combo.SelectedIndex;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var steel in Steel.SteelDataList)
            {
                tempSteelDataList.Add(steel);
            }
            SteelList.ItemsSource = tempSteelDataList;
            SteelList.DisplayMemberPath = "Name";
            SteelList.SelectionMode = SelectionMode.Single;
            fyTextbox_unit.Content = StaticVariables.StressUnit;
            ETextbox_unit.Content = StaticVariables.StressUnit;
        }

        private void SteelList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SteelList.SelectedItem != null)
            {

                SteelName.IsEnabled = !((SteelData)SteelList.SelectedItem).isDefault;
                fyTextbox.IsEnabled = !((SteelData)SteelList.SelectedItem).isDefault;
                ETextbox.IsEnabled = !((SteelData)SteelList.SelectedItem).isDefault;

                SteelName.Text = ((SteelData)SteelList.SelectedItem).Name;
                fyTextbox.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetStress(((SteelData)SteelList.SelectedItem).fy));
                ETextbox.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetStress(((SteelData)SteelList.SelectedItem).E));

            }
        }
        private void addnewpile_button_Click(object sender, RoutedEventArgs e)
        {
            tempSteelDataList.Add(new SteelData() { isDefault = false, Name = "S235", fy = 235000, E = 200000000 });
            SteelList.SelectedIndex = tempSteelDataList.Count - 1;
        }
        private void deletepile_button_Click(object sender, RoutedEventArgs e)
        {
            if (!((SteelData)SteelList.SelectedItem).isDefault)
            {
                tempSteelDataList.RemoveAt(SteelList.SelectedIndex);
                SteelList.SelectedIndex = 0;
            }
        }

        private void cancel_button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void save_button_Click(object sender, RoutedEventArgs e)
        {
            SaveChanges();
        }

        private void save_close_button_Click(object sender, RoutedEventArgs e)
        {
            SaveChanges();
            comboBox.SelectedIndex = 0;
            comboBox.SelectedIndex = SteelList.SelectedIndex;
            this.Close();
        }
        private void SaveChanges()
        {
            Steel.SteelDataList.Clear();
            foreach (var steel in tempSteelDataList)
            {

                Steel.SteelDataList.Add(steel);
            }
            Steel.SteelSave();
        }

        private void SteelName_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            tempSteelDataList[SteelList.SelectedIndex].Name = textBox.Text;
            SteelList.Items.Refresh();
        }

        private void fyTextbox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void fyTextbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
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
        private void fyTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                tempSteelDataList[SteelList.SelectedIndex].fy = WpfUtils.GetValueStress(result);
            }
        }
        private void ETextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                tempSteelDataList[SteelList.SelectedIndex].E = WpfUtils.GetValueStress(result);
            }
        }
    }
}
