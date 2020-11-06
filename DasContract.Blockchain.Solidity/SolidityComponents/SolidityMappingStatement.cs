using DasContract.Abstraction.Data;
using Liquid.NET;
using Liquid.NET.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DasContract.Blockchain.Solidity.SolidityComponents
{
    public class SolidityMappingStatement : SolidityComponent
    {
        List<Property> nestedProperties;
        string lastKeyType;
        string lastValueType;
        string mappingName;

        static readonly LiquidTemplate innerTemplate = LiquidTemplate.Create(
            "mapping({{keyType}} => {{valueType}})").LiquidTemplate;

        static readonly LiquidTemplate outerTemplate = LiquidTemplate.Create(
            "{{indent}}{{mappingStatement}} {{mappingName}}").LiquidTemplate;

        public SolidityMappingStatement (string lastKeyType, string lastValueType, string mappingName)
        {
            this.lastKeyType = lastKeyType;
            this.lastValueType = lastValueType;
            this.mappingName = mappingName;
            nestedProperties = new List<Property>();
        }

        public SolidityMappingStatement(string lastKeyType, string lastValueType, string mappingName, List<Property> nestedProperties)
        {
            this.lastKeyType = lastKeyType;
            this.lastValueType = lastValueType;
            this.mappingName = mappingName;
            this.nestedProperties = nestedProperties;
        }

        public override LiquidString ToLiquidString(int indent)
        {
            return LiquidString.Create(ToString(indent));
        }

        LiquidString GetMappingStatement(List<Property> properties)
        {
            ITemplateContext ctx = new TemplateContext();
            if (properties.Count == 0)
            {
                ctx.DefineLocalVariable("keyType", LiquidString.Create(lastKeyType)).
                    DefineLocalVariable("valueType", LiquidString.Create(lastValueType));
            }
            else
            {
                var currProperty = properties.First();
                ctx.DefineLocalVariable("keyType", LiquidString.Create(Helpers.PropertyTypeToString(currProperty.DataType))).
                    DefineLocalVariable("valueType", GetMappingStatement(properties.Skip(1).ToList()));
            }
            return LiquidString.Create(innerTemplate.Render(ctx).Result);
        }

        public override string ToString(int indent = 0)
        {
            ITemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("indent", CreateIndent(indent)).
                DefineLocalVariable("mappingStatement", GetMappingStatement(nestedProperties)).
                DefineLocalVariable("mappingName", LiquidString.Create(mappingName));
            return outerTemplate.Render(ctx).Result;
        }
    }
}
