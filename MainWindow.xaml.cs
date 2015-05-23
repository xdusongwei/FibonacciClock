using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace FibonacciClock
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private readonly int[] _BLOCK_SEQ = new int[] { 5, 3, 2, 1, 1 };
        private readonly int[] _ID_SEQ = new int[] { 5, 3, 2, 1, 0 };
        private readonly SolidColorBrush _RED = new SolidColorBrush(Colors.Crimson);
        private readonly SolidColorBrush _GREEN = new SolidColorBrush(Colors.LimeGreen);
        private readonly SolidColorBrush _BLUE = new SolidColorBrush(Colors.DodgerBlue);
        private readonly SolidColorBrush _WHITE = new SolidColorBrush(Colors.AliceBlue);

        private IEnumerable<SolidColorBrush> MixColor(IEnumerable<int> aSeqA, IEnumerable<int> aSeqB)
        {
            var dictPower = new Dictionary<int, int>();
            var dictColor = new Dictionary<int, SolidColorBrush>();
            foreach (var i in _ID_SEQ)
                dictPower[i] = 0;
            foreach (var i in aSeqA)
                dictPower[SelectIndex(dictPower, i)] += 1;
            foreach (var i in aSeqB)
                dictPower[SelectIndex(dictPower, i)] += 2;
            foreach (var kv in dictPower)
                if (kv.Value == 1)
                    dictColor[kv.Key] = _RED;
                else if (kv.Value == 2)
                    dictColor[kv.Key] = _GREEN;
                else if (kv.Value == 3)
                    dictColor[kv.Key] = _BLUE;
                else
                    dictColor[kv.Key] = _WHITE;
            return dictColor.Values;
        }

        private static int SelectIndex(Dictionary<int, int> dictPower, int i)
        {
            var index = i;
            if (i == 1)
                if (dictPower[i] == 0)
                    index = i;
                else if (dictPower[0] == 0)
                    index = 0;
                else
                    index = 0;
            return index;
        }

        private IEnumerable<int> GetTurnOnBlock(int aCost)
        {
            return _BLOCK_SEQ.Where(i =>
                {
                    if (i <= aCost)
                    {
                        aCost -= i;
                        return true;
                    }
                    return false;
                });
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var quoteTimer = new Timer(1000);
            quoteTimer.Elapsed += (qsender, qe) =>
            {
                try
                {
                    var now = DateTime.Now;
                    var hour = now.Hour % 12;
                    var minute = now.Minute / 5;
                    var hourOn = GetTurnOnBlock(hour);
                    var minuteOn = GetTurnOnBlock(minute);
                    this.Dispatcher.Invoke(() =>
                    {
                        var colors = MixColor(hourOn, minuteOn).ToList();
                        bdFive.Background = colors[0];
                        bdThree.Background = colors[1];
                        bdTwo.Background = colors[2];
                        bdOneB.Background = colors[3];
                        bdOneA.Background = colors[4];
                    });
                }
                catch { }
            };
            quoteTimer.Start();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
