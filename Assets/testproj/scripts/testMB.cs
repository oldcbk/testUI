using UnityEngine;

public class testMB: MonoBehaviour
{
    public IntEC m_EC;
    private void Start()
    {
        if (m_EC != null)
        {
            m_EC.p1evt += OnStart;
        }
    }

    private void OnDestroy()
    {
        if (m_EC != null)
        {
            m_EC.p1evt -= OnStart;
        }
    }

    void OnStart(int i)
    {
        print($"{i}");
    }
}
