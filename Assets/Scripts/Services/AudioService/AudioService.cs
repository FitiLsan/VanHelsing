using UnityEngine;
using Extensions;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;


namespace BeastHunter
{
    public sealed class AudioService : Service, System.IDisposable
    {
        #region Fields

        private readonly GameContext _context;
        private readonly GameObject _ambienceSourceInstance;
        private AudioSource _mainAmbienceMusicSource;
        private AudioSource _blendingAmbienceMusicSource;
        private AudioSource _mainAmbienceSFXSource;
        private AudioSource _blendingAmbienceSFXSource;

        private Queue<SoundQueueElement> _ambienceMusicQueue;
        private Queue<SoundQueueElement> _ambienceSFXQueue;

        private Sequence _blendingMusicSequence;
        private Sequence _blendingSFXSequence;

        #endregion


        #region Properties

        public AudioData AudioData { get; }
        private bool IsBlendingMusic { get { return _blendingMusicSequence.IsPlaying(); } }
        private bool IsBlendingSFX { get { return _blendingSFXSequence.IsPlaying(); } }

        #endregion


        #region ClassLifeCycle

        public AudioService(Contexts contexts) : base(contexts)
        {
            _context = contexts as GameContext;
            AudioData = Data.AudioData;
            _ambienceSourceInstance = GameObject.Instantiate(AudioData.AmbienceObjectPrefab);

            if(_ambienceSourceInstance.transform.TryFindDeep(AudioData.FirstAmbientMusicSourceObjectName, 
                out Transform foundFirstMusicObjectTransform))
            {
                if(foundFirstMusicObjectTransform.gameObject.TryGetComponent(out AudioSource foundFirstMusicSource))
                {
                    _mainAmbienceMusicSource = foundFirstMusicSource;
                }
                else
                {
                    throw new System.Exception("Can't find audio source on object with name: " + 
                        foundFirstMusicObjectTransform.name);
                }
            }
            else
            {
                throw new System.Exception("Can't find transform with name: " + 
                    AudioData.FirstAmbientMusicSourceObjectName);
            }

            if (_ambienceSourceInstance.transform.TryFindDeep(AudioData.SecondAmbientMusicSourceObjectName,
                out Transform foundSecondMusicObjectTransform))
            {
                if (foundSecondMusicObjectTransform.gameObject.TryGetComponent(out AudioSource foundSecondMusicSource))
                {
                    _blendingAmbienceMusicSource = foundSecondMusicSource;
                }
                else
                {
                    throw new System.Exception("Can't find audio source on object with name: " + 
                        foundSecondMusicObjectTransform.name);
                }
            }
            else
            {
                throw new System.Exception("Can't find transform with name: " + 
                    AudioData.SecondAmbientMusicSourceObjectName);
            }

            if (_ambienceSourceInstance.transform.TryFindDeep(AudioData.FirstAmbientSFXSourceObjectName,
                out Transform foundFirstSFXObjectTransform))
            {
                if (foundFirstSFXObjectTransform.gameObject.TryGetComponent(out AudioSource foundFirstSFXSource))
                {
                    _mainAmbienceSFXSource = foundFirstSFXSource;
                }
                else
                {
                    throw new System.Exception("Can't find audio source on object with name: " +
                        foundFirstSFXObjectTransform.name);
                }
            }
            else
            {
                throw new System.Exception("Can't find transform with name: " +
                    AudioData.FirstAmbientSFXSourceObjectName);
            }

            if (_ambienceSourceInstance.transform.TryFindDeep(AudioData.SecondAmbientSFXSourceObjectName,
                out Transform foundSecondSFXObjectTransform))
            {
                if (foundSecondSFXObjectTransform.gameObject.TryGetComponent(out AudioSource foundSecondSFXSource))
                {
                    _blendingAmbienceSFXSource = foundSecondSFXSource;
                }
                else
                {
                    throw new System.Exception("Can't find audio source on object with name: " +
                        foundSecondSFXObjectTransform.name);
                }
            }
            else
            {
                throw new System.Exception("Can't find transform with name: " +
                    AudioData.SecondAmbientSFXSourceObjectName);
            }

            _ambienceMusicQueue = new Queue<SoundQueueElement>();
            _ambienceSFXQueue = new Queue<SoundQueueElement>();
            _blendingMusicSequence = DOTween.Sequence();
            _blendingMusicSequence.Complete();
            _blendingSFXSequence = DOTween.Sequence();
            _blendingSFXSequence.Complete();

            ChangeAmbientMusic(AudioData.AmbientMusicArray[0]);
            ChangeAmbientSFX(AudioData.AmbientSFXArray[0]);
        }

        #endregion


        #region Methods

        public void Initialize(CharacterModel characterModel)
        {
            characterModel.CurrentCharacterState.Subscribe(OnCharacterStateChange);
        }

        public void Dispose()
        {
            _context.CharacterModel.CurrentCharacterState.Dispose();
        }

        #region MusicMethods

