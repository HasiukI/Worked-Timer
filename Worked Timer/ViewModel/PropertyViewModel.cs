using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Worked_Timer.Model;

namespace Worked_Timer.ViewModel
{
    public class PropertyViewModel : ViewModelBase
    {
        private Configuration _configuration;

        public PropertyViewModel(Configuration configuration)
        {
            _configuration = configuration;
            setImages();
            SetColor();
            setLanguages();

        }

        #region Values

        private List<ImageSourse> ImageSourses;
        public List<Language> Languages { get; set; }
        public BackgroundColor CurentBackGround { get; set; }
        public List<ColorMain> MainColors { get; set; }
        public List<ColorAccent> AccentColors { get; set; }

        private Language _curentLanguage;
        public Language CurentLanguage
        {
            get => _curentLanguage;
            set
            {
                if (_curentLanguage != value)
                {
                    _curentLanguage = value;
                    Task.Run(() => _configuration.SetParametr(new AppConfig() { Language = value.LanguageName }));
                    onPropertyChanged(nameof(CurentLanguage));
                }

            }
        }

        private ImageSourse _curentImageSourse;
        public ImageSourse CurentImageSourse
        {
            get => _curentImageSourse;
            set
            {
                if (_curentImageSourse != value)
                {
                    _curentImageSourse = value;
                    onPropertyChanged(nameof(CurentImageSourse));
                }
            }
        }

        private ColorAccent _curentAccentColor;
        public ColorAccent CurentAccentColor
        {
            get => _curentAccentColor;
            set
            {
                if (value != _curentAccentColor)
                {
                    _curentAccentColor = value;
                    CurentBackGround.Accent = value;
                    onPropertyChanged(nameof(CurentBackGround));
                    Task.Run(() => _configuration.SetParametr(new AppConfig() { ColorOther = value.Name }));
                    onPropertyChanged(nameof(CurentAccentColor));
                }
            }
        }

        private ColorMain _curentColorMain;
        public ColorMain CurentColorMain
        {
            get => _curentColorMain;
            set
            {
                if (value != _curentColorMain)
                {
                    _curentColorMain = value;
                    CurentBackGround.Main = value;
                    CurentImageSourse = ImageSourses.Single(i => i.NameBrush == value.IamgeBrush);
                    Task.Run(() => _configuration.SetParametr(new AppConfig() { ColorMain = value.Name }));
                    onPropertyChanged(nameof(CurentBackGround));
                    onPropertyChanged(nameof(CurentColorMain));
                }
            }
        }
        #endregion


        /// <summary>
        /// Fills the paths to photos and light and dark
        /// </summary>
        private void setImages()
        {
            string path = "pack://application:,,,/images/";
            Dictionary<string, string> black = new Dictionary<string, string>();
            black.Add("Arrow", path + "ArrowBlack.svg");
            black.Add("Break", path + "BreakBlack.svg");
            black.Add("Close", path + "CloseBlack.svg");
            black.Add("Cycle", path + "CycleBlack.svg");
            black.Add("Destroy", path + "DestroyBlack.svg");
            black.Add("Home", path + "HomeBlack.svg");
            black.Add("Lunch", path + "LunchBlack.svg");
            black.Add("Pause", path + "PauseBlack.svg");
            black.Add("Play", path + "PlayBlack.svg");
            black.Add("Setting", path + "SettingBlack.svg");
            black.Add("Work", path + "WorkBlack.svg");

            Dictionary<string, string> white = new Dictionary<string, string>();
            white.Add("Arrow", path + "ArrowWhite.svg");
            white.Add("Break", path + "BreakWhite.svg");
            white.Add("Close", path + "CloseWhite.svg");
            white.Add("Cycle", path + "CycleWhite.svg");
            white.Add("Destroy", path + "DestroyWhite.svg");
            white.Add("Home", path + "HomeWhite.svg");
            white.Add("Lunch", path + "LunchWhite.svg");
            white.Add("Pause", path + "PauseWhite.svg");
            white.Add("Play", path + "PlayWhite.svg");
            white.Add("Setting", path + "SettingWhite.svg");
            white.Add("Work", path + "WorkWhite.svg");

            ImageSourses = new List<ImageSourse>();
            ImageSourses.Add(new ImageSourse() { NameBrush="Black", Sourse = black });
            ImageSourses.Add(new ImageSourse() { NameBrush="White", Sourse = white });

          
        }

