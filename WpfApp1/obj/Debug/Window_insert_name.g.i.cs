﻿#pragma checksum "..\..\Window_insert_name.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "CD33741619BD4A35A347FF86B4F6AA03992EE5F54B4A17EC764A21FC00F1DDC7"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ten kod został wygenerowany przez narzędzie.
//     Wersja wykonawcza:4.0.30319.42000
//
//     Zmiany w tym pliku mogą spowodować nieprawidłowe zachowanie i zostaną utracone, jeśli
//     kod zostanie ponownie wygenerowany.
// </auto-generated>
//------------------------------------------------------------------------------

using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Transitions;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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


namespace WpfApp1 {
    
    
    /// <summary>
    /// Window_insert_name
    /// </summary>
    public partial class Window_insert_name : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 2 "..\..\Window_insert_name.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal WpfApp1.Window_insert_name window;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\Window_insert_name.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock TextBlock_1;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\Window_insert_name.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TextBox_1;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\Window_insert_name.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Button_1;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\Window_insert_name.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image image;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Raport Maker V3;component/window_insert_name.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Window_insert_name.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.window = ((WpfApp1.Window_insert_name)(target));
            
            #line 20 "..\..\Window_insert_name.xaml"
            this.window.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Window_MouseDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.TextBlock_1 = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.TextBox_1 = ((System.Windows.Controls.TextBox)(target));
            
            #line 31 "..\..\Window_insert_name.xaml"
            this.TextBox_1.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.TextBox_1_TextChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.Button_1 = ((System.Windows.Controls.Button)(target));
            
            #line 36 "..\..\Window_insert_name.xaml"
            this.Button_1.Click += new System.Windows.RoutedEventHandler(this.Button_1_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 38 "..\..\Window_insert_name.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Close_Window);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 41 "..\..\Window_insert_name.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Minimalize_Window);
            
            #line default
            #line hidden
            return;
            case 7:
            this.image = ((System.Windows.Controls.Image)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

