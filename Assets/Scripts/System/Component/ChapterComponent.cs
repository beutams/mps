using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterComponent : BaseComponent
{
    public static List<ChapterData> allChapter = new List<ChapterData>();
    public ChapterData currentChapter { get; private set; }

    public ChapterData GetChapter()
    {
        if(currentChapter != null)
            return currentChapter;
        return null;
    }
}
public class ChapterData
{
    public string chapterScene;
    public string chapterName;
    public string chapterDescription;
    public int chapterId;
}
