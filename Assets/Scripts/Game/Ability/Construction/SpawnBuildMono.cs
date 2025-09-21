using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.UI.GridLayoutGroup;

public class SpawnBuildMono : MonoBehaviour, IPointerClickHandler
{
    protected GameObjectController controller;
    private void Awake()
    {
        controller = GetComponent<GameObjectController>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (RoomController.instance.localPlayer != controller.player) return;
        GameEntry.UIComponent.ShowUI("SelectBuildingUI");
        GameEntry.EventComponent.Subscribe(GameEvent.BuildSelectEvent, Spawn);
    }
    protected void Spawn(object data)
    {
        GameEntry.EventComponent.Desubscribe(GameEvent.BuildSelectEvent, Spawn);
        string name = (string)data;
        if (name != null)
        {
            Debug.Log($"SpawnBuildMono : Get building name :{name}");
            GameObject obj = GameEntry.ObjectPoolComponent.Get(name);
            obj.GetComponent<GameObjectController>().events.onSpawn?.Invoke(controller.player);
            obj.transform.position = transform.position;
            GameEntry.ObjectPoolComponent.Release(gameObject);
        }
    }
}