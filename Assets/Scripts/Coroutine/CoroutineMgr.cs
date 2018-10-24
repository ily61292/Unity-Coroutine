#define USE_OBJECTPOOL 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hi
{

    public class CoroutineMgr : MonoBehaviour
    {

        public static CoroutineMgr Instance;

        static CoroutineMgr()
        {
            GameObject go = new GameObject("CoroutineMgr");
            Instance = go.AddComponent<CoroutineMgr>();
        }
#if USE_OBJECTPOOL
        private ObjectPool<Coroutine> m_pool = new ObjectPool<Coroutine>(null, element => element.Reset());
#endif
        private List<Coroutine> m_yieldList = new List<Coroutine>();
        private List<Coroutine> m_swapList = new List<Coroutine>();



        // 协程是在Update之后，LateUpdate之前执行 https://docs.unity3d.com/Manual/ExecutionOrder.html
        // 而该执行顺序Unity未暴露出接口，只能将就着放在LateUpdate中
        void LateUpdate()
        {
            if (m_yieldList.Count > 0)
            {
                var temp = m_yieldList;
                var oriCount = m_yieldList.Count;
                Coroutine curCoro;
                for (int i = 0; i < m_yieldList.Count; i++)
                {
                    curCoro = m_yieldList[i];
                    if (curCoro.IsDone(Time.deltaTime))
                    {
#if USE_OBJECTPOOL
                        m_pool.Release(curCoro);
#else
                        curCoro.Reset();
#endif
                    }
                    else
                    {
                        //PS(1)，由于嵌套协程，所以位于最里层的协程要最先执行
                        if (i >= oriCount)
                        {
                            m_swapList.Insert(0, curCoro);
                        }
                        else
                        {
                            m_swapList.Add(curCoro);
                        }
                    }
                }
                m_yieldList = m_swapList;
                m_swapList = temp;
                m_swapList.Clear();
            }
        }


        public new Coroutine StartCoroutine(IEnumerator e)
        {
#if USE_OBJECTPOOL
            Coroutine coro = m_pool.Get();
            coro.Enumerator = e;
#else
            Coroutine coro = new Coroutine(e);
#endif

            m_yieldList.Add(coro);
            return coro;
        }


        public Coroutine StartCoroutine_Auto()
        {
            throw new UnityException("Can not use this method, use \"StartCoroutine\" instead.");
        }


        public new void StopCoroutine(IEnumerator e)
        {

        }
    }
}

