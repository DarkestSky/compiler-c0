using compiler_c0.analyser;
using compiler_c0.global_config;

namespace compiler_c0
{
    class Program
    {
        static void Main(string[] args)
        {
            var globalConfig = GlobalConfig.Instance;
            
            globalConfig.InputFilePath = args[0];
            globalConfig.OutputFilePath = args[1];

            var analyser = Analyser.Instance;
            
            analyser.Analyse();
        }
    }
}