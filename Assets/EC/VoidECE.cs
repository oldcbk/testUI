#if UNITY_EDITOR

using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

[CustomEditor(typeof(VoidEC), true)]
public class VoidECE: Editor
{
    private VoidEC m_EC;
    public override VisualElement CreateInspectorGUI()
    {
        VisualElement container = new();
        container.Add(new Button(() =>
        {
            var targetObj = target;
            Selection.activeObject = null;
            EditorApplication.delayCall += () => Selection.activeObject = targetObj;
        })
        { text = "刷新" });
        InspectorElement.FillDefaultInspector(container, serializedObject, this);
        UnityEngine.UIElements.PopupWindow popupWnd = new() { text = "p0evt" };
        container.Add(popupWnd);
        popupWnd.Add(new Label("Subscribers:"));
        var type = typeof(VoidEC);
        var fieldInfo = type.GetField("p0evt", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.IsNotNull(fieldInfo);
        var dlgt = (Delegate)fieldInfo.GetValue(m_EC);
        if (dlgt == null) return container;
        var invocations = dlgt.GetInvocationList();
        if (invocations != null && invocations.Length > 0)
        {
            foreach (var invocation in invocations)
            {
                var obj = invocation.Target;
                if (obj is UnityEngine.Object uObj)
                {
                    ObjectField objField = new(uObj.name);
                    objField.SetValueWithoutNotify(uObj);
                    objField.SetEnabled(false);
                    popupWnd.Add(objField);
                }
                else
                {
                    popupWnd.Add(new Label($"Type: {obj.GetType()}"));
                }
            }
            Button button = new(() => { m_EC.Invoke(); }) { text = "Invoke" };
            popupWnd.Add(button);
        }
        return container;
    }

    private void OnEnable()
    {
        m_EC = target as VoidEC;
    }

    private void OnDisable()
    {
        m_EC = null;
    }
}

#endif
