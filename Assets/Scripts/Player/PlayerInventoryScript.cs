using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class PlayerInventoryScript : MonoBehaviour
{
    [SerializeField] private GameObject[] _inventoryUI;
    [SerializeField] private Transform _rightHand;

    private GameObject[] _guns;
    private GameObject _currentGun;

    private GameObject _takeItem;

    private void Start()
    {
        _guns = new GameObject[3]; 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (_currentGun != _guns[0] && _guns[0] != null) 
            {
                _currentGun?.SetActive(false);
                _currentGun = _guns[0];
                _currentGun.SetActive(true);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (_currentGun != _guns[1] && _guns[1] != null)
            {
                _currentGun?.SetActive(false);
                _currentGun = _guns[1];
                _currentGun.SetActive(true);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (_currentGun != _guns[2] && _guns[2] != null)
            {
                _currentGun?.SetActive(false);
                _currentGun = _guns[2];
                _currentGun.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && _takeItem != null)
        {
            TakeItem();
        }

        if (Input.GetKeyDown(KeyCode.G) && _currentGun != null)
        {
            DropItem(_currentGun);
        }
    }

    private void TakeItem()
    {
        var itemObject = _takeItem;

        var item = (IItem)itemObject.GetComponent(typeof(IItem));
        if (item.TypeItem == TypeItems.Gun)
        {
            var gunItem = (IGun)itemObject.GetComponent(typeof(IGun));
            var gunId = (int)gunItem.TypeGun;

            if (_guns[gunId] != null)
            {
                DropItem(_guns[gunId]);
            }
            
            if (_currentGun != null)
            {
                _currentGun.SetActive(false);
            }

            itemObject.transform.SetParent(_rightHand);
            itemObject.transform.localPosition = new Vector3(0, 0, 0);

            foreach (var script in itemObject.GetComponents<MonoBehaviour>())
            {
                script.enabled = true;
            }

            itemObject.transform.GetChild(itemObject.transform.childCount - 1).gameObject.SetActive(false);

            _inventoryUI[gunId].transform.GetChild(0).GetComponent<Text>().text = itemObject.name.Substring(0, itemObject.name.Length - 7);
            _inventoryUI[gunId].transform.GetChild(1).GetComponent<Image>().sprite = itemObject.GetComponent<SpriteRenderer>().sprite;
            _inventoryUI[gunId].transform.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1, 100);
            _inventoryUI[gunId].transform.GetChild(2).GetChild(1).GetComponent<Text>().text = itemObject.GetComponent<RifleScript>().PriceBullet.ToString();

            _guns[gunId] = itemObject; 
            _currentGun = _guns[gunId];
        }
    }

    private void DropItem(GameObject item)
    {
        if (_currentGun == item)
        {
            _currentGun = null;
        }

        var gunId = (int)item.GetComponent<RifleScript>().TypeGun;
        _inventoryUI[gunId].transform.GetChild(0).GetComponent<Text>().text = string.Empty;
        _inventoryUI[gunId].transform.GetChild(1).GetComponent<Image>().sprite = null;
        _inventoryUI[gunId].transform.GetChild(1).GetComponent<Image>().color = new Color(0, 0, 0, 0);
        _inventoryUI[gunId].transform.GetChild(2).GetChild(1).GetComponent<Text>().text = "0";

        foreach (var script in item.GetComponents<MonoBehaviour>())
        {
            script.enabled = false;
        }

        item.transform.rotation = Quaternion.identity;
        item.transform.SetParent(null);
        item.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var item = collision.GetComponent(typeof(IItem));
        if (item != null)
        {
            _takeItem?.transform.GetChild(collision.transform.childCount - 1).gameObject.SetActive(false);
            _takeItem = item.gameObject;
            collision.transform.GetChild(collision.transform.childCount - 1).gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == _takeItem)
        {
            _takeItem = null;
            collision.transform.GetChild(collision.transform.childCount - 1).gameObject.SetActive(false);
        }
    }
}