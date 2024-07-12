using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json.Bson;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Security.RightsManagement;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using Worked_Timer.Model;
using Worked_Timer.Pages;

namespace Worked_Timer.ViewModel
{
    public class TimerViewModel :ViewModelBase
    {
        private readonly string[] TypesImageForSets = { "pack://application:,,,/images/Work.svg", "pack://application:,,,/images/Break.svg", "pack://application:,,,/images/Lunch.svg" };
        private readonly string[] TypesImageForMain = { "pack://application:,,,/images/workView.png", "pack://application:,,,/images/breakView.png", "pack://application:,,,/images/lunchView.png", "pack://application:,,,/images/warningView.png", "pack://application:,,,/images/Welldone.png" };
      
        private System.Timers.Timer _curentTimer;
        private System.Timers.Timer _displayTimer;
        private DispatcherTimer _extraTimer;
        private Stopwatch _stopwatch;

        public ObservableCollection<MomentTimer> Cycles { get; }

        private double _timeToLeftPause=0;
        private double _timeToGlobalPause = 0;

        private AnimationViewModel _animationViewModel;
        private MessageViewModel _message;
        private PropertyViewModel _property;
        private Configuration _config;
        private WindowViewModel _window;

        public TimerViewModel(WindowViewModel window,Configuration config, AnimationViewModel animationViewModel, MessageViewModel message, PropertyViewModel property)
        {
            _property = property;
            _config = config;
            _animationViewModel = animationViewModel;
            _message = message;
            _window = window;

            Cycles = new ObservableCollection<MomentTimer>();
            IsVisibleLastProperty();
            VisibilityButtonsPlayPause = true;
            VisibilityButtonsPlay = true;
        }

        #region Property

        #region Seters

        private string _totalMinutes;
        public string TotalMinutes { 
            get=>_totalMinutes;
            set { 
                if(value != _totalMinutes)
                {
                    _totalMinutes = value;
                    onPropertyChanged(nameof(TotalMinutes));
                }
               
            }
        }

        public string TimeWorkInMinuteExeption { get; set; }
        public bool TimeWorkInMinuteVisibility { get; set; }
        private string _timeWorkInMinute;
        public string TimeWorkInMinute
        {
            get => _timeWorkInMinute;
            set
            {
                var rez = IsCorectField(_timeWorkInMinute, value, false);

                if(rez == ExeptionValue.ok)
                {
                    _timeWorkInMinute = value;
                    onPropertyChanged(nameof(TimeWorkInMinute));

                    TimeWorkInMinuteExeption = String.Empty;
                    TimeWorkInMinuteVisibility = false;
                    onPropertyChanged(nameof(TimeWorkInMinuteExeption));
                    onPropertyChanged(nameof(TimeWorkInMinuteVisibility));
                }
                else
                {
                    TimeWorkInMinuteExeption = _property.GetExeption(rez);
                    TimeWorkInMinuteVisibility = true;
                    onPropertyChanged(nameof(TimeWorkInMinuteExeption));
                    onPropertyChanged(nameof(TimeWorkInMinuteVisibility));
                }

                SetTotalTime();
               
            }
        }

        public string TimeBreakInMinutesExeption { get; set; }
        public bool TimeBreakInMinutesVisibility { get; set; }
        private string _timeBreakInMinutes;
        public string TimeBreakInMinutes
        {
            get => _timeBreakInMinutes;
            set
            {
                var rez = IsCorectField(_timeWorkInMinute, value, false);

                if (rez == ExeptionValue.ok)
                {
                    _timeBreakInMinutes = value;
                    onPropertyChanged(nameof(TimeBreakInMinutes));

                    TimeBreakInMinutesExeption = String.Empty;
                    TimeBreakInMinutesVisibility = false;
                    onPropertyChanged(nameof(TimeBreakInMinutesExeption));
                    onPropertyChanged(nameof(TimeBreakInMinutesVisibility));
                }
                else
                {
                    TimeBreakInMinutesExeption = _property.GetExeption(rez);
                    TimeBreakInMinutesVisibility = true;
                    onPropertyChanged(nameof(TimeBreakInMinutesExeption));
                    onPropertyChanged(nameof(TimeBreakInMinutesVisibility));
                }

                SetTotalTime();

               
            }
        }

