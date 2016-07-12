using Akka.Actor;
using Akka.DI.AutoFac;
using Akka.DI.Core;
using Autofac;
using System;
using Akka.Tdd.Core;

namespace Akka.Tdd.AutoFac
{
    public class AutoFacAkkaDependencyResolver : IAkkaDependencyResolver
    {
        private ContainerBuilder Builder { set; get; }

        public IContainer Container { set; get; }

        public AutoFacAkkaDependencyResolver(IContainer container = null)
        {
            container = container ?? new ContainerBuilder().Build();
            Builder = new ContainerBuilder();
            TypeRegistrer = (type, isGenericType) =>
            {
                if (type.IsGenericType)
                {
                    Builder.RegisterGeneric(type);
                }
                else
                {
                    Builder.RegisterType(type);
                }
            };
            TypeIterationCompleted = (actorSystem) =>
            {
                Builder.Update(container);
                IDependencyResolver resolver = new AutoFacDependencyResolver(container, actorSystem);
                Container = container;
            };
        }

        public Action<Type, bool> TypeRegistrer { get; set; }
        public Action<ActorSystem> TypeIterationCompleted { get; set; }
    }
}