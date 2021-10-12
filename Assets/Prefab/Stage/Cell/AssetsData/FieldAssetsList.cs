using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FieldAssetsList : ScriptableObject
{
    [SerializeField] public Material[] skyboxes;
    [SerializeField]public FieldAssetsData[] datas;
}