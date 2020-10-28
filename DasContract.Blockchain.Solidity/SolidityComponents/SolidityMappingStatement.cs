using Liquid.NET;
using Liquid.NET.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Blockchain.Solidity.SolidityComponents
{
    public class SolidityMappingStatement : SolidityComponent
    {
        static readonly LiquidTemplate template = LiquidTemplate.Create(
            "{{indent}}mapping()").LiquidTemplate;

        public override LiquidString ToLiquidString(int indent)
        {
            throw new NotImplementedException();
        }

        public override string ToString(int indent = 0)
        {
            throw new NotImplementedException();
        }
    }
}
