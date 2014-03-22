﻿#pragma checksum "..\..\..\MainWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "FCC52C743EBA86D2E4FB4DAD3D402A78"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18449
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using AvalonDock;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
using VisualizationLib;
using WeListenPlayer.Control.General;


namespace WeListenPlayer {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 31 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSkipBck;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button PlayButton;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button PauseButton;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button StopButton;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSkipFwd;
        
        #line default
        #line hidden
        
        
        #line 87 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Menu MainMenu;
        
        #line default
        #line hidden
        
        
        #line 90 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem OpenFileMenuItem;
        
        #line default
        #line hidden
        
        
        #line 94 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem CloseMenuItem;
        
        #line default
        #line hidden
        
        
        #line 98 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem DefaultThemeMenuItem;
        
        #line default
        #line hidden
        
        
        #line 102 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem ExpressionDarkMenuItem;
        
        #line default
        #line hidden
        
        
        #line 106 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem ExpressionLightMenuItem;
        
        #line default
        #line hidden
        
        
        #line 111 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem SoundSelect;
        
        #line default
        #line hidden
        
        
        #line 119 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal AvalonDock.DockingManager dockManager;
        
        #line default
        #line hidden
        
        
        #line 125 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal AvalonDock.DockableContent content_playlist;
        
        #line default
        #line hidden
        
        
        #line 142 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnStartReceiving;
        
        #line default
        #line hidden
        
        
        #line 145 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgvPlayList;
        
        #line default
        #line hidden
        
        
        #line 217 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridTextColumn Title;
        
        #line default
        #line hidden
        
        
        #line 218 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridTextColumn Artist;
        
        #line default
        #line hidden
        
        
        #line 222 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAddSongs;
        
        #line default
        #line hidden
        
        
        #line 223 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnImport;
        
        #line default
        #line hidden
        
        
        #line 225 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnMoveUp;
        
        #line default
        #line hidden
        
        
        #line 236 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnRemoveSong;
        
        #line default
        #line hidden
        
        
        #line 238 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnMoveDown;
        
        #line default
        #line hidden
        
        
        #line 244 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal AvalonDock.DockableContent content_db;
        
        #line default
        #line hidden
        
        
        #line 256 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnUploadFolderToDB;
        
        #line default
        #line hidden
        
        
        #line 259 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblProgress;
        
        #line default
        #line hidden
        
        
        #line 260 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblProgressCount;
        
        #line default
        #line hidden
        
        
        #line 261 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblAdded;
        
        #line default
        #line hidden
        
        
        #line 262 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblAddedCount;
        
        #line default
        #line hidden
        
        
        #line 263 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblRejected;
        
        #line default
        #line hidden
        
        
        #line 264 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblRejectedCount;
        
        #line default
        #line hidden
        
        
        #line 274 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal AvalonDock.DockablePane pane_main;
        
        #line default
        #line hidden
        
        
        #line 291 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal VisualizationLib.AlbumArtDisplay albumArtPanel;
        
        #line default
        #line hidden
        
        
        #line 295 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal VisualizationLib.SpectrumAnalyzer spectrumAnalyzer;
        
        #line default
        #line hidden
        
        
        #line 306 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider sldrVolume;
        
        #line default
        #line hidden
        
        
        #line 317 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal VisualizationLib.DigitalClock clockDisplay;
        
        #line default
        #line hidden
        
        
        #line 340 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtUserName;
        
        #line default
        #line hidden
        
        
        #line 341 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbUserName;
        
        #line default
        #line hidden
        
        
        #line 342 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtPassword;
        
        #line default
        #line hidden
        
        
        #line 343 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox pwbPassword;
        
        #line default
        #line hidden
        
        
        #line 344 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSubmit;
        
        #line default
        #line hidden
        
        
        #line 345 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnRegisterLink;
        
        #line default
        #line hidden
        
        
        #line 354 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal AvalonDock.DockablePane subMain_songInfo;
        
        #line default
        #line hidden
        
        
        #line 370 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbTitle_Copy1;
        
        #line default
        #line hidden
        
        
        #line 371 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbArtist_Copy1;
        
        #line default
        #line hidden
        
        
        #line 372 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbAlbum_Copy1;
        
        #line default
        #line hidden
        
        
        #line 373 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbYear_Copy1;
        
        #line default
        #line hidden
        
        
        #line 374 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbGenre_Copy1;
        
        #line default
        #line hidden
        
        
        #line 375 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbPath_Copy1;
        
        #line default
        #line hidden
        
        
        #line 376 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbAmazonTitleInfo;
        
        #line default
        #line hidden
        
        
        #line 377 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbAmazonArtistInfo;
        
        #line default
        #line hidden
        
        
        #line 378 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbAmazonAlbumInfo;
        
        #line default
        #line hidden
        
        
        #line 379 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbAmazonYearInfo;
        
        #line default
        #line hidden
        
        
        #line 380 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbAmazonAsinInfo;
        
        #line default
        #line hidden
        
        
        #line 381 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbAmazonPriceInfo;
        
