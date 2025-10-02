using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// 单元测试类
/// 对各个系统进行详细的单元测试
/// </summary>
public static class UnitTests
{
    /// <summary>
    /// 测试所有 UI 相关的类
    /// </summary>
    [MenuItem("Tools/Unit Tests/Test UI System")]
    public static void TestUISystem()
    {
        Debug.Log("=== UI 系统单元测试 ===");
        
        TestUIBaseInheritance();
        TestSelectBuildingUIImplementation();
        TestUIManagerIntegration();
    }
    
    /// <summary>
    /// 测试建筑系统
    /// </summary>
    [MenuItem("Tools/Unit Tests/Test Construction System")]
    public static void TestConstructionSystem()
    {
        Debug.Log("=== 建筑系统单元测试 ===");
        
        TestSpawnBuildAbility();
        TestSpawnBuildMonoComponent();
        TestConstructionDataIntegrity();
    }
    
    /// <summary>
    /// 测试游戏对象系统
    /// </summary>
    [MenuItem("Tools/Unit Tests/Test GameObject System")]
    public static void TestGameObjectSystem()
    {
        Debug.Log("=== 游戏对象系统单元测试 ===");
        
        TestGameObjectControllerAbstract();
        TestAbilitySystemIntegration();
        TestGameObjectEventsSystem();
    }
    
    /// <summary>
    /// 运行所有单元测试
    /// </summary>
    [MenuItem("Tools/Unit Tests/Run All Unit Tests")]
    public static void RunAllUnitTests()
    {
        Debug.Log("========== 开始运行所有单元测试 ==========");
        
        TestUISystem();
        TestConstructionSystem();
        TestGameObjectSystem();
        TestNetworkIntegration();
        TestDataIntegrity();
        
        Debug.Log("========== 所有单元测试完成 ==========");
    }
    
