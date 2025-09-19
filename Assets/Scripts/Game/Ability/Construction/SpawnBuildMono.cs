using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.UI.GridLayoutGroup;

public class SpawnBuildMono : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GameEntry.UIComponent.ShowUI("BuildSelectUI");
        GameEntry.EventComponent.Subscribe(GameEvent.BuildSelectEvent, Spawn);
    }
    protected void Spawn(object data)
    {
        GameEntry.EventComponent.Desubscribe(GameEvent.BuildSelectEvent, Spawn);
        string name = (string)data;
        if (name != null)
        {
            GameObject obj = GameEntry.ObjectPoolComponent.Get(name);
            obj.transform.position = transform.position;
            GameEntry.ObjectPoolComponent.Release(gameObject);
        }
    }
}