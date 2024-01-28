using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DietAppClient.Helpers
{
    internal class JsonParser
    {
        public static async void WriteJson<T>(T item, string filePath) 
        {
            string json = JsonConvert.SerializeObject(item);
            string targetFile = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, filePath);
            using FileStream outputStream = System.IO.File.OpenWrite(targetFile);
            using StreamWriter streamWriter = new StreamWriter(outputStream);
            await streamWriter.WriteAsync(json);
        }

        public static T ReadJson<T>(T accumlator, string filePath)
        {
            if (File.Exists(filePath))
            {
                string jsonContent = File.ReadAllText(filePath);
                if (!string.IsNullOrEmpty(jsonContent))
                   accumlator = JsonConvert.DeserializeObject<T>(jsonContent);
            }
            return accumlator;
        }

    }
}
