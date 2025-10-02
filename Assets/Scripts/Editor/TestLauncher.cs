using UnityEngine;
using UnityEditor;

/// <summary>
/// 测试启动器 - 提供快速访问所有测试功能的统一入口
/// </summary>
public class TestLauncher : EditorWindow
{
    [MenuItem("Tools/Test Launcher")]
    public static void ShowWindow()
    {
        GetWindow<TestLauncher>("测试启动器");
    }
    
    private void OnGUI()
    {
        GUILayout.Label("MPS 测试启动器", EditorStyles.boldLabel);
        GUILayout.Space(10);
        
        GUILayout.Label("快速测试选项:", EditorStyles.label);
        
        // 系统测试
        if (GUILayout.Button("打开系统测试窗口", GUILayout.Height(30)))
        {
            SystemTests.ShowWindow();
        }
        
        GUILayout.Space(5);
        
        // 单元测试
        if (GUILayout.Button("运行所有单元测试", GUILayout.Height(30)))
        {
            UnitTests.RunAllUnitTests();
        }
        
        GUILayout.Space(5);
        
        // 个别系统测试
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("UI系统"))
        {
            UnitTests.TestUISystem();
        }
        if (GUILayout.Button("建筑系统"))
        {
            UnitTests.TestConstructionSystem();
        }
        if (GUILayout.Button("游戏对象"))
        {
            UnitTests.TestGameObjectSystem();
        }
        EditorGUILayout.EndHorizontal();
        
        GUILayout.Space(10);
        
        GUILayout.Label("运行时测试:", EditorStyles.label);
        EditorGUILayout.HelpBox("在游戏运行时，将 RuntimeTestManager 组件添加到场景中的任意游戏对象上，然后按 F9 键打开测试面板。", MessageType.Info);
        
        if (GUILayout.Button("查找运行时测试管理器"))
        {
            var rtm = FindObjectOfType<RuntimeTestManager>();
            if (rtm != null)
            {
                Selection.activeGameObject = rtm.gameObject;
                EditorGUIUtility.PingObject(rtm.gameObject);
                Debug.Log("找到运行时测试管理器: " + rtm.gameObject.name);
            }
            else
            {
                Debug.LogWarning("场景中未找到 RuntimeTestManager 组件");
            }
        }
        
        GUILayout.Space(10);
        
        GUILayout.Label("测试说明:", EditorStyles.label);
        EditorGUILayout.HelpBox(
            "1. 系统测试 - 在编辑器中测试各系统的基本功能\n" +
            "2. 单元测试 - 详细的代码级别测试\n" +
            "3. 运行时测试 - 在游戏运行时测试实际功能\n\n" +
            "建议按顺序进行测试，先进行单元测试确保代码正确性，再进行系统测试验证功能集成，最后进行运行时测试确保实际运行效果。",
            MessageType.Info
        );
        
        GUILayout.Space(10);
        
        if (GUILayout.Button("清空控制台", GUILayout.Height(25)))
        {
            var logEntries = System.Type.GetType("UnityEditor.LogEntries,UnityEditor.dll");
            var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            clearMethod.Invoke(null, null);
        }
    }
}

