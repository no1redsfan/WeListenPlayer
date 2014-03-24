using System.Windows;
using System.Windows.Input;

namespace WeListenPlayer.Control.General
{
    /// <summary>
    /// Interaction logic for WindowHeaderControl.xaml
    /// </summary>
    public partial class WindowHeaderControl
    {
        #region Dependency properties

        public static readonly DependencyProperty CanMaximizeProperty = DependencyProperty.Register ("CanMaximize", typeof (bool), typeof (WindowHeaderControl), new FrameworkPropertyMetadata (true));
        public static readonly DependencyProperty CanMinimizeProperty = DependencyProperty.Register ("CanMinimize", typeof (bool), typeof (WindowHeaderControl), new FrameworkPropertyMetadata (true));

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

        #region Constructor

        public WindowHeaderControl ()
        {
            InitializeComponent ();
        }

        #endregion

        #region Button events

        /// <summary>
        /// Closes window that contains the control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButtonMouseLeftButtonUp (object sender, MouseButtonEventArgs e)
        {
            Window uiWindow = Window.GetWindow (this);

            if (uiWindow == null)
            {
                return;
            }

            uiWindow.Close ();
        }

        /// <summary>
        /// Maximizes the window that contains the control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MaximizeButtonMouseLeftButtonUp (object sender, MouseButtonEventArgs e)
        {
            //
            // Get parent from WindowControl
            //

            Window uiWindow = Window.GetWindow (this);

            if (uiWindow == null)
            {
                return;
            }

            if (uiWindow.WindowState == WindowState.Normal)
            {
                uiWindow.WindowState = WindowState.Maximized;
                MaximizeButton.Text = "2";
            }
            else
            {
                uiWindow.WindowState = WindowState.Normal;
                MaximizeButton.Text = "1";
            }
        }

        /// <summary>
        /// Minimizes the Window that contains the control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MinimizeButtonMouseLeftButtonUp (object sender, MouseButtonEventArgs e)
        {
            Window uiWindow = Window.GetWindow (this);

            if (uiWindow == null)
            {
                return;
            }

            uiWindow.WindowState = WindowState.Minimized;
        }

        #endregion
    }
}