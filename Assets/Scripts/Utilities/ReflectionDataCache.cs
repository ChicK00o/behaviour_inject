/*
    Author - Rohit Bhosle
*/

using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourInject
{
    public class ClassDataHolder
    {
        public PropertyInfo[] PropertyInfos;
        public bool[] PropertyAttributes;
        public FieldInfo[] FieldInfos;
        public bool[] FieldAttributes;
    }

    public static class ReflectionDataCache
    {
        private static Dictionary<Type, ClassDataHolder> _cachedValues = new Dictionary<Type, ClassDataHolder>();

        public static ClassDataHolder GetClassData(Type type)
        {
            if (!_cachedValues.ContainsKey(type))
                ReflectValues(type);
            return _cachedValues[type];
        }

        //public static PropertyInfo[] GetPropertyInfos(Type type)
        //{
        //    if (!_cachedValues.ContainsKey(type))
        //        ReflectValues(type);
        //    return _cachedValues[type].PropertyInfos;
        //}

        private static void ReflectValues(Type type)
        {
            ClassDataHolder data = new ClassDataHolder();
            data.PropertyInfos = type.GetProperties();
            data.PropertyAttributes = new bool[data.PropertyInfos.Length];
            for(int i = 0; i < data.PropertyInfos.Length; i++)
            {
                object[] attributes = data.PropertyInfos[i].GetCustomAttributes(typeof(InjectAttribute), true);
                data.PropertyAttributes[i] =  attributes.Length == 0;
            }
            data.FieldInfos = type.GetFields();
            data.FieldAttributes = new bool[data.FieldInfos.Length];
            for (int i = 0; i < data.FieldInfos.Length; i++)
            {
                object[] attributes = data.FieldInfos[i].GetCustomAttributes(typeof(InjectAttribute), true);
                data.FieldAttributes[i] = attributes.Length == 0;
            }
            _cachedValues.Add(type, data);
        }

        //public static FieldInfo[] GetFieldInfos(Type type)
        //{
        //    if (!_cachedValues.ContainsKey(type))
        //        ReflectValues(type);
        //    return _cachedValues[type].FieldInfos;
        //}

        public static void PreFetchReflectionData<T>(T type) where T : MonoBehaviour
        {
            Type componenType = type.GetType();
            if (!_cachedValues.ContainsKey(componenType))
                ReflectValues(componenType);
        }
    }
}
