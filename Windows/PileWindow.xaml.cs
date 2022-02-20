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
    /// PileWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class PileWindow : Window
    {
        private ObservableCollection<PileData> tempPileDiameterDatas = new ObservableCollection<PileData>();
        private ComboBox comboBox;
        public char separator = ',';
        public PileWindow()
        {
            InitializeComponent();
        }
        
        public void SelectPile(ComboBox combo)
        {
            comboBox = combo;
            PileList.SelectedIndex = combo.SelectedIndex;
        }
       
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var pile in Pile.PileDiameterDataList)
            {
                tempPileDiameterDatas.Add((PileData)pile.Clone());
            }
            PileList.ItemsSource = tempPileDiameterDatas;
            PileList.DisplayMemberPath = "Name";
            PileList.SelectionMode = SelectionMode.Single;
            pilediameterunit.Content = StaticVariables.CurrentUnit.ToString().Split('_')[1];
        }

        private void PileList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(PileList.SelectedItem != null)
            {
                
                    pilenametextbox.IsEnabled= !((PileData)PileList.SelectedItem).isDefault;
                    pilediametertextbox.IsEnabled= !((PileData)PileList.SelectedItem).isDefault;
                
                pilenametextbox.Text = ((PileData)PileList.SelectedItem).Name;
                pilediametertextbox.Text =WpfUtils.ChangeDecimalOptions( WpfUtils.GetDimension(((PileData)PileList.SelectedItem).t));
            }

        }

        private void deletepile_button_Click(object sender, RoutedEventArgs e)
        {
            if(!((PileData)PileList.SelectedItem).isDefault)
            {
                tempPileDiameterDatas.RemoveAt(PileList.SelectedIndex);
                PileList.SelectedIndex = 0;
            }
           
        }

        private void addnewpile_button_Click(object sender, RoutedEventArgs e)
        {
            tempPileDiameterDatas.Add(new PileData() { isDefault = false, Name = "ф50", t = 0.5 });
            PileList.SelectedIndex = tempPileDiameterDatas.Count-1;
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
            comboBox.SelectedIndex = PileList.SelectedIndex;
            this.Close();
        }

        private void pilenametextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            tempPileDiameterDatas[PileList.SelectedIndex].Name = textBox.Text;
            PileList.Items.Refresh();            
        }

        private void pilediametertextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (double.TryParse(textBox.Text, out double result))
            {
                tempPileDiameterDatas[PileList.SelectedIndex].t = WpfUtils.GetValueDimension(result);
            }
        }

        private void pilediametertextbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

        private void pilediametertextbox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void SaveChanges()
        {
            Pile.PileDiameterDataList.Clear();
            foreach (var pile in tempPileDiameterDatas)
            {

                Pile.PileDiameterDataList.Add(pile);
            }
            Pile.pileDiameterSave();
        }
    }
}
