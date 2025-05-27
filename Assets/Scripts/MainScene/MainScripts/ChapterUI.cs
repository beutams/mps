using System.Collections;
using System.Collections.Generic;
using Michsky.UI.Shift;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ChapterUI : MonoBehaviour
{
    protected ChapterData chapterData;
    public SingleSubUI parent;
    protected void Start()
    {
        ChapterButton button = GetComponent<ChapterButton>();
        button.onClick.AddListener(parent.OpenInfo);
    }
    public void OnStartClicked()
    {
        SceneManager.LoadSceneAsync("GameScene");
    }
    protected virtual void CloseDescription()
    {

    }
}
