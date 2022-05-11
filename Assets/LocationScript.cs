using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

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
        CloseLocation();

        var enemies = Resources.LoadAll<GameObject>("Characters/Enemies/Default");
        var allSpawnPoints = _spawnPoints.GetComponentsInChildren<Transform>();
        
        for (int i = 0; i < allSpawnPoints.Length; i++)
        {
            var enemy = Instantiate(enemies[Random.Range(0, enemies.Length)],
                        allSpawnPoints[i].position,
                        Quaternion.identity);
            enemy.GetComponent<EnemyScript>().player = _player;

            _quantityEnemies++;
        }
    }

    private void SpawnChest()
    {
        var allSpawnChest = _spawnChest.GetComponentsInChildren<Transform>();
        Instantiate(Resources.Load("ItemsFromChests/Chest"), allSpawnChest[Random.Range(0, allSpawnChest.Length)].position, Quaternion.identity);
    }

    private void CloseLocation()
    {
        var walls = GetComponentsInChildren<Tilemap>()[1];
        walls.CompressBounds();

        walls.SetTile(new Vector3Int(1, -1, 0), null);
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