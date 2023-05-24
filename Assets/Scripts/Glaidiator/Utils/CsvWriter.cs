using System;
using System.IO;
using UnityEngine;

namespace Glaidiator.Utils
{
    public class CsvWriter
    {
        private readonly string _filepath;
        private readonly char _separator;
        public CsvWriter(string path, string filename, string[] columns, char separator)
        {
            string dir = Path.Join(Application.dataPath, path);
            if (!dir.EndsWith('/')) dir += '/';
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            _filepath = $"{dir}{filename}.csv";
            _separator = separator;
            using var w = new StreamWriter(_filepath, false);
            w.WriteLine(string.Join(_separator, columns));
        }
        
        public void Write(string[] values)
        {
            using var w = new StreamWriter(_filepath, true);
            w.WriteLine(string.Join(_separator, values));
        }
    }
}