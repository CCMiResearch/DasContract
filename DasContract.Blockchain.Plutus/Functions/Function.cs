using System.Collections.Generic;

namespace DasContract.Blockchain.Plutus.Functions
{
    public class Function
    {
        public string Name { get; set; }

        public string GeneratedFunction { get; set; }

        public string TemplateSourceCode { get; set; }

        public IList<string> Libraries { get; set; } = new List<string>();

        public void RenderTemplate ()
        {
            var renderer = new FluidTemplateRenderer();

            GeneratedFunction = renderer.Assemble(TemplateSourceCode, this);
        }
    }
}
