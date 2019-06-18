using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TimesheetCore {
    public class ConfigurationLayer {

        public static string GetConfig(string key) {
 
            return XElement.Load("Config.xml").Element(key).Attribute("Value").Value;
            

        }

        public static void SetConfig(string key, string value) {

            var config = XElement.Load("Config.xml");

            config.Element(key).Attribute("Value").Value = value;

            config.Save("Config.xml");

        }
    }
}
