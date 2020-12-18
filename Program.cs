using compiler_c0.analyser;
using compiler_c0.global_config;
using compiler_c0.symbol_manager;

namespace compiler_c0
{
    class Program
    {
        static void Main(string[] args)
        {
            var globalConfig = GlobalConfig.Instance;
            var symbolManager = SymbolManager.Instance;
            
            globalConfig.InputFilePath = args[0];
            globalConfig.OutputFilePath = args[1];

            var analyser = Analyser.Instance;
            
            analyser.Analyse();
            symbolManager.Generator();
        }
    }
}