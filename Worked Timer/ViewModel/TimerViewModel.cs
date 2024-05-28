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
using System.Windows.Forms;
using System.Collections.ObjectModel;
using Worked_Timer.Model;
using System.Windows.Threading;

namespace Worked_Timer.ViewModel
{
    class TimerViewModel :ViewModelBase
    {
        private readonly string[] TypesTimerLabel = { "Work", "Rest", "Big Rest" };
        private readonly string[] TypesMessage = { "It's time to get to work", "You should take a break", "Don't forget to eat" };

        private int curentPosition;

        private System.Timers.Timer _curentTimer;
        private System.Timers.Timer _displayTimer;
        private DispatcherTimer _extraTimer;
        private Stopwatch _stopwatch;

        public ObservableCollection<MomentTimer> Cycles { get; }

        public TimerViewModel()
        {
            Cycles = new ObservableCollection<MomentTimer>();
        }

        #region Property

        #region Seters
        private string _timeWorkInMinute;
        public string TimeWorkInMinute
        {
            get => _timeWorkInMinute;
            set
            {
                if (_timeWorkInMinute != value)
                {
                    if (value.Length <= 3)
                    {
                        if (value.All(char.IsDigit))
                        {
                            _timeWorkInMinute = value;
                            onPropertyChanged(nameof(TimeWorkInMinute));
                        }
                    }

                }
            }
        }

        private string _timeBreakInMinutes;
        public string TimeBreakInMinutes
        {
            get => _timeBreakInMinutes;
            set
            {
                if (_timeBreakInMinutes != value)
                {
                    if (value.Length <= 3)
                    {
                        if (value.All(char.IsDigit))
                        {
                            _timeBreakInMinutes = value;
                            onPropertyChanged(nameof(TimeBreakInMinutes));
                        }
                    }
                }
            }
        }

        private string _timeBigRestInMinutes;
        public string TimeBigRestInMinutes
        {
            get => _timeBigRestInMinutes;
            set
            {
                if (_timeBigRestInMinutes != value)
                {
                    if (value.Length <= 3)
                    {
                        if (value.All(char.IsDigit))
                        {
                            _timeBigRestInMinutes = value;
                            onPropertyChanged(nameof(TimeBigRestInMinutes));
                        }
                    }
                }
            }
        }

        private int _countCycles;
        public int CountCycles
        {
            get => _countCycles;
            set
            {
                if (value != _countCycles)
                {
                    _countCycles = value;
                    onPropertyChanged(nameof(CountCycles));
                    updateSeters();
                }
            }
        }

        private int _SelectedBigRest;
        public int SelectedBigRest
        {
            get => _SelectedBigRest;
            set
            {
                if (value != _SelectedBigRest && value % 2 != 0)
                {
                    _SelectedBigRest = value;
                    onPropertyChanged(nameof(SelectedBigRest));
                    updateSeters();
                }
            }
        }
        #endregion

        #region View
        private string _typeTimerLabel;
        public string TypeTimerLabel
        {
            get => _typeTimerLabel;
            set
            {
                if (_typeTimerLabel != value)
                {
                    _typeTimerLabel = value;
                    onPropertyChanged(nameof(TypeTimerLabel));
                }
            }
        }

