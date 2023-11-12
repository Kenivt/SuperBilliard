//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace GameFramework
{
    /// <summary>
    /// 自定义的比较类,严格限制为比较引用,原因是protobuf的Equals方法会比较值,导致获取对象异常
    /// </summary>
    public class CustomEqualityComparer : IEqualityComparer<IReference>
    {
        public bool Equals(IReference x, IReference y)
        {
            return ReferenceEquals(x, y);
        }

        //这个GetHashCode可能会有异常操作...
        //TODO:将要修改的地方，最好依据ID来分配Hash
        public int GetHashCode(IReference obj)
        {
            if (obj == null)
            {
                return 0;
            }

            return obj.GetHashCode();
        }
    }

    public static partial class ReferencePool
    {
        private sealed class ReferenceCollection
        {
            private readonly Queue<IReference> m_References;
            private readonly Type m_ReferenceType;
            private int m_UsingReferenceCount;
            private int m_AcquireReferenceCount;
            private int m_ReleaseReferenceCount;
            private int m_AddReferenceCount;
            private int m_RemoveReferenceCount;
            //自定义
            private readonly HashSet<IReference> m_ReferencesSet;
            public ReferenceCollection(Type referenceType)
            {
                m_References = new Queue<IReference>();
                m_ReferenceType = referenceType;
                m_UsingReferenceCount = 0;
                m_AcquireReferenceCount = 0;
                m_ReleaseReferenceCount = 0;
                m_AddReferenceCount = 0;
                m_RemoveReferenceCount = 0;
                //自定义比较器
                m_ReferencesSet = new HashSet<IReference>(new CustomEqualityComparer());
            }

            public Type ReferenceType
            {
                get
                {
                    return m_ReferenceType;
                }
            }

            public int UnusedReferenceCount
            {
                get
                {
                    return m_References.Count;
                }
            }

            public int UsingReferenceCount
            {
                get
                {
                    return m_UsingReferenceCount;
                }
            }

            public int AcquireReferenceCount
            {
                get
                {
                    return m_AcquireReferenceCount;
                }
            }

            public int ReleaseReferenceCount
            {
                get
                {
                    return m_ReleaseReferenceCount;
                }
            }

            public int AddReferenceCount
            {
                get
                {
                    return m_AddReferenceCount;
                }
            }

            public int RemoveReferenceCount
            {
                get
                {
                    return m_RemoveReferenceCount;
                }
            }

            public T Acquire<T>() where T : class, IReference, new()
            {
                if (typeof(T) != m_ReferenceType)
                {
                    throw new Exception("Type is invalid.");
                }
                T instance = null;
                m_UsingReferenceCount++;
                m_AcquireReferenceCount++;
                lock (m_References)
                {
                    if (m_References.Count > 0)
                    {
                        instance = (T)m_References.Dequeue();
                    }
                }
                if (instance != null)
                {
                    lock (m_ReferencesSet)
                    {
                        m_ReferencesSet.Remove(instance);
                    }
                    return instance;
                }

                m_AddReferenceCount++;
                return new T();
            }
            public IReference Acquire()
            {
                m_UsingReferenceCount++;
                m_AcquireReferenceCount++;
                IReference reference = null;
                lock (m_References)
                {
                    if (m_References.Count > 0)
                    {
                        reference = m_References.Dequeue();
                    }
                }
                if (reference != null)
                {
                    lock (m_ReferencesSet)
                    {
                        m_ReferencesSet.Remove(reference);
                    }
                    return reference;
                }
                m_AddReferenceCount++;
                return (IReference)Activator.CreateInstance(m_ReferenceType);
            }
            public void Release(IReference reference)
            {
                reference.Clear();
                lock (m_References)
                {
                    if (m_EnableStrictCheck && m_ReferencesSet.Contains(reference))
                    {
                        throw new Exception("The reference has been released.");
                    }

                    m_References.Enqueue(reference);
                }
                lock (m_ReferencesSet)
                {
                    m_ReferencesSet.Add(reference);
                }

                m_ReleaseReferenceCount++;
                m_UsingReferenceCount--;
            }

            public void Add<T>(int count) where T : class, IReference, new()
            {
                if (typeof(T) != m_ReferenceType)
                {
                    throw new Exception("Type is invalid.");
                }
                Queue<IReference> instances = new Queue<IReference>(count);

                lock (m_References)
                {
                    m_AddReferenceCount += count;
                    while (count-- > 0)
                    {
                        T t = new T();
                        m_References.Enqueue(t);
                        instances.Enqueue(t);
                    }
                }
                lock (m_ReferencesSet)
                {
                    while (instances.Count > 0)
                    {
                        m_ReferencesSet.Add(instances.Dequeue());
                    }
                }
            }

            public void Add(int count)
            {
                Queue<IReference> references = new Queue<IReference>(count);

                lock (m_References)
                {
                    m_AddReferenceCount += count;
                    while (count-- > 0)
                    {
                        IReference reference = (IReference)Activator.CreateInstance(m_ReferenceType);
                        references.Enqueue(reference);
                        m_References.Enqueue(reference);
                    }
                }
                lock (m_ReferencesSet)
                {
                    while (references.Count > 0)
                    {
                        m_ReferencesSet.Add(references.Dequeue());
                    }
                }
            }

            public void Remove(int count)
            {
                Queue<IReference> references = new Queue<IReference>(count);
                lock (m_References)
                {
                    if (count > m_References.Count)
                    {
                        count = m_References.Count;
                    }

                    m_RemoveReferenceCount += count;
                    while (count-- > 0)
                    {
                        references.Enqueue(m_References.Dequeue());
                    }
                }
                lock (m_ReferencesSet)
                {
                    while (references.Count > 0)
                    {
                        m_ReferencesSet.Remove(references.Dequeue());
                    }
                }
            }

            public void RemoveAll()
            {
                lock (m_References)
                {
                    m_RemoveReferenceCount += m_References.Count;
                    m_References.Clear();
                }
                lock (m_ReferencesSet)
                {
                    m_References.Clear();
                }
            }
        }
    }
    //public static partial class ReferencePool
    //{
    //    private sealed class ReferenceCollection
    //    {
    //        private readonly Queue<IReference> m_References;
    //        private readonly Type m_ReferenceType;
    //        private int m_UsingReferenceCount;
    //        private int m_AcquireReferenceCount;
    //        private int m_ReleaseReferenceCount;
    //        private int m_AddReferenceCount;
    //        private int m_RemoveReferenceCount;

    //        public ReferenceCollection(Type referenceType)
    //        {
    //            m_References = new Queue<IReference>();
    //            m_ReferenceType = referenceType;
    //            m_UsingReferenceCount = 0;
    //            m_AcquireReferenceCount = 0;
    //            m_ReleaseReferenceCount = 0;
    //            m_AddReferenceCount = 0;
    //            m_RemoveReferenceCount = 0;
    //        }

    //        public Type ReferenceType
    //        {
    //            get
    //            {
    //                return m_ReferenceType;
    //            }
    //        }

    //        public int UnusedReferenceCount
    //        {
    //            get
    //            {
    //                return m_References.Count;
    //            }
    //        }

    //        public int UsingReferenceCount
    //        {
    //            get
    //            {
    //                return m_UsingReferenceCount;
    //            }
    //        }

    //        public int AcquireReferenceCount
    //        {
    //            get
    //            {
    //                return m_AcquireReferenceCount;
    //            }
    //        }

    //        public int ReleaseReferenceCount
    //        {
    //            get
    //            {
    //                return m_ReleaseReferenceCount;
    //            }
    //        }

    //        public int AddReferenceCount
    //        {
    //            get
    //            {
    //                return m_AddReferenceCount;
    //            }
    //        }

    //        public int RemoveReferenceCount
    //        {
    //            get
    //            {
    //                return m_RemoveReferenceCount;
    //            }
    //        }

    //        public T Acquire<T>() where T : class, IReference, new()
    //        {
    //            if (typeof(T) != m_ReferenceType)
    //            {
    //                throw new Exception("Type is invalid.");
    //            }

    //            m_UsingReferenceCount++;
    //            m_AcquireReferenceCount++;
    //            lock (m_References)
    //            {
    //                if (m_References.Count > 0)
    //                {
    //                    return (T)m_References.Dequeue();
    //                }
    //            }

    //            m_AddReferenceCount++;
    //            return new T();
    //        }

    //        public IReference Acquire()
    //        {
    //            m_UsingReferenceCount++;
    //            m_AcquireReferenceCount++;
    //            lock (m_References)
    //            {
    //                if (m_References.Count > 0)
    //                {
    //                    return m_References.Dequeue();
    //                }
    //            }

    //            m_AddReferenceCount++;
    //            return (IReference)Activator.CreateInstance(m_ReferenceType);
    //        }

    //        public void Release(IReference reference)
    //        {
    //            reference.Clear();
    //            lock (m_References)
    //            {
    //                if (m_EnableStrictCheck && m_References.Contains(reference))
    //                {
    //                    throw new Exception("The reference has been released.");
    //                }

    //                m_References.Enqueue(reference);
    //            }

    //            m_ReleaseReferenceCount++;
    //            m_UsingReferenceCount--;
    //        }

    //        public void Add<T>(int count) where T : class, IReference, new()
    //        {
    //            if (typeof(T) != m_ReferenceType)
    //            {
    //                throw new Exception("Type is invalid.");
    //            }

    //            lock (m_References)
    //            {
    //                m_AddReferenceCount += count;
    //                while (count-- > 0)
    //                {
    //                    m_References.Enqueue(new T());
    //                }
    //            }
    //        }

    //        public void Add(int count)
    //        {
    //            lock (m_References)
    //            {
    //                m_AddReferenceCount += count;
    //                while (count-- > 0)
    //                {
    //                    m_References.Enqueue((IReference)Activator.CreateInstance(m_ReferenceType));
    //                }
    //            }
    //        }

    //        public void Remove(int count)
    //        {
    //            lock (m_References)
    //            {
    //                if (count > m_References.Count)
    //                {
    //                    count = m_References.Count;
    //                }

    //                m_RemoveReferenceCount += count;
    //                while (count-- > 0)
    //                {
    //                    m_References.Dequeue();
    //                }
    //            }
    //        }

    //        public void RemoveAll()
    //        {
    //            lock (m_References)
    //            {
    //                m_RemoveReferenceCount += m_References.Count;
    //                m_References.Clear();
    //            }
    //        }
    //    }
    //}
}