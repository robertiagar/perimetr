using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Perimetr.WindowsUniversal.Messages
{
    public class NavigationMessage
    {
        public NavigationMessage(Type DestinationPage, object parameters)
        {
            this.DestinationPage = DestinationPage;
            this.Arguments = parameters;
        }

        public Type DestinationPage { get; private set; }
        public object Arguments { get; private set; }
    }
}
