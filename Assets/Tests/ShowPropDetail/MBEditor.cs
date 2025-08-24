#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace ShowPropDetail
{
	/// <summary>
	/// 或许用自定义属性更好,会比较灵活
	/// </summary>
	[CustomEditor(typeof(MB))]
	public class MBEditor: Editor
	{
		private Editor m_SOE;
		private void OnEnable()
		{
			m_SOE = null;
		}
		public override VisualElement CreateInspectorGUI()
		{
			VisualElement root = new();
			InspectorElement.FillDefaultInspector(root, serializedObject, this);
			PropertyField propField = root.Query<PropertyField>().Where(e => e.name.Contains("<SO>"));
			Box box = new();
			propField.parent.Insert(propField.parent.IndexOf(propField), box);
			box.Add(propField);
			Foldout foldout = new() { text = "SO Inspector" };
			box.Add(foldout);
			propField.RegisterValueChangeCallback(evt =>
			{
				if (m_SOE != null) DestroyImmediate(m_SOE);
				if (evt.changedProperty.objectReferenceValue == null)
				{
					foldout.style.display = DisplayStyle.None;
					return;
				}
				foldout.style.display = DisplayStyle.Flex;
				m_SOE = CreateEditor(evt.changedProperty.objectReferenceValue);
				foldout.Clear();
				foldout.Add(new InspectorElement(m_SOE));
			});
			m_SOE = CreateEditor((target as MB).SO);
			if (m_SOE != null)
			{
				foldout.Add(new InspectorElement(m_SOE));
			}
			return root;
		}
		private void OnDisable()
		{
			if (m_SOE != null)
			{
				DestroyImmediate(m_SOE);
				m_SOE = null;
			}
		}
		/*
		private void OnValidate()
		{
			var sel = Selection.activeGameObject;
			Selection.activeGameObject = null;
			Selection.activeGameObject = sel;
		}
		*/
	}
}
#endif