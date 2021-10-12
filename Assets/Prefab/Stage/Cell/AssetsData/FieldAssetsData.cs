using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FieldAssetsData : ScriptableObject
{
    [SerializeField, Header("���O")] public string assetsName;
    [SerializeField, Header("prefab���X�g ")]public GameObject[] prefabList;
}