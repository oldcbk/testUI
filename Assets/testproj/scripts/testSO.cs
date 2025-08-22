using Unity.Properties;
using UnityEngine;

[CreateAssetMenu(fileName = "testSO", menuName = "Scriptable Objects/testSO")]
public class testSO: ScriptableObject
{
    [SerializeField, DontCreateProperty]
    private string m_str;

    [CreateProperty]
    public string str
    {
        get => m_str;
        set => m_str = value;
    }

    [field: SerializeField, DontCreateProperty]
    [CreateProperty]
    public string str2 { get; set; }

    public string str3;
}
