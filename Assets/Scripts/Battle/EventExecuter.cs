using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventExecuter : MonoBehaviour
{
    // Start is called before the first frame update
    public enum CheckMode { TimeCount, EnemyCount }
    public enum EventType { OpenGate,Dammy}
    //�C�x���g���J�n����Time.time;
    float eventStartTime = -1;
    //�i�s�x
    [SerializeField]
    int progress;
    [SerializeField] LevelData levelData;

    LevelData.Command curCommand;
    /// <summary>
    /// 0�`1�̊����Ŏ���
    /// </summary>
    [SerializeField] public float timeProgress;
    void Start()
    {
        levelData = BattleLevelManager.Instance.levelDatas[BattleLevelManager.Instance.currentLevel];
        progress = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //�C�x���g���I����Ă��Ȃ���΃��[�v
        if (progress < levelData.commands.Count)
        {
            MainLoop();
        }

    }
    void MainLoop()
    {
        curCommand = levelData.commands[progress];
        switch (curCommand.commandType)
        {
            case LevelData.CommandType.CheckTrigger:

                switch ((CheckMode)int.Parse(curCommand.arguments[0]))
                {
                    case CheckMode.EnemyCount:

                        if (EnemyManager.Instance.GetEnemyCount() <= int.Parse(curCommand.arguments[1]))
                        {
                            progress++;
                        }

                        break;
                    case CheckMode.TimeCount:
                        StartCoroutine(TimeTriggerCount(curCommand.arguments));
                        break;
                }
                break;
            case LevelData.CommandType.Stop:
                break;
            case LevelData.CommandType.ObjectSpawn:
                //����[0]��������I�u�W�F�N�g[1]��������ʒu(0123)[2]�������鐔
                for (int i = 0; i < int.Parse(curCommand.arguments[2]); i++)
                {
                    GameObject obj = ObjectPoolManager.Instance.GetObject(curCommand.arguments[0]);
                    obj.transform.position = BattleSceneManager.Instance.spawnPoints[int.Parse(curCommand.arguments[1])].position;
                }
                progress++;
                break;
            case LevelData.CommandType.DebugLog:

                Debug.Log(curCommand.arguments[0]);
                //infoMationLog.AddInfomationLog("[" + PlaySceneManager.Instance.GetPlayTimeString() + "]" + curCommand.arguments[0]);
                progress++;
                break;
            case LevelData.CommandType.GameClear:
                BattleSceneManager.Instance.StageClear();
                progress++;
                break;
            case LevelData.CommandType.Event:
                {
                    if (int.Parse(curCommand.arguments[0]) == 0)
                    {
                        string id = "Gate" + curCommand.arguments[1];
                        GameObject.FindWithTag(id).GetComponent<Gate>().Open();
                    }
                }
                break;

        }
    }
    private void Stop()
    {

    }

    bool end = false;
    IEnumerator TimeTriggerCount(string[] arg)
    {
        if (eventStartTime == -1)
            eventStartTime = Time.time;

        end = false;

        float percentage = Time.time /( eventStartTime + float.Parse(arg[1]));
        timeProgress = percentage;
        if (percentage >= 1)
        {
            end = true;
        }

        if (end)
        {
            progress++;
            eventStartTime = -1;
            yield return null;
        }
    }
    public void TimeCountEnd()
    {
        end = true;
    }
}