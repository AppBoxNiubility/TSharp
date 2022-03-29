using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TSharp.Core.Osgi;
using System.Collections.Concurrent;
using TSharp.Core.Osgi.Internal;

namespace TSharp.Core.Simple
{
    internal class SimpleLocatorWrapper : IServiceLocator, IDisposable
    {
        // Fields
        private SimpleLocatorWrapper _parent;
        private ConcurrentDictionary<Type, Implement> cache;

        // Methods
        public SimpleLocatorWrapper(Level l)
        {
            this.cache = new ConcurrentDictionary<Type, Implement>();
            this.Level = l;
        }

        public SimpleLocatorWrapper(SimpleLocatorWrapper parent, Level l)
            : this(l)
        {
            this._parent = parent;
        }

        public object BuildUp(Type type, object instance, params object[] existing)
        {
            throw new NotImplementedException();
        }

        internal SimpleLocatorWrapper CreateChildContainer(Level l)
        {
            return new SimpleLocatorWrapper(this, l);
        }

        public void Dispose()
        {
        }

        private RegServiceAttribute FindImplType(Type intfType, Level level)
        {
            RegServiceAttribute implType = null;
            if (level.HasFlag(Level.THREAD))
            {
                implType = ServiceManager.ThreadMap.FirstOrDefault<RegServiceAttribute>(x => x.IntfType == intfType);
                if (implType != null)
                {
                    return implType;
                }
                implType = ServiceManager.RootMap.FirstOrDefault<RegServiceAttribute>(x => x.IntfType == intfType);
                if (implType != null)
                {
                    return implType;
                }
            }
            else
            {
                if (level.HasFlag(Level.REQUSET))
                {
                    implType = ServiceManager.RequsetMap.FirstOrDefault<RegServiceAttribute>(x => x.IntfType == intfType);
                    if (implType != null)
                    {
                        return implType;
                    }
                    implType = ServiceManager.SessionMap.FirstOrDefault<RegServiceAttribute>(x => x.IntfType == intfType);
                    if (implType != null)
                    {
                        return implType;
                    }
                    implType = ServiceManager.RootMap.FirstOrDefault<RegServiceAttribute>(x => x.IntfType == intfType);
                    if (implType != null)
                    {
                        return implType;
                    }
                }
                else if (level.HasFlag(Level.SESSION))
                {
                    implType = ServiceManager.SessionMap.FirstOrDefault<RegServiceAttribute>(x => x.IntfType == intfType);
                    if (implType != null)
                    {
                        return implType;
                    }
                    implType = ServiceManager.RootMap.FirstOrDefault<RegServiceAttribute>(x => x.IntfType == intfType);
                    if (implType != null)
                    {
                        return implType;
                    }
                }
                else if (level.HasFlag(Level.ROOT))
                {
                    implType = ServiceManager.RootMap.FirstOrDefault<RegServiceAttribute>(x => x.IntfType == intfType);
                    if (implType != null)
                    {
                        return implType;
                    }
                }
            }

            throw new NotImplementedException(intfType.Name + "未注册实现类！");
        }

        public TService Get<TService>(params object[] existing)
        {
            return (TService)this.Get(typeof(TService));
        }

        public TService Get<TService>(string name, params object[] existing)
        {
            return (TService)this.Get(typeof(TService), name);
        }

        public object Get(Type type, params object[] existing)
        {
            if (type.IsClass)
            {
                return this.cache.GetOrAdd(type, x => new Implement(x, LoadingPriority.__InnerLowest)).New();
            }
            return this.cache.GetOrAdd(type, x => new Implement(this.FindImplType(x, this.Level).ImplType, LoadingPriority.__InnerLowest)).New();
        }

        public object Get(Type type, string name, params object[] existing)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TService> GetAll<TService>(params object[] existing)
        {
            return this.GetAll(typeof(TService)).Cast<TService>();
        }

        public IEnumerable<object> GetAll(Type type, params object[] existing)
        {
            throw new NotImplementedException();
        }

        // Properties
        public Level Level { get; private set; }

        // Nested Types
        private class ImplCollection
        {
            // Fields
            private readonly List<SimpleLocatorWrapper.Implement> _list = new List<SimpleLocatorWrapper.Implement>(1);
            private bool _sorted = true;

            // Methods
            public void Add(Type implType, LoadingPriority priority)
            {
                this._list.Add(new SimpleLocatorWrapper.Implement(implType, priority));
                this._sorted = this._list.Count < 2;
            }

            internal List<T> GetAllImpls<T>()
            {
                if (!this._sorted)
                {
                    this._list.Sort();
                }
                List<T> result = new List<T>();
                for (int i = this._list.Count - 1; i >= 0; i--)
                {
                    result.Add((T)this._list[i].New());
                }
                return result;
            }

            public object GetHighestImpl()
            {
                if (!this._sorted)
                {
                    this._list.Sort();
                }
                return ((this._list.Count > 0) ? this._list[this._list.Count - 1].New() : null);
            }

            public void Remove(Type implType)
            {
                if (!this._sorted)
                {
                    this._list.Sort();
                }
                int i = 0;
                int c = this._list.Count;
                while (i < c)
                {
                    SimpleLocatorWrapper.Implement item = this._list[i];
                    if (item.ImplType.Equals(implType))
                    {
                        this._list.RemoveAt(i);
                        break;
                    }
                    i++;
                }
            }

            // Properties
            public int Count
            {
                get
                {
                    return this._list.Count;
                }
            }
        }

        private class Implement : IComparable<SimpleLocatorWrapper.Implement>
        {
            // Fields
            public Func<object> New;

            // Methods
            public Implement(Type implType, LoadingPriority priority)
            {
                this.ImplType = implType;
                this.Priority = (byte)priority;
                this.New = Expression.Lambda<Func<object>>(Expression.New(implType), new ParameterExpression[0]).Compile();
            }

            public int CompareTo(SimpleLocatorWrapper.Implement other)
            {
                int result = this.Priority - other.Priority;
                if (result == 0)
                {
                    result = string.Compare(this.ImplType.FullName, other.ImplType.FullName);
                }
                return result;
            }

            // Properties
            public Type ImplType { get; private set; }

            public byte Priority { get; private set; }
        }
    }


}
