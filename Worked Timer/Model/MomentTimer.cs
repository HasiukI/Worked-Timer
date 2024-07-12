using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Worked_Timer.Model
{
    public class MomentTimer : ViewModel.ViewModelBase
    {
        private int _position;
        public int Position { 
            get => _position;
            set { 
                if(value != _position)
                {
                    _position = value;
                    onPropertyChanged(nameof(Position));
                }
            } 
        }

        private ActionOfTime _actionOfTime;
        public ActionOfTime ActionOfTime { 
            get =>_actionOfTime;
            set { 
                if( value != _actionOfTime)
                {
                    _actionOfTime = value;
                    onPropertyChanged(nameof(ActionOfTime));
                }
            } 
        }

        private string _message;
        public string Message {
            get => _message;
            set { 
                if(value != _message)
                {
                    _message = value;
                    onPropertyChanged(nameof(Message));
                }    
            } 
        }

        private string _intervalTime;
        public string IntervalTime
        {
            get => _intervalTime;
            set
            {
                if (value != _intervalTime)
                {
                    _intervalTime = value;
                    onPropertyChanged(nameof(IntervalTime));
                }
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

        private string _nameImageForMain;
        public string NameImageForMain
        {
            get => _nameImageForMain;
            set
            {
                if (value != _nameImageForMain)
                {
                    _nameImageForMain = value;
                    onPropertyChanged(nameof(NameImageForMain));
                }
            }
        }
    }

    public enum ActionOfTime
    {
        Work,
        Rest,
        BigRest
    }
}
