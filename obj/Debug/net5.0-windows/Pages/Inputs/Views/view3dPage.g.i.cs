﻿#pragma checksum "..\..\..\..\..\..\Pages\Inputs\Views\view3dPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "EE6A4303FD78AFAB1A844062B0E9ABEDB8193E7A"
//------------------------------------------------------------------------------
// <auto-generated>
//     Bu kod araç tarafından oluşturuldu.
//     Çalışma Zamanı Sürümü:4.0.30319.42000
//
//     Bu dosyada yapılacak değişiklikler yanlış davranışa neden olabilir ve
//     kod yeniden oluşturulursa kaybolur.
// </auto-generated>
//------------------------------------------------------------------------------

using ExDesign.Pages.Inputs.Views;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace ExDesign.Pages.Inputs.Views {
    
    
    /// <summary>
    /// View3dPage
    /// </summary>
    public partial class View3dPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\..\..\..\..\Pages\Inputs\Views\view3dPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ContentControl contentControl2;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\..\..\..\Pages\Inputs\Views\view3dPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Viewport3D viewport3d_main;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\..\..\..\Pages\Inputs\Views\view3dPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label scroll_3dview;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\..\..\..\Pages\Inputs\Views\view3dPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Position_Reset_bttn;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.1.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ExDesign;component/pages/inputs/views/view3dpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\..\Pages\Inputs\Views\view3dPage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.1.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 11 "..\..\..\..\..\..\Pages\Inputs\Views\view3dPage.xaml"
            ((System.Windows.Controls.Grid)(target)).MouseWheel += new System.Windows.Input.MouseWheelEventHandler(this.viewport3d_main_MouseWheel);
            
            #line default
            #line hidden
            
            #line 11 "..\..\..\..\..\..\Pages\Inputs\Views\view3dPage.xaml"
            ((System.Windows.Controls.Grid)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.viewport3d_main_MouseDown);
            
            #line default
            #line hidden
            
            #line 11 "..\..\..\..\..\..\Pages\Inputs\Views\view3dPage.xaml"
            ((System.Windows.Controls.Grid)(target)).MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.viewport3d_main_MouseUp);
            
            #line default
            #line hidden
            
            #line 11 "..\..\..\..\..\..\Pages\Inputs\Views\view3dPage.xaml"
            ((System.Windows.Controls.Grid)(target)).MouseMove += new System.Windows.Input.MouseEventHandler(this.viewport3d_main_MouseMove);
            
            #line default
            #line hidden
            
            #line 11 "..\..\..\..\..\..\Pages\Inputs\Views\view3dPage.xaml"
            ((System.Windows.Controls.Grid)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.viewport3d_main_MouseLeave);
            
            #line default
            #line hidden
            return;
            case 2:
            this.contentControl2 = ((System.Windows.Controls.ContentControl)(target));
            return;
            case 3:
            this.viewport3d_main = ((System.Windows.Controls.Viewport3D)(target));
            return;
            case 4:
            this.scroll_3dview = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.Position_Reset_bttn = ((System.Windows.Controls.Button)(target));
            
            #line 17 "..\..\..\..\..\..\Pages\Inputs\Views\view3dPage.xaml"
            this.Position_Reset_bttn.Click += new System.Windows.RoutedEventHandler(this.Position_Reset_bttn_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

