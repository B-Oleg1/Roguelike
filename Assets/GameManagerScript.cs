using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript Instance { get; private set; }

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void SpawnOtherItems(OtherTypeItems typeItem, int maxQuantity, Vector2 spawnPosition)
    {
        var quantityItems = Random.Range(0, maxQuantity);
        for (int i = 0; i < quantityItems; i++)
        {
            Instantiate(Resources.Load<GameObject>($"ItemsFromChests/{typeItem}"),
                        new Vector2(spawnPosition.x + Random.Range(-1f, 1f),
                                    spawnPosition.y - Random.Range(0f, 0.75f)),
                        Quaternion.identity);
        }
    }
}
