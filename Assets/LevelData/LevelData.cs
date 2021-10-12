using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class LevelData : ScriptableObject
{
    public enum CommandType { ObjectSpawn, Stop, CheckTrigger, DebugLog, GameClear,Event}
    public static string[] commandNames = { "オブジェクト生成", "イベントを止める", "トリガーのチェック", "ログ表示", "ゲームクリア","イベント実行"};
    [SerializeField] public string stageName;
    [SerializeField] public string stageDescription;
    [SerializeField] public int stageID;
    [SerializeField] public int stageType;
    [SerializeField] public List<GameObject> preInstantPrefabs;
    [SerializeField] public TextAsset map_csv;
    [SerializeField] public TextAsset obj_csv;
    /// <summary>
    /// コマンド一覧
    /// </summary>
    public List<Command> commands;

    /// <summary>
    /// コマンドデータ
    /// </summary>
    [System.Serializable]
    public class Command
    {

        [SerializeField]
        public CommandType commandType;
        [SerializeField]
        public CommandType prevCommandType;
        //object型でやると実行するたびにnullになる(´_ゝ｀)
        //public object[] arguments;
        [SerializeField]
        public string[] arguments;
    }

}
