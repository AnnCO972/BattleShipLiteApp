using BattleShipLiteWPF.View;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BattleShipLiteWPF.ViewModel
{
    public partial class BattleShipViewModel  :ObservableObject
    {

        private ContentControl _mainWindow;

        public ContentControl mainWindow
        {
            get => _mainWindow; 
            set => SetProperty(ref _mainWindow, value); 
        }

        public BattleShipViewModel()
        {
          mainWindow = new ContentControl();
          mainWindow.Content = new StartView();
        }

    }
    
}