        public string TimeBigRestInMinutesExeption { get; set; }
        public bool TimeBigRestInMinutesVisibility { get; set; }
        private string _timeBigRestInMinutes;
        public string TimeBigRestInMinutes
        {
            get => _timeBigRestInMinutes;
            set
            {
                var rez = IsCorectField(_timeBigRestInMinutes, value, false);

                if (rez == ExeptionValue.ok)
                {
                    _timeBigRestInMinutes = value;
                    onPropertyChanged(nameof(TimeBigRestInMinutes));

                    TimeBigRestInMinutesExeption = String.Empty;
                    TimeBigRestInMinutesVisibility = false;
                    onPropertyChanged(nameof(TimeBigRestInMinutesExeption));
                    onPropertyChanged(nameof(TimeBigRestInMinutesVisibility));
                }
                else
                {
                    TimeBigRestInMinutesExeption = _property.GetExeption(rez);
                    TimeBigRestInMinutesVisibility = true;
                    onPropertyChanged(nameof(TimeBigRestInMinutesExeption));
                    onPropertyChanged(nameof(TimeBigRestInMinutesVisibility));
                }

                if (value != string.Empty)
                {
                    UpdateTimeInList();
                }
                SetTotalTime();
             
            }
        }

        public string CountCyclesExeption { get; set; }
        public bool CountCyclesVisibility { get; set; }
        private string _countCycles;
        public string CountCycles
        {
            get => _countCycles;
            set
            {
                var rez = IsCorectField(_timeWorkInMinute, value, true);

                if (rez == ExeptionValue.ok)
                {
                    _countCycles = value;
                    onPropertyChanged(nameof(CountCycles));

                    CountCyclesExeption = String.Empty;
                    CountCyclesVisibility = false;
                    onPropertyChanged(nameof(CountCyclesExeption));
                    onPropertyChanged(nameof(CountCyclesVisibility));
                }
                else
                {
                    CountCyclesExeption = _property.GetExeption(rez);
                    CountCyclesVisibility = true;
                    onPropertyChanged(nameof(CountCyclesExeption));
                    onPropertyChanged(nameof(CountCyclesVisibility));
                }

                if (value != string.Empty)
                {
                    updateSeters();
                }
                SetTotalTime();

            }
        }

        #endregion

        #region View

        private TimeSpan _extraTimerLabel;
        public TimeSpan ExtraTimeLabel
        {
            get => _extraTimerLabel;
            set
            {
                if (value == _extraTimerLabel) return;

                _extraTimerLabel = value;
                onPropertyChanged(nameof(ExtraTimeLabel));
            }
        }

        private string _nameImage;
        public string NameImage
        {
            get => _nameImage;
            set
            {
                if (value != _nameImage)
                {
                    _nameImage = value;
                    onPropertyChanged(nameof(NameImage));
                }
            }
        }

        private int _curentPosition;
        public int CurentPosition
        {
            get => _curentPosition;
            set
            {
                if(value != _curentPosition)
                {
                    _curentPosition = value;
                    onPropertyChanged(nameof(CurentPosition));
                }
            }
        }

        private string _labelTimer;
        public string LabelTimer
        {
            get => _labelTimer;
            set
            {
                if (_labelTimer != value)
                {
                    _labelTimer = value;
                    onPropertyChanged(nameof(LabelTimer));
                }
            }
        }
      
        private int _valueProgress;
        public int ValueProgress
        {
            get=> _valueProgress;
            set
            {
                if( value!= _valueProgress)
                {
                    _valueProgress = value;
                    onPropertyChanged(nameof(ValueProgress));
                }
            }
        }

        private TimeSpan _extraTimeWork;
        private TimeSpan _extraTimeBreak;

        private bool _visibilitySetDefault;
        public bool VisibilitySetDefault
        {
            get => _visibilitySetDefault;
            set
            {
                if (value != _visibilitySetDefault)
                {
                    _visibilitySetDefault = value;
                    onPropertyChanged(nameof(VisibilitySetDefault));
                }
            }
        }

