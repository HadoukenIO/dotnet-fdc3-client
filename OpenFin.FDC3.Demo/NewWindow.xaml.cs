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
    /// Interaction logic for SecondWindow.xaml
    /// </summary>
    public partial class NewWindow : Window
    {
        private bool contextChanging = false;
        private Connection connection;

        public NewWindow()
        {
            InitializeComponent();
            this.Loaded += (object sender, RoutedEventArgs e) => initialize();
            this.Closed += async (s, e) => await connection.DisconnectAsync();
        }

        private async void initialize()
        {
            var connectionID = Guid.NewGuid().ToString();
            this.Title = $"ConnectionID : {connectionID}";
            connection = await ConnectionManager.CreateConnectionAsync(connectionID);
            connection.AddContextHandler(ContextChanged);

            await Dispatcher.InvokeAsync(async () =>
            {
                tbAppId.Text = FDC3.Uuid;

                var channels = await connection.GetSystemChannelsAsync();
                foreach (var channel in channels)
                {
                    if (channel.ChannelType == ChannelType.System)
                    {
                        ChannelListComboBox.Items.Add(new ComboBoxItem() { Content = channel.VisualIdentity.Name, Tag = channel });
                    }
                }

                ChannelListComboBox.SelectedValue = "Default";
            });
        }

        private void ContextChanged(ContextBase obj)
        {
            Dispatcher.Invoke(() =>
            {
                contextChanging = true;
                TickerComboBox.SelectedValue = obj.Id["ticker"];
                contextChanging = false;
            });
        }

        private async void TickerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!contextChanging)
            {
                var newTicker = TickerComboBox.SelectedValue as string;
                var context = new InstrumentContext()
                {
                    Name = newTicker,
                    Id = new Dictionary<string, string>()
                    {
                        { "default", newTicker },
                        { "ticker", newTicker }
                    }
                };

                try
                {

                    await connection.BroadcastAsync(context);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private async void ChannelListComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SystemChannel newChannel = (ChannelListComboBox.SelectedItem as ComboBoxItem).Tag as SystemChannel;

            // 'Color' can technically be any CSS colour string
            // For now, we assume that it'll always be in "#000000" format
            int color = Int32.Parse(newChannel.VisualIdentity.Color.Substring(1), System.Globalization.NumberStyles.HexNumber);
            var newColor = Color.FromRgb(
                (byte)((color >> 16) & 0xFF),
                (byte)((color >> 8) & 0xFF),
                (byte)(color & 0xFF));

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