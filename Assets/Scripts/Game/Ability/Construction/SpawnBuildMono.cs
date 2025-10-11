using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.UI.GridLayoutGroup;

public class SpawnBuildMono : MonoBehaviour, IPointerClickHandler
{
    protected GameObjectController owner;
    private void Awake()
    {
        owner = GetComponent<GameObjectController>();
    }
    public void Init(GameObjectController owner)
    {
        this.owner = owner;
        transform.SetParent(GameObject.Find("SpawnBuildCanvas").transform);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (RoomController.instance.localPlayer != owner.player) return;
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
            obj.GetComponent<GameObjectController>().events.onSpawn?.Invoke(owner.player);
            obj.transform.position = owner.transform.position;
            GameEntry.ObjectPoolComponent.Release(gameObject);
            GameEntry.ObjectPoolComponent.Release(owner.gameObject);
        }
    }
    private void Update()
    {
        Vector3 point = owner.transform.position + new Vector3(0, 5, 0);
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(point);
        GetComponent<RectTransform>().position = screenPoint;
    }
}