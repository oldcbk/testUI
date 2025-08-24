using UnityEngine;

[CreateAssetMenu(fileName = "nasm_Desc", menuName = "SO/DescSO")]
public class DescSO: ScriptableObject
{
	[TextArea(1, 20)]
	public string m_描述;
}
