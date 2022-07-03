using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WpfApp
{
    public class SideMenu
    {
        private Grid _sidePanel;

        private DispatcherTimer _timer;

        public Action StartTimerEventHandler;

        private bool _isHidden;

        private double _panelWidth;
        public SideMenu(Grid sideMenu)
        {
            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            _timer.Tick += TimerTick;
            _sidePanel = sideMenu;
            _panelWidth = sideMenu.Width;
            StartTimerEventHandler += _timer.Start;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (_isHidden)
            {
                _sidePanel.Width += 1;
                if (_sidePanel.Width >= _panelWidth)
                {
                    _timer.Stop();
                    _isHidden = false;
                }
            }
            else
            {
                _sidePanel.Width -= 1;
                if (_sidePanel.Width <= 30)
                {
                    _timer.Stop();
                    _isHidden = true;
                }
            }
        }
    }
}
