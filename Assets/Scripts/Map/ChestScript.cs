using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enums;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    private GameObject[] _allGuns;

    private bool _isOpen = false;

    private void Start()
    {
        _allGuns = Resources.LoadAll<GameObject>("Guns");
    }

    private void OpenChest()
    {
        int i = 0;
        while (i < _allGuns.Length)
        {
            var random = Random.Range(0, 100);
            if (_allGuns[i].TryGetComponent(out RifleScript rifleScript))
            {
                if ((rifleScript.RarityItem == RarityItems.Common && random >= 30 && random <= 65) ||
                    (rifleScript.RarityItem == RarityItems.Rare && random > 65 && random <= 85) ||
                    (rifleScript.RarityItem == RarityItems.Epic && random > 85 && random <= 95) ||
                    (rifleScript.RarityItem == RarityItems.Legendary && random > 95))
                {
                    Instantiate(_allGuns[i], new Vector2(transform.position.x, transform.position.y - 0.5f), Quaternion.identity);
                    break;
                }
            }

            i++; 
        }

        GameManagerScript.Instance.SpawnOtherItems(OtherTypeItems.Coin, 3, transform.position);
        GameManagerScript.Instance.SpawnOtherItems(OtherTypeItems.Health, 3, transform.position);
        GameManagerScript.Instance.SpawnOtherItems(OtherTypeItems.Energy, 7, transform.position);

        StartCoroutine(HideChest());
    }

    private IEnumerator HideChest()
    {
        var chestSprite = GetComponent<SpriteRenderer>().color;
        yield return new WaitForSeconds(1f);

        while (chestSprite.a > 0f)
        {
            var alpha = chestSprite;
            alpha.a -= Time.deltaTime;
            chestSprite = alpha;

            yield return null;
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !_isOpen)
        {
            _isOpen = true;

            OpenChest();
        }
    }
}
