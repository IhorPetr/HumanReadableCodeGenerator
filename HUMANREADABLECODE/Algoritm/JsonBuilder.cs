using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using  Newtonsoft.Json;

namespace HUMANREADABLECODE.Algoritm
{
    public static class JsonBuilder
    {
        public static void Add36Algoritm()
        {
            string json = File.ReadAllText("conf.json");
            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            for (int i = 0; i < 100; i++)
            {
                jsonObj[(i+1).ToString()] = Generator.GetBase36();
            }
            string output = JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText("conf.json", output);
        }

        public static void ClearJson()
        {
            string json = File.ReadAllText("conf.json");
            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            for (int i = 0; i < 100; i++)
            {
                jsonObj.Property((i + 1).ToString()).Remove();
            }
            string output = JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText("conf.json", output);
        }
    }
}
