using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using NUnit.Framework;
using Bonsai.Utils.Property;

namespace Bonsai.Tests.Property
{
    public class EnumAttributeGetterExtensions
    {
        enum EnumAttributeGetterExtensionsFooEnum
        {
            [Display(Name = "FooyDisplayName")]
            Fooy,


            Foon
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetDisplayName()
        {
            Assert.AreEqual("FooyDisplayName", EnumAttributeGetterExtensionsFooEnum.Fooy.GetDisplayName());
            Assert.AreEqual("Foon", EnumAttributeGetterExtensionsFooEnum.Foon.GetDisplayName());
        }
    }
}
