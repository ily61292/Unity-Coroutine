using System.Collections.Generic;
using System;


namespace Hi
{
    public class ObjectPool<T> where T : new()
    {
        protected readonly Stack<T> m_stack = new Stack<T>();
        protected readonly Action<T> m_actionOnGet;
        protected readonly Action<T> m_actionOnRelease;

        public int CountAll { get; protected set; }
        public int CountActive { get { return CountAll - CountInactive; } }
        public int CountInactive { get { return m_stack.Count; } }


        public ObjectPool(Action<T> actionOnGet = null, Action<T> actionOnRelease = null)
        {
            m_actionOnGet = actionOnGet;
            m_actionOnRelease = actionOnRelease;
        }


        public T Get()
        {
            T element;
            if (m_stack.Count == 0)
            {
                element = new T();
                CountAll++;
            }
            else
            {
                element = m_stack.Pop();
            }
            //m_actionOnGet?.Invoke(element);
            if (m_actionOnGet != null)
            {
                m_actionOnGet(element);
            }
            return element;
        }


        public void Release(T element)
        {
            if (m_stack.Count > 0 && ReferenceEquals(m_stack.Peek(), element))
            {
                UnityEngine.Debug.LogError("Internal error. Trying to destroy object that is already released to pool.");

            }
            if (m_actionOnRelease != null)
            {
                m_actionOnRelease(element);
            }
            m_stack.Push(element);
        }
    }

}

