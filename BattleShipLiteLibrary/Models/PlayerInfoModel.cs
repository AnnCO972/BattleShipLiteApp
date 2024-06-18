using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipLiteLibrary.Models
{
    public class PlayerInfoModel
    {
        public string UsersName { get; set; }
        public List<GridSpotModel> ShotGrid { get; set; } = new List<GridSpotModel>();
        public List<ShipModel> Fleet {  get; set; } = new List<ShipModel>();
    }
}
