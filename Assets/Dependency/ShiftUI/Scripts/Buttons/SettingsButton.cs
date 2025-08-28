using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;

namespace Michsky.UI.Shift
{
    public class SettingsButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        protected Animator buttonAnimator;

        [Header("Event")]
        public UnityEvent onEnter;
        public UnityEvent onExit;
        public UnityEvent onClick;
        private void Awake()
        {
            buttonAnimator = GetComponent<Animator>();
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
#if !UNITY_ANDROID && !UNITY_IOS
            if (!buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Highlighted"))
            {
                buttonAnimator.Play("Highlighted");
                onEnter?.Invoke();
            }
#endif
        }
        public void OnPointerExit(PointerEventData eventData)
        {
#if !UNITY_ANDROID && !UNITY_IOS
            if (!buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Normal"))
            {
                buttonAnimator.Play("Normal");
                onExit?.Invoke();
            }
#endif
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            onClick?.Invoke();
        }
    }
}