#define UNITY
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using UnityEditor;
using UnityEngine;

// https://stackoverflow.com/questions/3862226/how-to-dynamically-create-a-class-in-c
namespace Core.Plugin.Unity.Editor
{
    /// <summary>
    /// Handles settings savings in the Unity Editor.
    /// </summary>
    internal static class SettingsSaver
    {
        public const string FileName = "DNAIEditorSettings.asset";

        //private static readonly TypeBuilder _typeBuilder;
        private static SettingsClassBuilder _settingsClassBuilder;

        static SettingsSaver()
        {
            //            const string typeSignature = "DNAISettings";
            //            var an = new AssemblyName(typeSignature);
            //            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
            //            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
            //            _typeBuilder = moduleBuilder.DefineType(typeSignature,
            //                    TypeAttributes.Public
            //                    | TypeAttributes.Class
            //                    | TypeAttributes.AutoClass
            //                    | TypeAttributes.AnsiClass
            //                    | TypeAttributes.BeforeFieldInit
            //                    | TypeAttributes.AutoLayout,
            //#if UNITY
            //                    typeof(ScriptableObject)
            //#else
            //                    null
            //#endif
            //                    );

            //            Type[] ctorParams = new Type[] { };
            //            ConstructorInfo classCtorInfo = typeof(SerializableAttribute).GetConstructor(ctorParams);
            //            CustomAttributeBuilder myCABuilder = new CustomAttributeBuilder(
            //                                classCtorInfo,
            //                                new object[] { });
            //            _typeBuilder.SetCustomAttribute(myCABuilder);

#if !UNITY
            _settingsClassBuilder = new SettingsClassBuilder();
#endif
        }

        internal static void AddItem(string name, object value)
        {
            LoadSettings();
            //_settingsClassBuilder.Properties.Add(new KeyValuePair<string, object>(name, value));
            //_settingsClassBuilder.Properties.Add(name, value);
            _settingsClassBuilder.Properties[name] = value;
        }

        internal static void AddItem(AUnitySerializable obj)
        {
            _settingsClassBuilder.Objects.Add(obj);
        }

        internal static T GetValue<T>(string name) where T : class
        {
            LoadSettings();
            Debug.Log("Get value => " + _settingsClassBuilder);
            Debug.Log("Contains ppty => " + _settingsClassBuilder.Properties.ContainsKey(name));
            if (!_settingsClassBuilder.Properties.ContainsKey(name))
                return null;
            return _settingsClassBuilder.Properties?[name] as T;
            //var obj = CompileResultType();
            //var instance = Activator.CreateInstance(obj);
            //return obj.GetProperty(name).GetValue(instance);
        }

        //private static Type CompileResultType()
        //{
        //    TypeBuilder tb = _typeBuilder;
        //    ConstructorBuilder constructor = tb.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);

        //    foreach (var field in _settingsClassBuilder.Properties)
        //        CreateProperty(field.Key, field.Value.GetType());

        //    return tb.CreateType();
        //}

        //private static void CreateProperty(string propertyName, Type propertyType)
        //{
        //    FieldBuilder fieldBuilder = _typeBuilder.DefineField(propertyName, propertyType, FieldAttributes.Public);

        //    PropertyBuilder propertyBuilder = _typeBuilder.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);
        //    MethodBuilder getPropMthdBldr = _typeBuilder.DefineMethod("get_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
        //    ILGenerator getIl = getPropMthdBldr.GetILGenerator();

        //    getIl.Emit(OpCodes.Ldarg_0);
        //    getIl.Emit(OpCodes.Ldfld, fieldBuilder);
        //    getIl.Emit(OpCodes.Ret);

        //    MethodBuilder setPropMthdBldr =
        //        _typeBuilder.DefineMethod("set_" + propertyName,
        //          MethodAttributes.Public
        //          | MethodAttributes.SpecialName
        //          | MethodAttributes.HideBySig,
        //          null, new[] { propertyType });

        //    ILGenerator setIl = setPropMthdBldr.GetILGenerator();
        //    Label modifyProperty = setIl.DefineLabel();
        //    Label exitSet = setIl.DefineLabel();

        //    setIl.MarkLabel(modifyProperty);
        //    setIl.Emit(OpCodes.Ldarg_0);
        //    setIl.Emit(OpCodes.Ldarg_1);
        //    setIl.Emit(OpCodes.Stfld, fieldBuilder);

        //    setIl.Emit(OpCodes.Nop);
        //    setIl.MarkLabel(exitSet);
        //    setIl.Emit(OpCodes.Ret);

        //    propertyBuilder.SetGetMethod(getPropMthdBldr);
        //    propertyBuilder.SetSetMethod(setPropMthdBldr);
        //}

        public static void LoadSettings()
        {
#if UNITY
            Directory.CreateDirectory(Constants.RootPath);
            _settingsClassBuilder = AssetDatabase.LoadAssetAtPath<SettingsClassBuilder>(Constants.RootPath + FileName);
            if (_settingsClassBuilder == null)
            {
                _settingsClassBuilder = ScriptableObject.CreateInstance<SettingsClassBuilder>();
                AssetDatabase.CreateAsset(_settingsClassBuilder, Constants.RootPath + FileName);
                AssetDatabase.SaveAssets();
                AssetDatabase.ImportAsset(Constants.RootPath + FileName);
            }
            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(_settingsClassBuilder);
#endif
        }

//        [Serializable]
//        private class SettingsClassBuilder
//#if UNITY
//            : ScriptableObject
//#endif
//        {
//            //[HideInInspector]
//            [SerializeField]
//            //public List<KeyValuePair<string, object>> Properties = new List<KeyValuePair<string, object>>();
//            public Dictionary<string, object> Properties = new Dictionary<string, object>();
//        }
    }
}