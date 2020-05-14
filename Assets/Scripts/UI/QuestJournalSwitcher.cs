using UnityEngine;


namespace BeastHunter
{
    public sealed class QuestJournalSwitcher : MonoBehaviour
    {
        Canvas canvas;

        public void Start()
        {
            canvas= gameObject.GetComponent<Canvas>();
            canvas.enabled = false;
        }

        public void Update()
        {
            if(Input.GetButtonDown("QuestJournal"))
            {
                Services.SharedInstance.EventManager.TriggerEvent(GameEventTypes.QuestJournalOpened, null);
                Switcher();
            }
        }

        public void On()
        {
            canvas.enabled = true;
            LockCharAction.LockAction(true);
        }

        public void Off()
        {
            canvas.enabled = false;
            LockCharAction.LockAction(false);
        }

        public void Switcher()
        {
            if(!canvas.enabled)
            {
                On();
            }
            else
            {
                Off();
            }
        }
    }
}
