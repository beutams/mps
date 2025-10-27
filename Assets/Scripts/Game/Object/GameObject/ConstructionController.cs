using System.Collections.Generic;

public class ConstructionController : GameObjectController
{
    public static string constructionHealthBar = "ConstructionHealthBar";
    public static string constructionMiniMap = "ConstructionMiniMapItem";
    protected override void OnObjectSpawn(Player player)
    {
        base.OnObjectSpawn(player);
        UIManager.instance.AddHealthBar(this, constructionHealthBar);
        UIManager.instance.AddMiniMapItem(this, constructionMiniMap);
        ORCAManager.instance.AddObstacle(gameObject, true);
    }

    protected override void OnObjectDead()
    {
        base.OnObjectDead();
        UIManager.instance.RemoveHealthBar(this);
        UIManager.instance.RemoveMiniMapItem(this);
        ORCAManager.instance.RemoveObstacle(gameObject);
    }

    protected override void Logout()
    {
        player.constructionList.Remove(this);
    }
}