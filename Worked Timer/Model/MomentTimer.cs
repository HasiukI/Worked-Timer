using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Worked_Timer.Model
{
    class MomentTimer
    {
        public int Position { get; set; }
        public ActionOfTime ActionOfTime { get; set; }
        public string Message { get; set; }
    }

    enum ActionOfTime
    {
        Work,
        Rest,
        BigRest
    }
}
