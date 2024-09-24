using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace NGS.AdvancedCullingSystem.Static
{
    public class CustomCullingTarget : CullingTarget
    {
        public UnityEvent<CullingTarget> OnVisible
        {
            get
            {
                return _onVisible;
            }
        }
        public UnityEvent<CullingTarget> OnInvisible
        {
            get
            {
                return _onInvisible;
            }
        }

        [field : SerializeField, HideInInspector]
        public bool IsOccluder { get; set; }

        [SerializeField]
        private UnityEvent<CullingTarget> _onVisible;

        [SerializeField]
        private UnityEvent<CullingTarget> _onInvisible;

        public void SetActions(UnityEvent<CullingTarget> onVisible, UnityEvent<CullingTarget> onInvisible)
        {
            _onVisible = onVisible;
            _onInvisible = onInvisible;
        }

        protected override void MakeVisible()
        {
            _onVisible.Invoke(this);
        }

        protected override void MakeInvisible()
        {
            _onInvisible.Invoke(this);
        }
    }
}
