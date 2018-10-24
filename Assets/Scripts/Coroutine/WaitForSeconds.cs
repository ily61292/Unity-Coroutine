using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hi
{
    public class WaitForSeconds : IWaitable
    {
        private float m_elapsedTime;
        private float m_endTime;

        public WaitForSeconds(float delay)
        {
            m_elapsedTime = 0;
            m_endTime = delay;
        }

        public bool IsTickOver(float deltaTime)
        {
            m_elapsedTime += deltaTime;
            return m_elapsedTime > m_endTime;
        }


        public void Reset()
        {
            m_elapsedTime = 0;
        }
    }
}
