﻿#pragma checksum "..\..\..\..\..\Views\BrigadeWindow\BrigadeWindowView.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "A46984711F1FDDA8FF2FCB6916754E6C6B907025"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
using Microsoft.VisualBasic;
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
using TheBureau.Controls;
using TheBureau.ViewModels;


namespace TheBureau.Views {
    
    
    /// <summary>
    /// BrigadeWindowView
    /// </summary>
    public partial class BrigadeWindowView : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 17 "..\..\..\..\..\Views\BrigadeWindow\BrigadeWindowView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid TopGrid;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\..\..\Views\BrigadeWindow\BrigadeWindowView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DockPanel MainWindowTop;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\..\..\Views\BrigadeWindow\BrigadeWindowView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button MainWindowClose;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\..\..\Views\BrigadeWindow\BrigadeWindowView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button MainWindowMinimize;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\..\..\Views\BrigadeWindow\BrigadeWindowView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DockPanel MainWorkspace;
        
        #line default
        #line hidden
        
        
        #line 59 "..\..\..\..\..\Views\BrigadeWindow\BrigadeWindowView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox A;
        
        #line default
        #line hidden
        
        
        #line 84 "..\..\..\..\..\Views\BrigadeWindow\BrigadeWindowView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.ToggleButton InfoToggle;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.12.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/TheBureau;component/views/brigadewindow/brigadewindowview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Views\BrigadeWindow\BrigadeWindowView.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.12.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.TopGrid = ((System.Windows.Controls.Grid)(target));
            
            #line 17 "..\..\..\..\..\Views\BrigadeWindow\BrigadeWindowView.xaml"
            this.TopGrid.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.TopGrid_OnMouseDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.MainWindowTop = ((System.Windows.Controls.DockPanel)(target));
            return;
            case 3:
            this.MainWindowClose = ((System.Windows.Controls.Button)(target));
            return;
            case 4:
            this.MainWindowMinimize = ((System.Windows.Controls.Button)(target));
            return;
            case 5:
            this.MainWorkspace = ((System.Windows.Controls.DockPanel)(target));
            return;
            case 6:
            this.A = ((System.Windows.Controls.ListBox)(target));
            return;
            case 7:
            this.InfoToggle = ((System.Windows.Controls.Primitives.ToggleButton)(target));
            return;
            case 8:
            
            #line 90 "..\..\..\..\..\Views\BrigadeWindow\BrigadeWindowView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

