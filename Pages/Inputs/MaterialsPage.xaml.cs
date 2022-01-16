using System;
using System.Collections.Generic;
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
    /// MaterialsPage.xaml etkileşim mantığı
    /// </summary>
    public partial class MaterialsPage : Page
    {
        Views.View3dPage view3DPage;
        public MaterialsPage()
        {
            InitializeComponent();
            
        }
        public void SetViewPages(Views.View3dPage view3d,Views.SideviewPage sideview)
        {
            view3d_main.Content = view3d;
            view3DPage = view3d;
            sideview_main.Content = sideview;
            
        }

        private void concretewall_height_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            //Int32 selectionStart = textBox.SelectionStart;
            //Int32 selectionLength = textBox.SelectionLength;

            //String newText = String.Empty;
            //foreach (Char c in textBox.Text.ToCharArray())
            //{
            //    if (Char.IsDigit(c) || Char.IsControl(c)) newText += c;
            //}

            //textBox.Text = newText;

            //textBox.SelectionStart = selectionStart <= textBox.Text.Length ?
            //    selectionStart : textBox.Text.Length;
            if(double.TryParse(textBox.Text, out double result))
            {
                view3DPage.ChangeWallHeight(result);
            }
            
        }
        private static readonly Regex _regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }
        private void TextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            //if (e.DataObject.GetDataPresent(typeof(double)))
            //{
            //    double text = (double)e.DataObject.GetData(typeof(double));
            //    //if (!IsTextAllowed(double))
            //    //{
            //    //    e.CancelCommand();
            //    //}
            //}
            //else
            //{
            //    e.CancelCommand();
            //}
        }
        private void concretewall_height_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !double.TryParse(e.Text, out double result);
        }
    }

}
