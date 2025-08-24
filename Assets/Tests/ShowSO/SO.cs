using System;
using UnityEngine;

namespace ShowSO
{
	[CreateAssetMenu(fileName = "name_SO", menuName = "Tests/ShowSO-SO")]
	public class SO: ScriptableObject
	{
		[SerializeField]
		private int m_int;
		[SerializeField]
		private Type m_type;
	}
}