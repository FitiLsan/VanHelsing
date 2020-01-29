using System.Collections;
using UnityEngine;

namespace Models.NPCScripts.NPC
{
    public class NPCIdleController : MonoBehaviour
    {
        public delegate void IdleWaiter();

        public static IdleWaiter IdleEvent;
        private float time;

        private Coroutine waiter;

        private IEnumerator WaitForEnd(float time)
        {
            var wait = new WaitForSeconds(time);
            yield return wait;
            IdleEvent();
        }

        public void Idle()
        {
            time = Random.Range(3, 10);
            Debug.Log("Wait: " + time);
            waiter = StartCoroutine(WaitForEnd(time));
        }
    }
}