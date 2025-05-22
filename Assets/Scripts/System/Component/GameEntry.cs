using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEntry : SingletonNetBehaviour<GameEntry>
{
    public static ChapterComponent ChapterComponent {  get; private set; }
    public static UserComponent UserComponent { get; private set; }
    public static EventComponent EventComponent { get; private set; }
    public static ObjectPoolComponent ObjectPoolComponent { get; private set; }
    public static ProcedureComponent ProcedureComponent { get; private set; }
    public static ResourceComponent ResourceComponent { get; private set; }
    public static SaveDataComponent SaveDataComponent { get; private set; }
    public static SettingComponent SettingComponent { get; private set; }
    public static UIComponent UIComponent { get; private set; }
    public static SceneComponent SceneComponent { get; private set; }

    private void Awake()
    {
        
    }
}
