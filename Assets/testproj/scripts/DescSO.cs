using UnityEngine;

public abstract class DescSO: ScriptableObject
{
    [TextArea(1, 20)]
    public string m_描述;
}
