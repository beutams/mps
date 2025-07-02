using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GlobalSkillItem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] protected Image mask;
    [SerializeField] protected Image skill;
    protected GlobalSkillData data;
    protected int index;
    protected float progress = 0;

    private void Start()
    {
        mask.type = Image.Type.Filled;
        mask.fillAmount = 1;
    }
    private void Update()
    {
        progress = data.ability.GetProgress();
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
        if (data.ability.CanDo())
            data.ability.Do();
        else if (data.ability.CanDo(obj))
            data.ability.Do(obj);
        else if (data.ability.CanDo(targetPosition))
            data.ability.Do(targetPosition);
    }
    public void Init(GlobalSkillData data,int index)
    {
        this.data = data;
        data.ability = Instantiate(data.ability);
        data.ability.Init(null);
        this.index = index;
        skill.sprite = GameEntry.ResourceComponent.GetImage(data.imgPath);
        gameObject.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
    }
}
