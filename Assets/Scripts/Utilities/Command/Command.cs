/*
    Create commands inheriting this
    
    Author - Rohit Bhosle
*/

using System;
using System.Reflection;

namespace BehaviourInject
{
    public abstract class Command : ICommand
    {
        private Context _context;

        public abstract void Execute(params object[] args);

        public void SetContext(string contextName)
        {
            _context = ContextRegistry.GetContext(contextName);
        }

        public virtual void ResolveSelf()
        {
            Type componentType = GetType();
            ClassDataHolder data = ReflectionDataCache.GetClassData(componentType);

            for (int i = 0; i < data.PropertyInfos.Length; i++)
            {
                PropertyInfo property = data.PropertyInfos[i];
                if (data.PropertyAttributes[i]) continue;

                object dependency = _context.Resolve(property.PropertyType);
                property.SetValue(this, dependency, null);
            }

            for (int i = 0; i < data.FieldInfos.Length; i++)
            {
                FieldInfo field = data.FieldInfos[i];
                if (data.FieldAttributes[i]) continue;

                object dependency = _context.Resolve(field.FieldType);
                field.SetValue(this, dependency);
            }
        }
    }

    public interface ICommand
    {
        void SetContext(string contextName);
        void ResolveSelf();
        void Execute(params object[] args);
    }
}
