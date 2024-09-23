#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RandomTreeSpawn : MonoBehaviour
{
    
    [SerializeField] private Vector2 _spawnArea;
    [SerializeField] private List<GameObject> _gameObjectPrefabs;
    [SerializeField] private Transform _parent;
    [SerializeField] private int _spawnValue;
    [SerializeField] private float _yAxis;
    private Vector3 _randomSpawnArea;
    private float _xAxis;
    private float _zAxis;
    public Vector3 _offset;

    [ContextMenu("Spawn")]    
    public void SpawnTree()
    {
        for (int index = 0; index < _spawnValue; index++)
        {
            RaycastHit hit;
            int randomIndex = Random.Range(0, _gameObjectPrefabs.Count);   
            _xAxis = Random.Range(-_spawnArea.x, _spawnArea.x);
            _zAxis = Random.Range(-_spawnArea.y, _spawnArea.y);
            _randomSpawnArea = new Vector3(_xAxis + _parent.transform.position.x, _yAxis, _zAxis + _parent.transform.position.z);
            if (Physics.Raycast(_randomSpawnArea, Vector3.down, out hit, Mathf.Infinity))
            {
                //Instantiate(_gameObjectPrefabs[randomIndex], hit.point + offset, Quaternion.identity, _parent);
                GameObject prefab = (GameObject)PrefabUtility.InstantiatePrefab(_gameObjectPrefabs[randomIndex]);
                prefab.transform.position = hit.point;
                prefab.transform.up = hit.normal;
                prefab.transform.SetParent(_parent);
            }
        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(_parent.position, new Vector3(_spawnArea.x * 2 , _yAxis, _spawnArea.y * 2));   
    }
}
#endif
