using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Worked_Timer.ViewModel
{
    public class MainViewModel
    {
        public WindowViewModel Window { get;}
        public TimerViewModel Timer { get;}
        public PropertyViewModel Property { get;}
        public AnimationViewModel Animation { get;}
        public Configuration Config { get;}
        public MessageViewModel Message { get;}


        public MainViewModel() {
            Config = new Configuration();
            Property = new PropertyViewModel(Config);
            Message = new MessageViewModel(Property);
            Animation = new AnimationViewModel();
            Window = new WindowViewModel(this);
            Timer = new TimerViewModel(Window, Config, Animation,Message,Property);
        }
    }
}
