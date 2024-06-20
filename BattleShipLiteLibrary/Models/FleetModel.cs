using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipLiteLibrary.Models
{
    public class FleetModel
    {
        public List<ShipModel> Ships { get; set; } = new List<ShipModel>();
    }
}