        private bool _visibilitySetDefaultInfo;
        public bool VisibilitySetDefaultInfo
        {
            get => _visibilitySetDefaultInfo;
            set
            {
                if (value != _visibilitySetDefaultInfo)
                {
                    _visibilitySetDefaultInfo = value;
                    onPropertyChanged(nameof(VisibilitySetDefaultInfo));
                }
            }
        }
        private bool _visibilityButtonsPlayPause;
        public bool VisibilityButtonsPlayPause
        {
            get => _visibilityButtonsPlayPause;
            set
            {
                if (value != _visibilityButtonsPlayPause)
                {
                    _visibilityButtonsPlayPause = value;
                    onPropertyChanged(nameof(VisibilityButtonsPlayPause));
                }
            }
        }
        private bool _visibilityButtonsPlay;
        public bool VisibilityButtonsPlay
        {
            get => _visibilityButtonsPlay;
            set
            {
                if (value != _visibilityButtonsPlay)
                {
                    _visibilityButtonsPlay = value;
                    onPropertyChanged(nameof(VisibilityButtonsPlay));
                }
            }
        }
        private bool _visibilityButtonsPause;
        public bool VisibilityButtonsPause
        {
            get => _visibilityButtonsPause;
            set
            {
                if (value != _visibilityButtonsPause)
                {
                    _visibilityButtonsPause = value;
                    onPropertyChanged(nameof(VisibilityButtonsPause));
                }
            }
        }
        #endregion

        #endregion

        #region Command

        /// <summary>
        /// processes for life to choose a lunch break
        /// </summary>
        private ICommand _setLunchCommand;
        public ICommand SetBigRest
        {
            get
            {
                if(_setLunchCommand == null)
                {
                    _setLunchCommand = new RelayCommand<object>(param =>
                    {
                        if (param is MomentTimer)
                        {
                            if ((param as MomentTimer).ActionOfTime == ActionOfTime.Work)
                            {
                                return;
                            }

                            var oldBigRestCycle = Cycles.Where(c => c.ActionOfTime == ActionOfTime.BigRest).FirstOrDefault();

                            if (oldBigRestCycle != null)
                            {
                                oldBigRestCycle.NameImage = _property.CurentImageSourse.Sourse["Break"];
                                oldBigRestCycle.NameImageForMain = TypesImageForMain[1];
                                oldBigRestCycle.ActionOfTime = ActionOfTime.Rest;
                            }

                            if (oldBigRestCycle != (param as MomentTimer))
                            {
                                (param as MomentTimer).ActionOfTime = ActionOfTime.BigRest;
                                (param as MomentTimer).NameImage = _property.CurentImageSourse.Sourse["Lunch"];
                                (param as MomentTimer).NameImageForMain = TypesImageForMain[2];
                                _animationViewModel.ChangeLunchDrop(false);

                            }
                            else
                            {
                                _animationViewModel.ChangeLunchDrop(true);
                                TimeBigRestInMinutes = String.Empty;
                                UpdateTimeInList();
                            }
                        }
                    });
                }
                return _setLunchCommand;
            }

            //get => new RelayCommand<object>(param =>
            //{
            //    if(param is MomentTimer)
            //    {
            //        if((param as MomentTimer).ActionOfTime == ActionOfTime.Work)
            //        {
            //            return;
            //        }

            //        var oldBigRestCycle = Cycles.Where(c => c.ActionOfTime == ActionOfTime.BigRest).FirstOrDefault();

            //        if (oldBigRestCycle != null )
            //        {
            //            oldBigRestCycle.NameImage = _property.CurentImageSourse.Sourse["Break"];
            //            oldBigRestCycle.NameImageForMain = TypesImageForMain[1];
            //            oldBigRestCycle.ActionOfTime = ActionOfTime.Rest;
            //            //oldBigRestCycle.Message = TypesMessage[1];
            //        }

            //        if(oldBigRestCycle != (param as MomentTimer))
            //        {
            //            (param as MomentTimer).ActionOfTime = ActionOfTime.BigRest;
            //            //(param as MomentTimer).Message = TypesMessage[2];
            //            (param as MomentTimer).NameImage = _property.CurentImageSourse.Sourse["Lunch"];
            //            (param as MomentTimer).NameImageForMain = TypesImageForMain[2];
            //            _animationViewModel.ChangeLunchDrop(false);
                        
            //        }
            //        else
            //        {
            //            _animationViewModel.ChangeLunchDrop(true);
            //            TimeBigRestInMinutes = String.Empty;
            //            UpdateTimeInList();
            //        }
            //    }
            //});
        }

