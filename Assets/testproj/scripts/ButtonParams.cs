using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

[UxmlElement]
public partial class ButtonParams: VisualElement
{
	private object m_obj;
	private MethodInfo m_methodInfo;
	private ParameterInfo[] m_paramInfos;
	private string m_show;
	public ButtonParams()
	{
		m_box = new();
		m_foldout = new();
		m_button = new();
		m_box.Add(m_foldout);
		m_box.Add(m_button);
		Add(m_box);
		m_foldout.text = "params:";
		m_button.text = "Invoke";
		m_button.clicked += OnClicked;
	}
	public ButtonParams(MethodInfo methodInfo) : this() => SetMethodInfo(methodInfo);
	~ButtonParams()
	{
		m_button.clicked -= OnClicked;
	}

	private bool isTypeMatch => m_obj.GetType() == m_methodInfo.DeclaringType || m_obj.GetType().IsSubclassOf(m_methodInfo.DeclaringType);
	public object obj
	{
		get => m_obj; set
		{
			if (m_obj == value) return;
			m_obj = value;
			if (m_methodInfo != null && !isTypeMatch)
			{
				m_button.SetEnabled(false);
				Debug.LogError("methodInfo和obj中存在错误绑定,无法Invoke");
				return;
			}
			m_button.SetEnabled(true);
		}
	}
	public MethodInfo methodInfo { get => m_methodInfo; set => SetMethodInfo(value); }

	private void SetMethodInfo(MethodInfo methodInfo)
	{
		if (methodInfo == m_methodInfo) return;
		if (m_obj != null && !isTypeMatch)
		{
			m_button.SetEnabled(false);
			Debug.LogError("methodInfo和obj中存在错误绑定,无法Invoke");
			return;
		}
		m_button.SetEnabled(true);
		m_foldout.Clear();
		m_foldoutContents.Clear();
		m_methodInfo = methodInfo;
		m_paramInfos = m_methodInfo.GetParameters();
		foreach (var paramInfo in m_paramInfos)
		{
			var type = paramInfo.ParameterType;
			var ve = GenInputField(type);
			m_foldout.Add(ve);
			m_foldoutContents.Add(ve);
		}
	}

	private void OnClicked()
	{
		if (m_methodInfo == null || m_obj == null || !isTypeMatch)
		{
			m_button.SetEnabled(false);
			Debug.LogError("methodInfo和obj中存在错误绑定,无法Invoke");
			return;
		}
		methodInfo.Invoke(m_obj, GetArgs());
	}

	private object[] GetArgs()
	{
		object[] args = new object[m_foldoutContents.Count];
		for (int i = 0; i < m_foldoutContents.Count; ++i)
		{
			if (m_foldoutContents[i] is Label)
			{
				args[i] = null;
				continue;
			}
			var type = FindGenericBaseClass(m_foldoutContents[i].GetType(), typeof(BaseField<>));
			Assert.IsNotNull(type);
			if (s_dict.ContainsKey(type.GenericTypeArguments[0]))
			{
				var propInfo = type.GetProperty("value");
				args[i] = propInfo.GetValue(m_foldoutContents[i]);
			}
			else
			{
				throw new ApplicationException();
			}
		}
		return args;
	}

	public static Type FindGenericBaseClass(Type derived, Type genericBase)
	{
		Assert.IsTrue(genericBase.IsGenericType);
		while (derived != null)
		{
			if (derived.IsGenericType && derived.GetGenericTypeDefinition() == genericBase)
			{
				return derived;
			}
			derived = derived.BaseType;
		}
		return null;
	}

	private VisualElement GenInputField(Type type)
	{
		if (type.IsClass) return new Label(type.Name);
		if (s_dict.TryGetValue(type, out var ctor))
		{
			return ctor(type.Name);
		}
		throw new NotImplementedException();
	}

	private delegate VisualElement VECtor(string label);
	private static readonly Dictionary<Type, VECtor> s_dict = new(10)
	{
		{ typeof(int),      label => new IntegerField(label)},
		{ typeof(float),    label => new FloatField(label)},
		{ typeof(double),   label => new DoubleField(label)},
		{ typeof(long),     label => new LongField(label)},
		{ typeof(Hash128),  label => new Hash128Field(label)},
		{ typeof(Vector2),  label => new Vector2Field(label)},
		{ typeof(Vector3),  label => new Vector3Field(label)},
		{ typeof(Vector4),  label => new Vector4Field(label)},
		{ typeof(Rect),     label => new RectField(label)},
		{ typeof(Bounds),   label => new BoundsField(label)},
	};

	[UxmlAttribute]
	private string methodInfoShow
	{
		get
		{
			m_show = $"{m_methodInfo?.Name}({m_paramInfos?.ToString()})";
			return m_show;
		}
		set { var _ = value; }
	}

	[UxmlAttribute]
	public string foldoutText { get => m_foldout.text; set => m_foldout.text = value; }
	[UxmlAttribute]
	public string buttonText { get => m_button.text; set => m_button.text = value; }

	private Box m_box;
	private Foldout m_foldout;
	private List<VisualElement> m_foldoutContents = new();
	private Button m_button;
}