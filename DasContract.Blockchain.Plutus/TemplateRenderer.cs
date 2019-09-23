using Fluid;
using System;

namespace DasContract.Blockchain.Plutus
{
    public class TemplateRenderer
    {
        public string Assemble ( string templateSourceCode, object templateModel )
        {
            if (FluidTemplate.TryParse(templateSourceCode, out var template))
            {
                var context = new TemplateContext();
                context.MemberAccessStrategy.Register(templateModel.GetType());
                context.SetValue("p", templateModel);

                return template.Render(context);
            }
            throw new Exception("Template parsing failed.");
        }
    }
}
