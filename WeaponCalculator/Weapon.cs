using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace WeaponCalculator
{
    [DebuggerDisplay("{Name}")]
    class Weapon
    {
        public string Name { get; set; } = "";
        public int WGS { get; private set; }
        public Wuerfel Wuerfel { get; private set; }

        public Attribute<int> Kritisch { get; private set; } = new Attribute<int>("Kritisch", 0);
        public Attribute<int> Durchdringung { get; private set; } = new Attribute<int>("Durchdringung", 0);
        public Attribute<int> Scharf { get; private set; } = new Attribute<int>("Scharf", 0);
        public Attribute<int> Exakt { get; private set; } = new Attribute<int>("Exakt", 0);

        public Attribute<bool> Wuchtig { get; private set; } = new Attribute<bool>("Wuchtig", false);
        public Attribute<bool> Vielseitig { get; private set; } = new Attribute<bool>("Vielseitig", false);

        public Weapon(int WGS, string diceNotation)
        {
            this.WGS = WGS;
            Wuerfel = new Wuerfel(diceNotation);
        }
        public Weapon(int WGS, string diceNotation, string attributes) : this(WGS, diceNotation, attributes.Split(';')) { }

        public Weapon(int WGS, string diceNotation, string[] attributes) : this(WGS, diceNotation)
        {
            Regex lookFor = new Regex(@"(?<Merkmal>[a-zA-ZäüöÄÜÖ]+)(?:.*\((?<Count>\d)\)|)", RegexOptions.Compiled);
            foreach (var attribute in attributes)
            {
                var match = lookFor.Match(attribute);
                if (match.Success)
                {
                    if (match.Groups["Merkmal"].Success)
                    {
                        if (match.Groups["Merkmal"].Value == Kritisch.Name) { Kritisch.Value = Convert.ToInt32(match.Groups["Count"].Value); }
                        else if (match.Groups["Merkmal"].Value == Durchdringung.Name) { Durchdringung.Value = Convert.ToInt32(match.Groups["Count"].Value); }
                        else if (match.Groups["Merkmal"].Value == Scharf.Name) { Scharf.Value = Convert.ToInt32(match.Groups["Count"].Value); }
                        else if (match.Groups["Merkmal"].Value == Exakt.Name) { Exakt.Value = Convert.ToInt32(match.Groups["Count"].Value); }
                        else if (match.Groups["Merkmal"].Value == Wuchtig.Name) { Wuchtig.Value = true; }
                        else if (match.Groups["Merkmal"].Value == Vielseitig.Name) { Wuchtig.Value = true; }
                    }
                }
            }
        }

        public double DPT(int EG = 0, int Armor = 0)
        {
            double result = (double)Roll(EG, Armor) / (double)WGS;
            return result;
        }
        public int Roll(int EG = 0, int Armor = 0)
        {
            int summe = 0;
            for (int i = 0; i < Wuerfel.Anzahl; i++)
            {
                int tmp = Wuerfel.Roll();
                if (Exakt.Value > 0)
                {
                    for (int exakt = 0; exakt < Exakt.Value; exakt++)
                    {
                        int zweitWurf = Wuerfel.Roll();
                        if (tmp < zweitWurf) tmp = zweitWurf;
                    }
                }
                if (Scharf.Value > 0 && tmp < Scharf.Value) { tmp = Scharf.Value; }
                if (Kritisch.Value > 0 && tmp == Wuerfel.Augen) { tmp += Kritisch.Value; }

                summe += tmp;
            }

            if (Armor > 0)
            {
                summe -= Math.Max(0, Armor - Durchdringung.Value);
            }

            if (Wuchtig.Value)
            {
                summe += EG * 2;
            }
            else
            {
                summe += EG;
            }

            if (Vielseitig.Value)
            {
                summe += 3;
            }

            summe += Wuerfel.Basis;

            return summe;
        }
    }
}
