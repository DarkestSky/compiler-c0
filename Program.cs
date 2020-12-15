using System;
using compiler_c0.char_parser;
using compiler_c0.global_config;
using compiler_c0.tokenizer;
using compiler_c0.tokenizer.token.token_extension;

namespace compiler_c0
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine('1'.IsFirstOfOperator());
            var globalConfig = GlobalConfig.Instance;
            
            globalConfig.InputFilePath = args[0];
            globalConfig.OutputFilePath = args[1];

            var charParser = CharParser.Instance;
            var tokenizer = Tokenizer.Instance;
            while (tokenizer.HasNext())
            {
                Console.WriteLine(tokenizer.NextToken());
            }
        }
    }
}