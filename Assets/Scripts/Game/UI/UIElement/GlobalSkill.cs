using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GlobalSkill : SingletonMonoBehaviour<GlobalSkill>
{
    protected List<GlobalSkillItem> items = new List<GlobalSkillItem>();
    public GlobalSkillItem item;
    public RectTransform info;

    protected GlobalSkillItem currentItem;
    protected GlobalSkillItem last;
    protected Timer timer;
    public void InitAbilities(List<GlobalSkillData> datas)
    {
        int index = 0;
        foreach (var data in datas)
        {
            index++;
            GlobalSkillItem globalItem = Instantiate(item);
            globalItem.Init(data, index);
            globalItem.transform.parent = transform;
            items.Add(globalItem);
        }
    }
    public void DoSkill(int index)
    {
        items[index].DoSkill(null,Vector3.zero);
    }
    public void ShowInfo()
    {
        info.gameObject.SetActive(true);
    }
    public void OnShowInfo(string name)
    {
        
    }
    public string GetIntroduce(string name)
    {
        return null;
    }
    #region ShowInfo
    private void Start()
    {
        timer = new Timer();
        timer.Init(3f, OnTimerComplete, false, false);
        TimerManager.instance.AddTimer(timer);
    }
    public void Update()
    {
        currentItem = RaycastItem<GlobalSkillItem>();
        WaitTime();
    }
    public void WaitTime()
    {
        if (currentItem == last) return;
        last = currentItem;
        info.gameObject.SetActive(false);
        if (last != null)
        {
            timer.Reset();
            timer.Lanuch();
        }
        else
        {
            timer.Pause();
            info.gameObject.SetActive(false);
        }
    }
    public T RaycastItem<T>() where T : MonoBehaviour
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        foreach (var result in results)
        {
            if (result.gameObject.TryGetComponent(out T t))
            {
                return t;
            }
        }
        return null;
    }
    public void OnTimerComplete()
    {
        timer.Pause();
        Vector3 position = Input.mousePosition;
        info.position = new Vector3(position.x - info.rect.width / 2, position.y - info.rect.height / 2, 0);
        info.gameObject.SetActive(true);
        info.GetChild(0).GetComponent<Image>().sprite = GameEntry.ResourceComponent.GetImage(currentItem.GetData().imgPath);
        info.GetChild(1).GetComponent<TextMeshProUGUI>().text = $"{currentItem.GetData().skillName}\n{currentItem.GetData().description}";
    }
    #endregion
}
