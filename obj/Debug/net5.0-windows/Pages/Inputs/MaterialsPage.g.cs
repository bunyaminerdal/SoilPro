﻿#pragma checksum "..\..\..\..\..\Pages\Inputs\MaterialsPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "C7933D92041EF06EDDE667847F3E541F0B5710F1"
//------------------------------------------------------------------------------
// <auto-generated>
//     Bu kod araç tarafından oluşturuldu.
//     Çalışma Zamanı Sürümü:4.0.30319.42000
//
//     Bu dosyada yapılacak değişiklikler yanlış davranışa neden olabilir ve
//     kod yeniden oluşturulursa kaybolur.
// </auto-generated>
//------------------------------------------------------------------------------

using SoilPro.Pages.Inputs;
using SoilPro.Pages.Inputs.Views;
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


namespace SoilPro.Pages.Inputs {
    
    
    /// <summary>
    /// MaterialsPage
    /// </summary>
    public partial class MaterialsPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 24 "..\..\..\..\..\Pages\Inputs\MaterialsPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Frame view3d_main;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\..\..\Pages\Inputs\MaterialsPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Frame sideview_main;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\..\..\Pages\Inputs\MaterialsPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox concretewall_height;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\..\..\..\Pages\Inputs\MaterialsPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label concretewall_height_unit;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\..\..\Pages\Inputs\MaterialsPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox concretewall_thickness;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\..\..\..\Pages\Inputs\MaterialsPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label concretewall_thickness_unit;
        
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
            System.Uri resourceLocater = new System.Uri("/SoilPro;component/pages/inputs/materialspage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Pages\Inputs\MaterialsPage.xaml"
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
            
            #line 10 "..\..\..\..\..\Pages\Inputs\MaterialsPage.xaml"
            ((SoilPro.Pages.Inputs.MaterialsPage)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Page_Loaded);
            
            #line default
            #line hidden
            
            #line 10 "..\..\..\..\..\Pages\Inputs\MaterialsPage.xaml"
            ((SoilPro.Pages.Inputs.MaterialsPage)(target)).Unloaded += new System.Windows.RoutedEventHandler(this.Page_Unloaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.view3d_main = ((System.Windows.Controls.Frame)(target));
            return;
            case 3:
            this.sideview_main = ((System.Windows.Controls.Frame)(target));
            return;
            case 4:
            this.concretewall_height = ((System.Windows.Controls.TextBox)(target));
            
            #line 45 "..\..\..\..\..\Pages\Inputs\MaterialsPage.xaml"
            this.concretewall_height.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.concretewall_height_TextChanged);
            
            #line default
            #line hidden
            
            #line 45 "..\..\..\..\..\Pages\Inputs\MaterialsPage.xaml"
            this.concretewall_height.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.concretewall_height_PreviewTextInput);
            
            #line default
            #line hidden
            
            #line 45 "..\..\..\..\..\Pages\Inputs\MaterialsPage.xaml"
            this.concretewall_height.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.concretewall_height_PreviewKeyDown);
            
            #line default
            #line hidden
            return;
            case 5:
            this.concretewall_height_unit = ((System.Windows.Controls.Label)(target));
            return;
            case 6:
            this.concretewall_thickness = ((System.Windows.Controls.TextBox)(target));
            
            #line 52 "..\..\..\..\..\Pages\Inputs\MaterialsPage.xaml"
            this.concretewall_thickness.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.concretewall_thickness_TextChanged);
            
            #line default
            #line hidden
            
            #line 52 "..\..\..\..\..\Pages\Inputs\MaterialsPage.xaml"
            this.concretewall_thickness.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.concretewall_thickness_PreviewTextInput);
            
            #line default
            #line hidden
            
            #line 52 "..\..\..\..\..\Pages\Inputs\MaterialsPage.xaml"
            this.concretewall_thickness.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.concretewall_thickness_PreviewKeyDown);
            
            #line default
            #line hidden
            return;
            case 7:
            this.concretewall_thickness_unit = ((System.Windows.Controls.Label)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