        public ICommand DestroySets
        {
            get => new RelayCommand(() =>
            {
                SetToDefaultPropertyAndStoppedTimer();
            });
        }

        /// <summary>
        /// Stops time
        /// </summary>
        private ICommand _pauseCommand;
        public ICommand PauseCommand
        {
            get
            {
                if( _pauseCommand == null)
                {
                    _pauseCommand = new RelayCommand(() =>
                    {

                        if (_timeToLeftPause == 0)
                        {
                            MomentTimer curentMoment = Cycles.Where(s => s.Position == CurentPosition).FirstOrDefault();
                            _timeToGlobalPause = GetSecondsOnMomentTime(curentMoment);
                            _timeToLeftPause = _timeToGlobalPause - _stopwatch.ElapsedMilliseconds;
                        }
                        else
                        {
                            _timeToLeftPause -= _stopwatch.ElapsedMilliseconds;
                        }


                        StopTimers();
                        VisibilityButtonsPause = true;
                        VisibilityButtonsPlay = false;
                    });
                }
                return _pauseCommand;
            }
            
        }

        /// <summary>
        /// Extends time
        /// </summary>
        private ICommand _playCommand;
        public ICommand PlayCommand
        {
            get
            {
                if(_playCommand == null)
                {
                    _playCommand = new RelayCommand(() =>
                    {
                        if (_timeToLeftPause != 0)
                        {
                            _curentTimer = new System.Timers.Timer(_timeToLeftPause);
                            _curentTimer.Elapsed += InvokeWindow;

                        }
                        StartTimers();
                        VisibilityButtonsPause = false;
                        VisibilityButtonsPlay = true;
                    });
                }
                return _playCommand;
            }
        }

        /// <summary>
        /// Starts a timer with new values ​​and saves them
        /// </summary>
        public ICommand Start
        {
            get => new RelayCommand(() =>
            {
                CurentPosition = 1;
               

                _curentTimer = new System.Timers.Timer(int.Parse(TimeWorkInMinute) * 60 * 1000);
                _curentTimer.Elapsed += InvokeWindow;

                _displayTimer = new System.Timers.Timer();
                _displayTimer.Elapsed += OnDisplayTimeElapsed;        

                _stopwatch = new Stopwatch();

                _extraTimer = new DispatcherTimer();
                _extraTimer.Interval = TimeSpan.FromSeconds(1);
                _extraTimer.Tick += ExtraTimeTimer_Tick;
                _extraTimeBreak = new TimeSpan(0,0,0);
                _extraTimeWork = new TimeSpan(0,0,0);

                var appconfig = new AppConfig() { PropertyWork = TimeWorkInMinute, PropertyBreak = TimeBreakInMinutes, PropertyCycle = CountCycles, PropertyTotalTime =TotalMinutes };

                var lunch = Cycles.Where(l => l.ActionOfTime == ActionOfTime.BigRest).FirstOrDefault();

                

                if (lunch != null)
                {
                    if (String.IsNullOrEmpty(TimeBigRestInMinutes))
                    {
                        lunch.NameImage = TypesImageForSets[1];
                        lunch.NameImageForMain = TypesImageForMain[1];
                        lunch.ActionOfTime = ActionOfTime.Rest;
                        //lunch.Message = TypesMessage[1];

                    }
                    else
                    {
                        appconfig.PropertyPositionLunch = lunch.Position.ToString();
                        appconfig.PropertyLunch = TimeBigRestInMinutes;
                    }
                }
                else
                {
                    appconfig.PropertyPositionLunch = "0";
                    appconfig.PropertyLunch = "0";
                }
               
                
               Task.Run(()=> _config.SetParametr(appconfig));

                NameImage = TypesImageForMain[0];
                StartTimers();
            });
        }

