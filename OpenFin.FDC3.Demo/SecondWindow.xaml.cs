using OpenFin.FDC3.Channels;
using OpenFin.FDC3.Context;
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
using System.Windows.Shapes;

namespace OpenFin.FDC3.Demo
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class SecondWindow : Window
    {
        private bool contextChanging = false;
        Connection connection;

        public SecondWindow()
        {
            InitializeComponent();
            this.Loaded += SecondWindow_Loaded;
        }

        private void SecondWindow_Loaded(object sender, RoutedEventArgs e)
        {
            initialize();
        }

        private async void initialize()
        {
            connection = await ConnectionManager.CreateConnectionAsync("secondWindow");
            var channels = await connection.GetDesktopChannelsAsync();

            connection.AddContextHandler(ContextChanged);

            await  Dispatcher.InvokeAsync(async () =>
            {
                tbAppId.Text = FDC3.Uuid;

                foreach (var channel in channels)
                {
                    if (channel.ChannelType == ChannelType.Desktop)
                    {
                        ChannelListComboBox.Items.Add(new ComboBoxItem() { Content = channel.Name, Tag = channel });
                    }
                }

                ChannelListComboBox.SelectedValue = "Global";
            });                       
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

                    await connection.BroadcastAsync(context);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        DesktopChannel newChannel;

        private async void ChannelListComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            newChannel = (ChannelListComboBox.SelectedItem as ComboBoxItem).Tag as DesktopChannel;

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