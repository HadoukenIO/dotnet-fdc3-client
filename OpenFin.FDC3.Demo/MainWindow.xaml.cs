using OpenFin.FDC3.Channels;
using OpenFin.FDC3.Context;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OpenFin.FDC3.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        private bool contextChanging = false;
        private string nameAlias = "";
        private Identity windowIdentity = new Identity { Name = "fdc3-service", Uuid = "fdc3-Service" };

        public MainWindow()
        {
            InitializeComponent();

            FDC3.DesktopAgent.InitializationComplete = DesktopAgent_Initialized;
            FDC3.DesktopAgent.Initialize();
        }

        private async void DesktopAgent_Initialized(Exception ex)
        {
            //FDC3.DesktopAgent.runtimeInstance.DesktopConnection.sendAction("get-all-external-windows", null,
            //    ack =>
            //    {
            //        var obj = ack.getJsonObject();

            //        foreach(var item in obj["data"])
            //        {
            //            if (item["name"].ToString() == "OpenFin.FDC3.Demo")
            //            {
            //                windowIdentity.Uuid =  "";
            //                break;
            //            }
            //        }
            //        Console.WriteLine(obj);
            //    },
            //    nack =>
            //    {
            //        Console.WriteLine("blah");
            //    }, null);

            var channels = await FDC3.ContextChannels.GetDesktopChannelsAsync();
            windowIdentity.Name = windowIdentity.Uuid = FDC3.DesktopAgent.runtimeInstance.Options.UUID;

            Dispatcher.Invoke(() =>
            {
                foreach (var channel in channels)
                {
                    if (channel.ChannelType == ChannelTypes.Desktop)
                    {
                        ChannelListComboBox.Items.Add(new ComboBoxItem() { Content = channel.Name, Tag = channel });
                    }
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
            if (!contextChanging)
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
            var newChannel = (ChannelListComboBox.SelectedItem as ComboBoxItem).Tag as DesktopChannel;

            var newColor = Color.FromRgb(
                (byte)(newChannel.Color >> 16),
                (byte)((newChannel.Color >> 8) & 0xFF),
                (byte)(newChannel.Color & 0xFF));

            Dispatcher.Invoke(() =>
            {
                ChannelColorEllipse.Fill = new SolidColorBrush() { Color = newColor };
            });

            try
            {
                await newChannel.JoinAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}