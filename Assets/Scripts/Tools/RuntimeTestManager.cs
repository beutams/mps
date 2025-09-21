using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 运行时测试管理器
/// 用于在游戏运行时测试各种系统功能
/// </summary>
public class RuntimeTestManager : MonoBehaviour
{
    [Header("测试设置")]
    [SerializeField] private bool enableTests = true;
    [SerializeField] private bool showTestUI = true;
    [SerializeField] private KeyCode testHotkey = KeyCode.F9;
    
    [Header("测试目标")]
    [SerializeField] private GameObject testGameObject;
    [SerializeField] private SpawnBuild testSpawnBuild;
    
    private bool testUIVisible = false;
    private Vector2 scrollPosition;
    
    private void Update()
    {
        if (enableTests && Input.GetKeyDown(testHotkey))
        {
            testUIVisible = !testUIVisible;
        }
    }
    
    private void OnGUI()
    {
        if (!enableTests || !showTestUI || !testUIVisible) return;
        
        // 创建测试窗口
        GUILayout.BeginArea(new Rect(10, 10, 300, 400), GUI.skin.box);
        GUILayout.Label("运行时测试面板", GUI.skin.label);
        GUILayout.Label($"按 {testHotkey} 键切换显示", GUI.skin.label);
        GUILayout.Space(10);
        
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(300));
        
        // UI系统测试
        if (GUILayout.Button("测试 SelectBuildingUI"))
        {
            TestSelectBuildingUIAtRuntime();
        }
        
        // 建筑系统测试
        if (GUILayout.Button("测试建筑生成"))
        {
            TestSpawnBuildAtRuntime();
        }
        
        // GameEntry 系统测试
        if (GUILayout.Button("测试 GameEntry 系统"))
        {
            TestGameEntrySystem();
        }
        
        // 对象池测试
        if (GUILayout.Button("测试对象池"))
        {
            TestObjectPool();
        }
        
        // 事件系统测试
        if (GUILayout.Button("测试事件系统"))
        {
            TestEventSystem();
        }
        
        // 网络测试
        if (GUILayout.Button("测试网络状态"))
        {
            TestNetworkStatus();
        }
        
        GUILayout.Space(10);
        
        if (GUILayout.Button("运行所有测试"))
        {
            StartCoroutine(RunAllRuntimeTests());
        }
        
