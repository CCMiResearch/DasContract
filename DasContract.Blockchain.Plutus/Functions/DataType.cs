using System.Collections.Generic;
using System.Linq;

namespace DasContract.Blockchain.Plutus.Functions
{
    public class DataType : Function
    {
        public IList<string> Members { get; set; } = new List<string>();

        public IList<string> MemberNames { get; set; } = new List<string>();

        public IList<string> DerivingTypes { get; set; } = new List<string>();

        public bool MakeLift { get; set; }

        public bool MakeIsData { get; set; }

        public bool AddMember ( string name, string type)
        {
            if (MemberNames.Any(f => Equals(name, f)))
            {
                return false;
            }
            MemberNames.Add(name);
            Members.Add($"{name} :: {type}");
            return true;
        }

        public bool AddDerivingType(string type)
        {
            if (DerivingTypes.Any(f => Equals(type, f)))
            {
                return false;
            }
            DerivingTypes.Add(type);
            return true;
        }
    }
}