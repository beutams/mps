using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Michsky.UI.Shift
{
    public class ChapterButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        Image backgroundImageObj;
        TextMeshProUGUI titleObj;
        TextMeshProUGUI descriptionObj;
        Transform statusNone;
        Transform statusLocked;
        Transform statusCompleted;
        Animator animator;

        public StatusItem statusItem;
        public UnityEvent onEnter;
        public UnityEvent onExit;
        public UnityEvent onClick;
        public enum StatusItem
        {
            None,
            Locked,
            Completed
        }

        void Start()
        {
            animator = GetComponent<Animator>();
            backgroundImageObj = gameObject.transform.Find("Content/Background").GetComponent<Image>();
            titleObj = gameObject.transform.Find("Content/Texts/Title").GetComponent<TextMeshProUGUI>();
            descriptionObj = gameObject.transform.Find("Content/Texts/Description").GetComponent<TextMeshProUGUI>();
            statusNone = gameObject.transform.Find("Content/Texts/Status/None").GetComponent<Transform>();
            statusLocked = gameObject.transform.Find("Content/Texts/Status/Locked").GetComponent<Transform>();
            statusCompleted = gameObject.transform.Find("Content/Texts/Status/Completed").GetComponent<Transform>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Press"))
            {
                onClick?.Invoke();
                animator.Play("Press");
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Normal"))
            {
                onExit?.Invoke();
                animator.Play("Normal");
            }

        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Highlighted"))
            {
                onEnter?.Invoke();
                animator.Play("Highlighted");
            }
        }
        public void Init(string title,string description,string back, StatusItem statu)
        {
            backgroundImageObj.sprite = GameEntry.ResourceComponent.GetImage(back);
            titleObj.text = title;
            descriptionObj.text = description;
            statusItem = statu;
            if (statusItem == StatusItem.None)
            {
                statusNone.gameObject.SetActive(true);
                statusLocked.gameObject.SetActive(false);
                statusCompleted.gameObject.SetActive(false);
            }
            else if (statusItem == StatusItem.Locked)
            {
                statusNone.gameObject.SetActive(false);
                statusLocked.gameObject.SetActive(true);
                statusCompleted.gameObject.SetActive(false);
            }
            else if (statusItem == StatusItem.Completed)
            {
                statusNone.gameObject.SetActive(false);
                statusLocked.gameObject.SetActive(false);
                statusCompleted.gameObject.SetActive(true);
            }
        }
    }
}