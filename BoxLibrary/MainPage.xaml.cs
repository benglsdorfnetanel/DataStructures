using Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Models;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BoxLibrary
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Manager manager = new Manager();
        uint boxamount = 0;
        List<Box> boxes;
        uint _height, _amount, _width;
        public MainPage()
        {
            this.InitializeComponent();
            _height = _amount = _width = 0;
            yes.Visibility = Visibility.Collapsed;
            no.Visibility = Visibility.Collapsed;
            manager.delegateEEH += Manager_delegateEEH;
        }
        private void Manager_delegateEEH(object sender, ExpiredEventArgs EEA)
        {
            screen.Text = EEA.result;
        }
        private async void addBoxB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                manager.CheckInput(height_tb.Text, out _height);
                manager.CheckInput(width_tb.Text, out _width);
                manager.CheckInput(amount_tb.Text, out _amount);
            }
            catch
            {
                await new MessageDialog($"PLEASE ENTER NUMBERS ONLY!").ShowAsync();
            }

            manager.AddBox(_width, _height, _amount);
            screen.Text = $"Added {_amount} boxes of box {_width}x{_height}";
            height_tb.Text = "";
            amount_tb.Text = "";
            width_tb.Text = "";
        }
        private async void checkBoxB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                manager.CheckInput(height_tb.Text, out _height);
                manager.CheckInput(width_tb.Text, out _width);
                manager.CheckInput(amount_tb.Text, out _amount);
            }
            catch
            {
                await new MessageDialog($"PLEASE ENTER NUMBERS ONLY!").ShowAsync();
            }
            screen.Text = manager.ShowBoxes(_width, _height);
            height_tb.Text = "";
            amount_tb.Text = "";
            width_tb.Text = "";
        }
        private async void BuyBox_Click(object sender, RoutedEventArgs e)
        {
            screen.Text = "";
            try
            {
                manager.CheckInput(height_tb.Text, out _height);
                manager.CheckInput(width_tb.Text, out _width);
                manager.CheckInput(amount_tb.Text, out _amount);
            }
            catch
            {
                await new MessageDialog($"PLEASE ENTER NUMBERS ONLY!").ShowAsync();
            }
            manager.CreateOrder(_width, _height, _amount, out var boxes);
            foreach (var item in boxes)
            {
                screen.Text += $"box {item.X}X{item.Y} we have {item.Amount} \n";
                boxamount += item.Amount;
            }
            this.boxes = boxes;
            if (boxamount < _amount)
            {
                screen.Text += $"we found {boxamount}/{_amount} \n";
            }
            screen.Text += "you want to buy?";
            yes.Visibility = Visibility.Visible;
            no.Visibility = Visibility.Visible;

        }
        private void yes_Click(object sender, RoutedEventArgs e)
        {
            manager.BuyBox(boxes);
            yes.Visibility = Visibility.Collapsed;
            no.Visibility = Visibility.Collapsed;
        }
        private void no_Click(object sender, RoutedEventArgs e)
        {
            screen.Text = "";
            height_tb.Text = "";
            amount_tb.Text = "";
            width_tb.Text = "";
            yes.Visibility = Visibility.Collapsed;
            no.Visibility = Visibility.Collapsed;
        }

    }
}
