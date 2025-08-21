using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Michsky.UI.Shift
{
    [ExecuteInEditMode]
    public class MainButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        Animator animator;
        TextMeshProUGUI textMeshN;
        TextMeshProUGUI textMeshH;
        TextMeshProUGUI textMeshP;
        public UnityEvent onEnter;
        public UnityEvent onExit;
        public UnityEvent onClick;
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Normal"))
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
        public void SetText(string text)
        {
            textMeshN = transform.Find("Normal/Text").GetComponent<TextMeshProUGUI>();
            textMeshH = transform.Find("Highlighted/Text").GetComponent<TextMeshProUGUI>();
            textMeshP = transform.Find("Pressed/Text").GetComponent<TextMeshProUGUI>();
            textMeshN.text = text;
            textMeshH.text = text;
            textMeshP.text = text;
        }
    }
}