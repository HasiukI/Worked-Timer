using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Worked_Timer.ViewModel
{
    class WindowViewModel : ViewModelBase
    {

        public WindowViewModel() {
            // Set Position in right and bottom
            Application.Current.Dispatcher.Invoke(() =>
            {
                var screen = SystemParameters.WorkArea;
                TopPosition = screen.Bottom - Application.Current.MainWindow.Height;
                LeftPosition = screen.Right - Application.Current.MainWindow.Width;
                Application.Current.MainWindow.UpdateLayout();
            }, System.Windows.Threading.DispatcherPriority.Loaded);
        }

        #region Property
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
                        });
                }
                return _showWindowCommand;
            }
        }

        public ICommand WindowLoadedCommand
        {
            get => new RelayCommand(() =>
            {
               
                //Disabled View
                Application.Current.MainWindow.Hide();
            });
        }


        /// <summary>
        /// Full Stop and Close Program
        /// </summary>
        public ICommand CloseProgram
        {
            get => new RelayCommand(() =>
            {


                Application.Current.Shutdown();
            });
        }

        /// <summary>
        /// HideProgram
        /// </summary>
        public ICommand HideProgram
        {
            get => new RelayCommand(() =>
            {
                Application.Current.MainWindow.Hide();
            });
        }
        #endregion
    }
}
