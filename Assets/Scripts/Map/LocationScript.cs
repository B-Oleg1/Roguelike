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

    private bool[] _cutWalls = new bool[4];
    private int _quantityEnemies = 0;
    private bool _isActivated = false;
    private bool _isFinished = false;

    private void Start()
    {
        var walls = GetComponentsInChildren<Tilemap>()[1];
        walls.CompressBounds();

        if (walls.GetTile(new Vector3Int((walls.size.x - 1) / 2, -1, 0)) == null)
        {
            _cutWalls[0] = true;
        }
        if (walls.GetTile(new Vector3Int(walls.size.x - 1, -(walls.size.y - 1) / 2 - 1, 0)) == null)
        {
            _cutWalls[1] = true;
        }
        if (walls.GetTile(new Vector3Int((walls.size.x - 1) / 2, -walls.size.y, 0)) == null)
        {
            _cutWalls[2] = true;
        }
        if (walls.GetTile(new Vector3Int(0, -(walls.size.y - 1) / 2 - 1, 0)) == null)
        {
            _cutWalls[3] = true;
        }
    }

    private void SpawnEnemy()
    {
        ChangeWalls(true);

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

    private void ChangeWalls(bool closeWalls)
    {
        var walls = GetComponentsInChildren<Tilemap>()[1];
        walls.CompressBounds();

        TileBase wallSprite = null;
        if (_cutWalls[0])
        {
            wallSprite = closeWalls ? walls.GetTile(new Vector3Int((walls.size.x - 1) / 2 - 2, -1, 0)) : null;
            walls.SetTile(new Vector3Int((walls.size.x - 1) / 2 - 1, -1, 0), wallSprite);
            walls.SetTile(new Vector3Int((walls.size.x - 1) / 2, -1, 0), wallSprite);
            walls.SetTile(new Vector3Int((walls.size.x - 1) / 2 + 1, -1, 0), wallSprite);
        }
        if (_cutWalls[1])
        {
            wallSprite = closeWalls ? walls.GetTile(new Vector3Int(walls.size.x - 1, -(walls.size.y - 1) / 2 + 1, 0)) : null;
            walls.SetTile(new Vector3Int(walls.size.x - 1, -(walls.size.y - 1) / 2, 0), wallSprite);
            walls.SetTile(new Vector3Int(walls.size.x - 1, -(walls.size.y - 1) / 2 - 1, 0), wallSprite);
            walls.SetTile(new Vector3Int(walls.size.x - 1, -(walls.size.y - 1) / 2 - 2, 0), wallSprite);
        }
        if (_cutWalls[2])
        {
            wallSprite = closeWalls ? walls.GetTile(new Vector3Int((walls.size.x - 1) / 2 - 2, -walls.size.y, 0)) : null;
            walls.SetTile(new Vector3Int((walls.size.x - 1) / 2 - 1, -walls.size.y, 0), wallSprite);
            walls.SetTile(new Vector3Int((walls.size.x - 1) / 2, -walls.size.y, 0), wallSprite);
            walls.SetTile(new Vector3Int((walls.size.x - 1) / 2 + 1, -walls.size.y, 0), wallSprite);
        }
        if (_cutWalls[3])
        {
            wallSprite = closeWalls ? walls.GetTile(new Vector3Int(0, -(walls.size.y - 1) / 2 + 1, 0)) : null;
            walls.SetTile(new Vector3Int(0, -(walls.size.y - 1) / 2, 0), wallSprite);
            walls.SetTile(new Vector3Int(0, -(walls.size.y - 1) / 2 - 1, 0), wallSprite);
            walls.SetTile(new Vector3Int(0, -(walls.size.y - 1) / 2 - 2, 0), wallSprite);
        }
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print(collision.gameObject.name);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !_isFinished) 
        {
            _quantityEnemies--;
            print(_quantityEnemies);
            if (_quantityEnemies <= 0)
            {
                _isFinished = true;
                ChangeWalls(false);
                SpawnChest();
            }
        }
    }
}