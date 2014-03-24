using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace WeListenPlayer.Control.General
{
    public class CustomWindow : Window
    {
        #region Dependency Properties
        public static readonly DependencyProperty CanMaximizeProperty = DependencyProperty.Register("CanMaximize", typeof(bool), typeof(CustomWindow), new FrameworkPropertyMetadata(true));
        public static readonly DependencyProperty CanMinimizeProperty = DependencyProperty.Register("CanMinimize", typeof(bool), typeof(CustomWindow), new FrameworkPropertyMetadata(true));
        #endregion

        #region Fields
        private TextBlock _headerTitle;

        #endregion

        #region Constructor

        static CustomWindow ()
        {
            DefaultStyleKeyProperty.OverrideMetadata (typeof (CustomWindow), new FrameworkPropertyMetadata (typeof (CustomWindow)));
        }

        #endregion

        #region Properties

        public bool CanMaximize
        {
            get
            {
                return (bool)GetValue(CanMaximizeProperty);
            }
            set
            {
                SetValue(CanMaximizeProperty, value);
            }
        }

        public bool CanMinimize
        {
            get
            {
                return (bool)GetValue(CanMinimizeProperty);
            }
            set
            {
                SetValue(CanMinimizeProperty, value);
            }
        }

        #endregion

        #region Overrides

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _headerTitle = GetTemplateChild ("PART_Title") as TextBlock;

            if (_headerTitle != null)
            {
                _headerTitle.MouseDown += HeaderMouseDown;
            }
        }

        #endregion

        #region Private methods

        private void HeaderMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        #endregion
    }
}
