using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using NUnit.Framework;
using Bonsai.Utils.Property;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace Bonsai.Tests.Property
{
    class PropertyAttributeGetterExtensionsFoo
    {
        [Description("DescriptionText")]
        [DisplayName("FooyDisplayName")]
        [Required]
        public string Fooy { get; set; } = "";

        public string Foon { get; set; }
    }

    public class PropertyAttributeGetterExtensions
    {

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void NameGet()
        {
            var foo = new PropertyAttributeGetterExtensionsFoo();
            Expression<Func<PropertyAttributeGetterExtensionsFoo, string>> expression1 = foo => foo.Fooy;
            Expression<Func<string>> expression2 = () => foo.Fooy;
            Expression<Func<PropertyAttributeGetterExtensionsFoo, object>> expression3 = foo => foo.Fooy;
            Expression<Func<string>> expression4 = () => foo.Fooy;

            Expression<Func<PropertyAttributeGetterExtensionsFoo, string>> expression5 = foo => foo.Foon;
            Expression<Func<string>> expression6 = () => foo.Foon;
            Expression<Func<PropertyAttributeGetterExtensionsFoo, object>> expression7 = foo => foo.Foon;
            Expression<Func<string>> expression8 = () => foo.Foon;

            Assert.AreEqual("Fooy", expression1.GetPropertyName());
            Assert.AreEqual("Fooy", expression2.GetPropertyName());
            Assert.AreEqual("Fooy", expression3.GetPropertyName());
            Assert.AreEqual("Fooy", expression4.GetPropertyName());
                                               
            Assert.AreEqual("Foon", expression5.GetPropertyName());
            Assert.AreEqual("Foon", expression6.GetPropertyName());
            Assert.AreEqual("Foon", expression7.GetPropertyName());
            Assert.AreEqual("Foon", expression8.GetPropertyName());
        }

        [Test]
        public void DisplayNameGet()
        {
            var foo = new PropertyAttributeGetterExtensionsFoo();
            Expression<Func<PropertyAttributeGetterExtensionsFoo, string>> expression1 = foo => foo.Fooy;
            Expression<Func<string>> expression2 = () => foo.Fooy;
            Expression<Func<PropertyAttributeGetterExtensionsFoo, object>> expression3 = foo => foo.Fooy;
            Expression<Func<string>> expression4 = () => foo.Fooy;

            Expression<Func<PropertyAttributeGetterExtensionsFoo, string>> expression5 = foo => foo.Foon;
            Expression<Func<string>> expression6 = () => foo.Foon;
            Expression<Func<PropertyAttributeGetterExtensionsFoo, object>> expression7 = foo => foo.Foon;
            Expression<Func<string>> expression8 = () => foo.Foon;

            Assert.AreEqual("FooyDisplayName", expression1.GetDisplayName());
            Assert.AreEqual("FooyDisplayName", expression2.GetDisplayName());
            Assert.AreEqual("FooyDisplayName", expression3.GetDisplayName());
            Assert.AreEqual("FooyDisplayName", expression4.GetDisplayName());

            Assert.AreEqual("Foon", expression5.GetDisplayName());
            Assert.AreEqual("Foon", expression6.GetDisplayName());
            Assert.AreEqual("Foon", expression7.GetDisplayName());
            Assert.AreEqual("Foon", expression8.GetDisplayName());
        }

        [Test]
        public void DescriptionGet()
        {
            var foo = new PropertyAttributeGetterExtensionsFoo();
            Expression<Func<PropertyAttributeGetterExtensionsFoo, string>> expression1 = foo => foo.Fooy;
            Expression<Func<string>> expression2 = () => foo.Fooy;
            Expression<Func<PropertyAttributeGetterExtensionsFoo, object>> expression3 = foo => foo.Fooy;
            Expression<Func<string>> expression4 = () => foo.Fooy;

            Expression<Func<PropertyAttributeGetterExtensionsFoo, string>> expression5 = foo => foo.Foon;
            Expression<Func<string>> expression6 = () => foo.Foon;
            Expression<Func<PropertyAttributeGetterExtensionsFoo, object>> expression7 = foo => foo.Foon;
            Expression<Func<string>> expression8 = () => foo.Foon;

            Assert.AreEqual("DescriptionText", expression1.GetDescription());
            Assert.AreEqual("DescriptionText", expression2.GetDescription());
            Assert.AreEqual("DescriptionText", expression3.GetDescription());
            Assert.AreEqual("DescriptionText", expression4.GetDescription());

            Assert.AreEqual("", expression5.GetDescription());
            Assert.AreEqual("", expression6.GetDescription());
            Assert.AreEqual("", expression7.GetDescription());
            Assert.AreEqual("", expression8.GetDescription());
        }

        [Test]
        public void RequiredGet()
        {
            var fooi = new PropertyAttributeGetterExtensionsFoo();
            Expression <Func<PropertyAttributeGetterExtensionsFoo, string>> expression1 = foo => foo.Fooy;
            Expression<Func<string>> expression2 = () => fooi.Fooy;
            Expression<Func<PropertyAttributeGetterExtensionsFoo, object>> expression3 = foo => foo.Fooy;
            Expression<Func<string>> expression4 = () => fooi.Fooy;

            Assert.IsTrue(expression1.HasRequiredAttribute());
            Assert.IsTrue(expression2.HasRequiredAttribute());
            Assert.IsTrue(expression3.HasRequiredAttribute());
            Assert.IsTrue(expression4.HasRequiredAttribute());
        }

        [Test]
        public void RequiredNotSetGet()
        {
            var fooi = new PropertyAttributeGetterExtensionsFoo();
            Expression<Func<PropertyAttributeGetterExtensionsFoo, string>> expression1 = foo => foo.Foon;
            Expression<Func<string>> expression2 = () => fooi.Foon;
            Expression<Func<PropertyAttributeGetterExtensionsFoo, object>> expression3 = foo => foo.Foon;
            Expression<Func<string>> expression4 = () => fooi.Foon;

            Assert.IsFalse(expression1.HasRequiredAttribute());
            Assert.IsFalse(expression2.HasRequiredAttribute());
            Assert.IsFalse(expression3.HasRequiredAttribute());
            Assert.IsFalse(expression4.HasRequiredAttribute());
        }
    }
}
