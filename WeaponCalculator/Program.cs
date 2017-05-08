namespace Programm
{
    class Programm
    {
        static void Main(string[] args)
        {
            string input = "Waffen.csv";
            string output = "output.csv";
            if (args.Length > 1 && args[1].Length > 0) input = args[1];
            if (args.Length > 2 && args[2].Length > 0) output = args[2];
            new Test.CSVTestrun(input, output);
        }
    }
}
