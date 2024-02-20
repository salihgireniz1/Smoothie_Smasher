using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class ResetSOData
{
#if UNITY_EDITOR
    [MenuItem("Tools/Reset SO Data")]
    public static void ResetSO()
    {
        List<FruitUpgradeObject> fruits = GetSO<FruitUpgradeObject>();
        for (int i = 0; i < fruits.Count; i++)
        {
            fruits[i].ResetDatas();
        }
    }

    public static List<T> GetSO<T>() where T : ScriptableObject
    {
        string[] guids = AssetDatabase.FindAssets("t:" + typeof(T));
        List<T> scriptableObjects = new List<T>();
        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            T so = AssetDatabase.LoadAssetAtPath(assetPath, typeof(T)) as T;
            scriptableObjects.Add(so);
        }
        return scriptableObjects;
    }
#endif
}
