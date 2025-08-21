using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace Michsky.UI.Shift
{
    [ExecuteInEditMode]
    public class MainPanelButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public Animator buttonAnimator;

        [Header("Event")]
        public UnityEvent onEnter;
        public UnityEvent onExit;
        public UnityEvent onClick;
        public UnityEvent onCancel;

        void OnEnable()
        {
            onClick.AddListener(InitGroup);
        }
        protected void InitGroup()
        {
            string group = GetComponent<UIGroup>()?.GetGroup();
            if (group != null && UIGroup.globalDic.ContainsKey(group))
            {
                foreach(var item in UIGroup.globalDic[group])
                {
                    item.GetComponent<MainPanelButton>().CancelClick();
                }
            }
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
#if !UNITY_ANDROID && !UNITY_IOS
            if (!buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Normal to Pressed"))
            {
                buttonAnimator.Play("Dissolve to Normal");
                onEnter?.Invoke();
            }
#endif
        }

        public void OnPointerExit(PointerEventData eventData)
        {
#if !UNITY_ANDROID && !UNITY_IOS
            if (!buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Normal to Pressed"))
            {
                buttonAnimator.Play("Normal to Dissolve");
                onExit?.Invoke();
            }
#endif
        }

        public void OnPointerClick(PointerEventData eventData)
        {
#if !UNITY_ANDROID && !UNITY_IOS
            if (!buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Normal to Pressed"))
            {
                buttonAnimator.Play("Normal to Pressed");
                onClick?.Invoke();
            }
#endif  
        }
        public void CancelClick()
        {
            if (buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Normal to Pressed"))
            {
                buttonAnimator.Play("Pressed to Dissolve");
                onCancel?.Invoke();
            }
        }
    }
}