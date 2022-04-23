using EventSystem.Events;
using EventSystem.Events.UI;
using UnityEngine;

namespace EventSystem
{
    public class ButtonEventTrigger : MonoBehaviour
    {
        public void OnButtonEnter()
        {
            EventManager.TriggerEvent<ButtonEnterEvent>();
        }

        public void OnButtonExit()
        {
            EventManager.TriggerEvent<ButtonExitEvent>();
        }

        public void OnButtonClickDown()
        {
            EventManager.TriggerEvent<ButtonClickDownEvent>();
        }
        
        public void OnButtonClickUp()
        {
            EventManager.TriggerEvent<ButtonClickUpEvent>();
        }
    }
}
