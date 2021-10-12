using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : SingletonMonoBehaviour<EnemyManager>
{
    [SerializeField] List<Character> enemies = new List<Character>();
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        //null又は非アクティブなオブジェクトをリストから削除
        for(int i = 0; i < enemies.Count;i++)
        {
            if (enemies[i] == null) enemies.RemoveAt(i);
            else if (!enemies[i].gameObject.activeSelf) RemoveEnemyList(enemies[i]);
        }
    }
    public void AddEnemyList(Character e)
    {
        enemies.Add(e);
    }
    public void RemoveEnemyList(Character e)
    {
        enemies.Remove(e);
    }
    public int GetEnemyCount()
    {
        return enemies.Count;
    }
}
