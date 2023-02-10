using UnityEngine;
using DG.Tweening;

namespace OCSFX.DoTweenExtensions
{
    [CreateAssetMenu(menuName = "TweenPathData")]
    public class TweenDataSO_Path : TweenDataSO
    {
        [Header("Paths")]
        [SerializeField] private TweenAnimScope _scope = new TweenAnimScope();
        public TweenAnimScope Scope => _scope;

        [SerializeField] private Vector3[] _path = null;

        [SerializeField] private PathType _pathType = PathType.Linear;
       
        [SerializeField] private PathMode _pathMode = PathMode.Ignore;


        public Vector3[] GetPath() => _path;
        public PathType GetPathType() => _pathType;
        public PathMode GetPathMode() => _pathMode;
    }
}
