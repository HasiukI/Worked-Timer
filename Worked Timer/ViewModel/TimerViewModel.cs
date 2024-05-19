using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Worked_Timer.ViewModel
{
    class TimerViewModel :ViewModelBase
    {
        private string _timerWork;
        public string TimerWork
        {
            get => _timerWork;
            set
            {
                if(_timerWork != value)
                {
                    _timerWork = value;
                    onPropertyChanged(nameof(TimerWork));
                }
            }
        }

        private string _timerRest;
        public string TimerRest
        {
            get => _timerRest;
            set
            {
                if (_timerRest != value)
                {
                    _timerRest = value;
                    onPropertyChanged(nameof(TimerRest));
                }
            }
        }

        public ICommand ShowTimer { get => new RelayCommand(() =>
        {
           
        }); }
    }
}
