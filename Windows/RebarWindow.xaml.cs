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
    /// RebarWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class RebarWindow : Window
    {
        private ObservableCollection<RebarData> tempRebarDataList = new ObservableCollection<RebarData>();
        private ComboBox comboBox;
        public char separator = ',';
        public RebarWindow()
        {
            InitializeComponent();
        }
        public void SelectRebar(ComboBox combo)
        {
            comboBox = combo;
            RebarList.SelectedIndex = combo.SelectedIndex;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var rebar in Rebar.RebarDataList)
            {
                tempRebarDataList.Add(rebar);
            }
            RebarList.ItemsSource = tempRebarDataList;
            RebarList.DisplayMemberPath = "Name";
            RebarList.SelectionMode = SelectionMode.Single;
            fykTextbox_unit.Content = StaticVariables.StressUnit;
            ETextbox_unit.Content = StaticVariables.StressUnit;
        }

        private void RebarList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RebarList.SelectedItem != null)
            {

                RebarName.IsEnabled = !((RebarData)RebarList.SelectedItem).isDefault;
                fykTextbox.IsEnabled = !((RebarData)RebarList.SelectedItem).isDefault;
                ETextbox.IsEnabled = !((RebarData)RebarList.SelectedItem).isDefault;

                RebarName.Text = ((RebarData)RebarList.SelectedItem).Name;
                fykTextbox.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetStress(((RebarData)RebarList.SelectedItem).fyk));
                ETextbox.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetStress(((RebarData)RebarList.SelectedItem).E));

            }
        }
        private void addnewpile_button_Click(object sender, RoutedEventArgs e)
        {
            tempRebarDataList.Add(new RebarData() { isDefault = false, Name = "B420C", fyk = 420000,  E = 200000000});
            RebarList.SelectedIndex = tempRebarDataList.Count - 1;
        }
        private void deletepile_button_Click(object sender, RoutedEventArgs e)
        {
            if (!((RebarData)RebarList.SelectedItem).isDefault)
            {
                tempRebarDataList.RemoveAt(RebarList.SelectedIndex);
                RebarList.SelectedIndex = 0;
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
            comboBox.SelectedIndex = RebarList.SelectedIndex;
            this.Close();
        }
        private void SaveChanges()
        {
            Rebar.RebarDataList.Clear();
            foreach (var rebar in tempRebarDataList)
            {

                Rebar.RebarDataList.Add(rebar);
            }
            Rebar.RebarSave();
        }

        private void RebarName_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            tempRebarDataList[RebarList.SelectedIndex].Name = textBox.Text;
            RebarList.Items.Refresh();
        }

        private void fykTextbox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void fykTextbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

        private void fykTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                tempRebarDataList[RebarList.SelectedIndex].fyk = WpfUtils.GetValueStress(result);
            }
        }

        private void ETextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                tempRebarDataList[RebarList.SelectedIndex].E = WpfUtils.GetValueStress(result);
            }
        }
    }
}
