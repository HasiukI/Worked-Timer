using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Worked_Timer.ViewModel
{
    public class AnimationViewModel : ViewModelBase
    {
        #region Values
        // Data from the change of which the animation is called

        public Int16 SeterTimeWork { get; set; }
        public Int16 SeterTimeBreak { get; set; }
        public Int16 SeterCycle { get; set; }
        public Int16 SeterDefault { get; set; }
        public Int16 SeterLunch { get; set; }
        public Int16 SeterLunchDrop { get; set; }
        public Int16 SettingLanguage { get; set; }
        public Int16 SettingColor { get; set; }
        public Int16 VisibilityExtraAndMain { get; set; }

        private bool _setterIsFilledMainFields;
        public bool SetterIsFilledMainFields
        {
            get => _setterIsFilledMainFields;
            set
            {
                if (value == _setterIsFilledMainFields) return;
                
                _setterIsFilledMainFields = value;
                onPropertyChanged(nameof(SetterIsFilledMainFields));
            }
        }
        #endregion


        private ICommand _showSetersCommand;
        public ICommand ShowSetersCommand
        {
            get {

                if( _showSetersCommand == null)
                {
                    _showSetersCommand = new RelayCommand<string>(param =>{ShowAnimation(param);});
                }
                return _showSetersCommand;
            }
        }

        /// <summary>
        /// A method that triggers an animation on input String
        /// </summary>
        /// <param name="param"> </param>
        private void ShowAnimation(string param) {
            switch (param)
            {
                case "Work":
                    SeterTimeWork = (SeterTimeWork == 1) ? (Int16)2 : (Int16)1;
                    onPropertyChanged(nameof(SeterTimeWork));
                    break;
                case "Break":
                    SeterTimeBreak = (SeterTimeBreak == 1) ? (Int16)2 : (Int16)1;
                    onPropertyChanged(nameof(SeterTimeBreak));
                    break;
                case "Cycle":
                    SeterCycle = (SeterCycle == 1) ? (Int16)2 : (Int16)1;
                    onPropertyChanged(nameof(SeterCycle));
                    break;
                case "Lunch":
                    if (SeterLunchDrop == (Int16)1 || SeterLunchDrop == (Int16)0)
                    {
                        SeterLunch = (SeterLunch == 1) ? (Int16)2 : (Int16)1;
                    }
                    else
                    {
                        SeterLunch = (SeterLunch == 3) ? (Int16)4 : (Int16)3;
                    }

                    onPropertyChanged(nameof(SeterLunch));
                    break;
                case "Language":
                    SettingLanguage = (SettingLanguage == 1) ? (Int16)2 : (Int16)1;
                    onPropertyChanged(nameof(SettingLanguage));
                    break;
                case "Color":
                    SettingColor = (SettingColor == 1) ? (Int16)2 : (Int16)1;
                    onPropertyChanged(nameof(SettingColor));
                    break;
                case "Default":
                    SeterDefault = (SeterDefault == 1) ? (Int16)2 : (Int16)1;
                    onPropertyChanged(nameof(SeterDefault));
                    break;

            }
        }

        /// <summary>
        /// The method that is called under certain conditions in other Wirevmodel starts the animation on LunchDrop
        /// </summary>
        /// <param name="isHide"></param>
        public void ChangeLunchDrop(bool isHide)
        {
            if (isHide)
            {
                SeterLunchDrop = (Int16)1;
            }
            else
            {  
                SeterLunchDrop = (Int16)2; 
            }
            onPropertyChanged(nameof(SeterLunchDrop));
        }

        /// <summary>
        /// The method that is called under certain conditions in other Wirevmodel starts the animation on ExtraAndMainInfo
        /// </summary>
        /// <param name="isStopExtra"></param>
        public void ChangeExtraAndMainInfo(bool isStopExtra)
        {
            if (isStopExtra)
            {
                VisibilityExtraAndMain = 2;
            }
            else
            {
                VisibilityExtraAndMain = 1;
            }

            onPropertyChanged(nameof(VisibilityExtraAndMain));
        }
       
  
    }
}

