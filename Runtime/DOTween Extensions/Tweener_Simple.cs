using DG.Tweening;
using UnityEngine;
using System.Collections;

namespace OCSFX.DoTweenExtensions
{
    public class Tweener_Simple : MonoBehaviour
    {
        [Tooltip("Defaults to owning GameObject if left empty.")]
        public GameObject TargetObject;

        public TweenDataSO TweenData;

        private TweenAnimType _tweenAnimType;

        public Tween TweenInstance { get; private set; }

        public Transform StartTransform { get; private set; }

        private Vector3 _startScale;
        private Vector3 _startPosition;
        private Vector3 _startRotation;

        private float _defaultDuration;

        private void Awake()
        {
            Initialize();
        }

        private void OnEnable()
        {
            if (TweenData.PlayOnEnable) PlayTween();
        }

        private void OnDisable()
        {
            TweenInstance?.Kill();

            if (TweenData.ResetOnDisable) ResetTransform();
        }

        private void Initialize()
        {
            UpdateTweenData();
        }

        private void UpdateTweenData()
        {
            _defaultDuration = TweenData.Duration;

            if (!TargetObject) TargetObject = gameObject;

            StartTransform = TargetObject.transform;

            _tweenAnimType = TweenData.TweenAnimType;

            SetStartValues();
            SetStartOffset();
        }

        private void SetStartValues()
        {
            _startScale = StartTransform.localScale;
            _startPosition = StartTransform.localPosition;
            _startRotation = StartTransform.localRotation.eulerAngles;
        }

        private void SetStartOffset()
        {
            var startOffset = TweenData.StartOffset;

            switch (_tweenAnimType)
            {
                case TweenAnimType.Scale:
                    _startScale += startOffset;
                    break;
                case TweenAnimType.Move:
                    _startPosition += startOffset;
                    break;
                case TweenAnimType.Rotate:
                    _startRotation += startOffset;
                    break;
            }
        }

        public void SetDuration(float duration)
        {
            TweenData.Duration = duration;
        }

        public void ResetDuration()
        {
            TweenData.ResetDuration();
        }

        public void SetDestination(Vector3 destination)
        {
            TweenData.Destination = destination;
        }

        public void PlayTween()
        {
            UpdateTweenData();

            TweenInstance = CreateTween();

            ResetTransform();

            if (TweenData.StartDelay > 0) StartCoroutine(Co_PlayTweenWithDelay(TweenData.StartDelay));
            else TweenInstance.Play();
        }

        public void PauseTween()
        {
            if (TweenInstance.IsPlaying()) TweenInstance.Pause();
        }

        private IEnumerator Co_PlayTweenWithDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            if (!TweenInstance.IsPlaying()) TweenInstance.Play();
        }

        public void ResetTransform()
        {
            switch (TweenData.TweenAnimType)
            {
                case TweenAnimType.Scale:
                    TargetObject.transform.localScale = _startScale;
                    break;
                case TweenAnimType.Move:
                    TargetObject.transform.localPosition = _startPosition;
                    break;
                case TweenAnimType.Rotate:
                    TargetObject.transform.localRotation = Quaternion.Euler(_startRotation);
                    break;
            }
        }

        private Tween CreateTween()
        {
            TweenInstance?.Kill();

            var destination = TweenData.Destination;
            var duration = TweenData.Duration;
            var easeSetting = TweenData.EaseSetting;
            var loops = TweenData.Loops;
            var loopSetting = TweenData.LoopSetting;

            if (TweenData.ConvertDurationToSpeed)
            {
                var speed = (_startPosition - destination).magnitude / duration;
                duration = speed;
            }

            Tween tween = null;

            switch (_tweenAnimType)
            {
                case TweenAnimType.Scale:
                    tween = CreateScaleTween(destination, duration, easeSetting, loops, loopSetting);
                    break;
                case TweenAnimType.Move:
                    tween = CreateMoveTween(destination, duration, easeSetting, loops, loopSetting);
                    break;
                case TweenAnimType.Rotate:
                    tween = CreateRotateTween(destination, duration, easeSetting, loops, loopSetting);
                    break;
            }

            return tween;
        }

        public Tween GetTween()
        {
            return TweenInstance;
        }

        #region Tween Creation Methods
        private Tween CreateScaleTween(Vector3 destination, float duration, Ease easeSetting, int loops, LoopType loopSetting)
        {
            var tween = StartTransform.DOScale(destination, duration).SetEase(easeSetting).SetLoops(loops, loopSetting).SetAutoKill(false);
            tween.Pause();

            return tween;
        }

        private Tween CreateMoveTween(Vector3 destination, float duration, Ease easeSetting, int loops, LoopType loopSetting)
        {
            var tween = StartTransform.DOMove(destination, duration).SetEase(easeSetting).SetLoops(loops, loopSetting).SetAutoKill(false);
            tween.Pause();

            return tween;
        }

        private Tween CreateRotateTween(Vector3 destination, float duration, Ease easeSetting, int loops, LoopType loopSetting)
        {
            var tween = StartTransform.DOLocalRotate(destination, duration).SetEase(easeSetting).SetLoops(loops, loopSetting).SetAutoKill(false);
            tween.Pause();

            return tween;
        }
        #endregion

        private void OnValidate()
        {
            if (!TargetObject) TargetObject = gameObject;
        }
    }
}