        /// <summary>
        /// Starts the timer with the old values
        /// </summary>
        public ICommand SetPastPropertyDefault
        {
            get => new RelayCommand(() =>
            {
                TimeWorkInMinute = _config.Config.PropertyWork;
                TimeBreakInMinutes = _config.Config.PropertyBreak;
                CountCycles = _config.Config.PropertyCycle;

                if(_config.Config.PropertyLunch !="0" && _config.Config.PropertyPositionLunch != "0")
                {
                    var lunch = Cycles.Single(l => l.Position == int.Parse(_config.Config.PropertyPositionLunch));

                    lunch.ActionOfTime = ActionOfTime.BigRest;
                    lunch.NameImage = TypesImageForSets[2];
                    lunch.NameImageForMain = TypesImageForMain[2];
                }

                Start.Execute(this);
            });
        }

        /// <summary>
        /// Stops overtime
        /// </summary>
        public ICommand StopExtra
        {
            get => new RelayCommand(() =>
            {
                VisibilityButtonsPlayPause = true;
                _extraTimer.Stop();
                MomentTimer curentMoment = Cycles.Where(s => s.Position == CurentPosition).FirstOrDefault();

                if (curentMoment == null)
                {
                    System.Windows.Forms.MessageBox.Show("Усe");
                    return;
                }
               
                _animationViewModel.ChangeExtraAndMainInfo(true);
                StartNextTimer(curentMoment);
            });
        }
        #endregion

        #region Method
        private void SetToDefaultPropertyAndStoppedTimer()
        {
            if (_extraTimer.IsEnabled)
            {
                _extraTimer.Stop();
            }

            _curentTimer.Stop();
            _curentTimer.Dispose();

            _displayTimer.Stop();
            _displayTimer.Dispose();

            _stopwatch.Stop();
            _stopwatch.Reset();

            TimeBigRestInMinutes = string.Empty;
            TimeBreakInMinutes = string.Empty;
            TimeWorkInMinute = string.Empty;

            CountCycles = string.Empty;
            CurentPosition = 0;

            Cycles.Clear();
        }

        private void StopTimers()
        {
            _stopwatch.Stop();
            _curentTimer.Stop();
            _displayTimer.Stop();
        }

        private void StartTimers()
        {
            _displayTimer.Start();
            _curentTimer.Start();
            _stopwatch.Restart();    
        }

        /// <summary>
        /// Show Window 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void InvokeWindow(Object source, ElapsedEventArgs e)
        {
            StopTimers();

            CurentPosition++;

            MomentTimer curentMoment = Cycles.Where(s => s.Position == CurentPosition).FirstOrDefault();



            if (curentMoment == null)
            {
                _message.IsLast = true;
                _message.WorkTime = (TimeSpan.FromMinutes(double.Parse(TimeWorkInMinute) * double.Parse(CountCycles)) + _extraTimeWork).ToString();
                _message.BreakTime = (TimeSpan.FromMinutes(double.Parse(TimeBreakInMinutes) * double.Parse(CountCycles)) + _extraTimeBreak).ToString();
                _message.Moment = curentMoment = new MomentTimer() { NameImageForMain = TypesImageForMain[4] };
                _window.ShowWindowMessage();

                _window.StopTimerCommand.Execute(curentMoment);
                SetToDefaultPropertyAndStoppedTimer();
                return;
            }


            _message.Moment = curentMoment;

            //App.Current.Dispatcher.Invoke(() =>
            //{

            _window.ShowWindowMessage();
           // if (_window.ShowWindowMessage() == true)
            if (_message.IsNext == true)
            {
                StartNextTimer(curentMoment);
            }
            else
            {
                _animationViewModel.ChangeExtraAndMainInfo(false);
                VisibilityButtonsPlayPause = false;
                ExtraTimeLabel = new TimeSpan(0, 0, 0);
                NameImage = TypesImageForMain[3];

                _extraTimer.Start();
            }
            //MessageWindow message = new MessageWindow() { Message = ms };
            //if (message.ShowDialog() == true)
            //{
            //    
            //}
            //else
            //{
            //    _animationViewModel.ChangeExtraAndMainInfo(false);
            //    VisibilityButtonsPlayPause = false;
            //    ExtraTimeLabel = new TimeSpan(0, 0, 0);
            //    _extraTimer.Start();

            //    NameImage = TypesImageForMain[3];
            //}
            //});
            //MessageWindow message = new MessageWindow() { Message = ms};

            //if (message.ShowDialog() == true)
            //{
            //    StartNextTimer(curentMoment);
            //}
            //else
            //{
            //    _animationViewModel.ChangeExtraAndMainInfo(false);
            //    _extraTimer.Start();
            //    NameImage = TypesImageForMain[3];
            //}

        }

