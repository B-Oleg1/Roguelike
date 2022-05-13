using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class LocationScript : MonoBehaviour
{
    [SerializeField] private GameObject _boss;
    [SerializeField] private GameObject _spawnPoints;
    [SerializeField] private GameObject _spawnChest;

    private GameObject _player;
    private GameObject[] _enemies;

    private int _quantityEnemies = 0;
    private float _cooldownSpawn = 0;
    private bool[] _cutWalls = new bool[4];
    private bool _isActivated = false;
    private bool _isFinished = false;
    private bool _bossSpawned = false;

    private void Start()
    {
        _enemies = Resources.LoadAll<GameObject>("Characters/Enemies/Default");

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

    private void Update()
    {
        if (_isActivated)
        {
            if (_cooldownSpawn > 0)
            {
                _cooldownSpawn -= Time.deltaTime;
            }

            if (_boss != null && _cooldownSpawn <= 0 && !_isFinished)
            {
                _cooldownSpawn = 15;
                SpawnEnemy();
            }
        }
    }

    private void SpawnEnemy()
    {
        ChangeWalls(true);

        var allSpawnPoints = _spawnPoints.GetComponentsInChildren<Transform>();
        
        for (int i = 0; i < allSpawnPoints.Length; i++)
        {
            var enemy = Instantiate(_enemies[Random.Range(0, _enemies.Length)],
                        allSpawnPoints[i].position,
                        Quaternion.identity);
            enemy.GetComponent<EnemyScript>().player = _player;

            _quantityEnemies++;
        }

        if (_boss != null && !_bossSpawned)
        {
            _bossSpawned = true;
            _cooldownSpawn = 15;

            var boss = Instantiate(_boss, _spawnPoints.transform.GetChild(Random.Range(0, _spawnPoints.transform.childCount)).position, Quaternion.identity);
            boss.GetComponent<EnemyScript>().player = _player;

            _quantityEnemies++;
        }
    }

    private void SpawnChest()
    {
        Instantiate(Resources.Load("ItemsFromChests/Chest"), _spawnChest.transform.position, Quaternion.identity);
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
                PlayerInfoScript.Instance.UpgradesPoints++;
                ChangeWalls(false);
                SpawnChest();

                StartCoroutine(LoadNewGame());
            }
        }
    }

    private IEnumerator LoadNewGame()
    {
        PlayerPrefs.SetInt("MaxHealth", PlayerInfoScript.Instance.MaxHealth);
        PlayerPrefs.SetInt("MaxEnergy", PlayerInfoScript.Instance.MaxEnergy);
        PlayerPrefs.SetInt("Coins", PlayerInfoScript.Instance.Coins);
        PlayerPrefs.SetInt("UpgradesPoints", PlayerInfoScript.Instance.UpgradesPoints);

        yield return new WaitForSeconds(7.5f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}