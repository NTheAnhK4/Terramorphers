using R3;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameCore.Utility.UI
{
   
    public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        private float holdTime = 0;
        private bool isHolding = false;
        public float holdThreshold = .3f;
        private bool isHoldTriggered;
        public ReactiveProperty<bool> IsHolding = new ReactiveProperty<bool>();
        public ReactiveCommand OnClick = new ReactiveCommand();
        public bool interactable = true;
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!interactable) return;
            holdTime = 0f;
            isHolding = true;
            isHoldTriggered = false;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if(!interactable) return;
            isHolding = false;
            if (isHoldTriggered) IsHolding.Value = false;
            else OnClick.Execute(default);
        }


        public void OnPointerExit(PointerEventData eventData)
        {
            if (!interactable) return;
            isHolding = false;

            if (isHoldTriggered) IsHolding.Value = false;
        }
        private void Update()
        {
            if (!interactable) return;
            if (!isHolding) return;

            holdTime += Time.deltaTime;

            if (!isHoldTriggered && holdTime >= holdThreshold)
            {
                isHoldTriggered = true;
                IsHolding.Value = true;
            }
        }

    }

}