        private TimeSpan _extraTimeWorkLabel;
        public TimeSpan ExtraTimeWorkLabel
        {
            get => _extraTimeWorkLabel;
            set
            {
                if (_extraTimeWorkLabel != value)
                {
                    _extraTimeWorkLabel = value;
                    onPropertyChanged(nameof(ExtraTimeWorkLabel));
                }
            }
        }
        private TimeSpan _extraTimeRestLabel;
        public TimeSpan ExtraTimeRestLabel
        {
            get => _extraTimeRestLabel;
            set
            {
                if (_extraTimeRestLabel != value)
                {
                    _extraTimeRestLabel = value;
                    onPropertyChanged(nameof(ExtraTimeRestLabel));
                }
            }
        }
        private string labelwork;
        public string Labelwork
        {
            get => labelwork;
            set
            {
                if (labelwork != value)
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
        #endregion

        #endregion


        #region Command
        public ICommand PauseCommand
        {
            get => new RelayCommand(() =>
            {
                StopTimers();
            });
        }

        public ICommand PlayCommand
        {
            get => new RelayCommand(() =>
            {
                StartTimers(false);
            });
        }

        public ICommand Start
        {
            get => new RelayCommand(() =>
            {
                curentPosition = 0;

                _curentTimer = new System.Timers.Timer(int.Parse(TimeWorkInMinute) * 1000);
                _curentTimer.Elapsed += InvokeWindow;

                _displayTimer = new System.Timers.Timer();
                _displayTimer.Elapsed += OnDisplayTimeElapsed;        

                _stopwatch = new Stopwatch();

                _extraTimer = new DispatcherTimer();
                _extraTimer.Interval = TimeSpan.FromSeconds(1); // Відстежує кожну секунду
                _extraTimer.Tick += ExtraTimeTimer_Tick;

                StartTimers(false);
                     
            });
        }

        public ICommand StopExtra
        {
            get => new RelayCommand(() =>
            {
                _extraTimer.Stop();
                MomentTimer curentMoment = Cycles.Where(s => s.Position == curentPosition).FirstOrDefault();

                if (curentMoment == null)
                {
                    System.Windows.Forms.MessageBox.Show("Усe");
                    return;
                }

                StartNextTimer(curentMoment);
            });
        }
        #endregion

        #region Method

        private void StopTimers()
        {
            _stopwatch.Stop();
            _curentTimer.Stop();
            _displayTimer.Stop();
        }
        private void StartTimers(bool IsRestart)
        {
            _curentTimer.Start();
            _displayTimer.Start();

            if(IsRestart)
                _stopwatch.Restart();
            else
                _stopwatch.Start();
        }

        /// <summary>
        /// Show Window 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void InvokeWindow(Object source, ElapsedEventArgs e)
        {
            StopTimers();

            curentPosition++;

            MomentTimer curentMoment = Cycles.Where(s => s.Position == curentPosition).FirstOrDefault();

            if (curentMoment == null)
            {
                System.Windows.Forms.MessageBox.Show("Усe");
                return;
            }

            var rez = System.Windows.Forms.MessageBox.Show(curentMoment.Message, "Перерва", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (rez == DialogResult.Yes)
            {
                StartNextTimer(curentMoment);
            }
            else
            {
                _extraTimer.Start();
            }

        }

        private void StartNextTimer(MomentTimer curentMoment) {
            switch (curentMoment.ActionOfTime)
            {
                case ActionOfTime.Work:
                    _curentTimer = new System.Timers.Timer(int.Parse(TimeWorkInMinute) * 1000);
                    TypeTimerLabel = TypesTimerLabel[0];
                    break;
                case ActionOfTime.Rest:
                    _curentTimer = new System.Timers.Timer(int.Parse(TimeBreakInMinutes) * 1000);
                    TypeTimerLabel = TypesTimerLabel[1];
                    break;
                case ActionOfTime.BigRest:
                    _curentTimer = new System.Timers.Timer(int.Parse(TimeBigRestInMinutes) * 1000);
                    TypeTimerLabel = TypesTimerLabel[2];
                    break;
            }

            _curentTimer.Elapsed += InvokeWindow;

            StartTimers(true);
        }

        private void ExtraTimeTimer_Tick(object sender, EventArgs e)
        {
           if( curentPosition % 2 == 0)
            {
                ExtraTimeRestLabel += _extraTimer.Interval;
               
            }
            else
            {
                ExtraTimeWorkLabel += _extraTimer.Interval;
            }
        }

        /// <summary>
        /// Show Method every second for view
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnDisplayTimeElapsed(Object source, ElapsedEventArgs e)
        {
            TimeSpan remaining = TimeSpan.FromSeconds(_curentTimer.Interval / 1000) - _stopwatch.Elapsed;
            Labelwork = $"Залишилось {remaining.Minutes}.{remaining.Seconds}";

        }

        private void updateSeters()
        {
            Cycles.Clear();
            for (int i = 0; i < CountCycles * 2 && i + 1 != CountCycles * 2; i++)
            {
                if (i % 2 == 0)
                {
                    Cycles.Add(new MomentTimer() { Position = i, ActionOfTime = ActionOfTime.Work, Message = TypesMessage[0] });
                }
                else
                {
                    if (SelectedBigRest == i)
                        Cycles.Add(new MomentTimer() { Position = i, ActionOfTime = ActionOfTime.BigRest, Message = TypesMessage[2] });
                    else
                        Cycles.Add(new MomentTimer() { Position = i, ActionOfTime = ActionOfTime.Rest, Message = TypesMessage[1] });
                }


            }
        }
    }
    #endregion



}
