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
    /// Interaction logic for PlaceShipView.xaml
    /// </summary>
    public partial class FieldView : UserControl
    {
        public Grid[] player;
        public FieldView()
        {
            InitializeComponent();
            player = new Grid[] { CellA1, CellA2, CellA3, CellA4, CellA5, CellA6, CellA7, CellA8, CellA9 };

        }

        private void Cell_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
