﻿using ExDesign.Datas;
using ExDesign.Scripts;
using Microsoft.Win32;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExDesign
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ProgramWindow : Window
    {
        
        Page InputsMain_Page ;
        Page OptionsMain_Page ; 
        Page ReportMain_Page ;
        
        
        public ProgramWindow()
        {
            InitializeComponent();

            
            StandartStart();
        }
        private void StandartStart()
        {
            EventManager.RegisterClassHandler(typeof(System.Windows.Controls.TextBox), System.Windows.Controls.TextBox.GotKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(OnGotKeyboardFocus));
            EventManager.RegisterClassHandler(typeof(System.Windows.Controls.TextBox), System.Windows.Controls.TextBox.PreviewDragOverEvent, new DragEventHandler(PreviewDragOver1));
            EventManager.RegisterClassHandler(typeof(System.Windows.Controls.TextBox), System.Windows.Controls.TextBox.PreviewDropEvent, new DragEventHandler(PreviewDragEnter1));
            CommandManager.RegisterClassCommandBinding(typeof(System.Windows.Controls.TextBox), new CommandBinding(ApplicationCommands.Paste, new ExecutedRoutedEventHandler(textBox_PreviewExecuted)));

            StaticVariables.view3DPage = new Pages.Inputs.Views.View3dPage();
            StaticVariables.view3DPage.SetViewModel();
            StaticVariables.SideviewPage = new Pages.Inputs.Views.SideviewPage();
            StaticVariables.SideviewPage.SetViewModel();
            StaticVariables.loadsAndFocesPage = new Pages.Inputs.Views.LoadsAndFocesPage();
            StaticVariables.loadsAndFocesPage.SetViewModel();
            InputsMain_Page = new Pages.Inputs.InputsMainPage();
            OptionsMain_Page = new Pages.Options.OptionsMainPage();
            ReportMain_Page = new Pages.Reports.ReportMainPage();
            MainScreen.Content = InputsMain_Page;
            this.Title = "Ex-Design | " + StaticVariables.viewModel.ProjectName;
        }

        private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
        }

        void OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var textBox = sender as System.Windows.Controls.TextBox;

            if (textBox != null && !textBox.IsReadOnly && e.KeyboardDevice.IsKeyDown(Key.Tab))
                textBox.SelectAll();
        }
        private void PreviewDragEnter1(object sender, DragEventArgs e)
        {
            e.Handled = true;            
        }
        private void PreviewDragOver1(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.All;
            e.Handled = true;
        }

        private void Options_Click(object sender, RoutedEventArgs e)
        {
            MainScreen.Content = OptionsMain_Page;
        }

        private void MainPage_Click(object sender, RoutedEventArgs e)
        {
            MainScreen.Content = InputsMain_Page;
        }

        private void Report_Click(object sender, RoutedEventArgs e)
        {
            MainScreen.Content = ReportMain_Page;
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
           
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpeningWindow openingWindow = new OpeningWindow();
            openingWindow.OpeningNew();
            WpfUtils.OpenWindow(openingWindow);

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }



        private void save_menuitem_Click(object sender, RoutedEventArgs e)
        {
            Save();
                
        }
        private void Save()
        {
            if (StaticVariables.viewModel.Path != "Untitled" && StaticVariables.viewModel.Path != null && StaticVariables.viewModel.Path != "")
            {
                ViewModel.ModelSave();
            }
            else
            {
                SaveAs();
            }
        }

        private void open_menuitem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Ex-Design files (*.exdb)|*.exdb";
            if (openFileDialog.ShowDialog() == true)
            {
                ViewModel.OpenModel(openFileDialog.FileName);
                StandartStart();
            }
        }

        private void saveas_menuitem_Click(object sender, RoutedEventArgs e)
        {
            SaveAs();
        }

        private void SaveAs()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Ex-Design files (*.exdb)|*.exdb";
            if (saveFileDialog.ShowDialog() == true)
            {
                ViewModel.ModelSaveAs(saveFileDialog.FileName, saveFileDialog.SafeFileName.Split('.')[0]);
                
            }
            this.Title = "Ex-Design | " + StaticVariables.viewModel.ProjectName;
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //MessageBoxResult messageBoxResult = MessageBox.Show(FindResource("ClosingMessage").ToString(), "Question",
            //  MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

            //if (messageBoxResult == MessageBoxResult.Cancel)
            //{
            //    e.Cancel = true;
            //}
            //else if (messageBoxResult == MessageBoxResult.Yes)
            //{
            //    Save();
            //}
        }

        
    }
}
