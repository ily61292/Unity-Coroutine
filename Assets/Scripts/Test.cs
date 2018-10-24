using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("自己 普通协程"))
        {
            Hi.CoroutineMgr.Instance.StartCoroutine(TestNormalCoroutine());
        }
        GUILayout.Space(20);
        if (GUILayout.Button("Unity 普通协程"))
        {
            StartCoroutine(UnityNormalCoroutine());
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(20);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("自己 带等待时间的协程"))
        {
            Hi.CoroutineMgr.Instance.StartCoroutine(TestWaitFor());
        }
        GUILayout.Space(20);
        if (GUILayout.Button("Unity 带等待时间的协程"))
        {
            StartCoroutine(UnityWaitFor());
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(20);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("自己 嵌套的协程"))
        {
            Hi.CoroutineMgr.Instance.StartCoroutine(TestNesting());
        }
        GUILayout.Space(20);
        if (GUILayout.Button("Unity 嵌套的协程"))
        {
            StartCoroutine(UnityNesting());
        }
        GUILayout.EndHorizontal();
    }

    IEnumerator TestNormalCoroutine()
    {
        Log("NormalCoroutine 1");
        yield return null;
        Log("NormalCoroutine 2");
        yield return 1;
        Log("NormalCoroutine 3");
        yield break;
        Log("NormalCoroutine 4");
    }


    IEnumerator UnityNormalCoroutine()
    {
        LogWarn("UnityNormalCoroutine 1");
        yield return null;
        LogWarn("UnityNormalCoroutine 2");
        yield return 1;
        LogWarn("UnityNormalCoroutine 3");
        yield break;
        LogWarn("UnityNormalCoroutine 4");
    }


    IEnumerator TestWaitFor()
    {
        Log("WaitForSeconds 1");
        yield return new Hi.WaitForSeconds(1.5f);
        Log("WaitForSeconds 2");
    }


    IEnumerator UnityWaitFor()
    {
        LogWarn("UnityWaitFor 1");
        yield return new UnityEngine.WaitForSeconds(1.5f);
        LogWarn("UnityWaitFor 2");
    }



    IEnumerator TestNesting()
    {
        Log("Nesting 1");
        yield return Hi.CoroutineMgr.Instance.StartCoroutine(TestNesting__());
        Log("Nesting 2");
    }


    IEnumerator TestNesting__()
    {
        Log("Nesting__ 1");
        yield return Hi.CoroutineMgr.Instance.StartCoroutine(TestNormalCoroutine());
        Log("Nesting__ 2");
        yield return Hi.CoroutineMgr.Instance.StartCoroutine(TestWaitFor());
        Log("Nesting__ 3");
    }




    IEnumerator UnityNesting()
    {
        LogWarn("UnityNesting 1");
        yield return StartCoroutine(UnityTesting__());
        LogWarn("UnityNesting 2");
    }


    IEnumerator UnityTesting__()
    {
        LogWarn("UnityTesting__ 1");
        yield return StartCoroutine(UnityNormalCoroutine());
        LogWarn("UnityTesting__ 2");
        yield return StartCoroutine(UnityWaitFor());
        LogWarn("UnityTesting__ 3");
    }


    void Log(string message)
    {
        Debug.LogFormat("<color=yellow>[{0}]</color>-<color=cyan>[{1}]</color>{2}", Time.frameCount, System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"), message);
    }

    void LogWarn(string message)
    {
        Debug.LogWarningFormat("<color=yellow>[{0}]</color>-<color=cyan>[{1}]</color>{2}", Time.frameCount, System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"), message);
    }
}