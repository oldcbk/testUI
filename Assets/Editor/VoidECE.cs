#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.Properties;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

[CustomEditor(typeof(VoidEC), true)]
public class VoidECE: Editor
{
    private static bool s_flush = false;
    [SerializeField]
    private VisualTreeAsset m_invocationListView;
    private VoidEC m_EC;
    [CreateProperty]
    private List<InfoHolder> m_infos = new();
    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new();
        InspectorElement.FillDefaultInspector(root, serializedObject, this);
        var box = m_invocationListView.Instantiate();
        root.Add(box);
        var listView = box.Q<ListView>();
        listView.headerTitle = "p0evt";
        listView.dataSource = this;
        listView.itemsSource = m_infos;
        if (m_infos.Count > 0)
        {
            var button = box.Q<Button>();
            button.style.display = DisplayStyle.Flex;
            button.clicked += () => { m_EC.Invoke(); };
        }
        return root;
    }

    private void OnEnable()
    {
        Assert.IsNotNull(m_invocationListView);
        s_flush = !s_flush;
        if (s_flush)
        {
            var targetObj = target;
            Selection.activeObject = null;
            EditorApplication.delayCall += () => Selection.activeObject = targetObj;
        }
        m_EC = target as VoidEC;
        m_infos = new();
        var type = typeof(VoidEC);
        var fieldInfo = type.GetField("p0evt", BindingFlags.NonPublic | BindingFlags.Instance);
        var dlgt = (Delegate)fieldInfo.GetValue(m_EC);
        if (dlgt == null) return;
        var invocations = dlgt.GetInvocationList();
        if (invocations != null && invocations.Length > 0)
        {
            foreach (var invocation in invocations)
            {
                if (invocation.Target is UnityEngine.Object uObj)
                {
                    m_infos.Add(new(uObj));
                }
                else
                {
                    Debug.LogWarning(invocation.Target);
                }
            }
        }
    }

    private void OnDisable()
    {
        m_EC = null;
        m_infos = null;
    }

    public class InfoHolder
    {
        [CreateProperty]
        public UnityEngine.Object m_obj;
        public InfoHolder(UnityEngine.Object obj) => m_obj = obj;
    }
}

#endif
