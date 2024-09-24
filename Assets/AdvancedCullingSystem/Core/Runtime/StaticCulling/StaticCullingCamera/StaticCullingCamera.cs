using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Static
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Camera))]
    public class StaticCullingCamera : MonoBehaviour
    {
        [SerializeField]
        private bool _drawCells;

        [Range(0, 1)]
        [SerializeField]
        private float _tolerance = 1f;

        private VisibilityTree _tree;
        private CameraZone[] _cameraZones;


        private void Start()
        {
            SceneUpdated();

            if (_cameraZones.Length == 0)
            {
                Debug.Log("StaticCullingCamera : Not found Camera Zones");
                enabled = false;
                return;
            }

            _tree = FindNearestVisibilityTree();

            if (_tree == null)
            {
                Debug.Log("StaticCullingCamera : Can't find nearest CameraZone");
                enabled = false;
                return;
            }
        }

        private void Update()
        {
            Vector3 point = transform.position;

            if (_tree == null || !_tree.Root.Bounds.Contains(point))
            {
                _tree = FindNearestVisibilityTree();

                if (_tree == null)
                    return;
            }
            
            _tree.SetVisible(point, _tolerance);
        }


        public void SceneUpdated()
        {
            _cameraZones = FindObjectsOfType<CameraZone>();
        }

        private VisibilityTree FindNearestVisibilityTree()
        {
            Vector3 point = transform.position;

            foreach (var zone in _cameraZones)
            {
                if (zone == null)
                    continue;

                VisibilityTree tree = zone.VisibilityTree;

                if (tree == null || tree.CullingTargets == null)
                    continue;

                if (tree.Root.Bounds.Contains(point))
                    return tree;
            }

            return null;
        }


#if UNITY_EDITOR

        public static bool DrawGizmo;

        private void OnDrawGizmos()
        {
            if (DrawGizmo)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireCube(transform.position, Vector3.one);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (!_drawCells || _tree == null)
                return;
            
            _tree.DrawCellsGizmo(transform.position, _tolerance);
        }

#endif
    }
}
