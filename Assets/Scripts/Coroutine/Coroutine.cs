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

        //当前是否不需要等待
        private bool m_noNeedWait = true;

        public Coroutine() { }

        public Coroutine(IEnumerator enumerator)
        {
            Enumerator = enumerator;
        }


        public bool IsDone(float deltaTime)
        {
            bool canMoveNext = true;
            if (m_noNeedWait)
            {
                canMoveNext = Enumerator.MoveNext();
            }
            if (canMoveNext)
            {
                var current = Enumerator.Current;
                if (current is IWaitable)
                {
                    IWaitable waitable = current as IWaitable;
                    m_noNeedWait = waitable.IsTickOver(deltaTime);
                    //PS(2)，如果等待的是协程结束，则直接在该帧执行接下来内容, 与Unity的结果保持一致
                    if (m_noNeedWait && waitable is Coroutine)
                    {
                        return IsDone(deltaTime);
                    }
                    return false;
                }
            }
            return !canMoveNext;
        }


        //由于协程执行完后，会触发Reset函数将Enumerator置为空
        //所以可直接判断迭代器是否为空即可知道'等待协程'是否结束
        public bool IsTickOver(float deltaTime)
        {
            return Enumerator == null;
        }


        public void Reset()
        {
            Enumerator = null;
            m_noNeedWait = true;
#if UNITY_EDITOR
            m_coroutineID = ++s_coroutineID;
#endif
        }

    }

}
