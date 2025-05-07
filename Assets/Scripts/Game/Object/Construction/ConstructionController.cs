using System.Collections.Generic;

public class ConstructionController : GameObjectController
{
    public static string constructionHealthBar = "ConstructionHealthBar";
    protected override void OnObjectSpawn(Player player)
    {
        base.OnObjectSpawn(player);
        UIManager.instance.AddHealthBar(this, constructionHealthBar);
    }

    protected override void OnObjectDead()
    {
        base.OnObjectDead();
        UIManager.instance.RemoveHealthBar(this);
    }

    protected override void Logout()
    {
        player.constructionList.Remove(this);
    }
}