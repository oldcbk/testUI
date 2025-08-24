#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace ButtonWithParams
{
	public static class ParamSOFactory
	{
		private static AssemblyBuilder m_asmBuilder;
		private static ModuleBuilder m_modBuilder;
		private static readonly Dictionary<MethodInfo, Type> m_cache = new();
		static ParamSOFactory()
		{
			var asmName = new AssemblyName("DynamicParamContainers");
			m_asmBuilder = AssemblyBuilder.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.Run);
			m_modBuilder = m_asmBuilder.DefineDynamicModule("Main");
		}
		public static ScriptableObject CreateParamSO(MethodInfo methodInfo)
		{
			if (m_cache.TryGetValue(methodInfo, out var cachedType))
				return ScriptableObject.CreateInstance(cachedType);
			string typeName = $"{methodInfo.DeclaringType.Name}_{methodInfo.Name}_Params";
			var typeBuilder = m_modBuilder.DefineType(typeName,
				TypeAttributes.Public | TypeAttributes.Class,
				typeof(ScriptableObject));
			foreach (var paramInfo in methodInfo.GetParameters())
			{
				var fieldBuilder = typeBuilder.DefineField(paramInfo.Name, paramInfo.ParameterType, FieldAttributes.Public);
				var ctorInfo = typeof(SerializeField).GetConstructor(Type.EmptyTypes);
				var customAttrBuilder = new CustomAttributeBuilder(ctorInfo, new object[] { });
				fieldBuilder.SetCustomAttribute(customAttrBuilder);
			}
			var newType = typeBuilder.CreateType();
			m_cache.Add(methodInfo, newType);
			return ScriptableObject.CreateInstance(newType);
		}
	}
}
#endif