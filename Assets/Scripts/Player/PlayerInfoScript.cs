using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerInfoScript : MonoBehaviour
{
    public static PlayerInfoScript Instance { get; private set; }

    [SerializeField] private Slider _healthSlider;
    [SerializeField] private Text _healthText;
    [SerializeField] private Slider _energySlider;
    [SerializeField] private Text _energyText;
    [SerializeField] private Text _coinsText;

    public int MaxHealth { get; private set; }
    public int MaxEnergy { get; private set; }
    public int Health { get; private set; }
    public int Energy { get; private set; }
    public int Power { get; private set; }
    public int Coins { get; private set; }

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        MaxHealth = 100;
        MaxEnergy = 100;

        Health = MaxHealth;
        Energy = MaxEnergy;
        Coins = 0;

        UpdateHeal(0);
        UpdateEnergy(0);
        UpdateCoins(0);
    }

    public bool UpdateHeal(int quantity)
    {
        bool take = true;

        if (quantity > 0 && Health >= MaxHealth)
        {
            take = false;
        }
        else if (Health + quantity <= 0)
        {
            // Game over
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            Health += quantity;
        }

        _healthSlider.value = Health / (float)MaxHealth;
        _healthText.text = Health.ToString();

        return take;
    }
    
    public bool UpdateEnergy(int quantity)
    {
        bool take = true;

        if (quantity > 0 && Energy >= MaxEnergy)
        {
            take = false;
        }
        else if (Energy + quantity >= MaxEnergy)
        {
            Energy = MaxEnergy;
        }
        else
        {
            Energy += quantity;
        }

        _energySlider.value = Energy / (float)MaxEnergy;
        _energyText.text = Energy.ToString();

        return take; 
    }
    
    public void UpdateCoins(int quantity)
    {
        Coins += quantity;

        _coinsText.text = Coins.ToString();
    }
}