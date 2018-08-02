using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Windows.Controls.Primitives;

namespace InfrastructureLight.Wpf.Common.Notify
{
    public abstract class NotifyWindowBase : Window
    {
        private DoubleAnimation _fadeInAnimation;
        private DoubleAnimation _fadeOutAnimation;
        private DispatcherTimer _activeTimer;

        static NotifyWindowBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NotifyWindowBase), 
                new FrameworkPropertyMetadata(typeof(NotifyWindowBase)));
        }

        protected NotifyWindowBase()
        {
            Visibility = Visibility.Visible;            
            Width = 350;
            Height = 75;            
            ShowInTaskbar = false;
            WindowStyle = WindowStyle.None;
            ResizeMode = ResizeMode.NoResize;
            Topmost = true;
            AllowsTransparency = true;
            Opacity = 0.8;
            BorderThickness = new Thickness(1);
            BorderBrush = Brushes.Black;
            Background = Brushes.White;
            ShowActivated = false;                        

            _fadeInAnimation = new DoubleAnimation();
            _fadeInAnimation.From = 0;
            _fadeInAnimation.To = 0.8;
            _fadeInAnimation.Duration = new Duration(TimeSpan.Parse("0:0:1.5"));
            
            _fadeOutAnimation = new DoubleAnimation();
            _fadeOutAnimation.To = 0;
            _fadeOutAnimation.Duration = new Duration(TimeSpan.Parse("0:0:1.5"));

            Loaded += new RoutedEventHandler(NotifyWindowBase_Loaded);
        }
        
        public override void OnApplyTemplate()
        {
            ButtonBase closeButton = Template.FindName("PART_CloseButton", this) as ButtonBase;
            if (closeButton != null)
                closeButton.Click += new RoutedEventHandler(closeButton_Click);
        }

        #region Private

        void NotifyWindowBase_Loaded(object sender, RoutedEventArgs e)
        {
            var workAreaRectangle = SystemParameters.WorkArea;
            Left = workAreaRectangle.Right - Width - BorderThickness.Right;
            Top = workAreaRectangle.Bottom - Height - BorderThickness.Bottom;

            _fadeInAnimation.Completed += new EventHandler(_fadeInAnimation_Completed);
            BeginAnimation(OpacityProperty, _fadeInAnimation);
        }
        void _fadeInAnimation_Completed(object sender, EventArgs e)
        {
            _activeTimer = new DispatcherTimer();
            _activeTimer.Interval = TimeSpan.Parse("0:0:10");
            _activeTimer.Tick += delegate (object obj, EventArgs ea) { FadeOut(); };
            _activeTimer.Start();
        }
        void FadeOut()
        {
            _fadeOutAnimation.Completed += delegate (object sender, EventArgs e) { Close(); };
            BeginAnimation(OpacityProperty, _fadeOutAnimation, HandoffBehavior.SnapshotAndReplace);
        }
        void closeButton_Click(object sender, RoutedEventArgs e)
        {
            _activeTimer.Stop();
            FadeOut();
        }

        #endregion
    }
}
