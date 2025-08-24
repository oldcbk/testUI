using System;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class InputFieldRegistry_Editor: DescSO
{
	public IReadOnlyList<Entry> entries => m_entries;
	private List<Entry> m_entries = new(11)
	{
		new(typeof(int),        s => new IntegerField(s),   typeof(IntegerField)),
		new(typeof(float),      s => new FloatField(s),     typeof(FloatField)),
		new(typeof(double),     s => new DoubleField(s),    typeof(DoubleField)),
		new(typeof(long),       s => new LongField(s),      typeof(LongField)),
		new(typeof(Hash128),    s => new Hash128Field(s),   typeof(Hash128Field)),
		new(typeof(Vector2),    s => new Vector2Field(s),   typeof(Vector2Field)),
		new(typeof(Vector3),    s => new Vector3Field(s),   typeof(Vector3Field)),
		new(typeof(Vector4),    s => new Vector4Field(s),   typeof(Vector4Field)),
		new(typeof(Rect),       s => new RectField(s),      typeof(RectField)),
		new(typeof(Bounds),     s => new BoundsField(s),    typeof(BoundsField)),
		new(typeof(UnityEngine.Object),	s => new ObjectField(s),	typeof(UnityEngine.Object)),
	};
	public bool Register(Entry entry)
	{
		if (m_entries.Find(e => e.m_valueTy == entry.m_valueTy) != null)
		{
			Debug.LogWarning($"表中已经有 valueTy == <{entry.m_valueTy}> 的项了!");
			return false;
		}
		m_entries.Add(entry);
		return true;
	}
	public class Entry
	{
		public delegate VisualElement Ctor(string label);
		public Type m_valueTy;
		public Ctor m_ctor;
		public Type m_elemTy;
		public Entry(Type valueTy, Ctor ctor, Type elemTy)
		{
			m_valueTy = valueTy;
			m_ctor = ctor;
			m_elemTy = elemTy;
		}
	}
}
