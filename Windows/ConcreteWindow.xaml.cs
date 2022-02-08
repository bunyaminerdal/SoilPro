
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
    /// ConcreteWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class ConcreteWindow : Window
    {
        private ObservableCollection<ConcreteData> tempConcreteDataList = new ObservableCollection<ConcreteData>();
        private ComboBox comboBox;
        public char separator = ',';
        public ConcreteWindow()
        {
            InitializeComponent();
        }
        public void SelectConcrete(ComboBox combo)
        {
            comboBox = combo;
            concreteList.SelectedIndex = combo.SelectedIndex;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var concrete in Concrete.ConcreteDataList)
            {
                tempConcreteDataList.Add(concrete);
            }
            concreteList.ItemsSource = tempConcreteDataList;
            concreteList.DisplayMemberPath = "Name";
            concreteList.SelectionMode = SelectionMode.Single;
            fckTextbox_unit.Content = StaticVariables.StressUnit;
            fctTextbox_unit.Content = StaticVariables.StressUnit;
            ETextbox_unit.Content = StaticVariables.StressUnit;
            GTextbox_unit.Content = StaticVariables.StressUnit;            
        }

        private void concreteList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (concreteList.SelectedItem != null)
            {

                ConcreteName.IsEnabled = !((ConcreteData)concreteList.SelectedItem).isDefault;
                fckTextbox.IsEnabled = !((ConcreteData)concreteList.SelectedItem).isDefault;
                fctTextbox.IsEnabled = !((ConcreteData)concreteList.SelectedItem).isDefault;
                ETextbox.IsEnabled = !((ConcreteData)concreteList.SelectedItem).isDefault;
                GTextbox.IsEnabled = !((ConcreteData)concreteList.SelectedItem).isDefault;

                ConcreteName.Text = ((ConcreteData)concreteList.SelectedItem).Name;
                fckTextbox.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetStress(((ConcreteData)concreteList.SelectedItem).fck));
                fctTextbox.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetStress(((ConcreteData)concreteList.SelectedItem).fct));
                ETextbox.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetStress(((ConcreteData)concreteList.SelectedItem).E));
                GTextbox.Text = WpfUtils.ChangeDecimalOptions(WpfUtils.GetStress(((ConcreteData)concreteList.SelectedItem).G));
                
            }
        }
        private void addnewpile_button_Click(object sender, RoutedEventArgs e)
        {
            tempConcreteDataList.Add(new ConcreteData() { isDefault = false, Name = "C20", fck = 20000, fct = 1570, E = 28534000, G = 11414000 });
            concreteList.SelectedIndex = tempConcreteDataList.Count - 1;
        }
        private void deletepile_button_Click(object sender, RoutedEventArgs e)
        {
            if (!((ConcreteData)concreteList.SelectedItem).isDefault)
            {
                tempConcreteDataList.RemoveAt(concreteList.SelectedIndex);
                concreteList.SelectedIndex = 0;
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
            comboBox.SelectedIndex = concreteList.SelectedIndex;
            this.Close();
        }
        private void SaveChanges()
        {
            Concrete.ConcreteDataList.Clear();
            foreach (var concrete in tempConcreteDataList)
            {
                Concrete.ConcreteDataList.Add(concrete);
            }
            
            Concrete.ConcreteSave();
        }
        private void ConcreteName_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            tempConcreteDataList[concreteList.SelectedIndex].Name = textBox.Text;
            concreteList.Items.Refresh();
        }

        private void fckTextbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

        private void fckTextbox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void fckTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                tempConcreteDataList[concreteList.SelectedIndex].fck = WpfUtils.GetValueStress(result);
            }
        }

        private void fctTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                tempConcreteDataList[concreteList.SelectedIndex].fct = WpfUtils.GetValueStress(result);
            }
        }

        private void ETextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                tempConcreteDataList[concreteList.SelectedIndex].E = WpfUtils.GetValueStress(result);
            }
        }

        private void GTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                tempConcreteDataList[concreteList.SelectedIndex].G = WpfUtils.GetValueStress(result);
            }
        }
    }
}
