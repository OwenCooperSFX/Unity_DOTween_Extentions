using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace OCSFX.DoTweenExtensions
{
    public class Tweener_Path : MonoBehaviour
    {
        [Tooltip("Defaults to owning GameObject if left empty.")]
        public GameObject TargetObject;

        public TweenDataSO_Path TweenPathData;

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
            if (TweenPathData.PlayOnEnable)
                PlayTween();
        }

        private void OnDisable()
        {
            TweenInstance.Kill();

            if (TweenPathData.ResetOnDisable)
                ResetTransform();
        }

        private void Initialize()
        {
            UpdateTweenData();
        }

        public void UpdateTweenData()
        {
            _defaultDuration = TweenPathData.Duration;

            if (!TargetObject)
                TargetObject = gameObject;

            StartTransform = TargetObject.transform;

            SetStartValues();
            SetStartOffset();
        }

        private void SetStartValues()
        {
            _startScale = StartTransform.localScale;
            _startPosition = StartTransform.localPosition;
            _startRotation = StartTransform.localRotation.eulerAngles;

            _defaultDuration = TweenPathData.Duration;
        }

        private void SetStartOffset()
        {
            Vector3 startOffset = TweenPathData.StartOffset;

            _startPosition += startOffset;
        }

        public void SetDuration(float duration)
        {
            TweenPathData.Duration = duration;
        }

        public void ResetDuration()
        {
            TweenPathData.ResetDuration();
        }

        public void SetDestination(Vector3 destination)
        {
            TweenPathData.Destination = destination;
        }

        public void PlayTween()
        {
            UpdateTweenData();

            TweenInstance = CreateTween();

            ResetTransform();

            if (TweenPathData.StartDelay > 0)
                StartCoroutine(PlayTweenWithDelay(TweenPathData.StartDelay));
            else
                TweenInstance.Play();
        }

        public void PauseTween()
        {
            if (TweenInstance.IsPlaying())
                TweenInstance.Pause();
        }

        IEnumerator PlayTweenWithDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            if (!TweenInstance.IsPlaying())
                TweenInstance.Play();
        }

        public void ResetTransform()
        {
            TargetObject.transform.localPosition = _startPosition;
            TargetObject.transform.localRotation = Quaternion.Euler(_startRotation);
        }

        private Tween CreateTween()
        {
            if (TweenInstance != null)
                TweenInstance.Kill();

            Vector3[] path = TweenPathData.GetPath();
            float duration = TweenPathData.Duration;
            Ease easeSetting = TweenPathData.EaseSetting;
            int loops = TweenPathData.Loops;
            LoopType loopSetting = TweenPathData.LoopSetting;
            PathType pathType = TweenPathData.GetPathType();
            PathMode pathMode = TweenPathData.GetPathMode();

            Tween tween = null;

            switch (TweenPathData.Scope)
            {
                case TweenAnimScope.World:
                    tween = CreatePathTween(path, duration, easeSetting, loops, loopSetting, pathType, pathMode);
                    break;
                case TweenAnimScope.Local:
                    tween = CreateLocalPathTween(path, duration, easeSetting, loops, loopSetting, pathType, pathMode);
                    break;
            }

            return tween;
        }

        public Tween GetTween()
        {
            return TweenInstance;
        }


        private Tween CreatePathTween(Vector3[] path, float duration, Ease easeSetting, int loops, LoopType loopSetting, PathType pathType = PathType.CatmullRom, PathMode pathMode = PathMode.Full3D)
        {

            var tween = StartTransform.DOPath(path, duration, pathType, pathMode, 10, Color.green).SetEase(easeSetting).SetLoops(loops, loopSetting).SetAutoKill(false);
            tween.Pause();

            return tween;
        }

        private Tween CreateLocalPathTween(Vector3[] path, float duration, Ease easeSetting, int loops, LoopType loopSetting, PathType pathType = PathType.CatmullRom, PathMode pathMode = PathMode.Full3D)
        {

            var tween = StartTransform.DOLocalPath(path, duration, pathType, pathMode, 10, Color.green).SetEase(easeSetting).SetLoops(loops, loopSetting).SetAutoKill(false);
            tween.Pause();

            return tween;
        }
    }
}