        #line default
        #line hidden
        
        
        #line 386 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal AvalonDock.DockablePane subMain_waveform;
        
        #line default
        #line hidden
        
        
        #line 391 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal VisualizationLib.WaveformTimeline waveformTimeline;
        
        #line default
        #line hidden
        
        
        #line 407 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal AvalonDock.DockablePane pane_module;
        
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
            System.Uri resourceLocater = new System.Uri("/WeListenPlayer;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
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
            
            #line 8 "..\..\..\MainWindow.xaml"
            ((WeListenPlayer.MainWindow)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.Window_Closed);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 21 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.Border)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.BorderMouseDown);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btnSkipBck = ((System.Windows.Controls.Button)(target));
            
            #line 39 "..\..\..\MainWindow.xaml"
            this.btnSkipBck.Click += new System.Windows.RoutedEventHandler(this.StopButton_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.PlayButton = ((System.Windows.Controls.Button)(target));
            
            #line 55 "..\..\..\MainWindow.xaml"
            this.PlayButton.Click += new System.Windows.RoutedEventHandler(this.PlayButton_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.PauseButton = ((System.Windows.Controls.Button)(target));
            
            #line 63 "..\..\..\MainWindow.xaml"
            this.PauseButton.Click += new System.Windows.RoutedEventHandler(this.PauseButton_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.StopButton = ((System.Windows.Controls.Button)(target));
            
            #line 71 "..\..\..\MainWindow.xaml"
            this.StopButton.Click += new System.Windows.RoutedEventHandler(this.StopButton_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.btnSkipFwd = ((System.Windows.Controls.Button)(target));
            
            #line 81 "..\..\..\MainWindow.xaml"
            this.btnSkipFwd.Click += new System.Windows.RoutedEventHandler(this.OnForwardClick);
            
            #line default
            #line hidden
            return;
            case 8:
            this.MainMenu = ((System.Windows.Controls.Menu)(target));
            return;
            case 9:
            this.OpenFileMenuItem = ((System.Windows.Controls.MenuItem)(target));
            
            #line 91 "..\..\..\MainWindow.xaml"
            this.OpenFileMenuItem.Click += new System.Windows.RoutedEventHandler(this.OpenFileMenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.CloseMenuItem = ((System.Windows.Controls.MenuItem)(target));
            
            #line 95 "..\..\..\MainWindow.xaml"
            this.CloseMenuItem.Click += new System.Windows.RoutedEventHandler(this.CloseMenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            this.DefaultThemeMenuItem = ((System.Windows.Controls.MenuItem)(target));
            
            #line 101 "..\..\..\MainWindow.xaml"
            this.DefaultThemeMenuItem.Checked += new System.Windows.RoutedEventHandler(this.DefaultThemeMenuItem_Checked);
            
            #line default
            #line hidden
            return;
            case 12:
            this.ExpressionDarkMenuItem = ((System.Windows.Controls.MenuItem)(target));
            
            #line 105 "..\..\..\MainWindow.xaml"
            this.ExpressionDarkMenuItem.Checked += new System.Windows.RoutedEventHandler(this.ExpressionDarkMenuItem_Checked);
            
            #line default
            #line hidden
            return;
            case 13:
            this.ExpressionLightMenuItem = ((System.Windows.Controls.MenuItem)(target));
            
            #line 109 "..\..\..\MainWindow.xaml"
            this.ExpressionLightMenuItem.Checked += new System.Windows.RoutedEventHandler(this.ExpressionLightMenuItem_Checked);
            
            #line default
            #line hidden
            return;
            case 14:
            this.SoundSelect = ((System.Windows.Controls.MenuItem)(target));
            return;
            case 15:
            this.dockManager = ((AvalonDock.DockingManager)(target));
            return;
            case 16:
            this.content_playlist = ((AvalonDock.DockableContent)(target));
            return;
            case 17:
            this.btnStartReceiving = ((System.Windows.Controls.Button)(target));
            
            #line 142 "..\..\..\MainWindow.xaml"
            this.btnStartReceiving.Click += new System.Windows.RoutedEventHandler(this.OnRecieveRequestClick);
            
            #line default
            #line hidden
            return;
            case 18:
            this.dgvPlayList = ((System.Windows.Controls.DataGrid)(target));
            
            #line 160 "..\..\..\MainWindow.xaml"
            this.dgvPlayList.Drop += new System.Windows.DragEventHandler(this.dgvPlayList_Drop);
            
            #line default
            #line hidden
            
            #line 161 "..\..\..\MainWindow.xaml"
            this.dgvPlayList.DragOver += new System.Windows.DragEventHandler(this.dgvPlayList_DragOver);
            
            #line default
            #line hidden
            
            #line 162 "..\..\..\MainWindow.xaml"
            this.dgvPlayList.DragLeave += new System.Windows.DragEventHandler(this.dgvPlayList_DragLeave);
            
            #line default
            #line hidden
            return;
            case 19:
            this.Title = ((System.Windows.Controls.DataGridTextColumn)(target));
            return;
            case 20:
            this.Artist = ((System.Windows.Controls.DataGridTextColumn)(target));
            return;
            case 21:
            this.btnAddSongs = ((System.Windows.Controls.Button)(target));
            
            #line 222 "..\..\..\MainWindow.xaml"
            this.btnAddSongs.Click += new System.Windows.RoutedEventHandler(this.fileSelector);
            
            #line default
            #line hidden
            return;
            case 22:
            this.btnImport = ((System.Windows.Controls.Button)(target));
            
            #line 223 "..\..\..\MainWindow.xaml"
            this.btnImport.Click += new System.Windows.RoutedEventHandler(this.dirSelector);
            
            #line default
            #line hidden
            return;
            case 23:
            this.btnMoveUp = ((System.Windows.Controls.Button)(target));
            
            #line 225 "..\..\..\MainWindow.xaml"
            this.btnMoveUp.Click += new System.Windows.RoutedEventHandler(this.OnMoveUpClick);
            
            #line default
            #line hidden
            return;
            case 24:
            this.btnRemoveSong = ((System.Windows.Controls.Button)(target));
            
            #line 236 "..\..\..\MainWindow.xaml"
            this.btnRemoveSong.Click += new System.Windows.RoutedEventHandler(this.OnRemoveSongClick);
            
            #line default
            #line hidden
            return;
            case 25:
            this.btnMoveDown = ((System.Windows.Controls.Button)(target));
            
            #line 238 "..\..\..\MainWindow.xaml"
            this.btnMoveDown.Click += new System.Windows.RoutedEventHandler(this.OnMoveDownClick);
            
            #line default
            #line hidden
            return;
            case 26:
            this.content_db = ((AvalonDock.DockableContent)(target));
            return;
            case 27:
            this.btnUploadFolderToDB = ((System.Windows.Controls.Button)(target));
            
            #line 257 "..\..\..\MainWindow.xaml"
            this.btnUploadFolderToDB.Click += new System.Windows.RoutedEventHandler(this.OnUploadFolderToDbClick);
            
            #line default
            #line hidden
            return;
            case 28:
            this.lblProgress = ((System.Windows.Controls.Label)(target));
            return;
            case 29:
            this.lblProgressCount = ((System.Windows.Controls.Label)(target));
            return;
            case 30:
            this.lblAdded = ((System.Windows.Controls.Label)(target));
            return;
            case 31:
            this.lblAddedCount = ((System.Windows.Controls.Label)(target));
            return;
            case 32:
            this.lblRejected = ((System.Windows.Controls.Label)(target));
            return;
            case 33:
            this.lblRejectedCount = ((System.Windows.Controls.Label)(target));
            return;
            case 34:
            this.pane_main = ((AvalonDock.DockablePane)(target));
            return;
            case 35:
            this.albumArtPanel = ((VisualizationLib.AlbumArtDisplay)(target));
            return;
            case 36:
            this.spectrumAnalyzer = ((VisualizationLib.SpectrumAnalyzer)(target));
            return;
            case 37:
            this.sldrVolume = ((System.Windows.Controls.Slider)(target));
            
            #line 312 "..\..\..\MainWindow.xaml"
            this.sldrVolume.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.OnSldrVolumeChanged);
            
            #line default
            #line hidden
            return;
            case 38:
            this.clockDisplay = ((VisualizationLib.DigitalClock)(target));
            return;
            case 39:
            this.txtUserName = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 40:
            this.tbUserName = ((System.Windows.Controls.TextBox)(target));
            return;
            case 41:
            this.txtPassword = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 42:
            this.pwbPassword = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 43:
            this.btnSubmit = ((System.Windows.Controls.Button)(target));
            
            #line 344 "..\..\..\MainWindow.xaml"
            this.btnSubmit.Click += new System.Windows.RoutedEventHandler(this.btnSubmit_Click);
            
            #line default
            #line hidden
            return;
            case 44:
            this.btnRegisterLink = ((System.Windows.Controls.Button)(target));
            
            #line 345 "..\..\..\MainWindow.xaml"
            this.btnRegisterLink.Click += new System.Windows.RoutedEventHandler(this.btnRegisterLink_Click);
            
            #line default
            #line hidden
            return;
            case 45:
            this.subMain_songInfo = ((AvalonDock.DockablePane)(target));
            return;
            case 46:
            this.tbTitle_Copy1 = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 47:
            this.tbArtist_Copy1 = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 48:
            this.tbAlbum_Copy1 = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 49:
            this.tbYear_Copy1 = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 50:
            this.tbGenre_Copy1 = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 51:
            this.tbPath_Copy1 = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 52:
            this.tbAmazonTitleInfo = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 53:
            this.tbAmazonArtistInfo = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 54:
            this.tbAmazonAlbumInfo = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 55:
            this.tbAmazonYearInfo = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 56:
            this.tbAmazonAsinInfo = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 57:
            this.tbAmazonPriceInfo = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 58:
            this.subMain_waveform = ((AvalonDock.DockablePane)(target));
            return;
            case 59:
            this.waveformTimeline = ((VisualizationLib.WaveformTimeline)(target));
            return;
            case 60:
            this.pane_module = ((AvalonDock.DockablePane)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

