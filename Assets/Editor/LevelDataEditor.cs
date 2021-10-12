using UnityEngine;
using System;
using UnityEditor;

[CustomEditor(typeof(LevelData))]
public class MissionDataEditor : Editor
{

    //  List<StageObject.Command> commands;
    Color[] colors = new Color[] { Color.red, Color.black, Color.blue, Color.gray, Color.yellow, Color.white, Color.blue };
    /// <summary>
    /// ウィンドウ開いたときに回る？
    /// </summary>
    private void OnEnable()
    {
        LevelData levelData = (LevelData)target;

    }
    /// <summary>
    /// とりあえずここに色々書くらしい
    /// </summary>
    public override void OnInspectorGUI()
    {
        LevelData levelData = (LevelData)target;
        EditorGUILayout.LabelField("名前");
        levelData.stageName = EditorGUILayout.TextField(levelData.stageName);
        EditorGUILayout.LabelField("説明");
        levelData.stageDescription = EditorGUILayout.TextArea(levelData.stageDescription);
        EditorGUILayout.LabelField("ステージID");
        levelData.stageID = EditorGUILayout.IntField(levelData.stageID);
        EditorGUILayout.LabelField("ステージタイプ(アセット種別変更)");
        levelData.stageType = EditorGUILayout.IntField(levelData.stageType);
        EditorGUILayout.LabelField("マップデータ");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("preInstantPrefabs"), true);
        levelData.map_csv = EditorGUILayout.ObjectField("マップCSV", levelData.map_csv, typeof(TextAsset), true) as TextAsset;
        levelData.obj_csv = EditorGUILayout.ObjectField("オブジェクトCSV", levelData.obj_csv, typeof(TextAsset), true) as TextAsset;

        EditorGUILayout.LabelField("");
        //なんかテキスト表示
        EditorGUILayout.LabelField("ステージイベント");
        try
        {
            // using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
            if (levelData.commands.Count != 0 && levelData.commands != null)
            {

                for (int i = 0; i < levelData.commands.Count; i++)
                {
                    LevelData.Command curCommand = levelData.commands[i];
                    //ヘッダー
                    //GUI.color =
                    GUI.backgroundColor = colors[(int)levelData.commands[i].commandType];
                    GUI.contentColor = Color.white;
                    EditorGUILayout.HelpBox("【Event" + (i + 1) + "】" + levelData.commands[i].commandType, MessageType.None);
                    //コマンドタイプの選択ドロップダウン
                    curCommand.commandType = (LevelData.CommandType)EditorGUILayout.Popup("イベントの種類", (int)levelData.commands[i].commandType, LevelData.commandNames);

                    //コマンドタイプに変更があった時に引数をリセット
                    if (curCommand.commandType != curCommand.prevCommandType)
                    {
                        curCommand.arguments = null;
                        curCommand.prevCommandType = curCommand.commandType;
                        Debug.Log("コマンドタイプの変更、引数をリセット");
                    }




                    switch (curCommand.commandType)
                    {

                        case LevelData.CommandType.ObjectSpawn:

                            CreateArgumentArray(curCommand, 3);


                            curCommand.arguments[0] = EditorGUILayout.TextField("生成するprefab名",curCommand.arguments[0]);
                            //生成位置 0,1,2,3 上,下,左,右で対応
                            curCommand.arguments[1] = EditorGUILayout.Popup("生成する位置", int.Parse(curCommand.arguments[1]), new string[] { "0", "1", "2", "3" }).ToString();

                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("生成する数");
                            curCommand.arguments[2] = EditorGUILayout.IntField(int.Parse(curCommand.arguments[2])).ToString();
                            EditorGUILayout.EndHorizontal();
                            break;

                        case LevelData.CommandType.CheckTrigger:
                            CreateArgumentArray(curCommand, 2);

                            curCommand.arguments[0] = EditorGUILayout.Popup("トリガーの種類", int.Parse(curCommand.arguments[0]), Enum.GetNames(typeof(EventExecuter.CheckMode))).ToString();
                            EditorGUILayout.BeginHorizontal();
                            if (int.Parse(curCommand.arguments[0]) == 0)
                            {
                                EditorGUILayout.LabelField("止める時間");
                                curCommand.arguments[1] = EditorGUILayout.FloatField(float.Parse(curCommand.arguments[1])).ToString();
                            }
                            else if (int.Parse(curCommand.arguments[0]) == 1)
                            {
                                EditorGUILayout.LabelField("敵の残り数");
                                curCommand.arguments[1] = EditorGUILayout.IntField(int.Parse(curCommand.arguments[1])).ToString();
                            }

                            EditorGUILayout.EndHorizontal();
                            break;
                        case LevelData.CommandType.DebugLog:
                            CreateArgumentArray(curCommand, 1);
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("表示する文字");
                            curCommand.arguments[0] = EditorGUILayout.TextField(curCommand.arguments[0]);
                            EditorGUILayout.EndHorizontal();
                            break;
                        case LevelData.CommandType.GameClear:
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.EndHorizontal();
                            break;
                        case LevelData.CommandType.Stop:
                            break;
                        case LevelData.CommandType.Event:
                            CreateArgumentArray(curCommand, 2);
                            curCommand.arguments[0] = EditorGUILayout.Popup("イベントタイプ", int.Parse(curCommand.arguments[0]), Enum.GetNames(typeof(EventExecuter.EventType))).ToString();
                            EditorGUILayout.BeginHorizontal();
                            if (int.Parse(curCommand.arguments[0]) == 0)
                            {
                                EditorGUILayout.LabelField("空ける扉のID");
                                curCommand.arguments[1] = EditorGUILayout.IntField(int.Parse(curCommand.arguments[1])).ToString();
                            }
                            else if (int.Parse(curCommand.arguments[0]) == 1)
                            {
                                EditorGUILayout.LabelField("ダミーID");
                                curCommand.arguments[1] = EditorGUILayout.IntField(int.Parse(curCommand.arguments[1])).ToString();
                            }

                            EditorGUILayout.EndHorizontal();
                            break;


                    }
                    GUI.backgroundColor = Color.white;
                    GUI.contentColor = Color.white;
                    //イベント削除ボタン
                    if (GUILayout.Button("Delete"))
                    {
                        levelData.commands.RemoveAt(i);
                    }
                    // Debug.Log(stageObject.name + " コマンド数" + stageObject.commands.Count);
                    EditorGUILayout.LabelField("");


                }
            }
        }
        catch (Exception e) { }

        GUI.color = Color.white;
        EditorGUILayout.LabelField("************************************************************************************************");

        if (GUILayout.Button("Add"))
        {
            LevelData.Command command = new LevelData.Command();
            if (levelData.commands == null) levelData.commands = new System.Collections.Generic.List<LevelData.Command>();
            levelData.commands.Add(command);
            Debug.Log("イベント追加");
        }

        if (GUILayout.Button("Save"))
        {
            EditorUtility.SetDirty(levelData);
            Debug.Log("保存");
            //保存する
            AssetDatabase.SaveAssets();
        }
        EditorUtility.SetDirty(levelData);
        serializedObject.ApplyModifiedProperties();
    }

    void CreateArgumentArray(LevelData.Command command, int length)
    {
        if (command.arguments != null)
            return;
        Debug.Log("引数配列を生成");
        command.arguments = new string[length];
        for (int i = 0; i < length; i++)
        {
            command.arguments[i] = "0";
        }
    }

}
