using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(IntEC))]
public class IntECE: Editor
{
	private IntEC m_EC;

	public override VisualElement CreateInspectorGUI()
	{
		VisualElement root = new();
		ButtonParams buttonParams = new(typeof(IntEC).GetMethod("Invoke")) { obj = m_EC };
		root.Add(buttonParams);
		return root;
	}

	private void OnEnable()
	{
		m_EC = (IntEC)target;
	}
	private void OnDisable()
	{
		m_EC = null;
	}
}
