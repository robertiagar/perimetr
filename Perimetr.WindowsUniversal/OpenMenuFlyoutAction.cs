using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Perimetr.WindowsUniversal
{
    public class OpenMenuFlyoutAction : DependencyObject, IAction
    {
        public object Execute(object sender, object parameter)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);

            flyoutBase.ShowAt(senderElement);

            return null;
        }
    }

    public class OpenStackPanel : DependencyObject, IAction
    {
        public object Execute(object sender, object parameter)
        {
            var panel = ElementToPlayWith as UIElement;
            switch (panel.Visibility)
            {
                case Visibility.Visible:
                    panel.Visibility = Visibility.Collapsed;
                    break;
                case Visibility.Collapsed:
                    panel.Visibility = Visibility.Visible;
                    break;
                default:
                    panel.Visibility = Visibility.Visible;
                    break;
            }

            return null;
        }

        public object ElementToPlayWith
        {
            get { return (object)GetValue(ElementToPlayWithProperty); }
            set { SetValue(ElementToPlayWithProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ElementToShow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ElementToPlayWithProperty =
            DependencyProperty.Register("ElementToPlayWith", typeof(object), typeof(OpenStackPanel), new PropertyMetadata(0));


    }
}
