using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Test
{
    class CSVTestrun
    {
        public CSVTestrun(string inputFile = "Waffen.csv", string outputFile = "output.csv")
        {
            if (!File.Exists(inputFile)) { Console.WriteLine("Please provide an Input File"); return; }
            var lines = File.ReadAllLines(inputFile);

            int numberOfTestruns = 100000;

            int maxArmor = 6;

            List<string> output = new List<string>();
            foreach (var row in lines)
            {
                var col = row.Split(';');
                if (col[0].Length <= 0 || col[1].Length <= 0 || col[2].Length <= 0) { continue; }
                int WGS = Convert.ToInt32(col[2]);
                var weapon = new WeaponCalculator.Weapon(WGS, col[1], row);
                weapon.Name = col[0];

                List<double> results = new List<double>();
                for (int armor = 0; armor <= maxArmor; armor += 2)
                {
                    for (int testrun = 0; testrun < numberOfTestruns / maxArmor; testrun++)
                    {
                        results.Add(weapon.DPT(WeaponCalculator.RNGCSP.RollDice(6) - 1, armor));
                    }
                }

                double Average = 0;
                results.ForEach(value => Average += value);
                Average /= results.Count;

                string toWrite = weapon.Name + ";" + weapon.Wuerfel + ";" + String.Format("{0:0.00}", Average) + ";" + String.Format("{0:0.00}", results.Min()) + ";" + String.Format("{0:0.00}", results.Max()) + ";" + weapon.Kritisch + ";" + weapon.Scharf + ";" + weapon.Durchdringung + ";" + weapon.Exakt + ";" + weapon.Wuchtig;
                output.Add(toWrite);
            }

            File.WriteAllLines(outputFile, output);
        }

    }
}
