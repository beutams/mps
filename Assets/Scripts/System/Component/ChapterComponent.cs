using System.Collections;
using System.Collections.Generic;
using Michsky.UI.Shift;
using Unity.VisualScripting;
using UnityEngine;

public class ChapterComponent : BaseComponent
{
    public static Dictionary<int, ChapterData> allChapter = new Dictionary<int, ChapterData>();
    public ChapterData currentChapter { get; private set; }
    protected ChapterButton button;
    protected bool isLock => button.statusItem == ChapterButton.StatusItem.Locked;
    protected bool isComplete => button.statusItem == ChapterButton.StatusItem.Completed;

    private void Awake()
    {
        currentChapter = null;
    }
    public ChapterData GetChapter()
    {
        if(currentChapter != null)
            return currentChapter;
        return null;
    }
    public void SetChapter(int chapterId)
    {
        if (chapterId == -1)
        {
            currentChapter = null;
            GameEntry.EventComponent.Notify(GameEvent.ChapterCancelEvent, null);
        }
        else
        {
            currentChapter = allChapter[chapterId];
            GameEntry.EventComponent.Notify(GameEvent.ChapterSelectEvent, currentChapter);
        }
    }
}
public class ChapterData
{
    public string chapterScene;
    public string chapterName;
    public string chapterDescription;
    public string chapterImage;
    public int chapterId;
}
