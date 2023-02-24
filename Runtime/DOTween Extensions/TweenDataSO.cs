using UnityEngine;
using DG.Tweening;

public enum TweenAnimType { Scale, Move, Rotate }
public enum TweenAnimScope { Local, World }

[CreateAssetMenu(menuName = "TweenData")]
public class TweenDataSO : ScriptableObject
{
    [SerializeField] private TweenAnimType _tweenAnimType;
    public TweenAnimType TweenAnimType => _tweenAnimType;

    [Tooltip("Offset from the starting local scale, position, or rotation Vector3.")]
    [SerializeField] private Vector3 _startOffset = Vector3.zero;
    [Tooltip("Ending local scale, position, or rotation Vector3.")]
    [SerializeField] private Vector3 _destination = Vector3.zero;
    [Tooltip("Time in seconds for tween to complete one loop.")]
    [SerializeField] private float _duration = 1f;
    private float _defaultDuration = 1f;
    [Tooltip("Converts the duration value instead into a units/second speed.")]
    [SerializeField] private bool _convertDurationToSpeed = false;

    public bool ConvertDurationToSpeed => _convertDurationToSpeed;

    public Vector3 StartOffset => _startOffset;
    public Vector3 Destination { get => _destination; set => _destination = value; }
    public float Duration { get => _duration; set => _duration = value; }

    [SerializeField] private Ease _easeSetting = Ease.Unset;
    [SerializeField] private LoopType _loopSetting = LoopType.Restart;
    public Ease EaseSetting => _easeSetting;
    public LoopType LoopSetting => _loopSetting;

    [Tooltip("Set to -1 for infinite looping")]
    [SerializeField] private int _loops = 0;
    [SerializeField] private float _startDelay = 0f;
    [SerializeField] private bool _playOnEnable = false;
    [SerializeField] private bool _resetOnDisable = true;
    public int Loops => _loops;
    public float StartDelay => _startDelay;
    public bool PlayOnEnable => _playOnEnable;
    public bool ResetOnDisable => _resetOnDisable;

    private void OnEnable()
    {
        _defaultDuration = _duration;
    }

    public void ResetDuration()
    {
        _duration = _defaultDuration;
    }
}
