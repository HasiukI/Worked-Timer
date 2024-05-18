using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Worked_Timer.ViewModel
{
    class MainViewModel
    {
        public WindowViewModel Window { get;}

        public MainViewModel() { 
            Window = new WindowViewModel();
        }
    }
}
