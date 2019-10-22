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
    public partial class MainWindow : Window
    {
        private bool contextChanging = false;
        private Connection connection;

        public MainWindow()
        {
            InitializeComponent();
            AppDomain.CurrentDomain.UnhandledException += (s, e) => { MessageBox.Show(e.ExceptionObject.ToString()); };
            FDC3.OnInitialized += initialized;
#if DEBUG            
            FDC3.Initialize($"{System.IO.Directory.GetCurrentDirectory()}\\app.json");
#else
            FDC3.Initialize($"{System.IO.Directory.GetCurrentDirectory()}\\app.release.json");
#endif
            this.Closed += async (s, e) => await connection.DisconnectAsync();
        }

        private async void initialized()
        {
            connection = await ConnectionManager.CreateConnectionAsync("mainwin");
            connection.AddContextHandler(ContextChanged);

            await Dispatcher.InvokeAsync(async () =>
            {
                tbAppId.Text = FDC3.Uuid;
                btnLaunch.IsEnabled = true;

                try
                {

                    var channels = await connection.GetSystemChannelsAsync();
                    ChannelBase defaultChannel = await connection.GetChannelByIdAsync("default");
                    ChannelListComboBox.Items.Add(new ComboBoxItem() { Content = "Default", Tag = defaultChannel });
                    foreach (var channel in channels)
                    {
                        if (channel.ChannelType == ChannelType.System)
                        {
                            ChannelListComboBox.Items.Add(new ComboBoxItem() { Content = channel.VisualIdentity.Name, Tag = channel });
                        }
                    }

                    ChannelListComboBox.SelectedValue = "Default";
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            });

        }

        private void ContextChanged(ContextBase obj)
        {
            Dispatcher.Invoke(() =>
            {
                contextChanging = true;
                if (obj.Id.ContainsKey("ticker"))
                {
                    TickerComboBox.SelectedValue = obj.Id["ticker"];
                }
                else
                {
                    TickerComboBox.SelectedValue = obj.Name;
                }
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
            ChannelBase newChannel = (ChannelListComboBox.SelectedItem as ComboBoxItem).Tag as ChannelBase;

            Color newColor;
            if (newChannel is SystemChannel)
            {
                DisplayMetadata metadata = (newChannel as SystemChannel).VisualIdentity;

                // 'Color' can technically be any CSS colour string
                // For now, we assume that it'll always be in "#000000" format
                int color = Int32.Parse(metadata.Color.Substring(1), System.Globalization.NumberStyles.HexNumber);
                newColor = Color.FromRgb(
                    (byte)((color >> 16) & 0xFF),
                    (byte)((color >> 8) & 0xFF),
                    (byte)(color & 0xFF));
            }
            else if (newChannel is DefaultChannel)
            {
                // Use white for default channel
                newColor = Color.FromRgb(255, 255, 255);
            }
            else if (newChannel != null)
            {
                // A new channel type has been added to the service
                Console.WriteLine($"Unexpected channel type {newChannel.ChannelType}");
                newColor = Color.FromRgb(0, 0, 0);
            }
            else
            {
                return;
            }

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

        private void BtnLaunch_Click(object sender, RoutedEventArgs e)
        {
            var win2 = new NewWindow();
            win2.Show();
        }
    }
}