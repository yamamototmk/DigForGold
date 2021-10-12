using UnityEngine;
using System;
using UnityEditor;

[CustomEditor(typeof(LevelData))]
public class MissionDataEditor : Editor
{

    //  List<StageObject.Command> commands;
    Color[] colors = new Color[] { Color.red, Color.black, Color.blue, Color.gray, Color.yellow, Color.white, Color.blue };
    /// <summary>
    /// �E�B���h�E�J�����Ƃ��ɉ��H
    /// </summary>
    private void OnEnable()
    {
        LevelData levelData = (LevelData)target;

    }
    /// <summary>
    /// �Ƃ肠���������ɐF�X�����炵��
    /// </summary>
    public override void OnInspectorGUI()
    {
        LevelData levelData = (LevelData)target;
        EditorGUILayout.LabelField("���O");
        levelData.stageName = EditorGUILayout.TextField(levelData.stageName);
        EditorGUILayout.LabelField("����");
        levelData.stageDescription = EditorGUILayout.TextArea(levelData.stageDescription);
        EditorGUILayout.LabelField("�X�e�[�WID");
        levelData.stageID = EditorGUILayout.IntField(levelData.stageID);
        EditorGUILayout.LabelField("�X�e�[�W�^�C�v(�A�Z�b�g��ʕύX)");
        levelData.stageType = EditorGUILayout.IntField(levelData.stageType);
        EditorGUILayout.LabelField("�}�b�v�f�[�^");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("preInstantPrefabs"), true);
        levelData.map_csv = EditorGUILayout.ObjectField("�}�b�vCSV", levelData.map_csv, typeof(TextAsset), true) as TextAsset;
        levelData.obj_csv = EditorGUILayout.ObjectField("�I�u�W�F�N�gCSV", levelData.obj_csv, typeof(TextAsset), true) as TextAsset;

        EditorGUILayout.LabelField("");
        //�Ȃ񂩃e�L�X�g�\��
        EditorGUILayout.LabelField("�X�e�[�W�C�x���g");
        try
        {
            // using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
            if (levelData.commands.Count != 0 && levelData.commands != null)
            {

                for (int i = 0; i < levelData.commands.Count; i++)
                {
                    LevelData.Command curCommand = levelData.commands[i];
                    //�w�b�_�[
                    //GUI.color =
                    GUI.backgroundColor = colors[(int)levelData.commands[i].commandType];
                    GUI.contentColor = Color.white;
                    EditorGUILayout.HelpBox("�yEvent" + (i + 1) + "�z" + levelData.commands[i].commandType, MessageType.None);
                    //�R�}���h�^�C�v�̑I���h���b�v�_�E��
                    curCommand.commandType = (LevelData.CommandType)EditorGUILayout.Popup("�C�x���g�̎��", (int)levelData.commands[i].commandType, LevelData.commandNames);

                    //�R�}���h�^�C�v�ɕύX�����������Ɉ��������Z�b�g
                    if (curCommand.commandType != curCommand.prevCommandType)
                    {
                        curCommand.arguments = null;
                        curCommand.prevCommandType = curCommand.commandType;
                        Debug.Log("�R�}���h�^�C�v�̕ύX�A���������Z�b�g");
                    }




                    switch (curCommand.commandType)
                    {

                        case LevelData.CommandType.ObjectSpawn:

                            CreateArgumentArray(curCommand, 3);


                            curCommand.arguments[0] = EditorGUILayout.TextField("��������prefab��",curCommand.arguments[0]);
                            //�����ʒu 0,1,2,3 ��,��,��,�E�őΉ�
                            curCommand.arguments[1] = EditorGUILayout.Popup("��������ʒu", int.Parse(curCommand.arguments[1]), new string[] { "0", "1", "2", "3" }).ToString();

                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("�������鐔");
                            curCommand.arguments[2] = EditorGUILayout.IntField(int.Parse(curCommand.arguments[2])).ToString();
                            EditorGUILayout.EndHorizontal();
                            break;

                        case LevelData.CommandType.CheckTrigger:
                            CreateArgumentArray(curCommand, 2);

                            curCommand.arguments[0] = EditorGUILayout.Popup("�g���K�[�̎��", int.Parse(curCommand.arguments[0]), Enum.GetNames(typeof(EventExecuter.CheckMode))).ToString();
                            EditorGUILayout.BeginHorizontal();
                            if (int.Parse(curCommand.arguments[0]) == 0)
                            {
                                EditorGUILayout.LabelField("�~�߂鎞��");
                                curCommand.arguments[1] = EditorGUILayout.FloatField(float.Parse(curCommand.arguments[1])).ToString();
                            }
                            else if (int.Parse(curCommand.arguments[0]) == 1)
                            {
                                EditorGUILayout.LabelField("�G�̎c�萔");
                                curCommand.arguments[1] = EditorGUILayout.IntField(int.Parse(curCommand.arguments[1])).ToString();
                            }

                            EditorGUILayout.EndHorizontal();
                            break;
                        case LevelData.CommandType.DebugLog:
                            CreateArgumentArray(curCommand, 1);
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("�\�����镶��");
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
                            curCommand.arguments[0] = EditorGUILayout.Popup("�C�x���g�^�C�v", int.Parse(curCommand.arguments[0]), Enum.GetNames(typeof(EventExecuter.EventType))).ToString();
                            EditorGUILayout.BeginHorizontal();
                            if (int.Parse(curCommand.arguments[0]) == 0)
                            {
                                EditorGUILayout.LabelField("�󂯂����ID");
                                curCommand.arguments[1] = EditorGUILayout.IntField(int.Parse(curCommand.arguments[1])).ToString();
                            }
                            else if (int.Parse(curCommand.arguments[0]) == 1)
                            {
                                EditorGUILayout.LabelField("�_�~�[ID");
                                curCommand.arguments[1] = EditorGUILayout.IntField(int.Parse(curCommand.arguments[1])).ToString();
                            }

                            EditorGUILayout.EndHorizontal();
                            break;


                    }
                    GUI.backgroundColor = Color.white;
                    GUI.contentColor = Color.white;
                    //�C�x���g�폜�{�^��
                    if (GUILayout.Button("Delete"))
                    {
                        levelData.commands.RemoveAt(i);
                    }
                    // Debug.Log(stageObject.name + " �R�}���h��" + stageObject.commands.Count);
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
            Debug.Log("�C�x���g�ǉ�");
        }

        if (GUILayout.Button("Save"))
        {
            EditorUtility.SetDirty(levelData);
            Debug.Log("�ۑ�");
            //�ۑ�����
            AssetDatabase.SaveAssets();
        }
        EditorUtility.SetDirty(levelData);
        serializedObject.ApplyModifiedProperties();
    }

    void CreateArgumentArray(LevelData.Command command, int length)
    {
        if (command.arguments != null)
            return;
        Debug.Log("�����z��𐶐�");
        command.arguments = new string[length];
        for (int i = 0; i < length; i++)
        {
            command.arguments[i] = "0";
        }
    }

}
