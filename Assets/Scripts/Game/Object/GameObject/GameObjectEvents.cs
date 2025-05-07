using UnityEngine.Events;

public class GameObjectEvents
{
    public UnityEvent<Player> onSpawn = new UnityEvent<Player>();
    public UnityEvent onDead = new UnityEvent();
}