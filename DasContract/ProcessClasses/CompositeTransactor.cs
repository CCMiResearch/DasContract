using System.Collections.Generic;

namespace DasContract
{
    public class CompositeTransactor : Transactor
    {
        public IList<ElementaryTransactor> Children { get; set; } = new List<ElementaryTransactor>();
    }
}