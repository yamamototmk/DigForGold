using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FieldAssetsData : ScriptableObject
{
    [SerializeField, Header("名前")] public string assetsName;
    [SerializeField, Header("prefabリスト ")]public GameObject[] prefabList;
}