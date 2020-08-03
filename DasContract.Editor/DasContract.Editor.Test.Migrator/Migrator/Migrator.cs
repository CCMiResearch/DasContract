using System;
using System.Linq.Expressions;
using System.Reflection;
using DasContract.Editor.Migrator;
using NUnit.Framework;

namespace DasContract.Tests.Migrator
{
    public class MigratorTests
    {
        class MigratableTestClass
        {
            public SimpleMigrator Migrator { get; set; } = new SimpleMigrator();

            public int Property1
            {
                get => property1;
                set
                {
                    if (property1 != value)
                        Migrator.Notify(
                            () => property1,
                            () => property1,
                            e => property1 = e);
                    property1 = value;
                }
            }
            int property1;


            public string Property2
            {
                get => property2;
                set
                {
                    /*if (property2 != value)
                        Migrator.Notify(
                            () => property2,
                            e => property2 = e);*/
                    var previousValue = property2;
                    if (property2 != value)
                        Migrator.Notify(
                            () => property2,
                            () => property2 = value,
                            () => property2 = previousValue);
                    property2 = value;
                }
            }
            string property2;
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void MigrationIndicatorsAndNofity()
        {
            var testClass = new MigratableTestClass()
            {
                Property1 = 1,
                Property2 = "xxx"
            };

            testClass.Migrator.StartTracingSteps();

            Assert.AreEqual(0, testClass.Migrator.MigrationsCount());
            Assert.IsFalse(testClass.Migrator.HasStepBackward());
            Assert.IsFalse(testClass.Migrator.HasStepForward());

            testClass.Property1++;

            Assert.AreEqual(1, testClass.Migrator.MigrationsCount());
            Assert.IsTrue(testClass.Migrator.HasStepBackward());
            Assert.IsFalse(testClass.Migrator.HasStepForward());

            testClass.Property1++;

            Assert.AreEqual(1, testClass.Migrator.MigrationsCount());
            Assert.IsTrue(testClass.Migrator.HasStepBackward());
            Assert.IsFalse(testClass.Migrator.HasStepForward());

            testClass.Property2 = Guid.NewGuid().ToString();

            Assert.AreEqual(2, testClass.Migrator.MigrationsCount());
            Assert.IsTrue(testClass.Migrator.HasStepBackward());
            Assert.IsFalse(testClass.Migrator.HasStepForward());

            testClass.Property1++;

            Assert.AreEqual(3, testClass.Migrator.MigrationsCount());
            Assert.IsTrue(testClass.Migrator.HasStepBackward());
            Assert.IsFalse(testClass.Migrator.HasStepForward());
        }

        [Test]
        public void DifferentExpressionInstances()
        {
            var testClass = new MigratableTestClass()
            {
                Property1 = 1,
                Property2 = "xxx"
            };

            testClass.Migrator.StartTracingSteps();

            Assert.AreEqual(0, testClass.Migrator.MigrationsCount());
            Assert.IsFalse(testClass.Migrator.HasStepBackward());
            Assert.IsFalse(testClass.Migrator.HasStepForward());

            testClass.Property1++;

            Assert.AreEqual(1, testClass.Migrator.MigrationsCount());
            Assert.IsTrue(testClass.Migrator.HasStepBackward());
            Assert.IsFalse(testClass.Migrator.HasStepForward());

            testClass.Property1++;

            Assert.AreEqual(1, testClass.Migrator.MigrationsCount());
            Assert.IsTrue(testClass.Migrator.HasStepBackward());
            Assert.IsFalse(testClass.Migrator.HasStepForward());

            testClass.Property2 = Guid.NewGuid().ToString();

            Assert.AreEqual(2, testClass.Migrator.MigrationsCount());
            Assert.IsTrue(testClass.Migrator.HasStepBackward());
            Assert.IsFalse(testClass.Migrator.HasStepForward());

            testClass.Property2 = Guid.NewGuid().ToString();

            Assert.AreEqual(2, testClass.Migrator.MigrationsCount());
            Assert.IsTrue(testClass.Migrator.HasStepBackward());
            Assert.IsFalse(testClass.Migrator.HasStepForward());

            testClass.Property1++;

            Assert.AreEqual(3, testClass.Migrator.MigrationsCount());
            Assert.IsTrue(testClass.Migrator.HasStepBackward());
            Assert.IsFalse(testClass.Migrator.HasStepForward());
        }

        [Test]
        public void SingleStepBack()
        {
            var testClass = new MigratableTestClass()
            {
                Property1 = 1,
                Property2 = "xxx"
            };

            testClass.Migrator.StartTracingSteps();

            testClass.Migrator.StepForward();
            testClass.Migrator.StepBackward();
            Assert.AreEqual(1, testClass.Property1);

            testClass.Property1 = 10;

            Assert.AreEqual(10, testClass.Property1);

            testClass.Migrator.StepForward();
            Assert.AreEqual(10, testClass.Property1);

            Assert.IsTrue(testClass.Migrator.HasStepBackward());
            testClass.Migrator.StepBackward();
            Assert.AreEqual(1, testClass.Property1);

            testClass.Migrator.StepBackward();
            Assert.AreEqual(1, testClass.Property1);
        }

        [Test]
        public void MultipleStepsBackOnSingleProperty()
        {
            var testClass = new MigratableTestClass()
            {
                Property1 = 1,
                Property2 = "xxx"
            };

            testClass.Migrator.StartTracingSteps();

            testClass.Migrator.StepForward();
            testClass.Migrator.StepBackward();
            Assert.AreEqual(1, testClass.Property1);

            testClass.Property1 = 10;
            Assert.AreEqual(10, testClass.Property1);

            testClass.Property1 = 20;
            Assert.AreEqual(20, testClass.Property1);

            testClass.Property1 = 30;
            Assert.AreEqual(30, testClass.Property1);

            testClass.Property1 = 40;
            Assert.AreEqual(40, testClass.Property1);

            testClass.Migrator.StepBackward();
            Assert.AreEqual(1, testClass.Property1);

            for (int i = 0; i < 10; i++)
            {
                testClass.Migrator.StepBackward();
                Assert.AreEqual(1, testClass.Property1);
            }
        }