        /// <summary>
        /// Fills languages
        /// </summary>
        private void setLanguages()
        {
            Language ukr = new Language() {
                LanguageName = "Ukr",
                Image = "pack://application:,,,/images/ukraine.png",
                Strings = new Dictionary<string, string>()
            };

            ukr.Strings.Add("DefaultNew", "Задати нові значення");
            ukr.Strings.Add("DefaultLast", "Використати попередні значення");
            ukr.Strings.Add("DefaultLastNull", "Ще не має збережених даних");

            ukr.Strings.Add("SettingLanguage", "Мова");
            ukr.Strings.Add("SettingColor", "Колір");
            ukr.Strings.Add("SettingColorMain", "Основний");
            ukr.Strings.Add("SettingColorAdditional", "Додатковий");

            ukr.Strings.Add("SetterWork", "Робота");
            ukr.Strings.Add("SetterWorkInfo", "К-сть хвилин на роботу");
            ukr.Strings.Add("SetterBreak", "Перерва");
            ukr.Strings.Add("SetterBreakInfo", "К-сть хвилин на перерву");
            ukr.Strings.Add("SetterCycle", "Повторення");
            ukr.Strings.Add("SetterCycleInfo", "К-сть повторів");    
            ukr.Strings.Add("SetterShowaAllSetter", "Заповність попередні поля");
            ukr.Strings.Add("SetterLunch", "Обід");
            ukr.Strings.Add("SetterLunchSelect", "Виберіть перерву щоб замінити на обід");
            ukr.Strings.Add("SetterLunchInfo", "К-сть хвилин на обід");

            ukr.Strings.Add("SetterExeprionLong", "Задовге значення");
            ukr.Strings.Add("SetterExeprionNumber", "Тільки числа");
            ukr.Strings.Add("SetterExeprionZero", "Не починати з 0");
            ukr.Strings.Add("SetterExeprionMax15", "Максимальне значення - 15");

            ukr.Strings.Add("MainPlay", "Продовжити");
            ukr.Strings.Add("MainPause", "Зупинити");
            ukr.Strings.Add("MainDestroy", "Виключити");
            ukr.Strings.Add("MainStopExtra", "Продовжити цикл");

            ukr.Strings.Add("OtherBack", "Назад");
            ukr.Strings.Add("OtherStart", "Розпочати");
            ukr.Strings.Add("OtherTotal", "Загальний час:");

            ukr.Strings.Add("WindowWork", "Пора продовжити роботу");
            ukr.Strings.Add("WindowBreak", "Озирніться на вкруги, відпочиньте від роботи");
            ukr.Strings.Add("WindowLunch", "Було б добре зараз поїсти");
            ukr.Strings.Add("WindowNext", "Далі");
            ukr.Strings.Add("WindowStop", "Зачекати");
            ukr.Strings.Add("WindowFinishLabel", "Хороша робота");
            ukr.Strings.Add("WindowFinishWork", "Час затрачений на роботу: ");
            ukr.Strings.Add("WindowFinishBreak", "Час затрачений на перерви: ");
            ukr.Strings.Add("WindowFinishButton", "Вітаю");
            

            Language eng = new Language()
            {
                LanguageName = "Eng",
                Image = "pack://application:,,,/images/united-states.png",
                Strings = new Dictionary<string, string>()
            };
        
            eng.Strings.Add("DefaultNew", "Set New Values");
            eng.Strings.Add("DefaultLast", "Use previous values");
            eng.Strings.Add("DefaultLastNull", "Doesn't have any data saved yet");

            eng.Strings.Add("SettingLanguage", "Language");
            eng.Strings.Add("SettingColor", "Color");
            eng.Strings.Add("SettingColorMain", "Main");
            eng.Strings.Add("SettingColorAdditional", "Other");
           

            eng.Strings.Add("SetterWork", "Work");
            eng.Strings.Add("SetterWorkInfo", "Number of minutes to work");
            eng.Strings.Add("SetterBreak", "Break");
            eng.Strings.Add("SetterBreakInfo", "Number of minutes for a break");
            eng.Strings.Add("SetterCycle", "Recurrence");
            eng.Strings.Add("SetterCycleInfo", "Number of repetitions");
            eng.Strings.Add("SetterShowaAllSetter", "Fill in the previous fields to continue");
            eng.Strings.Add("SetterLunch", "Lunch");
            eng.Strings.Add("SetterLunchSelect", "Choose a break to replace with lunch");
            eng.Strings.Add("SetterLunchInfo", "Number of minutes for lunch");

            eng.Strings.Add("SetterExeprionLong", "Too long");
            eng.Strings.Add("SetterExeprionNumber", "Numbers only");
            eng.Strings.Add("SetterExeprionZero", "Can't start with 0");
            eng.Strings.Add("SetterExeprionMax15", "Max count - 15");

            eng.Strings.Add("MainPlay", "Play");
            eng.Strings.Add("MainPause", "Pausa");
            eng.Strings.Add("MainDestroy", "Destroy");
            eng.Strings.Add("MainStopExtra", "Continue the cycle");

            eng.Strings.Add("OtherBack", "Back");
            eng.Strings.Add("OtherStart", "Start");
            eng.Strings.Add("OtherTotal", "Total time:");

            eng.Strings.Add("WindowWork", "It's time to get on with it");
            eng.Strings.Add("WindowBreak", "Take a look around, take a break from work");
            eng.Strings.Add("WindowLunch", "It would be nice to eat now");
            eng.Strings.Add("WindowNext", "Next step");
            eng.Strings.Add("WindowStop", "Wait");
            eng.Strings.Add("WindowFinishLabel", "Well done");
            eng.Strings.Add("WindowFinishWork", "Time spent on work: ");
            eng.Strings.Add("WindowFinishBreak", "Time spent on breaks: ");
            eng.Strings.Add("WindowFinishButton", "Congratulations");

            Languages = new List<Language>();
            Languages.Add(ukr);
            Languages.Add(eng);

           
            CurentLanguage = Languages.Single(l => l.LanguageName == _configuration.Config.Language);
        }

