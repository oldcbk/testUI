#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ShowSO
{
	public class EditorWnd: EditorWindow
	{
		private SO m_SO;
		IMGUIContainer m_IMGUIContainer;

		[MenuItem("Window/Tests/EditorWnd")]
		public static void ShowExample()
		{
			EditorWnd wnd = GetWindow<EditorWnd>();
			wnd.titleContent = new GUIContent("EditorWnd");
		}

		public void CreateGUI()
		{
			m_IMGUIContainer = new(() =>
			{
				EditorGUILayout.HelpBox("选中一个SO以显示其信息", MessageType.Warning);
			});
			OnSelectionChange();
		}

		private void OnSelectionChange()
		{
			var root = rootVisualElement;
			root.Clear();
			if (Selection.objects.Length == 1 && Selection.activeObject is SO)
			{
				m_SO = (SO)Selection.objects[0];
				SerializedObject so = new(m_SO);
				var sp = so.FindProperty("m_int");
				Box box = new();
				root.Add(box);
				InspectorElement ie = new(so);
				box.Add(ie);
				PropertyField propField = new(sp);
				propField.Bind(so);
				root.Add(propField);
			}
			else
			{
				root.Add(m_IMGUIContainer);
			}
		}
	}
}
#endif