        [Test]
        public void MultipleStepsBackOnMultipleProperties()
        {
            var testClass = new MigratableTestClass()
            {
                Property1 = 1,
                Property2 = "xxx"
            };

            testClass.Migrator.StartTracingSteps();

            testClass.Migrator.StepForward();
            testClass.Migrator.StepBackward();
            Assert.AreEqual(1, testClass.Property1);

            testClass.Property1 = 10;
            testClass.Property2 = "100";
            Assert.AreEqual(10, testClass.Property1);

            testClass.Property1 = 20;
            testClass.Property2 = "200";
            Assert.AreEqual(20, testClass.Property1);

            testClass.Property1 = 30;
            testClass.Property2 = "300";
            Assert.AreEqual(30, testClass.Property1);

            testClass.Property1 = 40;
            testClass.Property2 = "400";
            Assert.AreEqual(40, testClass.Property1);

            testClass.Migrator.StepBackward();
            Assert.AreEqual(40, testClass.Property1);
            Assert.AreEqual("300", testClass.Property2);

            testClass.Migrator.StepBackward();
            Assert.AreEqual(30, testClass.Property1);
            Assert.AreEqual("300", testClass.Property2);

            testClass.Migrator.StepBackward();
            Assert.AreEqual(30, testClass.Property1);
            Assert.AreEqual("200", testClass.Property2);

            testClass.Migrator.StepBackward();
            Assert.AreEqual(20, testClass.Property1);
            Assert.AreEqual("200", testClass.Property2);

            testClass.Migrator.StepBackward();
            Assert.AreEqual(20, testClass.Property1);
            Assert.AreEqual("100", testClass.Property2);

            testClass.Migrator.StepBackward();
            Assert.AreEqual(10, testClass.Property1);
            Assert.AreEqual("100", testClass.Property2);

            testClass.Migrator.StepBackward();
            Assert.AreEqual(10, testClass.Property1);
            Assert.AreEqual("xxx", testClass.Property2);

            testClass.Migrator.StepBackward();
            Assert.AreEqual(1, testClass.Property1);
            Assert.AreEqual("xxx", testClass.Property2);
        }

        [Test]
        public void SingleStepBackAndForward()
        {
            var testClass = new MigratableTestClass()
            {
                Property1 = 1,
                Property2 = "xxx"
            };

            testClass.Migrator.StartTracingSteps();

            testClass.Property1 = 10;

            for(int i = 0; i < 5; i++)
            {
                testClass.Migrator.StepBackward();
                Assert.AreEqual(1, testClass.Property1);

                testClass.Migrator.StepForward();
                Assert.AreEqual(10, testClass.Property1);
            }
        }

        [Test]
        public void VariousStepsBackAndForward()
        {
            var testClass = new MigratableTestClass()
            {
                Property1 = 1,
                Property2 = "xxx"
            };

            testClass.Migrator.StartTracingSteps();

            testClass.Migrator.StepForward();
            testClass.Migrator.StepBackward();
            Assert.AreEqual(1, testClass.Property1);

            testClass.Property1 = 10;
            testClass.Property2 = "100";
            Assert.AreEqual(10, testClass.Property1);

            testClass.Property1 = 20;
            testClass.Property2 = "200";
            Assert.AreEqual(20, testClass.Property1);

            testClass.Property1 = 30;
            testClass.Property2 = "300";
            Assert.AreEqual(30, testClass.Property1);

            testClass.Migrator.StepBackward();
            Assert.AreEqual("200", testClass.Property2);
            testClass.Migrator.StepBackward();
            Assert.AreEqual(20, testClass.Property1);
            
            testClass.Migrator.StepForward();
            Assert.AreEqual(30, testClass.Property1);
            Assert.AreEqual("200", testClass.Property2);
            testClass.Migrator.StepForward();
            Assert.AreEqual(30, testClass.Property1);
            Assert.AreEqual("300", testClass.Property2);

            testClass.Property1 = 40;
            testClass.Property2 = "400";
            Assert.AreEqual(40, testClass.Property1);
            Assert.AreEqual("400", testClass.Property2);

            testClass.Migrator.StepBackward();
            testClass.Migrator.StepBackward();

            testClass.Property1 = 50;
            testClass.Property2 = "500";

            testClass.Migrator.StepForward();
            testClass.Migrator.StepForward();
            Assert.AreEqual(50, testClass.Property1);
            Assert.AreEqual("500", testClass.Property2);

            testClass.Migrator.StepBackward();
            testClass.Migrator.StepBackward();
            Assert.AreEqual(30, testClass.Property1);
            Assert.AreEqual("300", testClass.Property2);

            testClass.Migrator.StepBackward();
            testClass.Migrator.StepBackward();
            Assert.AreEqual(20, testClass.Property1);
            Assert.AreEqual("200", testClass.Property2);

            testClass.Migrator.StepBackward();
            testClass.Migrator.StepBackward();
            Assert.AreEqual(10, testClass.Property1);
            Assert.AreEqual("100", testClass.Property2);

            testClass.Migrator.StepBackward();
            testClass.Migrator.StepBackward();
            Assert.AreEqual(1, testClass.Property1);
            Assert.AreEqual("xxx", testClass.Property2);
        }

    }
}