        /// <summary>
        /// Fill colors
        /// </summary>
        private void SetColor()
        {
            AccentColors = new List<ColorAccent>();
            AccentColors.Add(new ColorAccent() { Name = "Blue", Accent = "#567bd2", Enter = "#4966c7", Font = "#ffffff" });
            AccentColors.Add(new ColorAccent() { Name = "Pink", Accent = "#d581d2", Enter = "#c96cc5", Font = "#ffffff" });
            AccentColors.Add(new ColorAccent() { Name = "Green", Accent = "#61b45f", Enter = "#4b9e49", Font = "#ffffff" });
            AccentColors.Add(new ColorAccent() { Name = "Yellow", Accent = "#e4d858", Enter = "#dcc633", Font = "#ffffff" });
            AccentColors.Add(new ColorAccent() { Name = "Red", Accent = "#e82121", Enter = "#c31212", Font = "#ffffff" });
            AccentColors.Add(new ColorAccent() { Name = "Cyan", Accent = "#21e4e8", Enter = "#0bc8cf", Font = "#ffffff" });

            MainColors = new List<ColorMain>();
            MainColors.Add(new ColorMain() { Name = "Light", Main = "#fbfbfd", Bottom = "#e2f1fc", Section = "#ffffff", Enter = "#dee2e7", Font = "#0d2b44", IamgeBrush = "Black", Shadow = "#999999" });
            MainColors.Add(new ColorMain() { Name = "Dark", Main = "#333333", Bottom = "#2e2e2e", Section = "#3b3b3b", Enter = "#454545", Font = "#ffffff", IamgeBrush = "White", Shadow = "#ededed" });

            CurentBackGround = new BackgroundColor()
            {
                Main = MainColors.Single(m => m.Name == _configuration.Config.ColorMain),
                Accent = AccentColors.Single(a => a.Name == _configuration.Config.ColorOther)
            };
            CurentImageSourse = ImageSourses.Single(i => i.NameBrush == CurentBackGround.Main.IamgeBrush);
        }

        /// <summary>
        /// processes the value argument and returns an error with the selected language
        /// </summary>
        /// <param name="exeption"></param>
        /// <returns></returns>
        public string GetExeption(ExeptionValue exeption)
        {
            switch(exeption)
            {
                case ExeptionValue.toLong:
                   return CurentLanguage.Strings["SetterExeprionLong"];
                case ExeptionValue.noteANumber:
                    return CurentLanguage.Strings["SetterExeprionNumber"];
                case ExeptionValue.startZero:
                    return CurentLanguage.Strings["SetterExeprionZero"];
                case ExeptionValue.Max15:
                    return CurentLanguage.Strings["SetterExeprionMax15"];
            }
            return string.Empty;
        }

        /// <summary>
        /// processes the value argument and returns a message in the selected language
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public string GetMessage(ActionOfTime action)
        {
            switch (action)
            {
                case ActionOfTime.Work: 
                    return CurentLanguage.Strings["WindowWork"];
                case ActionOfTime.Rest: 
                    return CurentLanguage.Strings["WindowBreak"];
                case ActionOfTime.BigRest: 
                    return CurentLanguage.Strings["WindowLunch"];
            }
            return string.Empty;
        }

       

    }
}
