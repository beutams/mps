using System.Collections;
using System.Collections.Generic;
using Michsky.UI.Shift;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SingleSubUI : SubUIBase<SingleSubUI>
{
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
    public virtual void OnEnterClick()
    {
        IRoomController room = Instantiate(GameEntry.ResourceComponent.prefabDic["OfflineRoomController"][0]).GetComponent<IRoomController>();
        room.armoryData = ArmorySubUI.data == null ? new ArmoryData() : ArmorySubUI.data;
        SceneManager.sceneLoaded += OfflineRoomController.instance.OnSceneLoaded;
        SceneManager.LoadScene("GameScene");
    }
}
