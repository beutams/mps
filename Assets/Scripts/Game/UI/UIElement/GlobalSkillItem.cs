using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GlobalSkillItem : MonoBehaviour
{
    [SerializeField] protected Image mask;
    [SerializeField] protected Image skill;
    [SerializeField] protected Image backImage;
    protected GlobalSkillData data;
    protected int index;
    protected float progress = 0;

    protected CoverAbility ability;
    private void Update()
    {
        progress = ability.GetProgress();
        if(progress > 0)
        {
            mask.fillAmount = progress;
        }
        else
        {
            mask.fillAmount = 0;
        }
    }
    public void DoSkill(GameObjectController obj, Vector3 targetPosition)
    {
        if (ability.CanDo())
            ability.Do();
        else if (ability.CanDo(obj))
            ability.Do(obj);
        else if (ability.CanDo(targetPosition))
            ability.Do(targetPosition);
    }
    public void Init(GlobalSkillData data,int index)
    {
        this.data = data;
        ability = Instantiate(data.ability);
        ability.Init(null);
        ability.Lanuch();
        this.index = index;
        Sprite sprite = GameEntry.ResourceComponent.GetImage(data.imgPath);
        skill.sprite = sprite;
        backImage.sprite = sprite;
        gameObject.SetActive(true);
    }
    public GlobalSkillData GetData()
    {
        return data;
    }
}
