using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace NGS.AdvancedCullingSystem.Static
{
    public class CustomStaticCullingSourceStrategy : IStaticCullingSourceStrategy
    {
        public Bounds LocalBounds
        {
            get
            {
                return _localBounds;
            }
            set
            {
                _localBounds = value;
            }
        }

        [SerializeField]
        private GameObject _context;

        [SerializeField]
        private bool _isOccluder;

        [SerializeField]
        private Bounds _localBounds;

        [SerializeField]
        private UnityEvent<CullingTarget> _onVisible;

        [SerializeField]
        private UnityEvent<CullingTarget> _onInvisible;

        [SerializeField]
        private List<Collider> _colliders;

        [SerializeField]
        private List<Collider> _createdColliders;


        public CustomStaticCullingSourceStrategy(GameObject context)
        {
            _context = context;

            CustomCullingTarget target = context.GetComponent<CustomCullingTarget>();

            if (target != null)
            {
                _localBounds = new Bounds(
                    target.Bounds.center - target.transform.position,
                    target.Bounds.size);

                _onVisible = target.OnVisible;
                _onInvisible = target.OnInvisible;
                _isOccluder = target.IsOccluder;
            }
            else
            {
                _localBounds = new Bounds(Vector3.zero, Vector3.one * 3);
                _isOccluder = false;
            }

            _colliders = new List<Collider>();
            _createdColliders = new List<Collider>();
        }

        public bool TryGetBounds(out Bounds bounds)
        {
            bounds = _localBounds;
            bounds.center += _context.transform.position;

            return true;
        }

        public bool Validate(out string errorMessage)
        {
            if (_onVisible == null && _onInvisible == null)
            {
                errorMessage = "Visible and Invisible actions not assigned";
                return false;
            }

            if (_isOccluder)
            {
                if (_colliders == null || _colliders.Count == 0)
                {
                    errorMessage = "Source marked as occluder but colliders not assigned";
                    return false;
                }
            }

            errorMessage = "";
            return true;
        }

        public CullingTarget CreateCullingTarget()
        {
            CustomCullingTarget cullingTarget = _context.AddComponent<CustomCullingTarget>();

            TryGetBounds(out Bounds bounds);

            cullingTarget.Bounds = bounds;
            cullingTarget.SetActions(_onVisible, _onInvisible);
            cullingTarget.IsOccluder = _isOccluder;

            return cullingTarget;
        }

        public void PrepareForBaking()
        {
            if (!_isOccluder)
                return;

            if (_colliders == null || _colliders.Count == 0)
                return;

            _createdColliders.Clear();

            for (int i = 0; i < _colliders.Count; i++)
            {
                _createdColliders.Add(CreateCollider(_colliders[i]));
            }
        }

        public void ClearAfterBaking()
        {
            if (_createdColliders == null || _createdColliders.Count == 0)
                return;

            for (int i = 0; i < _createdColliders.Count; i++)
                Object.Destroy(_createdColliders[i].gameObject);

            _createdColliders.Clear();
        }


        public Collider CreateCollider(Collider source)
        {
            Collider collider = Object.Instantiate(source);
            GameObject colliderGo = collider.gameObject;

            colliderGo.layer = StaticCullingPreferences.Layer;
            colliderGo.transform.parent = _context.transform;
            colliderGo.transform.localPosition = Vector3.zero;
            colliderGo.transform.localEulerAngles = Vector3.zero;
            colliderGo.transform.localScale = Vector3.one;

            return collider;
        }
    }
}
