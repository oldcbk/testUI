using UnityEngine;

public class testMB: MonoBehaviour
{
    public VoidEC m_EC;
    private void Start()
    {
        if (m_EC != null)
        {
            m_EC.p0evt += OnStart;
        }
    }

    private void OnDestroy()
    {
        if (m_EC != null)
        {
            m_EC.p0evt -= OnStart;
        }
    }

    void OnStart()
    {
        print("hello");
    }
}
