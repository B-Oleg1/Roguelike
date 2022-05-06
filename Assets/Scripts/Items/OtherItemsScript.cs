using Assets.Scripts.Enums;
using UnityEngine;

public class OtherItemsScript : MonoBehaviour
{
    [SerializeField] private OtherTypeItems _otherTypeItems;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            switch (_otherTypeItems)
            {
                case OtherTypeItems.Heal:
                    PlayerInfoScript.Instantiate.UpdateHeal(1);
                    break;
                case OtherTypeItems.Energy:
                    PlayerInfoScript.Instantiate.UpdateEnergy(5);
                    break;
                case OtherTypeItems.Coin:
                    PlayerInfoScript.Instantiate.UpdateCoins(1);
                    break;
            }

            Destroy(gameObject);
        }
    }
}