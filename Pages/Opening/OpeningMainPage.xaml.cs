using ExDesign.Datas;
using ExDesign.Scripts;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace ExDesign.Pages.Opening
{
    /// <summary>
    /// OpeningMainPage.xaml etkileşim mantığı
    /// </summary>
    public partial class OpeningMainPage : Page
    {
        Page newsPage = new Pages.Opening.newsPage();
        Page newProjectPage = new Pages.Opening.NewProjectPage();
        public OpeningMainPage()
        {
            InitializeComponent();
            OpeningScreen.Content = newsPage;
            RecentlyProjects();
        }

        private void RecentlyProjects()
        {
            ProgramModel.LoadModel();
            if (ProgramModel.programModel.ModelPaths.Count > 0)
            {
                path1.Text = ProgramModel.programModel.ModelPaths[ProgramModel.programModel.ModelPaths.Count - 1];
                ViewModelData modelData1 = new ViewModelData();
                using (StreamReader file = File.OpenText(path1.Text))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    modelData1 = (ViewModelData)serializer.Deserialize(file, typeof(ViewModelData));
                }
                if (modelData1 != null)
                {
                    projectname1.Text = modelData1.ProjectName;
                    date1.Text = modelData1.SaveDate;
                    recentproject1.Visibility = Visibility.Visible;
                }
            }
            if (ProgramModel.programModel.ModelPaths.Count > 1)
            {
                path2.Text = ProgramModel.programModel.ModelPaths[ProgramModel.programModel.ModelPaths.Count - 2];
                ViewModelData modelData2 = new ViewModelData();
                using (StreamReader file = File.OpenText(path2.Text))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    modelData2 = (ViewModelData)serializer.Deserialize(file, typeof(ViewModelData));
                }
                if (modelData2 != null)
                {
                    projectname2.Text = modelData2.ProjectName;
                    date2.Text = modelData2.SaveDate;
                    recentproject2.Visibility = Visibility.Visible;
                }
            }
            if (ProgramModel.programModel.ModelPaths.Count > 2)
            {
                path3.Text = ProgramModel.programModel.ModelPaths[ProgramModel.programModel.ModelPaths.Count - 3];
                ViewModelData modelData3 = new ViewModelData();
                using (StreamReader file = File.OpenText(path3.Text))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    modelData3 = (ViewModelData)serializer.Deserialize(file, typeof(ViewModelData));
                }
                if (modelData3 != null)
                {
                    projectname3.Text = modelData3.ProjectName;
                    date3.Text = modelData3.SaveDate;
                    recentproject3.Visibility = Visibility.Visible;
                }
            }
                 
        }

        private void NewProjectButton_Click(object sender, RoutedEventArgs e)
        {
            NewProjectOpening();
        }
        public void NewProjectOpening()
        {
            OpeningScreen.Content = newProjectPage;
        }

        private void OpenProjectButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Ex-Design files (*.exdb)|*.exdb";
            if (openFileDialog.ShowDialog() == true)
            {
                ViewModel.OpenModel(openFileDialog.FileName);
                ProgramWindow mainWindow = new ProgramWindow();
                WpfUtils.OpenWindow(mainWindow);
            }
        }

        private void recentproject1_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.OpenModel(path1.Text);
            ProgramWindow mainWindow = new ProgramWindow();
            WpfUtils.OpenWindow(mainWindow);
        }

        private void recentproject2_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.OpenModel(path2.Text);
            ProgramWindow mainWindow = new ProgramWindow();
            WpfUtils.OpenWindow(mainWindow);
        }

        private void recentproject3_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.OpenModel(path3.Text);
            ProgramWindow mainWindow = new ProgramWindow();
            WpfUtils.OpenWindow(mainWindow);
        }
    }
}
