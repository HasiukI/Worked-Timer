using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Worked_Timer.Model;

namespace Worked_Timer.ViewModel
{
    public class MessageViewModel : ViewModelBase
    {
        private PropertyViewModel _properrty;
        public bool IsNext;

        public MessageViewModel(PropertyViewModel properrty)
        {
            _properrty = properrty;
            VisibilityMain = true;
            VisibilityLast = false;
        }

        #region Values

        public bool VisibilityMain { get; set; }
        public bool VisibilityLast { get; set; }
        public string WorkTime { get; set; }
        public string BreakTime { get; set; }


        private bool _isLast;
        public bool IsLast
        {
            get => _isLast;
            set
            {
                if (_isLast == value) return;
                _isLast = value;
                VisibilityMain = false;
                VisibilityLast = true;
                onPropertyChanged(nameof(WorkTime));
                onPropertyChanged(nameof(BreakTime));
            }
        }
        
        private MomentTimer _moment;
        public MomentTimer Moment
        {
            get => _moment;
            set
            {
                if (value != _moment)
                {
                    _moment = value;
                    _moment.Message = _properrty.GetMessage(value.ActionOfTime);
                    onPropertyChanged(nameof(Moment));
                }
            }
        }

        #endregion

        /// <summary>
        /// Checks what was pressed or further or additional time
        /// </summary>
        public ICommand ClickButtonCommand
        {
            get => new RelayCommand<string>(param =>
                {
                    if (param == "Next")
                        IsNext = true;
                    else
                        IsNext = false;
                });
        }

   
    }
}
