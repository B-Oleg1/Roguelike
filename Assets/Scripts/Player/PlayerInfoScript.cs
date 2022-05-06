using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoScript : MonoBehaviour
{
    public static PlayerInfoScript Instance { get; private set; }

    [SerializeField] private Slider _healthSlider;
    [SerializeField] private Text _healthText;
    [SerializeField] private Slider _energySlider;
    [SerializeField] private Text _energyText;
    [SerializeField] private Text _coinsText;

    public int Health { get; private set; }
    public int Energy { get; private set; }
    public int Coins { get; private set; }

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        Health = 100;
        Energy = 100;
        Coins = 0;

        UpdateHeal(0);
        UpdateEnergy(0);
        UpdateCoins(0);
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

        _healthSlider.value = Health / 100f;
        _healthText.text = Health.ToString();
    }
    
    public void UpdateEnergy(int quantity)
    {
        if (Energy + quantity > 100)
        {
            Energy = 100;
        }
        else
        {
            Energy += quantity;
        }

        _energySlider.value = Energy / 100f;
        _energyText.text = Energy.ToString();
    }
    
    public void UpdateCoins(int quantity)
    {
        Coins += quantity;

        _coinsText.text = Coins.ToString();
    }
}