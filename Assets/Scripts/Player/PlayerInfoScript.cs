using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerInfoScript : MonoBehaviour
{
    public static PlayerInfoScript Instance { get; private set; }

    [SerializeField] private GameObject _upgradesPanel;
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
    public int UpgradesPoints { get; set; }

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        if (!PlayerPrefs.HasKey("MaxHealth"))
        {
            PlayerPrefs.SetInt("MaxHealth", 100);
        }
        if (!PlayerPrefs.HasKey("MaxEnergy"))
        {
            PlayerPrefs.SetInt("MaxEnergy", 100);
        }
        if (!PlayerPrefs.HasKey("Coins"))
        {
            PlayerPrefs.SetInt("Coins", 0);
        }
        if (!PlayerPrefs.HasKey("UpgradesPoints"))
        {
            PlayerPrefs.SetInt("UpgradesPoints", 0);
        }

        MaxHealth = PlayerPrefs.GetInt("MaxHealth");
        MaxEnergy = PlayerPrefs.GetInt("MaxEnergy");
        Coins = PlayerPrefs.GetInt("Coins");
        UpgradesPoints = PlayerPrefs.GetInt("UpgradesPoints", 0);

        Health = MaxHealth;
        Energy = MaxEnergy;

        UpdateHeal(0);
        UpdateEnergy(0);
        UpdateCoins(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            _upgradesPanel.SetActive(!_upgradesPanel.activeInHierarchy);
        }
        print(UpgradesPoints + " " + MaxHealth);
    }

    public void UpgradeSkills(string name)
    {
        if (name == "PlusHealth" && UpgradesPoints > 0)
        {
            UpgradesPoints--;
            MaxHealth += 10;
            UpdateHeal(0);
        }
        else if (name == "MinusHealth")
        {
            UpgradesPoints++;
            MaxHealth -= 10;
            UpdateHeal(0);
        }
        else if (name == "PlusEnergy" && UpgradesPoints > 0)
        {
            UpgradesPoints--;
            MaxEnergy += 10;
            UpdateEnergy(0);
        }
        else if (name == "MinusEnergy")
        {
            UpgradesPoints++;
            MaxEnergy -= 10;
            UpdateEnergy(0);
        }
        else if (name == "PlusPower" && UpgradesPoints > 0)
        {
            UpgradesPoints--;
            Power += 3;
        }
        else if (name == "MinusPower")
        {
            UpgradesPoints++;
        }

        if (UpgradesPoints > 0)
        {

        }
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
            PlayerPrefs.SetInt("MaxHealth", 100);
            PlayerPrefs.SetInt("MaxEnergy", 100);
            PlayerPrefs.SetInt("Coins", 0);
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