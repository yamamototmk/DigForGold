using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleLevelManager : SingletonMonoBehaviour<BattleLevelManager>
{
    // Start is called before the first frame update
    [SerializeField] public int currentLevel { get; private set; }
    public List<LevelData> levelDatas;

    public enum StartType { GoUp, GetDown, Retry }
    public StartType startType;

    public Dictionary<int, DeadObjectList> deadObjectLists;

    public void Init()
    {
        startType = StartType.GoUp;
        deadObjectLists = new Dictionary<int, DeadObjectList>();
    }
    /// <summary>
    /// 指定したオブジェクトが存在するか
    /// </summary>
    /// <param name="levelNum">ステージ名</param>
    /// <param name="checkPos">オブジェクトの座標</param>
    /// <returns></returns>
    public bool ObjectExists(int levelNum, Vector3 checkPos)
    {
        DeadObjectList list;
        //リストが無ければ作成
        if (!deadObjectLists.TryGetValue(levelNum, out list))
        {
            deadObjectLists.Add(levelNum, new DeadObjectList());
            return false;
        }
        //リストをなめてオブジェクトが存在すればtrueを返す
        foreach (Vector3 pos in list.objectPositions)
        {
            if (pos.Equals(checkPos)) return true;
        }
        return false;
    }
    public void AddObj(Vector3 position)
    {
        Debug.Log("Add pos：" + position);
        deadObjectLists[currentLevel].objectPositions.Add(position);
    }

    public void SetLevel(int level)
    {
        currentLevel = level;
    }
    public string GetCurrentFloorName()
    {
        return levelDatas[currentLevel].stageName;
    }
}

public class DeadObjectList
{
    /// <summary>
    /// vector3にはcellX,cellYを入れる
    /// </summary>
    public List<Vector3> objectPositions = new List<Vector3>();
}
