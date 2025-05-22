using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChapterUI : MonoBehaviour
{
    public ChapterData chapterData;
    public GameObject chapterInfo;

    public HallManager hallManager;
    private void Awake()
    {
        transform.parent.Find("ChapterInfo");

    }
    public void OnChapterClicked()
    {
        chapterInfo.SetActive(true);
    }
    public void OnStartClicked()
    {
        SceneManager.LoadSceneAsync("GameScene");
    }
}
