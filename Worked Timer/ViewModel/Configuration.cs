using Newtonsoft.Json;
using SharpVectors.Renderers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Worked_Timer.Model;

namespace Worked_Timer.ViewModel
{
    public class Configuration
    {
        private string path = string.Empty;
        public AppConfig Config { get; set; }

        public Configuration() {
            path = path = System.IO.Directory.GetCurrentDirectory() + "\\config.json";
            
            var json = File.ReadAllText(path);
            Config = JsonConvert.DeserializeObject<AppConfig>(json);
        }


        /// <summary>
        /// Iterates data that is not null and writes it to Cached data
        /// </summary>
        /// <param name="newConfig"></param>
        /// <returns></returns>
        public async Task SetParametr(AppConfig newConfig)
        {
            Type type = typeof(AppConfig);
            PropertyInfo[] properties = type.GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(newConfig);
                if (value != null)
                {
                    property.SetValue(Config, value);
                }
            }
           await Save();
        }

        /// <summary>
        /// Saves the Config file locally on the computer
        /// </summary>
        /// <returns></returns>
        private async Task Save()
        {
            var json = JsonConvert.SerializeObject(Config);
            await File.WriteAllTextAsync(path, json);
        }

    }
}