        /// <summary>
        /// Starts a new timer
        /// </summary>
        /// <param name="curentMoment"></param>
        private void StartNextTimer(MomentTimer curentMoment) {

            _timeToLeftPause = 0;
            _timeToGlobalPause = 0;

            NameImage = curentMoment.NameImageForMain;
            _curentTimer = new System.Timers.Timer(GetSecondsOnMomentTime(curentMoment));
            _curentTimer.Elapsed += InvokeWindow;

            StartTimers();
        }

        /// <summary>
        /// Returns the time for the next timer
        /// </summary>
        /// <param name="curentMoment"></param>
        /// <returns></returns>
        private double GetSecondsOnMomentTime(MomentTimer curentMoment)
        {
            switch (curentMoment.ActionOfTime)
            {
                case ActionOfTime.Work:
                    return int.Parse(TimeWorkInMinute) * 60 * 1000;
                case ActionOfTime.Rest:
                    return int.Parse(TimeBreakInMinutes) * 60 * 1000;
                case ActionOfTime.BigRest:
                    return int.Parse(TimeBigRestInMinutes) * 60 * 1000;
            }
            return 0;
        }

        /// <summary>
        /// Includes an additional timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExtraTimeTimer_Tick(object sender, EventArgs e)
        {
            ExtraTimeLabel += _extraTimer.Interval;

           if( CurentPosition % 2 == 0)
            {
                _extraTimeWork += _extraTimer.Interval;
            }
            else
            {
                _extraTimeBreak += _extraTimer.Interval; 
            }
        }

        /// <summary>
        /// Show Method every second for view
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnDisplayTimeElapsed(Object source, ElapsedEventArgs e)
        {
            double totalSeconds;
            TimeSpan remaining;

            if (_timeToLeftPause != 0)
            {
                totalSeconds = _timeToGlobalPause / 1000;
                remaining = TimeSpan.FromSeconds(totalSeconds) - (TimeSpan.FromSeconds(totalSeconds - (_timeToLeftPause / 1000)) + _stopwatch.Elapsed);
            }
            else
            {
                totalSeconds = _curentTimer.Interval / 1000;
                remaining = TimeSpan.FromSeconds(totalSeconds) - _stopwatch.Elapsed;
            }

            //LabelTimer = $"{remaining.Minutes:D2}:{remaining.Seconds:D2}";
            LabelTimer = remaining.ToString(@"hh\:mm\:ss");
            ValueProgress =(int)(((totalSeconds - remaining.TotalSeconds) / totalSeconds) * 100);
        }

        /// <summary>
        /// Updates the data in the List
        /// </summary>
        private void updateSeters()
        {
            Cycles.Clear();
            for (int i = 0; i < int.Parse(CountCycles) * 2 && i + 1 != int.Parse(CountCycles) * 2; i++)
            {
                if (i % 2 == 0)
                {
                    Cycles.Add(new MomentTimer() { Position = i + 1, ActionOfTime = ActionOfTime.Work, NameImage = _property.CurentImageSourse.Sourse["Work"], NameImageForMain = TypesImageForMain[0] });
                }
                else
                {
                    Cycles.Add(new MomentTimer() { Position = i + 1 , ActionOfTime = ActionOfTime.Rest, NameImage = _property.CurentImageSourse.Sourse["Break"], NameImageForMain = TypesImageForMain[1] });
                }


            }
            UpdateTimeInList();
        }