        public void ChangeAmbientMusic(Sound sound, float blendTime = 0f, bool doOverlap = false)
        {
            if(sound.SoundClip != _mainAmbienceMusicSource.clip)
            {
                _ambienceMusicQueue.Enqueue(new SoundQueueElement(sound, blendTime, doOverlap));
                PlayNextAmbientMusicInList();
            }
        }

        private void PlayNextAmbientMusicInList()
        {
            if (!IsBlendingMusic && _ambienceMusicQueue.Count > 0)
            {
                BlendAmbientMusicFromQueue();
            }
        }

        private void BlendAmbientMusicFromQueue()
        {
            SoundQueueElement nextMusicElement = _ambienceMusicQueue.Peek();
            _blendingAmbienceMusicSource.clip = nextMusicElement.Sound.SoundClip;
            _blendingAmbienceMusicSource.pitch = nextMusicElement.Sound.Pitch;
            _blendingAmbienceMusicSource.loop = nextMusicElement.Sound.DoLoop;
            _blendingAmbienceMusicSource.Play();

            _blendingMusicSequence = DOTween.Sequence();
            _blendingMusicSequence.Append(_mainAmbienceMusicSource.DOFade(0f, nextMusicElement.BlendTIme));
 
            if (nextMusicElement.DoOverlap)
            {
                _blendingMusicSequence.Join(_blendingAmbienceMusicSource.DOFade(nextMusicElement.Sound.Volume, 
                    nextMusicElement.BlendTIme));
            }
            else
            {
                _blendingMusicSequence.Append(_blendingAmbienceMusicSource.DOFade(nextMusicElement.Sound.Volume,
                    nextMusicElement.BlendTIme));
            }

            _blendingMusicSequence.OnComplete(SwitchAmbientMusicSources);
        }

        private void SwitchAmbientMusicSources()
        {
            _ambienceMusicQueue.Dequeue();
            (_mainAmbienceMusicSource, _blendingAmbienceMusicSource) = 
                (_blendingAmbienceMusicSource, _mainAmbienceMusicSource);
            _blendingAmbienceMusicSource.Stop();
            PlayNextAmbientMusicInList();
        }

        #endregion

        #region SFXMethods

        public void ChangeAmbientSFX(Sound sound, float blendTime = 0f, bool doOverlap = false)
        {
            if(sound.SoundClip != _mainAmbienceSFXSource.clip)
            {
                _ambienceSFXQueue.Enqueue(new SoundQueueElement(sound, blendTime, doOverlap));
                PlayNextAmbientSFXInList();
            }
        }

        private void PlayNextAmbientSFXInList()
        {
            if (!IsBlendingSFX && _ambienceSFXQueue.Count > 0)
            {
                BlendAmbientSFXFromQueue();
            }
        }

        private void BlendAmbientSFXFromQueue()
        {
            SoundQueueElement nextSFXElement = _ambienceSFXQueue.Peek();
            _blendingAmbienceSFXSource.clip = nextSFXElement.Sound.SoundClip;
            _blendingAmbienceSFXSource.pitch = nextSFXElement.Sound.Pitch;
            _blendingAmbienceSFXSource.loop = nextSFXElement.Sound.DoLoop;
            _blendingAmbienceSFXSource.Play();

            _blendingSFXSequence = DOTween.Sequence();
            _blendingSFXSequence.Append(_mainAmbienceSFXSource.DOFade(0f, nextSFXElement.BlendTIme));

            if (nextSFXElement.DoOverlap)
            {
                _blendingSFXSequence.Join(_blendingAmbienceSFXSource.DOFade(nextSFXElement.Sound.Volume,
                    nextSFXElement.BlendTIme));
            }
            else
            {
                _blendingSFXSequence.Append(_blendingAmbienceSFXSource.DOFade(nextSFXElement.Sound.Volume,
                    nextSFXElement.BlendTIme));
            }

            _blendingSFXSequence.OnComplete(SwitchAmbientSFXSources);
        }

        private void SwitchAmbientSFXSources()
        {
            _ambienceSFXQueue.Dequeue();
            (_mainAmbienceSFXSource, _blendingAmbienceSFXSource) =
                (_blendingAmbienceSFXSource, _mainAmbienceSFXSource);
            _blendingAmbienceSFXSource.Stop();
            PlayNextAmbientSFXInList();
        }

        #endregion

        private void OnCharacterStateChange(CharacterBaseState currentState)
        {
            switch (currentState?.StateName)
            {
                case CharacterStatesEnum.Sneaking:
                    AudioData.MainMixer.SetFloat(AudioData.CharacterMovementVolumeName,
                        _context.CharacterModel.CharacterCommonSettings.SneakingCharacterMovementMixerVolume);
                    break;
                case CharacterStatesEnum.Movement:
                    AudioData.MainMixer.SetFloat(AudioData.CharacterMovementVolumeName,
                        _context.CharacterModel.CharacterCommonSettings.StandartCharacterMovementMixerVolume);
                    break;
                case CharacterStatesEnum.KnockedDown:
                    AudioData.MainMixer.SetFloat(AudioData.CharacterMovementVolumeName,
                        _context.CharacterModel.CharacterCommonSettings.StandartCharacterMovementMixerVolume);
                    break;
                default:
                    break;
            }
        }

        #endregion
    }
}
