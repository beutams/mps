using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GlobalSkillItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image mask;
    [SerializeField] protected Image skill;
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
            progress = 0;
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
        skill.sprite = GameEntry.ResourceComponent.GetImage(data.imgPath);
        gameObject.SetActive(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }
}
