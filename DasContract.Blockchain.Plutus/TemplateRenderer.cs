using Fluid;

namespace DasContract.Blockchain.Plutus
{
    public static class TemplateRenderer
    {
        public static string Assemble ( string templateSourceCode, object templateModel )
        {
            if (FluidTemplate.TryParse(templateSourceCode, out var template))
            {
                var context = new TemplateContext();
                context.MemberAccessStrategy.Register(templateModel.GetType());
                context.SetValue("p", templateModel);

                return template.Render(context);
            }
            return null;
        }
    }
}
