using System;
using System.Linq.Expressions;
using NUnit.Framework;
using Bonsai.Utils.Property;

namespace Bonsai.Tests.Property
{
    class PropertyRecorderFooInner
    {

    }

    class PropertyRecorderFoo
    {
        public string Fooy { get; set; } = "x";

        public int Foon { get; set; } = 0;

        public PropertyRecorderFooInner Fooi { get; set; } = new PropertyRecorderFooInner();

        public PropertyRecorder FooyRec { get; set; } = null;

        public PropertyRecorder FoonRec { get; set; } = null;

        public PropertyRecorder FooiRec { get; set; } = null;

        public PropertyRecorderFoo StartRecording()
        {
            Expression<Func<string>> fooyExpr = () => Fooy;
            Expression<Func<int>> foonExpr = () => Foon;
            Expression<Func<PropertyRecorderFooInner>> fooiExpr = () => Fooi;
            FooyRec ??= new PropertyRecorder(this, fooyExpr).StartRecording();
            FoonRec ??= new PropertyRecorder(this, foonExpr).StartRecording();
            FooiRec ??= new PropertyRecorder(this, fooiExpr).StartRecording();
            return this;
        }
    }

    public class PropertyRecorderTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SimpleRecord()
        {
            var model = new PropertyRecorderFoo().StartRecording();

            Assert.IsFalse(model.FooyRec.ValueChanged());
            Assert.IsFalse(model.FoonRec.ValueChanged());
            Assert.IsFalse(model.FooiRec.ValueChanged());

            model.Fooy = "y";

            Assert.IsTrue(model.FooyRec.ValueChanged());
            Assert.IsFalse(model.FoonRec.ValueChanged());
            Assert.IsFalse(model.FooiRec.ValueChanged());

            model.Foon = 1;

            Assert.IsTrue(model.FooyRec.ValueChanged());
            Assert.IsTrue(model.FoonRec.ValueChanged());
            Assert.IsFalse(model.FooiRec.ValueChanged());

            model.Fooi = new PropertyRecorderFooInner();

            Assert.IsTrue(model.FooyRec.ValueChanged());
            Assert.IsTrue(model.FoonRec.ValueChanged());
            Assert.IsTrue(model.FooiRec.ValueChanged());
        }

        [Test]
        public void NullValue()
        {
            var model = new PropertyRecorderFoo().StartRecording();
            Assert.IsFalse(model.FooiRec.ValueChanged());

            var original = model.Fooi;
            model.Fooi = null;
            Assert.IsTrue(model.FooiRec.ValueChanged());

            model.FooiRec.StopRecording().StartRecording();
            Assert.IsFalse(model.FooiRec.ValueChanged());

            model.Fooi = original;
            Assert.IsTrue(model.FooiRec.ValueChanged());
        }

        [Test]
        public void ChangeValueBack()
        {
            var model = new PropertyRecorderFoo().StartRecording();

            Assert.IsFalse(model.FooyRec.ValueChanged());
            Assert.IsFalse(model.FoonRec.ValueChanged());
            Assert.IsFalse(model.FooiRec.ValueChanged());

            model.Fooy = "y";

            Assert.IsTrue(model.FooyRec.ValueChanged());
            Assert.IsFalse(model.FoonRec.ValueChanged());
            Assert.IsFalse(model.FooiRec.ValueChanged());

            model.Foon = 1;

            Assert.IsTrue(model.FooyRec.ValueChanged());
            Assert.IsTrue(model.FoonRec.ValueChanged());
            Assert.IsFalse(model.FooiRec.ValueChanged());

            var originalFooi = model.Fooi;
            model.Fooi = new PropertyRecorderFooInner();

            Assert.IsTrue(model.FooyRec.ValueChanged());
            Assert.IsTrue(model.FoonRec.ValueChanged());
            Assert.IsTrue(model.FooiRec.ValueChanged());

            model.Fooy = "x";
            model.Foon = 0;
            model.Fooi = originalFooi;

            Assert.IsFalse(model.FooyRec.ValueChanged());
            Assert.IsFalse(model.FoonRec.ValueChanged());
            Assert.IsFalse(model.FooiRec.ValueChanged());
        }

        [Test]
        public void StartStopStart()
        {
            var model = new PropertyRecorderFoo().StartRecording();

            Assert.IsFalse(model.FooyRec.ValueChanged());
            Assert.IsFalse(model.FoonRec.ValueChanged());
            Assert.IsFalse(model.FooiRec.ValueChanged());

            model.Fooy = "y";

            Assert.IsTrue(model.FooyRec.ValueChanged());
            Assert.IsFalse(model.FoonRec.ValueChanged());
            Assert.IsFalse(model.FooiRec.ValueChanged());

            model.Foon = 1;

            Assert.IsTrue(model.FooyRec.ValueChanged());
            Assert.IsTrue(model.FoonRec.ValueChanged());
            Assert.IsFalse(model.FooiRec.ValueChanged());

            var originalFooi = model.Fooi;
            model.Fooi = new PropertyRecorderFooInner();

            Assert.IsTrue(model.FooyRec.ValueChanged());
            Assert.IsTrue(model.FoonRec.ValueChanged());
            Assert.IsTrue(model.FooiRec.ValueChanged());

            model.Fooy = "x";
            model.Foon = 0;
            model.Fooi = originalFooi;

            Assert.IsFalse(model.FooyRec.ValueChanged());
            Assert.IsFalse(model.FoonRec.ValueChanged());
            Assert.IsFalse(model.FooiRec.ValueChanged());

            model.FooyRec.StopRecording();
            model.FoonRec.StopRecording();
            model.FooiRec.StopRecording();

            Assert.IsFalse(model.FooyRec.ValueChanged());
            Assert.IsFalse(model.FoonRec.ValueChanged());
            Assert.IsFalse(model.FooiRec.ValueChanged());

            var anotherFooi = new PropertyRecorderFooInner();
            model.Fooy = "y";
            model.Foon = 1;
            model.Fooi = anotherFooi;

            Assert.IsTrue(model.FooyRec.ValueChanged());
            Assert.IsTrue(model.FoonRec.ValueChanged());
            Assert.IsTrue(model.FooiRec.ValueChanged());

            model.FooyRec.StartRecording();
            model.FoonRec.StartRecording();
            model.FooiRec.StartRecording();

            Assert.IsFalse(model.FooyRec.ValueChanged());
            Assert.IsFalse(model.FoonRec.ValueChanged());
            Assert.IsFalse(model.FooiRec.ValueChanged());

            model.Fooy = "x";
            model.Foon = 0;
            model.Fooi = originalFooi;

            Assert.IsTrue(model.FooyRec.ValueChanged());
            Assert.IsTrue(model.FoonRec.ValueChanged());
            Assert.IsTrue(model.FooiRec.ValueChanged());
        }
    }
}
