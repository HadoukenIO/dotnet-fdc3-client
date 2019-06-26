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
using OpenFin.FDC3.Context;
using FDC3 = OpenFin.FDC3;

namespace OpenFin.FDC3.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool contextChanging = false;

        public MainWindow()
        {
            InitializeComponent();

            FDC3.DesktopAgent.InitializationComplete = DesktopAgent_Initialized;
            FDC3.DesktopAgent.Initialize();
        }

        private async void DesktopAgent_Initialized(Exception ex)
        {
            var channels = await FDC3.ContextChannels.Instance.GetAllChannelsAsync();

            Dispatcher.Invoke(() =>
            {
                foreach (var channel in channels)
                {
                    ChannelListComboBox.Items.Add(new ComboBoxItem() { Content = channel.Name, Tag = channel });
                }

                ChannelListComboBox.SelectedValue = "Global";
            });

            FDC3.DesktopAgent.AddContextListener(ContextChanged);
        }

        private void ContextChanged(ContextBase obj)
        {
            Dispatcher.Invoke(() =>
            {
                contextChanging = true;
                TickerComboBox.SelectedValue = obj.Name;
                contextChanging = false;
            });
        }

        private async void TickerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(!contextChanging)
            {
                var newTicker = TickerComboBox.SelectedValue as string;
                var context = new SecurityContext()
                {
                    Name = newTicker,
                    Id = new Dictionary<string, string>()
                    {
                        { "default", newTicker }
                    }
                };

                try
                {
                    await FDC3.DesktopAgent.Broadcast(context);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        private async void ChannelListComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var newChannel = (ChannelListComboBox.SelectedItem as ComboBoxItem).Tag as Channels.Channel;

            var newColor = Color.FromRgb(
                (byte) (newChannel.Color >> 16),
                (byte) ((newChannel.Color >> 8) & 0xFF),
                (byte) (newChannel.Color & 0xFF));

            Dispatcher.Invoke(() =>
            {
                ChannelColorEllipse.Fill = new SolidColorBrush() { Color = newColor };
            });

            try
            {
                await FDC3.ContextChannels.Instance.JoinChannelAsync(newChannel.Id);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
