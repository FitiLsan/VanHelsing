using System;
using UnityEngine;


namespace BeastHunter
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(ParticleSystem))]
    public sealed class QuestAreaBehaviour : MonoBehaviour
    {
        #region Fields

        [SerializeField] private int _questId;
        [SerializeField] private int _areaId;
        [SerializeField] private float _cooldown;
        [SerializeField] private int _taskId;
        [SerializeField] private bool _npcTrack = false;
        [SerializeField] private bool _enemyTrack = false;
        private Collider _collider;
        private ParticleSystem _particleSystem;
        private float _lastTrigger;
        private float _npcLastTrigger;
        private float _enemyLastTrigger;

        #endregion


        #region Methods

        private void Start()
        {
            _collider = GetComponent<Collider>();
            _particleSystem = GetComponent<ParticleSystem>();
            Services.SharedInstance.EventManager.StartListening(GameEventTypes.QuestAccepted, OnQuestAccept);
            Services.SharedInstance.EventManager.StartListening(GameEventTypes.QuestReported, OnQuestRemove);
            Services.SharedInstance.EventManager.StartListening(GameEventTypes.QuestAbandoned, OnQuestRemove);
            Services.SharedInstance.EventManager.StartListening(GameEventTypes.QuestTaskUpdated, OnTaskComplete);
        }

        private void OnTaskComplete(EventArgs arg0)
        {
            if (!(arg0 is TaskUpdatedArgs idArgs)) throw new Exception("QuestAreaBehaviour::OnTaskComplete: Invalid event arguments");
            if (_taskId != idArgs.Id || idArgs.CurrentAmount<idArgs.NeededAmount) return;
            _collider.enabled = false;
            _particleSystem.Stop();
        }   

        private void OnQuestRemove(EventArgs arg0)
        {
            if (!(arg0 is IdArgs idArgs)) throw new Exception("QuestAreaBehaviour::OnQuestAccept: Invalid event arguments");
            if (_questId != idArgs.Id) return;
            _collider.enabled = false;
            _particleSystem.Stop();
        }

        private void OnQuestAccept(EventArgs arg0)
        {
            if (!(arg0 is IdArgs idArgs)) throw new Exception("QuestAreaBehaviour::OnQuestAccept: Invalid event arguments");
            if (_questId != idArgs.Id) return;
            _collider.enabled = true;
            _particleSystem.Play();
        }

        private void OnTriggerEnter(Collider other)
        {
            var t = Time.time;
            if (!(t - _lastTrigger < _cooldown))
            {
                if (!other.gameObject.CompareTag("Player")) return;
                _lastTrigger = t;
                Services.SharedInstance.EventManager.TriggerEvent(GameEventTypes.AreaEnter, new IdArgs(_areaId));

#if UNITY_EDITOR
                Debug.Log($"Event[Enter] fired for area [{_areaId}] at {t}");
            
#endif
            }
            if (_npcTrack && !(t - _npcLastTrigger < _cooldown))
            {
                if (!other.gameObject.CompareTag("NPC")) return;
                _npcLastTrigger = t;
                //TODO: Get Npc Id from GO
                Services.SharedInstance.EventManager.TriggerEvent(GameEventTypes.NpcAreaEnter, new NpcAreaEnterArgs{AreaId = _areaId});

#if UNITY_EDITOR
                Debug.Log($"Event[Enter] fired for area [{_areaId}] at {t} with npc [TODO: GET NPC ID]");
            
#endif
            }
            if (_enemyTrack && !(t - _enemyLastTrigger < _cooldown))
            {
                if (!other.gameObject.CompareTag("Enemy")) return;
                _enemyLastTrigger = t;
                //TODO: Get Enemy Npc Id from GO
                Services.SharedInstance.EventManager.TriggerEvent(GameEventTypes.NpcAreaEnter, new NpcAreaEnterArgs{AreaId = _areaId});

#if UNITY_EDITOR
                Debug.Log($"Event[Enter] fired for area [{_areaId}] at {t} with enemy [TODO: GET NPC ID]");
            
#endif
            }
        }

        private void OnTriggerStay(Collider other)
        {
            var t = Time.time;
            if (!(t - _lastTrigger < _cooldown))
            {
                if (!other.gameObject.CompareTag("Player")) return;
                _lastTrigger = t;
                Services.SharedInstance.EventManager.TriggerEvent(GameEventTypes.AreaEnter, new IdArgs(_areaId));

#if UNITY_EDITOR
                Debug.Log($"Event[Enter/Stay] fired for area [{_areaId}] at {t}");

#endif
            }
            if (_npcTrack && !(t - _npcLastTrigger < _cooldown))
            {
                if (!other.gameObject.CompareTag("NPC")) return;
                _npcLastTrigger = t;
                //TODO: Get Npc Id from GO
                Services.SharedInstance.EventManager.TriggerEvent(GameEventTypes.NpcAreaEnter, new NpcAreaEnterArgs { AreaId = _areaId });

#if UNITY_EDITOR
                Debug.Log($"Event[Enter/Stay] fired for area [{_areaId}] at {t} with npc [TODO: GET NPC ID]");

#endif
            }
            if (_enemyTrack && !(t - _enemyLastTrigger < _cooldown))
            {
                if (!other.gameObject.CompareTag("Enemy")) return;
                _enemyLastTrigger = t;
                //TODO: Get Enemy Npc Id from GO
                Services.SharedInstance.EventManager.TriggerEvent(GameEventTypes.NpcAreaEnter, new NpcAreaEnterArgs { AreaId = _areaId });

#if UNITY_EDITOR
                Debug.Log($"Event[Enter/Stay] fired for area [{_areaId}] at {t} with enemy [TODO: GET NPC ID]");

#endif
            }
        }

        #endregion
    }
}