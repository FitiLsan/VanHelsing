using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BeastHunter
{
    public class BuffPanelController : MonoBehaviour
    {
        public Transform BackGround;
        public GameObject BuffPrefab;
        private Image _buffImage;
        private GameContext _context;
        private CharacterModel _characterModel;
        private List<GameObject> _buffList;


        private void Start()
        {
            DG.Tweening.DOVirtual.DelayedCall(2, Init);
            _buffList = new List<GameObject>();
        }
        private void AddBuffImage(TemporaryBuff temporaryBuff)
        {
          var buff =  Instantiate(BuffPrefab, BackGround);
            _buffList.Add(buff);
            buff.name = temporaryBuff.Name;
            _buffImage = buff.GetComponent<Image>();
            _buffImage.sprite = temporaryBuff.Sprite;
            _buffImage.color = temporaryBuff.Type.Equals(BuffType.Debuf) ? Color.red : Color.green;
            RemoveBuffImageByTime(buff, temporaryBuff.Time);
        }
        private void AddBuffImage(PermanentBuff permanentBuff)
        {
            var buff = Instantiate(BuffPrefab, BackGround);
            _buffList.Add(buff);
            buff.name = permanentBuff.Name;
            _buffImage = buff.GetComponent<Image>();
            _buffImage.sprite = permanentBuff.Sprite;
            _buffImage.color = permanentBuff.Type.Equals(BuffType.Debuf) ? Color.red : Color.green;
        }

        private void RemoveBuffImage(TemporaryBuff temporaryBuff)
        {
            var buff = _buffList.Find(x => x.name.Equals(temporaryBuff.Name));
            if (buff!=null)
            {
                _buffList.Remove(buff);
                Destroy(buff);
            }
        }
        private void RemoveBuffImage(PermanentBuff permanentBuff)
        {
            var buff = _buffList.Find(x => x.name.Equals(permanentBuff.Name));
            if (buff != null)
            {
                _buffList.Remove(buff);
                Destroy(buff);
            }
        }
        private void RemoveBuffImageByTime(GameObject buff, float time)
        {
            DG.Tweening.DOVirtual.DelayedCall(time, DestroyBuff);

            void DestroyBuff()
            {
                if (_buffList.Contains(buff))
                {
                    _buffList.Remove(buff);
                    Destroy(buff);
                }
            }
        }


        private void Init()
        {
            _context = Services.SharedInstance.Context;
            _characterModel = _context.CharacterModel;
            if(_characterModel != null)
            {
                _characterModel.CurrentStats.BuffHolder.PerammentBuffAdded += AddBuffImage;
                _characterModel.CurrentStats.BuffHolder.TemporaryBuffAdded += AddBuffImage;
                _characterModel.CurrentStats.BuffHolder.PerammentBuffRemoved += RemoveBuffImage;
                _characterModel.CurrentStats.BuffHolder.TemporaryBuffRemoved += RemoveBuffImage;
            }
        }

        private void OnDestroy()
        {
            if (_characterModel != null)
            {
                _characterModel.CurrentStats.BuffHolder.PerammentBuffAdded -= AddBuffImage;
                _characterModel.CurrentStats.BuffHolder.TemporaryBuffAdded -= AddBuffImage;
                _characterModel.CurrentStats.BuffHolder.PerammentBuffRemoved -= RemoveBuffImage;
                _characterModel.CurrentStats.BuffHolder.TemporaryBuffRemoved -= RemoveBuffImage;
            }
        }
    }
}