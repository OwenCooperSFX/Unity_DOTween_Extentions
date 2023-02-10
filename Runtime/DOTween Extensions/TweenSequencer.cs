using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace OCSFX.DoTweenExtensions
{
    public class TweenSequencer : MonoBehaviour
    {
        public List<TweenData> tweenDataList;

        private Sequence _sequence;

        private bool _sequenceTriggered;
        public bool SequenceTriggered => _sequenceTriggered;

        private void Awake()
        {
            _sequence = DOTween.Sequence();
            BuildSequence();
        }

        private void BuildSequence()
        {
            _sequence.Pause();
            _sequence.SetAutoKill(false);

            foreach (TweenData tweenData in tweenDataList)
            {
                if (tweenData.join)
                    _sequence.Join(tweenData.GetTween());
                else
                    _sequence.Append(tweenData.GetTween());
            }
        }

        public void PlaySequence()
        {
            if (!_sequenceTriggered)
            {
                PlayForward();
            }
            else
            {
                PlayBackwards();
            }
        }

        public void PlayForward()
        {
            _sequence.PlayForward();
            _sequenceTriggered = true;
        }

        public void PlayBackwards()
        {
            _sequence.PlayBackwards();
            _sequenceTriggered = false;
        }

        public Sequence GetSequence()
        {
            return _sequence;
        }
    }
}
