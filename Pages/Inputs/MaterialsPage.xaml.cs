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

    }
}
