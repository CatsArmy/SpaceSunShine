using System;
using System.Reflection;
using UnityEngine;



namespace SpaceSunShine.Pun.Utills
{
    public static class CopyComponents
    {
        public static Component CopyComponent(this Component original, GameObject destination)
        {
            Type type = original.GetType();
            Component component = destination.AddComponent(type);
            FieldInfo[] fields = type.GetFields();
            foreach (FieldInfo fieldInfo in fields)
            {
                fieldInfo.SetValue(component, fieldInfo.GetValue(original));
            }

            return component;
        }
        public static Component CopyComponent(this GameObject destination, Component original)
        {
            Type type = original.GetType();
            Component component = destination.AddComponent(type);
            FieldInfo[] fields = type.GetFields();
            foreach (FieldInfo fieldInfo in fields)
            {
                fieldInfo.SetValue(component, fieldInfo.GetValue(original));
            }

            return component;
        }
    }

}