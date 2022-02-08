using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using ExDesign.Datas;

namespace ExDesign.Windows
{
    
    /// <summary>
    /// TextureWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class TextureWindow : Window
    {
        private ObservableCollection<SoilTextureData> tempTextureDatas = new ObservableCollection<SoilTextureData>();
        private SoilTextureData currentTexture;
        private SoilTypeLibrary soilLibrary;
        public TextureWindow()
        {
            InitializeComponent();
        }
        public void SetLibrary( SoilTypeLibrary _soilLibrary)
        {
            soilLibrary = _soilLibrary;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var texture in SoilTexture.tempSoilTextureDataList)
            {
                tempTextureDatas.Add(texture);
            }
            InitializeTextureList();
        }

        private void InitializeTextureList()
        {
            TexturePanel.Children.Clear();
            int textureCount = 4;
            DockPanel dock=new DockPanel();
            dock.HorizontalAlignment = HorizontalAlignment.Left;
            dock.VerticalAlignment = VerticalAlignment.Top;
            TexturePanel.Children.Add(dock);
            foreach (var texture in tempTextureDatas)
            {
                if(textureCount<0)
                {
                    dock = new DockPanel();
                    dock.HorizontalAlignment = HorizontalAlignment.Left;
                    dock.VerticalAlignment = VerticalAlignment.Top;
                    TexturePanel.Children.Add(dock);
                    textureCount = 4;
                }
                RadioButton button = new RadioButton();
                StackPanel stackPanel = new StackPanel();
                Image image = new Image();
                image.Height = 100;
                image.Width = 100;
                image.Source = new BitmapImage(texture.TextureUri);
                image.Stretch = Stretch.Fill;
                Label label = new Label();
                label.Content = texture.TextureName;
                stackPanel.Children.Add(image);
                stackPanel.Children.Add(label);
                button.Content = stackPanel;
                button.Name = "button_"+tempTextureDatas.IndexOf(texture);
                button.Click += SetCurrentTexture;
                
                button.Margin = new Thickness(5);
                dock.Children.Add(button);
                textureCount--;
            }
            
        }
        private void SetCurrentTexture(object sender, RoutedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            int result = 0;
            if (button != null) int.TryParse(button.Name.Split('_')[1], out result);

            currentTexture = tempTextureDatas[result];
        }

        private void cancel_button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void select_button_Click(object sender, RoutedEventArgs e)
        {
            soilLibrary.SetTexture(currentTexture);
            this.Close();
        }
    }
}
