using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// 系统测试工具类
/// 用于测试游戏中的各个核心系统功能
/// </summary>
public class SystemTests : EditorWindow
{
    private Vector2 scrollPosition;
    private bool showUITests = true;
    private bool showConstructionTests = true;
    private bool showGameObjectTests = true;
    
    [MenuItem("Tools/System Tests")]
    public static void ShowWindow()
    {
        GetWindow<SystemTests>("系统测试工具");
    }
    
    private void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        
        GUILayout.Label("MPS 系统测试工具", EditorStyles.boldLabel);
        GUILayout.Space(10);
        
        // UI 系统测试
        showUITests = EditorGUILayout.Foldout(showUITests, "UI 系统测试");
        if (showUITests)
        {
            EditorGUI.indentLevel++;
            
            if (GUILayout.Button("测试 SelectBuildingUI"))
            {
                TestSelectBuildingUI();
            }
            
            if (GUILayout.Button("测试 UIBase 基础功能"))
            {
                TestUIBase();
            }
            
            EditorGUI.indentLevel--;
        }
        
        GUILayout.Space(10);
        
        // 建筑系统测试
        showConstructionTests = EditorGUILayout.Foldout(showConstructionTests, "建筑系统测试");
        if (showConstructionTests)
        {
            EditorGUI.indentLevel++;
            
            if (GUILayout.Button("测试 SpawnBuild 功能"))
            {
                TestSpawnBuild();
            }
            
            if (GUILayout.Button("测试 SpawnBuildMono 交互"))
            {
                TestSpawnBuildMono();
            }
            
            EditorGUI.indentLevel--;
        }
        
        GUILayout.Space(10);
        
        // GameObjectController 测试
        showGameObjectTests = EditorGUILayout.Foldout(showGameObjectTests, "游戏对象系统测试");
        if (showGameObjectTests)
        {
            EditorGUI.indentLevel++;
            
            if (GUILayout.Button("测试 GameObjectController 初始化"))
            {
                TestGameObjectController();
            }
            
            if (GUILayout.Button("测试能力系统"))
            {
                TestAbilitySystem();
            }
            
            EditorGUI.indentLevel--;
        }
        
        GUILayout.Space(20);
        
        if (GUILayout.Button("运行所有测试", GUILayout.Height(30)))
        {
            RunAllTests();
        }
        