    private static void TestUIBaseInheritance()
    {
        Debug.Log("--- 测试 UIBase 继承体系 ---");
        
        try
        {
            var uiBaseType = typeof(UIBase);
            var selectBuildingUIType = typeof(SelectBuildingUI);
            
            // 检查继承关系
            bool isInherited = selectBuildingUIType.IsSubclassOf(uiBaseType);
            Debug.Log($"SelectBuildingUI 继承自 UIBase: {(isInherited ? "✓" : "✗")}");
            
            // 检查必要的接口实现
            bool implementsID = typeof(ID).IsAssignableFrom(uiBaseType);
            Debug.Log($"UIBase 实现 ID 接口: {(implementsID ? "✓" : "✗")}");
            
            // 检查虚方法重写
            var onOpenMethod = selectBuildingUIType.GetMethod("OnOpen");
            bool overridesOnOpen = onOpenMethod.DeclaringType == selectBuildingUIType;
            Debug.Log($"SelectBuildingUI 重写 OnOpen 方法: {(overridesOnOpen ? "✓" : "✗")}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"✗ UIBase 继承测试失败: {e.Message}");
        }
    }
    
    private static void TestSelectBuildingUIImplementation()
    {
        Debug.Log("--- 测试 SelectBuildingUI 实现 ---");
        
        try
        {
            var type = typeof(SelectBuildingUI);
            var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            
            bool hasBuildingField = false;
            foreach (var field in fields)
            {
                if (field.Name == "building" && field.FieldType.IsGenericType)
                {
                    var genericType = field.FieldType.GetGenericTypeDefinition();
                    if (genericType == typeof(List<>))
                    {
                        hasBuildingField = true;
                        break;
                    }
                }
            }
            
            Debug.Log($"包含 building 字段: {(hasBuildingField ? "✓" : "✗")}");
            
            // 检查方法存在性
            var onOpenMethod = type.GetMethod("OnOpen");
            bool hasOnOpen = onOpenMethod != null;
            Debug.Log($"包含 OnOpen 方法: {(hasOnOpen ? "✓" : "✗")}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"✗ SelectBuildingUI 实现测试失败: {e.Message}");
        }
    }
    
    private static void TestUIManagerIntegration()
    {
        Debug.Log("--- 测试 UIManager 集成 ---");
        
        try
        {
            // 检查 UIManager 类
            var uiManagerType = typeof(UIManager);
            Debug.Log($"UIManager 类存在: ✓");
            
            // 检查关键方法
            var showUIMethod = uiManagerType.GetMethod("ShowUI", new[] { typeof(string) });
            var closeUIMethod = uiManagerType.GetMethod("CloseUI");
            
            Debug.Log($"包含 ShowUI 方法: {(showUIMethod != null ? "✓" : "✗")}");
            Debug.Log($"包含 CloseUI 方法: {(closeUIMethod != null ? "✓" : "✗")}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"✗ UIManager 集成测试失败: {e.Message}");
        }
    }
    
    private static void TestSpawnBuildAbility()
    {
        Debug.Log("--- 测试 SpawnBuild 能力 ---");
        
        try
        {
            var spawnBuildType = typeof(SpawnBuild);
            var abilityType = typeof(Ability);
            
            // 检查继承关系
            bool inheritsAbility = spawnBuildType.IsSubclassOf(abilityType);
            Debug.Log($"SpawnBuild 继承自 Ability: {(inheritsAbility ? "✓" : "✗")}");
            
            // 检查重写方法
            var initMethod = spawnBuildType.GetMethod("Init");
            bool overridesInit = initMethod != null && initMethod.DeclaringType == spawnBuildType;
            Debug.Log($"重写 Init 方法: {(overridesInit ? "✓" : "✗")}");
            
            // 检查 CreateAssetMenu 特性
            var attributes = spawnBuildType.GetCustomAttributes(typeof(CreateAssetMenuAttribute), false);
            bool hasCreateAssetMenu = attributes.Length > 0;
            Debug.Log($"包含 CreateAssetMenu 特性: {(hasCreateAssetMenu ? "✓" : "✗")}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"✗ SpawnBuild 能力测试失败: {e.Message}");
        }
    }
    
    private static void TestSpawnBuildMonoComponent()
    {
        Debug.Log("--- 测试 SpawnBuildMono 组件 ---");
        
        try
        {
            var spawnBuildMonoType = typeof(SpawnBuildMono);
            
            // 检查 MonoBehaviour 继承
            bool inheritsMonoBehaviour = spawnBuildMonoType.IsSubclassOf(typeof(MonoBehaviour));
            Debug.Log($"继承自 MonoBehaviour: {(inheritsMonoBehaviour ? "✓" : "✗")}");
            
            // 检查接口实现
            var interfaces = spawnBuildMonoType.GetInterfaces();
            bool implementsPointerClick = false;
            
            foreach (var interfaceType in interfaces)
            {
                if (interfaceType.Name.Contains("IPointerClickHandler"))
                {
                    implementsPointerClick = true;
                    break;
                }
            }
            
            Debug.Log($"实现 IPointerClickHandler: {(implementsPointerClick ? "✓" : "✗")}");
            
            // 检查方法实现
            var onPointerClickMethod = spawnBuildMonoType.GetMethod("OnPointerClick");
            var spawnMethod = spawnBuildMonoType.GetMethod("Spawn", BindingFlags.NonPublic | BindingFlags.Instance);
            
            Debug.Log($"包含 OnPointerClick 方法: {(onPointerClickMethod != null ? "✓" : "✗")}");
            Debug.Log($"包含 Spawn 方法: {(spawnMethod != null ? "✓" : "✗")}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"✗ SpawnBuildMono 组件测试失败: {e.Message}");
        }
    }
    
    private static void TestConstructionDataIntegrity()
    {
        Debug.Log("--- 测试建筑数据完整性 ---");
        
        try
        {
            // 检查建筑数据文件
            string constructionDataPath = "Data/ConstructionData.xlsx";
            bool dataFileExists = System.IO.File.Exists(constructionDataPath);
            Debug.Log($"建筑数据文件存在: {(dataFileExists ? "✓" : "✗")}");
            
            // 检查 ScriptableObject 资源
            string[] constructionAssets = AssetDatabase.FindAssets("t:ScriptableObject", new[] { "Assets/ScriptableObjects/Stats/Construction" });
            Debug.Log($"找到 {constructionAssets.Length} 个建筑 ScriptableObject");
            
            if (constructionAssets.Length > 0)
            {
                Debug.Log("✓ 建筑 ScriptableObject 资源存在");
            }
            else
            {
                Debug.LogWarning("✗ 未找到建筑 ScriptableObject 资源");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"✗ 建筑数据完整性测试失败: {e.Message}");
        }
    }
    
    private static void TestGameObjectControllerAbstract()
    {
        Debug.Log("--- 测试 GameObjectController 抽象类 ---");
        
        try
        {
            var gameObjectControllerType = typeof(GameObjectController);
            
            // 检查抽象类
            bool isAbstract = gameObjectControllerType.IsAbstract;
            Debug.Log($"是抽象类: {(isAbstract ? "✓" : "✗")}");
            
            // 检查 NetworkBehaviour 继承
            bool inheritsNetworkBehaviour = gameObjectControllerType.IsSubclassOf(typeof(Mirror.NetworkBehaviour));
            Debug.Log($"继承自 NetworkBehaviour: {(inheritsNetworkBehaviour ? "✓" : "✗")}");
            
            // 检查抽象方法
            var logoutMethod = gameObjectControllerType.GetMethod("Logout", BindingFlags.NonPublic | BindingFlags.Instance);
            bool hasAbstractLogout = logoutMethod != null && logoutMethod.IsAbstract;
            Debug.Log($"包含抽象 Logout 方法: {(hasAbstractLogout ? "✓" : "✗")}");
            
            // 检查关键属性
            var abilitiesProperty = gameObjectControllerType.GetProperty("abilities");
            var eventsProperty = gameObjectControllerType.GetProperty("events");
            var statusProperty = gameObjectControllerType.GetProperty("status");
            
            Debug.Log($"包含 abilities 属性: {(abilitiesProperty != null ? "✓" : "✗")}");
            Debug.Log($"包含 events 属性: {(eventsProperty != null ? "✓" : "✗")}");
            Debug.Log($"包含 status 属性: {(statusProperty != null ? "✓" : "✗")}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"✗ GameObjectController 抽象类测试失败: {e.Message}");
        }
    }
    
    private static void TestAbilitySystemIntegration()
    {
        Debug.Log("--- 测试能力系统集成 ---");
        
        try
        {
            // 检查 Ability 基类
            var abilityType = typeof(Ability);
            Debug.Log($"Ability 基类存在: ✓");
            
            // 查找所有能力实现
            var assembly = System.Reflection.Assembly.GetAssembly(abilityType);
            var abilityTypes = new List<System.Type>();
            
            foreach (var type in assembly.GetTypes())
            {
                if (type.IsSubclassOf(abilityType) && !type.IsAbstract)
                {
                    abilityTypes.Add(type);
                }
            }
            
            Debug.Log($"找到 {abilityTypes.Count} 个能力实现:");
            foreach (var type in abilityTypes)
            {
                Debug.Log($"  - {type.Name}");
            }
            
            // 检查能力的关键方法
            var initMethod = abilityType.GetMethod("Init");
            var canDoMethod = abilityType.GetMethod("CanDo", new System.Type[0]);
            var doMethod = abilityType.GetMethod("Do", new System.Type[0]);
            
            Debug.Log($"包含 Init 方法: {(initMethod != null ? "✓" : "✗")}");
            Debug.Log($"包含 CanDo 方法: {(canDoMethod != null ? "✓" : "✗")}");
            Debug.Log($"包含 Do 方法: {(doMethod != null ? "✓" : "✗")}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"✗ 能力系统集成测试失败: {e.Message}");
        }
    }
    
    private static void TestGameObjectEventsSystem()
    {
        Debug.Log("--- 测试游戏对象事件系统 ---");
        
        try
        {
            // 检查 GameObjectEvents 类
            var gameObjectEventsType = typeof(GameObjectEvents);
            Debug.Log($"GameObjectEvents 类存在: ✓");
            
            // 检查事件字段
            var onSpawnField = gameObjectEventsType.GetField("onSpawn");
            var onDeadField = gameObjectEventsType.GetField("onDead");
            
            Debug.Log($"包含 onSpawn 事件: {(onSpawnField != null ? "✓" : "✗")}");
            Debug.Log($"包含 onDead 事件: {(onDeadField != null ? "✓" : "✗")}");
            
            if (onSpawnField != null)
            {
                bool isUnityEvent = onSpawnField.FieldType.IsSubclassOf(typeof(UnityEngine.Events.UnityEventBase));
                Debug.Log($"onSpawn 是 UnityEvent: {(isUnityEvent ? "✓" : "✗")}");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"✗ 游戏对象事件系统测试失败: {e.Message}");
        }
    }
    
    private static void TestNetworkIntegration()
    {
        Debug.Log("=== 网络集成单元测试 ===");
        
        try
        {
            // 检查 Mirror 网络组件
            var networkManagerType = typeof(Mirror.NetworkManager);
            Debug.Log($"Mirror NetworkManager 可用: ✓");
            
            // 检查自定义网络类
            var roomControllerType = typeof(RoomController);
            var playerType = typeof(Player);
            
            Debug.Log($"RoomController 类存在: ✓");
            Debug.Log($"Player 类存在: ✓");
            
            // 检查网络同步属性
            var gameObjectControllerType = typeof(GameObjectController);
            bool inheritsNetworkBehaviour = gameObjectControllerType.IsSubclassOf(typeof(Mirror.NetworkBehaviour));
            Debug.Log($"GameObjectController 继承 NetworkBehaviour: {(inheritsNetworkBehaviour ? "✓" : "✗")}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"✗ 网络集成测试失败: {e.Message}");
        }
    }
    
    private static void TestDataIntegrity()
    {
        Debug.Log("=== 数据完整性单元测试 ===");
        
        try
        {
            // 检查数据文件
            string[] dataFiles = {
                "Data/ConstructionData.xlsx",
                "Data/HeroData.xlsx",
                "Data/ShopData.xlsx"
            };
            
            foreach (string dataFile in dataFiles)
            {
                bool exists = System.IO.File.Exists(dataFile);
                Debug.Log($"{dataFile}: {(exists ? "✓" : "✗")}");
            }
            
            // 检查 ScriptableObject 目录
            string[] scriptableObjectDirs = {
                "Assets/ScriptableObjects/Ability",
                "Assets/ScriptableObjects/Stats"
            };
            
            foreach (string dir in scriptableObjectDirs)
            {
                bool exists = System.IO.Directory.Exists(dir);
                Debug.Log($"{dir}: {(exists ? "✓" : "✗")}");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"✗ 数据完整性测试失败: {e.Message}");
        }
    }
}

