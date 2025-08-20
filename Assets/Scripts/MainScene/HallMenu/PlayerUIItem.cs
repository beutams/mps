using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIItem : MonoBehaviour, ID
{
    public TextMeshProUGUI playerName;
    public Image hero;
    public List<Image> skills;
    public GameObject ready;
    public GameObject unReady;
    [Header("ID")]
    public int id;
    public IDType type;
    public int ID => id;
    public IDType searchName => type;

    public void Refresh(PlayerInfo playerInfo)
    {
        playerName.text = playerInfo.name;
        hero.sprite = GameEntry.ResourceComponent.GetImage((GameEntry.ResourceComponent.GetDataResource("HeroStats", playerInfo.data.hero) as HeroStats).imgPath);
        for(int i = 0;i<skills.Count;i++)
        {
            skills[i].sprite = GameEntry.ResourceComponent.GetImage((GameEntry.ResourceComponent.GetDataResource("GlobalSkillData", playerInfo.data.globalSkills[i]) as GlobalSkillData).imgPath);
        }
        ready.SetActive(playerInfo.ready);
        unReady.SetActive(!playerInfo.ready);
    }
}
