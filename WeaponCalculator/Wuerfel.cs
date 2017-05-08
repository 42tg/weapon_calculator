using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace WeaponCalculator
{
    [DebuggerDisplay("{Anzahl}W{Augen}+{Basis}")]
    internal class Wuerfel
    {
        public int Anzahl { get; private set; }
        public int Augen { get; private set; }
        public int Basis { get; private set; }
        public Wuerfel(int Anzahl, int Augen, int Basis = 0)
        {
            this.Anzahl = Anzahl;
            this.Augen = Augen;
            this.Basis = Basis;
        }

        public Wuerfel(string diceString)
        {
            Regex diceNotation = new Regex(@"(?<Anzahl>\d+)W(?<Augen>\d+)(?:\+(?<Basis>\d)|)", RegexOptions.Compiled);
            Match match = diceNotation.Match(diceString);
            if (match.Success)
            {
                Anzahl = Convert.ToInt32(match.Groups["Anzahl"].Value);
                Augen = Convert.ToInt32(match.Groups["Augen"].Value);
                if (match.Groups["Basis"].Success)
                {
                    Basis = Convert.ToInt32(match.Groups["Basis"].Value);
                }
                else
                {
                    Basis = 0;
                }
            }
        }

        public int Roll()
        {
            int randomNumber = RNGCSP.RollDice((byte)Augen);
            return randomNumber;
        }

        public override string ToString()
        {
            return Anzahl + "W" + Augen + "+" + Basis;
        }
    }


}