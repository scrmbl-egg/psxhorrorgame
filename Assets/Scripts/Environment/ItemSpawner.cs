using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header( "Properties" )]
    //
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private int poolCapacity;
    private Queue<GameObject> _itemPool = new Queue<GameObject>();

    private ParticleSystem _particleSystem;

    #region MonoBehaviour

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    #endregion

    #region Public methods

    public void SpawnItem()
    {
        GameObject newItem = Instantiate( itemPrefab, transform.position, Quaternion.identity );
        bool poolIsFull = _itemPool.Count == poolCapacity;

        if (poolIsFull)
        {
            Destroy( _itemPool.Peek() );
            _itemPool.Dequeue();
        }

        _itemPool.Enqueue( newItem );
        _particleSystem.Play();
    }

    #endregion
}