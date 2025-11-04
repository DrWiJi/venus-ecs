using NUnit.Framework;
using VenusECS.Core.Reflection;
using VenusECS.Core.Reflection.Attributes;

namespace VenusECS.Unity.Tests
{
    public class DependencyInjectionTests
    {
        [Test]
        public void DependencyInjectionTestsSimplePasses()
        {
            TestInjectionTarget target = new();
            InstanceDependency inst = new();
            inst.OtherData = 1111;
            DependencyInjector injector = new ();
            injector.AddService(typeof(InstanceDependency), inst);
            injector.AutoSharedInstantiateType(typeof(DynamicInstanceDependency));
            injector.InjectDependencies(target);
            Assert.That(target.GetDynamic().SomeData == 1121);
            Assert.That(target.GetInstance().OtherData == 1111);
        }
        
        [Test]
        public void MultipleDependencyInjectionTestsSimplePasses()
        {
            TestInjectionTarget target = new();
            TestInjectionTarget secondtarget = new();
            InstanceDependency inst = new();
            inst.OtherData = 1111;
            DependencyInjector injector = new ();
            injector.AddService(typeof(InstanceDependency), inst);
            injector.AutoSharedInstantiateType(typeof(DynamicInstanceDependency));
            injector.InjectDependencies(target);
            injector.InjectDependencies(secondtarget);
            secondtarget.GetDynamic().SomeData = 4321;
            
            Assert.That(target.GetDynamic().SomeData == 4321);
            Assert.That(target.GetInstance().OtherData == 1111);
            
            Assert.That(secondtarget.GetDynamic().SomeData == 4321);
            Assert.That(secondtarget.GetInstance().OtherData == 1111);
        }
    }

    internal class TestInjectionTarget
    {
        [VenusInject] private readonly DynamicInstanceDependency _dynamicInstance;
        [VenusInject] private readonly InstanceDependency _regularInstance;

        public DynamicInstanceDependency GetDynamic()
        {
            return _dynamicInstance;
        }

        public InstanceDependency GetInstance()
        {
            return _regularInstance;
        }
    }

    internal class DynamicInstanceDependency
    {
        public int SomeData = 1121;
    }

    internal class InstanceDependency
    {
        public int OtherData;
    }
}
