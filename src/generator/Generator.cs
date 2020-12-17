namespace compiler_c0.analyser
{
    public class Generator
    {
        private Generator()
        {
        }

        private static Generator _instance;

        public static Generator Instance
        {
            get { return _instance ??= new Generator(); }
        }
    }
}