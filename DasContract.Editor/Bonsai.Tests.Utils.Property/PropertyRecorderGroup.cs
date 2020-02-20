using System;
using System.Linq.Expressions;
using NUnit.Framework;
using Bonsai.Utils.Property;

namespace Bonsai.Tests.Property
{
    class PropertyRecorderGroupFooInner
    {

    }

    class PropertyRecorderGroupFoo
    {
        public string Fooy { get; set; } = "x";

        public int Foon { get; set; } = 0;

        public PropertyRecorderFooInner Fooi { get; set; } = new PropertyRecorderFooInner();

        public PropertyRecorderGroup RecGroup { get; set; } = new PropertyRecorderGroup();

        public PropertyRecorderGroupFoo StartRecording()
        {
            Expression<Func<string>> fooyExpr = () => Fooy;
            Expression<Func<int>> foonExpr = () => Foon;
            Expression<Func<PropertyRecorderFooInner>> fooiExpr = () => Fooi;
            RecGroup.AddRecorder(this, fooyExpr)
                .AddRecorder(this, foonExpr)
                .AddRecorder(this, fooiExpr)
                .StartRecording();
            return this;
        }
    }

    public class PropertyRecorderGroupTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SimpleRecord()
        {
            var model = new PropertyRecorderGroupFoo().StartRecording();

            Assert.IsFalse(model.RecGroup.SomeValueChanged());

            model.Fooy = "y";

            Assert.IsTrue(model.RecGroup.SomeValueChanged());

            model.Foon = 1;

            Assert.IsTrue(model.RecGroup.SomeValueChanged());

            model.Fooi = new PropertyRecorderFooInner();

            Assert.IsTrue(model.RecGroup.SomeValueChanged());
        }

        [Test]
        public void ChangeValueBack()
        {
            var model = new PropertyRecorderGroupFoo().StartRecording();

            Assert.IsFalse(model.RecGroup.SomeValueChanged());

            model.Fooy = "y";

            Assert.IsTrue(model.RecGroup.SomeValueChanged());

            model.Foon = 1;

            Assert.IsTrue(model.RecGroup.SomeValueChanged());

            var originalFooi = model.Fooi;
            model.Fooi = new PropertyRecorderFooInner();

            Assert.IsTrue(model.RecGroup.SomeValueChanged());

            model.Fooy = "x";
            model.Foon = 0;
            model.Fooi = originalFooi;

            Assert.IsFalse(model.RecGroup.SomeValueChanged());
        }

        [Test]
        public void StartStopStart()
        {
            var model = new PropertyRecorderGroupFoo().StartRecording();

            Assert.IsFalse(model.RecGroup.SomeValueChanged());

            model.Fooy = "y";

            Assert.IsTrue(model.RecGroup.SomeValueChanged());

            model.Foon = 1;

            Assert.IsTrue(model.RecGroup.SomeValueChanged());

            var originalFooi = model.Fooi;
            model.Fooi = new PropertyRecorderFooInner();

            Assert.IsTrue(model.RecGroup.SomeValueChanged());

            model.Fooy = "x";
            model.Foon = 0;
            model.Fooi = originalFooi;

            Assert.IsFalse(model.RecGroup.SomeValueChanged());

            model.RecGroup.StopRecording();

            Assert.IsFalse(model.RecGroup.SomeValueChanged());

            var anotherFooi = new PropertyRecorderFooInner();
            model.Fooy = "y";
            model.Foon = 1;
            model.Fooi = anotherFooi;

            Assert.IsTrue(model.RecGroup.SomeValueChanged());

            model.RecGroup.StartRecording();

            Assert.IsFalse(model.RecGroup.SomeValueChanged());

            model.Fooy = "x";
            model.Foon = 0;
            model.Fooi = originalFooi;

            Assert.IsTrue(model.RecGroup.SomeValueChanged());
        }
    }
}
