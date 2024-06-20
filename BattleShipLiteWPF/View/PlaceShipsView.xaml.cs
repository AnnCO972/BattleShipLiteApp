using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BattleShipLiteWPF.View
{
    /// <summary>
    /// Interaction logic for PlaceShips.xaml
    /// </summary>
    public partial class PlaceShipsView : UserControl
    {
        public PlaceShipsView()
        {
            InitializeComponent();
            this.fieldView = new FieldView();
        }

        private void ship_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
