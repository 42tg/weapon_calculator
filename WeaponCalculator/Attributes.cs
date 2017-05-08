using System.Diagnostics;

namespace WeaponCalculator
{
    [DebuggerDisplay("{Name} ({Value})")]
    internal class Attribute<T>
    {
        public string Name { get; private set; }
        public T Value { get; set; }

        public Attribute(string Name, T Value)
        {
            this.Name = Name;
            this.Value = Value;
        }

        public override string ToString()
        {
            return Name + " (" + Value + ")";
        }
    }
}