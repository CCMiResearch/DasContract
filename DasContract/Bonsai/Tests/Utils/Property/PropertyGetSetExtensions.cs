using System;
using System.Linq.Expressions;
using NUnit.Framework;
using Bonsai.Utils.Property;

namespace Bonsai.Tests.Property
{
    class PropertyGetSetExtensionsFoo
    {
        public string Fooy { get; set; } = "x";

        public string Foon { get; set; }
    }

    public class PropertyGetSetExtensions
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetSimpleFunc()
        {
            var model = new PropertyGetSetExtensionsFoo();
            model.Foon = "Hoy!";

            Expression<Func<string>> expressionFoon = () => model.Foon;
            Expression<Func<object>> expressionFooy = () => model.Fooy;

            Assert.AreEqual("Hoy!", model.GetPropertyValue<PropertyGetSetExtensionsFoo, string>(expressionFoon));
            Assert.AreEqual("x", model.GetPropertyValue<PropertyGetSetExtensionsFoo, string>(expressionFooy));
        }

        [Test]
        public void GetInputFunc()
        {
            var model = new PropertyGetSetExtensionsFoo();
            model.Foon = "Hoy!";

            Expression<Func<PropertyGetSetExtensionsFoo, string>> expressionFoon = m => m.Foon;
            Expression<Func<object, object>> expressionFooy = m => (m as PropertyGetSetExtensionsFoo).Fooy;

            Assert.AreEqual("Hoy!", model.GetPropertyValue<PropertyGetSetExtensionsFoo, string>(expressionFoon));
            Assert.AreEqual("x", model.GetPropertyValue<PropertyGetSetExtensionsFoo, string>(expressionFooy));
        }

        [Test]
        public void SetSimpleFunc()
        {
            var model = new PropertyGetSetExtensionsFoo();

            Expression<Func<string>> expressionFooy = () => model.Fooy;
            Expression<Func<object>> expressionFoon = () => model.Foon;

            model.SetPropertyValue(expressionFooy, "Hoy!");
            model.SetPropertyValue<PropertyGetSetExtensionsFoo, string>(expressionFoon, default);

            Assert.AreEqual("Hoy!", model.Fooy);
            Assert.AreEqual(default, model.Foon);
        }

        [Test]
        public void SetInputFunc()
        {
            var model = new PropertyGetSetExtensionsFoo();

            Expression<Func<PropertyGetSetExtensionsFoo, string>> expressionFooy = model => model.Fooy;
            Expression<Func<object, object>> expressionFoon = model => (model as PropertyGetSetExtensionsFoo).Foon;

            model.SetPropertyValue(expressionFooy, "Hoy!");
            model.SetPropertyValue<PropertyGetSetExtensionsFoo, string>(expressionFoon, default);

            Assert.AreEqual("Hoy!", model.Fooy);
            Assert.AreEqual(default, model.Foon);
        }
    }
}
