﻿#pragma checksum "..\..\..\..\..\Pages\Inputs\InputsMainPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "320857332BAE8F952ED4F65B20D437AFF6FE1D98"
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
    /// InputsMainPage
    /// </summary>
    public partial class InputsMainPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 14 "..\..\..\..\..\Pages\Inputs\InputsMainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RowDefinition MenuRow;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\..\..\Pages\Inputs\InputsMainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RowDefinition MainRow;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\..\..\Pages\Inputs\InputsMainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RowDefinition StatusRow;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\..\..\Pages\Inputs\InputsMainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox UnitCombobox;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\..\..\Pages\Inputs\InputsMainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton MaterialsBttn;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\..\..\Pages\Inputs\InputsMainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton ExDesignBttn;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\..\..\Pages\Inputs\InputsMainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton SolidMethodBttn;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\..\..\Pages\Inputs\InputsMainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Frame Main_pro;
        
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
            System.Uri resourceLocater = new System.Uri("/SoilPro;component/pages/inputs/inputsmainpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Pages\Inputs\InputsMainPage.xaml"
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
            
            #line 9 "..\..\..\..\..\Pages\Inputs\InputsMainPage.xaml"
            ((SoilPro.Pages.Inputs.InputsMainPage)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Page_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.MenuRow = ((System.Windows.Controls.RowDefinition)(target));
            return;
            case 3:
            this.MainRow = ((System.Windows.Controls.RowDefinition)(target));
            return;
            case 4:
            this.StatusRow = ((System.Windows.Controls.RowDefinition)(target));
            return;
            case 5:
            this.UnitCombobox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 20 "..\..\..\..\..\Pages\Inputs\InputsMainPage.xaml"
            this.UnitCombobox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.UnitCombobox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            this.MaterialsBttn = ((System.Windows.Controls.RadioButton)(target));
            
            #line 39 "..\..\..\..\..\Pages\Inputs\InputsMainPage.xaml"
            this.MaterialsBttn.Checked += new System.Windows.RoutedEventHandler(this.MaterialsBttn_Checked);
            
            #line default
            #line hidden
            return;
            case 7:
            this.ExDesignBttn = ((System.Windows.Controls.RadioButton)(target));
            
            #line 40 "..\..\..\..\..\Pages\Inputs\InputsMainPage.xaml"
            this.ExDesignBttn.Checked += new System.Windows.RoutedEventHandler(this.ExDesignBttn_Checked);
            
            #line default
            #line hidden
            return;
            case 8:
            this.SolidMethodBttn = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 9:
            this.Main_pro = ((System.Windows.Controls.Frame)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

