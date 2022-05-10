using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LocationScript : MonoBehaviour
{
    [SerializeField] private GameObject _spawnPoints;
    [SerializeField] private GameObject _spawnChest;

    private GameObject _player;

    private int _quantityEnemies = 0;
    private bool _isActivated = false;
    private bool _isFinished = false;
    
    private void SpawnEnemy()
    {
        var enemies = Resources.LoadAll<GameObject>("Characters/Enemies/Default");
        var allSpawnPoints = _spawnPoints.GetComponentsInChildren<GameObject>();
        
        for (int i = 0; i < allSpawnPoints.Length; i++)
        {
            var enemy = Instantiate(enemies[Random.Range(0, enemies.Length)],
                        allSpawnPoints[i].transform.position,
                        Quaternion.identity);
            enemy.GetComponent<EnemyScript>().player = _player;

            _quantityEnemies++;
        }
    }

    private void SpawnChest()
    {
        var allSpawnChest = _spawnChest.GetComponentsInChildren<GameObject>();
        Instantiate(Resources.Load("ItemsFromChests/Chest"), allSpawnChest[Random.Range(0, allSpawnChest.Length)].transform.position, Quaternion.identity);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !_isActivated)
        {
            _isActivated = true;
            _player = collision.gameObject;
            SpawnEnemy();
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !_isFinished)
        {
            _quantityEnemies--;
            if (_quantityEnemies <= 0)
            {
                _isFinished = true;
                SpawnChest();
            }
        }
    }
}