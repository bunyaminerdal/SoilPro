﻿#pragma checksum "..\..\..\..\Windows\PileWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "415CF333F94CE4690DA1AF5B54D5E606403AB098"
//------------------------------------------------------------------------------
// <auto-generated>
//     Bu kod araç tarafından oluşturuldu.
//     Çalışma Zamanı Sürümü:4.0.30319.42000
//
//     Bu dosyada yapılacak değişiklikler yanlış davranışa neden olabilir ve
//     kod yeniden oluşturulursa kaybolur.
// </auto-generated>
//------------------------------------------------------------------------------

using ExDesign.Windows;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
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


namespace ExDesign.Windows {
    
    
    /// <summary>
    /// PileWindow
    /// </summary>
    public partial class PileWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\..\..\Windows\PileWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox PileList;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\..\..\Windows\PileWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button deletepile_button;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\..\Windows\PileWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox pilenametextbox;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\..\Windows\PileWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox pilediametertextbox;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\..\Windows\PileWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label pilediameterunit;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\..\Windows\PileWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button cancel_button;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\..\Windows\PileWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button save_button;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\..\Windows\PileWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button addnewpile_button;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\..\Windows\PileWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button save_close_button;
        
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
            System.Uri resourceLocater = new System.Uri("/ExDesign;component/windows/pilewindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Windows\PileWindow.xaml"
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
            
            #line 8 "..\..\..\..\Windows\PileWindow.xaml"
            ((ExDesign.Windows.PileWindow)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.PileList = ((System.Windows.Controls.ListBox)(target));
            
            #line 10 "..\..\..\..\Windows\PileWindow.xaml"
            this.PileList.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.PileList_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.deletepile_button = ((System.Windows.Controls.Button)(target));
            
            #line 11 "..\..\..\..\Windows\PileWindow.xaml"
            this.deletepile_button.Click += new System.Windows.RoutedEventHandler(this.deletepile_button_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.pilenametextbox = ((System.Windows.Controls.TextBox)(target));
            
            #line 14 "..\..\..\..\Windows\PileWindow.xaml"
            this.pilenametextbox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.pilenametextbox_TextChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.pilediametertextbox = ((System.Windows.Controls.TextBox)(target));
            
            #line 17 "..\..\..\..\Windows\PileWindow.xaml"
            this.pilediametertextbox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.pilediametertextbox_TextChanged);
            
            #line default
            #line hidden
            
            #line 17 "..\..\..\..\Windows\PileWindow.xaml"
            this.pilediametertextbox.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.pilediametertextbox_PreviewTextInput);
            
            #line default
            #line hidden
            
            #line 17 "..\..\..\..\Windows\PileWindow.xaml"
            this.pilediametertextbox.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.pilediametertextbox_PreviewKeyDown);
            
            #line default
            #line hidden
            return;
            case 6:
            this.pilediameterunit = ((System.Windows.Controls.Label)(target));
            return;
            case 7:
            this.cancel_button = ((System.Windows.Controls.Button)(target));
            
            #line 19 "..\..\..\..\Windows\PileWindow.xaml"
            this.cancel_button.Click += new System.Windows.RoutedEventHandler(this.cancel_button_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.save_button = ((System.Windows.Controls.Button)(target));
            
            #line 20 "..\..\..\..\Windows\PileWindow.xaml"
            this.save_button.Click += new System.Windows.RoutedEventHandler(this.save_button_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.addnewpile_button = ((System.Windows.Controls.Button)(target));
            
            #line 21 "..\..\..\..\Windows\PileWindow.xaml"
            this.addnewpile_button.Click += new System.Windows.RoutedEventHandler(this.addnewpile_button_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.save_close_button = ((System.Windows.Controls.Button)(target));
            
            #line 22 "..\..\..\..\Windows\PileWindow.xaml"
            this.save_close_button.Click += new System.Windows.RoutedEventHandler(this.save_close_button_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
