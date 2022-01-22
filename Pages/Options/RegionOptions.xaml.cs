using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Threading;

namespace ExDesign.Pages.Options
{
    /// <summary>
    /// RegionOptions.xaml etkileşim mantığı
    /// </summary>
    public partial class RegionOptions : Page
    {
        public RegionOptions()
        {
            InitializeComponent();
            LanguageCombobox.SelectedIndex = 0;
        }


        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ResourceDictionary langDic = new ResourceDictionary();
            switch (LanguageCombobox.SelectedIndex)
            {
                case 0:
                    langDic.Source = new Uri("..\\..\\Resources\\Langs\\LanguageDictionary.xaml", UriKind.Relative);
                    break;
                case 1:
                    langDic.Source = new Uri("..\\..\\Resources\\Langs\\LanguageDictionary.tr-TR.xaml", UriKind.Relative);
                    break;
                default:
                    langDic.Source = new Uri("..\\..\\Resources\\Langs\\LanguageDictionary.xaml", UriKind.Relative);
                    break;
            }
            App.Current.Resources.MergedDictionaries.Add(langDic);
        }
    }
}
