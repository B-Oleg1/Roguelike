using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enums;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    private bool _isOpen = false;

    private void OpenChest()
    {
        var allGuns = Resources.LoadAll<GameObject>("Guns");
        
        int i = 0;
        while (i < allGuns.Length)
        {
            var random = Random.Range(0, 100);
            if (allGuns[i].TryGetComponent(out RifleScript rifleScript))
            {
                if ((rifleScript.RarityItem == RarityItems.Common && random >= 30 && random <= 65) ||
                    (rifleScript.RarityItem == RarityItems.Rare && random > 65 && random <= 85) ||
                    (rifleScript.RarityItem == RarityItems.Epic && random > 85 && random <= 95) ||
                    (rifleScript.RarityItem == RarityItems.Rare && random > 95))
                {
                    var newItem = Instantiate(allGuns[i], 
                                            new Vector2(transform.position.x, transform.position.y - 0.5f),
                                            Quaternion.identity);
                    break;
                }
            }

            i++; 
        }

        SpawnOtherItems("Coin", 3);
        SpawnOtherItems("Health", 1);
        SpawnOtherItems("Energy", 5);
    }

    private void SpawnOtherItems(string itemName, int maxQuantity)
    {
        var quantityCoins = Random.Range(0, maxQuantity);
        for (int j = 0; j < quantityCoins; j++)
        {
            Instantiate(Resources.Load<GameObject>($"ItemsFromChests/{itemName}"),
                new Vector2(transform.position.x + Random.Range(-1f, 1f),
                    transform.position.y - Random.Range(0f, 1f)),
                Quaternion.identity);
        }
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
