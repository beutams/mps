using System.Collections;
using System.Collections.Generic;
using Michsky.UI.Shift;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SingleSubUI : SubUIBase
{
    public GameObject roomConntroller;
    public GameObject chapterInfo;
    public Button enter;
    public Button exit;
    private void Start()
    {
        enter.onClick.AddListener(OnEnterClick);
        exit.onClick.AddListener(OnExitClick);
    }
    public void SelectChapter(int index)
    {
        GameEntry.ChapterComponent.SetChapter(index);
    }
    public void CancelChapter()
    {
        GameEntry.ChapterComponent.SetChapter(-1);
    }

    protected override void OnOpen()
    {
        
    }

    protected override void OnClose()
    {
        chapterInfo.SetActive(false);
    }
    public virtual void CloseInfo()
    {

    }
    public virtual void OpenInfo()
    {
        chapterInfo.SetActive(true);
        chapterInfo.GetComponentInChildren<SpotlightButton>().PlayAnimation();
    }
    protected virtual void OnExitClick()
    {
        OnClose();
    }
    protected virtual void OnEnterClick()
    {
        Instantiate(roomConntroller);
        SceneManager.sceneLoaded += RoomController.instance.OnSceneLoadedSingle;
        SceneManager.LoadScene("GameScene");
    }
}
