using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Worked_Timer.Model
{
    public class Language
    {
        public string LanguageName {  get; set; }
        public string Image {  get; set; }
        public Dictionary<string, string> Strings { get; set; } 
    }
}