        EditorGUILayout.EndScrollView();
    }
    
    private void TestSelectBuildingUI()
    {
        Debug.Log("=== 测试 SelectBuildingUI ===");
        
        // 检查是否存在 SelectBuildingUI prefab
        string[] prefabPaths = AssetDatabase.FindAssets("SelectBuildingUI t:Prefab");
        if (prefabPaths.Length == 0)
        {
            Debug.LogWarning("未找到 SelectBuildingUI prefab");
            return;
        }
        
        Debug.Log($"找到 {prefabPaths.Length} 个 SelectBuildingUI 相关资源");
        
        // 测试脚本是否可以编译
        try
        {
            var script = typeof(SelectBuildingUI);
            Debug.Log("✓ SelectBuildingUI 脚本编译正常");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"✗ SelectBuildingUI 脚本编译错误: {e.Message}");
        }
    }
    
    private void TestUIBase()
    {
        Debug.Log("=== 测试 UIBase 基础功能 ===");
        
        try
        {
            // 测试 UIBase 类是否存在
            var uiBaseType = typeof(UIBase);
            Debug.Log("✓ UIBase 类存在");
            
            // 检查必要的方法
            var onOpenMethod = uiBaseType.GetMethod("OnOpen");
            var onCloseMethod = uiBaseType.GetMethod("OnClose");
            var closeMethod = uiBaseType.GetMethod("Close");
            
            if (onOpenMethod != null) Debug.Log("✓ OnOpen 方法存在");
            else Debug.LogWarning("✗ OnOpen 方法不存在");
            
            if (onCloseMethod != null) Debug.Log("✓ OnClose 方法存在");
            else Debug.LogWarning("✗ OnClose 方法不存在");
            
            if (closeMethod != null) Debug.Log("✓ Close 方法存在");
            else Debug.LogWarning("✗ Close 方法不存在");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"✗ UIBase 测试失败: {e.Message}");
        }
    }
    
    private void TestSpawnBuild()
    {
        Debug.Log("=== 测试 SpawnBuild 功能 ===");
        
        try
        {
            // 检查 SpawnBuild ScriptableObject
            string[] spawnBuildAssets = AssetDatabase.FindAssets("t:SpawnBuild");
            Debug.Log($"找到 {spawnBuildAssets.Length} 个 SpawnBuild 资源");
            
            if (spawnBuildAssets.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(spawnBuildAssets[0]);
                SpawnBuild spawnBuild = AssetDatabase.LoadAssetAtPath<SpawnBuild>(path);
                
                if (spawnBuild != null)
                {
                    Debug.Log("✓ SpawnBuild 资源加载成功");
                    Debug.Log($"资源路径: {path}");
                }
                else
                {
                    Debug.LogWarning("✗ SpawnBuild 资源加载失败");
                }
            }
            else
            {
                Debug.LogWarning("未找到 SpawnBuild 资源，请确保已创建相应的 ScriptableObject");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"✗ SpawnBuild 测试失败: {e.Message}");
        }
    }
    
    private void TestSpawnBuildMono()
    {
        Debug.Log("=== 测试 SpawnBuildMono 交互 ===");
        
        try
        {
            var spawnBuildMonoType = typeof(SpawnBuildMono);
            Debug.Log("✓ SpawnBuildMono 脚本存在");
            
            // 检查必要的接口实现
            var interfaces = spawnBuildMonoType.GetInterfaces();
            bool hasPointerClick = false;
            
            foreach (var interfaceType in interfaces)
            {
                if (interfaceType.Name == "IPointerClickHandler")
                {
                    hasPointerClick = true;
                    break;
                }
            }
            
            if (hasPointerClick)
                Debug.Log("✓ 实现了 IPointerClickHandler 接口");
            else
                Debug.LogWarning("✗ 未实现 IPointerClickHandler 接口");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"✗ SpawnBuildMono 测试失败: {e.Message}");
        }
    }
    
    private void TestGameObjectController()
    {
        Debug.Log("=== 测试 GameObjectController 初始化 ===");
        
        try
        {
            var gameObjectControllerType = typeof(GameObjectController);
            Debug.Log("✓ GameObjectController 类存在");
            
            // 检查抽象类的关键方法
            var initAbilityMethod = gameObjectControllerType.GetMethod("InitAbility");
            var initStatsMethod = gameObjectControllerType.GetMethod("InitStats", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            if (initAbilityMethod != null)
                Debug.Log("✓ InitAbility 方法存在");
            else
                Debug.LogWarning("✗ InitAbility 方法不存在");
                
            if (initStatsMethod != null)
                Debug.Log("✓ InitStats 方法存在");
            else
                Debug.LogWarning("✗ InitStats 方法不存在");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"✗ GameObjectController 测试失败: {e.Message}");
        }
    }
    
    private void TestAbilitySystem()
    {
        Debug.Log("=== 测试能力系统 ===");
        
        try
        {
            // 检查 Ability 基类
            var abilityType = typeof(Ability);
            Debug.Log("✓ Ability 基类存在");
            
            // 查找所有能力类
            string[] abilityScripts = AssetDatabase.FindAssets("t:MonoScript Ability");
            List<string> abilityClasses = new List<string>();
            
            foreach (string guid in abilityScripts)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                string fileName = System.IO.Path.GetFileNameWithoutExtension(path);
                if (fileName.Contains("Ability") && fileName != "Ability")
                {
                    abilityClasses.Add(fileName);
                }
            }
            
            Debug.Log($"找到 {abilityClasses.Count} 个能力类: {string.Join(", ", abilityClasses)}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"✗ 能力系统测试失败: {e.Message}");
        }
    }
    
    private void RunAllTests()
    {
        Debug.Log("========== 开始运行所有测试 ==========");
        
        TestUIBase();
        TestSelectBuildingUI();
        TestSpawnBuild();
        TestSpawnBuildMono();
        TestGameObjectController();
        TestAbilitySystem();
        
        Debug.Log("========== 所有测试完成 ==========");
    }
}
