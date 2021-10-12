using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class HomeManager : SingletonMonoBehaviour<HomeManager>
{
    [SerializeField] public int progress;
    [SerializeField] GameObject[] tutorialObjexts;
    private void Awake()
    {
        base.Awake();
        progress = DataManager.Instance.saveData.game_progress;
    }
    private void Start()
    {
        SetProgress(progress);
    }
    private void Update()
    {
        DataManager.Instance.saveData.game_progress = progress;
    }
    public void SetProgress(int progress)
    {
        this.progress = progress;
        DataManager.Instance.saveData.game_progress = progress;
        if (progress != 0 && tutorialObjexts[progress - 1] != null)
            tutorialObjexts[progress - 1].SetActive(false);
        if (tutorialObjexts[progress] != null)
            tutorialObjexts[progress].SetActive(true);
    }
    public void ProgressNext(int nextProgress)
    {
        if (Instance.progress == nextProgress - 1)
            Instance.SetProgress(nextProgress);
    }
}