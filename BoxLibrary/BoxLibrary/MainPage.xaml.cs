using Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BoxLibrary
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Manager manager = new Manager();
        public MainPage()
        {
            this.InitializeComponent();
            AddBoxesForTesting();
            manager.CreateOrder(1, 1, 100, out var boxes);
            // do you want to buy this list {boxes}
            manager.BuyBox(boxes);
        }
        private void AddBoxesForTesting()
        {
            manager.AddBox(1, 1, 1);
            manager.AddBox(1, 2, 1);
            manager.AddBox(1, 3, 1);
            manager.AddBox(1, 4, 1);
            manager.AddBox(1, 5, 1);
            manager.AddBox(2, 1, 1);
            manager.AddBox(2, 2, 1);
            manager.AddBox(2, 3, 1);
            manager.AddBox(2, 4, 1);
            manager.AddBox(2, 5, 1);
            manager.AddBox(3, 5, 4);
            manager.AddBox(4, 5, 10);
        }
    }
}
