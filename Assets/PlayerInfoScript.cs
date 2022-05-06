using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoScript : MonoBehaviour
{
    public static PlayerInfoScript Instantiate { get; private set; }

    public int MaxHealth { get; }
    public int MaxEnergy { get; }
    public int Health { get; private set; }
    public int Energy { get; private set; }
    public int Coins { get; private set; }

    private void Start()
    {
        if (Instantiate == null)
        {
            Instantiate = this;
        }
    }

    public void UpdateHeal(int quantity)
    {
        if (Health + quantity > 100)
        {
            Health = 100;
        }
        else if (Health - quantity <= 0)
        {
            // Game over
        }
        else
        {
            Health += quantity;
        }
    }
    
    public void UpdateEnergy(int quantity)
    {
        if (Energy + quantity > 100)
        {
            Energy = 100;
        }
        else
        {
            Health += quantity;
        }
    }
    
    public void UpdateCoins(int quantity)
    {
        Coins += quantity;
    }
}