using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Timers;
using System.Diagnostics;

namespace Worked_Timer.ViewModel
{
    class TimerViewModel :ViewModelBase
    {
        public System.Timers.Timer TimerWork;
        public System.Timers.Timer breakTimer;
        public System.Timers.Timer TimerDisplay;
        private  Stopwatch workStopwatch;
        private  Stopwatch breakStopwatch;
        private System.Timers.Timer displayTimer;
        private  int repeatCount = 3;

        private string _timerWorkString;
        public string TimerWorkString
        {
            get => _timerWorkString;
            set
            {
                if(_timerWorkString != value)
                {
                    if (value.Length <= 3)
                    {
                        if (value.All(char.IsDigit))
                        {
                            _timerWorkString = value;
                            onPropertyChanged(nameof(TimerWorkString));
                        }
                    }
                   
                }
            }
        }

        public TimerViewModel()
        {
           
        }


        private string _timerRestString;
        public string TimerRestString
        {
            get => _timerRestString;
            set
            {
                if (_timerRestString != value)
                { 
                    if (value.Length <= 3)
                    {
                        if (value.All(char.IsDigit))
                        {
                            _timerRestString = value;
                            onPropertyChanged(nameof(TimerRestString));
                        }
                    }
                }
            }
        }

        private string labelwork;
        public string Labelwork
        {
            get => labelwork;
            set
            {
                if(labelwork != value)
                {
                    labelwork = value;
                    onPropertyChanged(nameof(Labelwork));
                }
            }
        }
        private string labelRest;
        public string LabelRest
        {
            get => labelRest;
            set
            {
                if (labelRest != value)
                {
                    labelRest = value;
                    onPropertyChanged(nameof(LabelRest));
                }
            }
        }
        public ICommand Start { get => new RelayCommand(() =>
        {
            TimerWork = new System.Timers.Timer(int.Parse(TimerWorkString) * 60 * 1000);
            breakTimer = new System.Timers.Timer(int.Parse(TimerRestString) * 60 * 1000);
            displayTimer = new System.Timers.Timer();

            TimerWork.Elapsed += OnWorkTimeElapsed;
            breakTimer.Elapsed += OnBreakTimeElapsed;
            displayTimer.Elapsed += OnDisplayTimeElapsed;

            TimerWork.AutoReset = false;
            breakTimer.AutoReset = false;
            displayTimer.AutoReset = true; // Repeat every second

            TimerWork.Start();
            workStopwatch = new Stopwatch();
            breakStopwatch = new Stopwatch();

            workStopwatch.Start();
            displayTimer.Start();
        });
        }

        private  void OnWorkTimeElapsed(Object source, ElapsedEventArgs e)
        {
            workStopwatch.Stop();
            MessageBox.Show("Час роботи завершено. Починається перерва.");

            repeatCount--;
            if (repeatCount > 0)
            {
                breakStopwatch.Start();
                breakTimer.Start();
            }
        }

        private  void OnDisplayTimeElapsed(Object source, ElapsedEventArgs e)
        {
            if (workStopwatch.IsRunning)
            {
                TimeSpan remaining = TimeSpan.FromMinutes(TimerWork.Interval / 60000) - workStopwatch.Elapsed;
                Labelwork = $"Залишилось {remaining.TotalMinutes} хвилин роботи.";
          
            }
            else if (breakStopwatch.IsRunning)
            {
                TimeSpan remaining = TimeSpan.FromMinutes(breakTimer.Interval / 60000) - breakStopwatch.Elapsed;
                LabelRest = $"Залишилось {remaining.TotalMinutes} хвилин перерви.";
      
            }
        }

        private  void OnBreakTimeElapsed(Object source, ElapsedEventArgs e)
        {
            breakStopwatch.Stop();
            Console.WriteLine("Перерва завершена. Починається час роботи.");

            if (repeatCount > 0)
            {
                workStopwatch.Reset();
                workStopwatch.Start();
                TimerWork.Start();
            }
        }
    }
}