        if (GUILayout.Button("清空日志"))
        {
            Debug.ClearDeveloperConsole();
        }
        
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }
    
    private void TestSelectBuildingUIAtRuntime()
    {
        Debug.Log("=== 运行时测试 SelectBuildingUI ===");
        
        try
        {
            if (GameEntry.UIComponent != null)
            {
                GameEntry.UIComponent.ShowUI("SelectBuildingUI");
                Debug.Log("✓ 成功调用 ShowUI(\"SelectBuildingUI\")");
            }
            else
            {
                Debug.LogWarning("✗ GameEntry.UIComponent 为空");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"✗ SelectBuildingUI 测试失败: {e.Message}");
        }
    }
    
    private void TestSpawnBuildAtRuntime()
    {
        Debug.Log("=== 运行时测试建筑生成 ===");
        
        try
        {
            if (testSpawnBuild != null && testGameObject != null)
            {
                // 模拟SpawnBuild的Init方法
                var controller = testGameObject.GetComponent<GameObjectController>();
                if (controller != null)
                {
                    testSpawnBuild.Init(controller);
                    Debug.Log("✓ SpawnBuild.Init() 调用成功");
                    
                    // 检查是否添加了SpawnBuildMono组件
                    var spawnBuildMono = testGameObject.GetComponent<SpawnBuildMono>();
                    if (spawnBuildMono != null)
                    {
                        Debug.Log("✓ SpawnBuildMono 组件添加成功");
                    }
                    else
                    {
                        Debug.LogWarning("✗ SpawnBuildMono 组件添加失败");
                    }
                }
                else
                {
                    Debug.LogWarning("✗ 测试对象没有 GameObjectController 组件");
                }
            }
            else
            {
                Debug.LogWarning("✗ 测试资源未设置（testSpawnBuild 或 testGameObject）");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"✗ 建筑生成测试失败: {e.Message}");
        }
    }
    
    private void TestGameEntrySystem()
    {
        Debug.Log("=== 运行时测试 GameEntry 系统 ===");
        
        try
        {
            // 测试各个组件是否存在
            bool uiComponent = GameEntry.UIComponent != null;
            bool eventComponent = GameEntry.EventComponent != null;
            bool objectPoolComponent = GameEntry.ObjectPoolComponent != null;
            
            Debug.Log($"UIComponent: {(uiComponent ? "✓" : "✗")}");
            Debug.Log($"EventComponent: {(eventComponent ? "✓" : "✗")}");
            Debug.Log($"ObjectPoolComponent: {(objectPoolComponent ? "✓" : "✗")}");
            
            if (uiComponent && eventComponent && objectPoolComponent)
            {
                Debug.Log("✓ GameEntry 系统基本正常");
            }
            else
            {
                Debug.LogWarning("✗ GameEntry 系统存在问题");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"✗ GameEntry 系统测试失败: {e.Message}");
        }
    }
    
    private void TestObjectPool()
    {
        Debug.Log("=== 运行时测试对象池 ===");
        
        try
        {
            if (GameEntry.ObjectPoolComponent != null)
            {
                // 这里可以测试对象池的基本功能
                // 由于没有具体的对象池实现，先测试组件存在性
                Debug.Log("✓ 对象池组件存在");
                
                // 可以添加更多具体的对象池测试
                // 例如: 获取对象、释放对象等
            }
            else
            {
                Debug.LogWarning("✗ 对象池组件不存在");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"✗ 对象池测试失败: {e.Message}");
        }
    }
    
    private void TestEventSystem()
    {
        Debug.Log("=== 运行时测试事件系统 ===");
        
        try
        {
            if (GameEntry.EventComponent != null)
            {
                // 测试事件订阅和取消订阅
                System.Action<object> testHandler = (data) => {
                    Debug.Log($"测试事件处理器收到数据: {data}");
                };
                
                // 这里需要根据实际的事件枚举进行测试
                // GameEntry.EventComponent.Subscribe(GameEvent.TestEvent, testHandler);
                // GameEntry.EventComponent.Desubscribe(GameEvent.TestEvent, testHandler);
                
                Debug.Log("✓ 事件系统组件存在");
            }
            else
            {
                Debug.LogWarning("✗ 事件系统组件不存在");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"✗ 事件系统测试失败: {e.Message}");
        }
    }
    
    private void TestNetworkStatus()
    {
        Debug.Log("=== 运行时测试网络状态 ===");
        
        try
        {
            // 检查 RoomController
            if (RoomController.instance != null)
            {
                Debug.Log("✓ RoomController 实例存在");
                Debug.Log($"游戏准备状态: {RoomController.instance.gameReady}");
                
                if (RoomController.instance.playerDic != null)
                {
                    Debug.Log($"玩家字典大小: {RoomController.instance.playerDic.Count}");
                }
            }
            else
            {
                Debug.LogWarning("✗ RoomController 实例不存在");
            }
            
            // 检查 Mirror 网络状态
            if (Mirror.NetworkManager.singleton != null)
            {
                var networkManager = Mirror.NetworkManager.singleton;
                Debug.Log($"网络管理器模式: {networkManager.mode}");
                Debug.Log($"是否为服务器: {Mirror.NetworkServer.active}");
                Debug.Log($"是否为客户端: {Mirror.NetworkClient.active}");
            }
            else
            {
                Debug.LogWarning("✗ Mirror NetworkManager 不存在");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"✗ 网络状态测试失败: {e.Message}");
        }
    }
    
    private IEnumerator RunAllRuntimeTests()
    {
        Debug.Log("========== 开始运行所有运行时测试 ==========");
        
        TestGameEntrySystem();
        yield return new WaitForSeconds(0.5f);
        
        TestEventSystem();
        yield return new WaitForSeconds(0.5f);
        
        TestObjectPool();
        yield return new WaitForSeconds(0.5f);
        
        TestSelectBuildingUIAtRuntime();
        yield return new WaitForSeconds(0.5f);
        
        TestSpawnBuildAtRuntime();
        yield return new WaitForSeconds(0.5f);
        
        TestNetworkStatus();
        yield return new WaitForSeconds(0.5f);
        
        Debug.Log("========== 所有运行时测试完成 ==========");
    }
    
    // 在Inspector中可以调用的测试方法
    [ContextMenu("快速测试")]
    public void QuickTest()
    {
        Debug.Log("执行快速测试...");
        TestGameEntrySystem();
    }
}
