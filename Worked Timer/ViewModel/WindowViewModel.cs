using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Worked_Timer.Model;
using Worked_Timer.Pages;

namespace Worked_Timer.ViewModel
{
    public class WindowViewModel : ViewModelBase
    {
        public bool IsTimerRun;
        private Dictionary<string, Page> _pages;
        private MessageWindow _messageWindow;
        private MainViewModel _mainViewModel;

        public WindowViewModel(MainViewModel mainViewModel) {
            _mainViewModel = mainViewModel;
            Application.Current.Dispatcher.Invoke(() =>
            {
                var screen = SystemParameters.WorkArea;
                TopPosition = screen.Bottom - Application.Current.MainWindow.Height - 10;
                LeftPosition = screen.Right - Application.Current.MainWindow.Width - 10;
                Application.Current.MainWindow.UpdateLayout();
            }, System.Windows.Threading.DispatcherPriority.Loaded);

            _pages = new Dictionary<string, Page>();
            _pages.Add("Main", new MainPage() { DataContext = mainViewModel});
            _pages.Add("Seters", new SetersPage() { DataContext = mainViewModel });
            _pages.Add("Setting", new SettingPage() { DataContext = mainViewModel });
            _pages.Add("Default", new DefaultPage() { DataContext = mainViewModel });

            CurentPage = _pages.Single(p => p.Key == "Default").Value;    
        }

        #region Property
        private Page _curentPage;
        public Page CurentPage
        {
            get => _curentPage;
            set
            {
                if (_curentPage != value)
                {
                    _curentPage = value;
                    onPropertyChanged(nameof(CurentPage));
                }
            }
        }

        private double _topPosition;
        public double TopPosition
        {
            get => _topPosition;
            set
            {
                if (_topPosition != value)
                {
                    _topPosition = value;
                    onPropertyChanged(nameof(TopPosition));
                }

            }
        }
        private double _leftPosition;
        public double LeftPosition
        {
            get => _leftPosition;
            set
            {
                if (_leftPosition != value)
                {
                    _leftPosition = value;
                    onPropertyChanged(nameof(LeftPosition));
                }
            }
        }
        #endregion

        #region Command

        /// <summary>
        /// Responsible for closing and hiding the program
        /// </summary>
        private ICommand _showWindowCommand;
        public ICommand ShowWindowCommand
        {
            get
            {
                if (_showWindowCommand == null)
                {
                    _showWindowCommand = new RelayCommand(
                        () =>
                        {
                            Application.Current.MainWindow.WindowState = WindowState.Normal;
                            Application.Current.MainWindow.Show();
                            Application.Current.MainWindow.Activate();
                        });
                }
                return _showWindowCommand;
            }
        }


        /// <summary>
        /// Changes pages
        /// </summary>
        private ICommand _changePageCommand;
        public ICommand ChangePageCommand
        {
            get
            {
                if(_changePageCommand == null)
                {
                    _changePageCommand = new RelayCommand<string>(param =>
                    {
                        if(param == "Default")
                        {
                            if (IsTimerRun)
                            {
                                CurentPage = _pages.Single(p => p.Key == "Main").Value;
                            }
                            else
                            {
                                CurentPage = _pages.Single(p => p.Key == "Default").Value;
                            }
                        }
                        else
                        {
                            CurentPage = _pages.Single(p => p.Key == param).Value;
                        }
                       
                    });
                }

                return _changePageCommand;
            }
        }

        private ICommand _hideWindowCommand;
        public ICommand HideWindowCommand
        {
            get
            {
                if (_hideWindowCommand == null)
                {
                    _hideWindowCommand = new RelayCommand(() =>
                    {
                        Application.Current.MainWindow.Hide();
                    });

                }
                return _hideWindowCommand;
            }
           
        }

        public ICommand CloseWindowCommand
        {
            get => new RelayCommand(() => {
                Application.Current.Shutdown();
            });
        }

        private ICommand _windowButtonCommand;
        public ICommand WindowButtonCommand
        {
            get
            {
                if(_windowButtonCommand == null)
                {
                    _windowButtonCommand = new RelayCommand<string>(param =>
                    {
                        if (param == "Close")
                            Application.Current.Shutdown();
                        else
                            Application.Current.MainWindow.Hide();
                    });
                }

                return _windowButtonCommand;
            }
        }


        public ICommand WindowLoadedCommand
        {
            get => new RelayCommand(() =>
            {
                Application.Current.MainWindow.Hide();
            });
        }

        public ICommand StartTimerCommand
        {
            get => new RelayCommand(() =>
            {
                IsTimerRun = true;
            });
        }

        public ICommand StopTimerCommand
        {
            get => new RelayCommand(() =>
            {
                IsTimerRun = false;
                CurentPage = _pages.Single(p => p.Key == "Default").Value;
            });
        }
        #endregion

        /// <summary>
        /// Opens a window announcing the end of the timer
        /// </summary>
        /// <returns></returns>
        public bool? ShowWindowMessage()
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                _messageWindow = new MessageWindow() { DataContext = _mainViewModel };
                bool? rez = _messageWindow.ShowDialog();
                _messageWindow.Close();
                return rez;
            });

            return null;
        }

    }
}
