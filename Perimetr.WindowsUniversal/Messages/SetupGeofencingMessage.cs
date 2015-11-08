using Perimetr.WindowsUniversal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perimetr.WindowsUniversal.Messages
{
    public class SetupGeofencingMessage
    {
        public SetupGeofencingMessage(IEnumerable<MapFriendViewModel> mapFriendViewModel)
        {
            this.MapFriendViewModel = mapFriendViewModel;
        }

        public IEnumerable<MapFriendViewModel> MapFriendViewModel { get; private set; }
    }
}
