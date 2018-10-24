using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hi
{
    public class WaitForFrame : IWaitable
    {
        private int m_elapsedFrame;
        private int m_endFrame;

        public WaitForFrame(int frame)
        {
            m_elapsedFrame = 0;
            m_endFrame = frame;
        }

        public bool IsTickOver(float deltaTime)
        {
            return ++m_elapsedFrame >= m_endFrame;
        }

        public void Reset()
        {
            m_elapsedFrame = 0;
        }
    }
}


