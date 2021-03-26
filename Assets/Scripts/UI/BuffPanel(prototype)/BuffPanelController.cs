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


        private void Start()
        {
            DG.Tweening.DOVirtual.DelayedCall(2, Init);
        }
        private void AddBuffImage(TemporaryBuff temporaryBuff)
        {
          var buff =  Instantiate(BuffPrefab, BackGround);
            buff.name = temporaryBuff.Name;
            _buffImage = buff.GetComponent<Image>();
            _buffImage.sprite = temporaryBuff.Sprite;
            _buffImage.color = temporaryBuff.Type.Equals(BuffType.Debuf) ? Color.red : Color.green;
            RemoveBuffImage(buff, temporaryBuff.Time);
        }
        private void AddBuffImage(PermanentBuff permanentBuff)
        {
            var buff = Instantiate(BuffPrefab, BackGround);
            buff.name = permanentBuff.Name;
            _buffImage = buff.GetComponent<Image>();
            _buffImage.sprite = permanentBuff.Sprite;
            _buffImage.color = permanentBuff.Type.Equals(BuffType.Debuf) ? Color.red : Color.green;
        }
        private void RemoveBuffImage(GameObject buff, float time)
        {
            Destroy(buff, time);
        }
        private void Init()
        {
            _context = Services.SharedInstance.Context;
            _characterModel = _context.CharacterModel;

            _characterModel.CurrentStats.BuffHolder.PerammentBuffAdded += AddBuffImage;
            _characterModel.CurrentStats.BuffHolder.TemporaryBuffAdded += AddBuffImage;
        }
    }
}