using Assets.Scripts.Enums;
using UnityEngine;

public class OtherItemsScript : MonoBehaviour
{
    [SerializeField] private OtherTypeItems _otherTypeItems;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            bool takeItem = false;

            switch (_otherTypeItems)
            {
                case OtherTypeItems.Health:
                    takeItem = PlayerInfoScript.Instance.UpdateHeal(5);
                    break;
                case OtherTypeItems.Energy:
                    takeItem = PlayerInfoScript.Instance.UpdateEnergy(10);
                    break;
                case OtherTypeItems.Coin:
                    PlayerInfoScript.Instance.UpdateCoins(1);
                    takeItem = true;
                    break;
            }

            if (takeItem)
            {
                Destroy(gameObject);
            }
        }
    }
}