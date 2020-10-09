using Liquid.NET.Constants;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DasContract.Blockchain.Solidity.SolidityComponents
{
    public class SolidityStatement : SolidityComponent
    {
        public List<Tuple<string, bool>> Statements = new List<Tuple<string, bool>>();

        public List<string> GetStatements()
        {
            return Statements.Select(_ => _.Item1).ToList();
        }

        public SolidityStatement(string statement, bool semicolon = true)
        {
            Add(statement, semicolon);
        }
        public SolidityStatement() { }
        public SolidityStatement(List<string> statements)
        {
            foreach (var s in statements)
            {
                Add(s, true);
            }
        }
        public void Add(string statement, bool semicolon)
        {
            Statements.Add(new Tuple<string, bool>(statement, semicolon));
        }

        public void Add(string statement)
        {
            Add(statement, true);
        }
        public override LiquidString ToLiquidString(int indent)
        {
            return LiquidString.Create(ToString(indent));
        }

        public override string ToString(int indent = 0)
        {
            string statement = "";
            foreach (var t in Statements)
            {
                statement += CreateIndent(indent) + t.Item1;
                if (t.Item2 == true) statement += ";";
                statement += "\n";
            }
            return statement;
        }

    }
}