        /// <summary>
        /// Sets the time interval value in the array
        /// </summary>
        private void UpdateTimeInList()
        {
            TimeSpan startTime= new TimeSpan(0,0,0);
            TimeSpan endTime = TimeSpan.FromMinutes(double.Parse(TimeWorkInMinute));
            Cycles[0].IntervalTime = $"{startTime.ToString(@"hh\:mm")}/{endTime.ToString(@"hh\:mm")}";

            startTime = endTime;

            for (int i=1; i < Cycles.Count; i++)
            {
                TimeSpan temp = startTime;

                switch(Cycles[i].ActionOfTime)
                {
                    case ActionOfTime.Work:

                        endTime = temp.Add(TimeSpan.FromMinutes(double.Parse(TimeWorkInMinute)));
                        break;
                       case ActionOfTime.Rest:
                        endTime = temp.Add(TimeSpan.FromMinutes(double.Parse(TimeBreakInMinutes)));
                        break;
                    case ActionOfTime.BigRest:
                        endTime = temp.Add(TimeSpan.FromMinutes(double.Parse(TimeBigRestInMinutes)));
                        break;
                }

                Cycles[i].IntervalTime = $"{startTime.ToString(@"hh\:mm")}/{endTime.ToString(@"hh\:mm")}";
                startTime = endTime;
            }
        }

        /// <summary>
        /// Checks whether the information is entered correctly in the main fields
        /// </summary>
        /// <returns></returns>
        public bool CheckIsFilledMainFields()
        {
            if (!String.IsNullOrEmpty(CountCycles) &&
                !String.IsNullOrEmpty(TimeWorkInMinute) &&
                !String.IsNullOrEmpty(TimeBreakInMinutes))
            {
                _animationViewModel.SetterIsFilledMainFields = true;
                return true;
            }

            _animationViewModel.SetterIsFilledMainFields = false;
            return false;
        }

        /// <summary>
        /// Calculates the total time
        /// </summary>
        public void SetTotalTime()
        {
            if (CheckIsFilledMainFields())
            {
                int temp = int.Parse(TimeWorkInMinute) * int.Parse(CountCycles);

                if (!String.IsNullOrEmpty(TimeBigRestInMinutes))
                {
                    temp += int.Parse(TimeBigRestInMinutes);
                    temp += int.Parse(TimeBreakInMinutes) * (int.Parse(CountCycles) - 2);
                }
                else
                {
                    temp += int.Parse(TimeBreakInMinutes) * (int.Parse(CountCycles) - 1);
                }

                 TotalMinutes = $"{temp / 60}:{(temp % 60)}";
              
            }
        }

        /// <summary>
        /// Checks a specific non-field for correctness of input
        /// </summary>
        /// <param name="output"></param>
        /// <param name="value"></param>
        /// <param name="isCountCycle"></param>
        /// <returns>returns the type of incorrect input</returns>
        private ExeptionValue IsCorectField(string output,string value, bool isCountCycle)
        {

            if (isCountCycle)
            {
                if (value.Length > 2)
                {
                    return ExeptionValue.toLong;
                }

                if (!String.IsNullOrEmpty(value))
                {
                    if (int.Parse(value) > 15)
                    {
                        return ExeptionValue.Max15;
                    }
                }

            }
            else
            {
                if (value.Length > 3)
                {
                    return ExeptionValue.toLong;
                }
               
            }
          
            if (!value.All(char.IsDigit))
            {
                return ExeptionValue.noteANumber;
            }
            if(value == "0")
            {
                return ExeptionValue.startZero;
            }

            return ExeptionValue.ok;    
        }

        /// <summary>
        /// Enables the visibility of the following values
        /// </summary>
        private void IsVisibleLastProperty()
        {
            if (!String.IsNullOrEmpty(_config.Config.PropertyCycle) &&
                !String.IsNullOrEmpty(_config.Config.PropertyWork) &&
                !String.IsNullOrEmpty(_config.Config.PropertyBreak))
            {
                VisibilitySetDefault = true;
                VisibilitySetDefaultInfo = false;
            }
            else
            {
                VisibilitySetDefault=false;
                VisibilitySetDefaultInfo = true;
            }
        }
        #endregion
    }

    public enum ExeptionValue
    {
       
        toLong,
        noteANumber,
        startZero,
        Max15,
        ok,
        

    }
}
