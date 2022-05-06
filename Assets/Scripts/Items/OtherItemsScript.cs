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
                    PlayerInfoScript.Instance.UpdateHeal(1);
                    break;
                case OtherTypeItems.Energy:
                    PlayerInfoScript.Instance.UpdateEnergy(5);
                    break;
                case OtherTypeItems.Coin:
                    PlayerInfoScript.Instance.UpdateCoins(1);
                    break;
            }

            Destroy(gameObject);
        }
    }
}