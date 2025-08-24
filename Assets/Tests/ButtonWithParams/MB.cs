using System;
using UnityEngine;

namespace ButtonWithParams
{
	public class MB: MonoBehaviour
	{
		[EditorCallable]
		public void foo(Vector3 v3, Type type, Color color)
		{
			Debug.Log($"foo: v3=({v3}),type=({type}),color=({color})");
		}
	}
}