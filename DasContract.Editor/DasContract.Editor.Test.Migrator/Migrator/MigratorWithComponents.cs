using System;
using System.Linq.Expressions;
using System.Reflection;
using DasContract.Editor.Migrator.Interfaces;
using NUnit.Framework;

namespace DasContract.Tests.Migrator
{
    public class MigratorWithComponentsTests
    {
        class MigratableTestClass: IMigratorProvider<IMigrator>
        {
            public IMigrator Migrator { get; set; } = new DasContract.Editor.Migrator.SimpleMigrator();

            public int Property1
            {
                get => property1;
                set
                {
                    if (property1 != value)
                        Migrator.Notify(
                            () => property1,
                            () => property1,
                            e => property1 = e,
                            MigratorMode.Smart);
                    property1 = value;
                }
            }
            int property1;


            public string Property2
            {
                get => property2;
                set
                {
                    if (property2 != value)
                        Migrator.Notify(
                            () => property2,
                            e => property2 = e,
                            MigratorMode.Smart);
                    property2 = value;
                }
            }
            string property2;

            public MigratableTestClassComponent Component1
            {
                get => component1.WithMigrator(Migrator);
                set
                {
                    if (value != component1)
                        Migrator.Notify(() => component1, e => component1 = e,
                            MigratorMode.Smart);
                    component1 = value;
                }
            }
            MigratableTestClassComponent component1;


            public IMigrator GetMigrator()
            {
                return Migrator;
            }
        }

        class MigratableTestClassComponent : IMigratableComponent<MigratableTestClassComponent, IMigrator>
        {
            public IMigrator Migrator { get; set; } = new DasContract.Editor.Migrator.SimpleMigrator();

            public MigratableTestClassComponent WithMigrator(IMigrator parentMigrator)
            {
                Migrator = parentMigrator;
                return this;
            }

            public int Property1
            {
                get => property1;
                set
                {
                    if (property1 != value)
                        Migrator.Notify(
                            () => property1,
                            () => property1,
                            e => property1 = e,
                            MigratorMode.Smart);
                    property1 = value;
                }
            }
            int property1;
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ComponentMigrations()
        {
            var testClass = new MigratableTestClass()
            {
                Property1 = 1,
                Property2 = "xxx",
                Component1 = new MigratableTestClassComponent()
                {
                    Property1 = 11
                }
            };

            testClass.Migrator.StartTracingSteps();

            testClass.Component1 = new MigratableTestClassComponent() { Property1 = 111 };
            Assert.AreEqual(111, testClass.Component1.Property1);

            testClass.Migrator.StepBackward();
            Assert.AreEqual(11, testClass.Component1.Property1);
            testClass.Migrator.StepForward();
            Assert.AreEqual(111, testClass.Component1.Property1);

            testClass.Component1.Property1 = 222;
            Assert.AreEqual(222, testClass.Component1.Property1);
            testClass.Migrator.StepBackward();
            Assert.AreEqual(111, testClass.Component1.Property1);
            testClass.Migrator.StepForward();
            Assert.AreEqual(222, testClass.Component1.Property1);
        }

    }
}
