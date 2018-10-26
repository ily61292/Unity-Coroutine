using System.Collections;
using System.Collections.Generic;


namespace Hi
{

    public class Coroutine : IWaitable
    {
#if UNITY_EDITOR
        static int s_coroutineID = 0;
        public int m_coroutineID = 0;
#endif 
        public IEnumerator Enumerator { get; set; }

        public Coroutine() { }

        public Coroutine(IEnumerator enumerator)
        {
            Enumerator = enumerator;
        }


        public bool IsDone(float deltaTime)
        {
            bool isNoNeedWait = true, isMoveOver = true;
            var current = Enumerator.Current;
            if (current is IWaitable)
            {
                IWaitable waitable = current as IWaitable;
                isNoNeedWait = waitable.IsTickOver(deltaTime);
            }
            if (isNoNeedWait)
            {
                isMoveOver = Enumerator.MoveNext();
            }
            return !isMoveOver;
        }


        public bool IsTickOver(float deltaTime)
        {
            return Enumerator == null;
        }


        public void Reset()
        {
            Enumerator = null;
#if UNITY_EDITOR
            m_coroutineID = ++s_coroutineID;
#endif
        }

    }

}
