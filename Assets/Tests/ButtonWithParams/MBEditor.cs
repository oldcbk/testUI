using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace ButtonWithParams
{
	[CustomEditor(typeof(MB))]
	public class MBEditor: Editor
	{
		public override VisualElement CreateInspectorGUI()
		{
			VisualElement root = new();
			InspectorElement.FillDefaultInspector(root, serializedObject, this);
			var methodInfo = target.GetType().GetMethod("foo", BindingFlags.Public | BindingFlags.Instance);
			Assert.IsNotNull(methodInfo.GetCustomAttribute<EditorCallableAttribute>());
			var paramSO = ParamSOFactory.CreateParamSO(methodInfo);
			SerializedObject so = new(paramSO);
			Foldout foldout = new() { text = methodInfo.Name };
			foreach (var paramInfo in methodInfo.GetParameters())
			{
				var sp = so.FindProperty(paramInfo.Name);
				if (sp != null)
				{
					PropertyField propField = new(sp, $"{paramInfo.ParameterType.Name} - {paramInfo.Name}");
					propField.Bind(so);
					foldout.Add(propField);
				}
				else
					foldout.Add(new Label($"{paramInfo.ParameterType.Name} - {paramInfo.Name}"));
			}
			Button button = new(() =>
			{
				so.ApplyModifiedProperties();
				object[] args = methodInfo.GetParameters()
					.Select(p => GetValueFromSO(paramSO, p.Name)).ToArray();
				methodInfo.Invoke(target, args);
			})
			{ text = "Invoke" };
			foldout.Add(button);
			root.Add(foldout);
			return root;
		}
		private object GetValueFromSO(object so, string fieldName)
		{
			var fieldInfo = so.GetType().GetField(fieldName);
			return fieldInfo.GetValue(so);
		}
	}
}