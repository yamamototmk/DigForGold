using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class LevelData : ScriptableObject
{
    public enum CommandType { ObjectSpawn, Stop, CheckTrigger, DebugLog, GameClear,Event}
    public static string[] commandNames = { "�I�u�W�F�N�g����", "�C�x���g���~�߂�", "�g���K�[�̃`�F�b�N", "���O�\��", "�Q�[���N���A","�C�x���g���s"};
    [SerializeField] public string stageName;
    [SerializeField] public string stageDescription;
    [SerializeField] public int stageID;
    [SerializeField] public int stageType;
    [SerializeField] public List<GameObject> preInstantPrefabs;
    [SerializeField] public TextAsset map_csv;
    [SerializeField] public TextAsset obj_csv;
    /// <summary>
    /// �R�}���h�ꗗ
    /// </summary>
    public List<Command> commands;

    /// <summary>
    /// �R�}���h�f�[�^
    /// </summary>
    [System.Serializable]
    public class Command
    {

        [SerializeField]
        public CommandType commandType;
        [SerializeField]
        public CommandType prevCommandType;
        //object�^�ł��Ǝ��s���邽�т�null�ɂȂ�(�L_�T�M)
        //public object[] arguments;
        [SerializeField]
        public string[] arguments;
    }

}
