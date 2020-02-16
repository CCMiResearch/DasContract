using DasContract.Abstraction.Data;

namespace DasContract.Abstraction.UserInterface
{
    public class FormField
    {
        public string Id { get; set; }
        public int Order { get; set; }
        public FormFieldType Type { get; set; }
        /// <summary>
        /// A visible name displayed next to the field. 
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// If false, an editor is shown. Otherwise property shown as a label. 
        /// </summary>
        public bool IsReadOnly { get; set; } = true; 
        /// <summary>
        /// An expression binding. Entity.Property
        /// </summary>
        public string PropertyExpression { get; set; }

        public string CustomConfiguration { get; set; }
    }
}
