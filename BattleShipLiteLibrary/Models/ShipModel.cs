using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipLiteLibrary.Models
{
    public class ShipModel
    {
        public string ShipName { get; set; }
        public int Size {  get; set; }
        public List<GridSpotModel> Location { get; set; } = new List<GridSpotModel>();
        public ShipStatus Status { get; set; } = ShipStatus.Floating;
    }
}
