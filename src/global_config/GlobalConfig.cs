using System;
using System.Collections;
using System.Collections.Generic;

namespace compiler_c0.global_config
{
    public class GlobalConfig
    {
        private GlobalConfig()
        {
        }

        private static GlobalConfig _instance;

        private readonly Dictionary<string, object> _configs = new();  

        public static GlobalConfig Instance
        {
            get { return _instance ??= new GlobalConfig(); }
        }

        public string InputFilePath
        {
            get => (string) _configs["InputFilePath"];
            set => _configs["InputFilePath"] = value;
        }

        public string OutputFilePath
        {
            get => (string) _configs["OutputFilePath"];
            set => _configs["OutputFilePath"] = value;
        }
    }
}