using System;
using System.Linq;
using System.Reflection;
using Autofac.Core.Activators.Reflection;

namespace Axity.Security.Infrastructure
{
    public class InternalConstructorFinder : IConstructorFinder
    {
        public ConstructorInfo[] FindConstructors(Type t) => t.GetTypeInfo().DeclaredConstructors.Where(c => !c.IsPrivate && !c.IsPublic).ToArray();
    }
}
