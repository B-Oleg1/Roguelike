using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryScript : MonoBehaviour
{
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
            if (_currentGun != _guns[0])
            {
                _currentGun.SetActive(false);
                _currentGun = _guns[0];
                _currentGun.SetActive(true);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (_currentGun != _guns[1])
            {
                _currentGun.SetActive(false);
                _currentGun = _guns[1];
                _currentGun.SetActive(true);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (_currentGun != _guns[2])
            {
                _currentGun.SetActive(false);
                _currentGun = _guns[2];
                _currentGun.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && _takeItem != null)
        {
            TakeItem();
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

            if (_currentGun != null && (int)((IGun)_currentGun?.GetComponent(typeof(IGun))).TypeGun != gunId)
            {
                _currentGun.SetActive(false);
            }

            _currentGun = itemObject;
            _currentGun.transform.SetParent(_rightHand);
            _currentGun.transform.localPosition = new Vector3(0, 0, 0);

            foreach (var script in _currentGun.GetComponents<MonoBehaviour>())
            {
                script.enabled = true;
            }

            _guns[gunId] = _currentGun;
        }
    }

    private void DropItem(GameObject item)
    {
        foreach (var script in item.GetComponents<MonoBehaviour>())
        {
            script.enabled = false;
        }

        item.transform.SetParent(null);
        item.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var item = collision.GetComponent(typeof(IItem));
        if (item != null)
        {
            _takeItem = item.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == _takeItem)
        {
            _takeItem = null;
        }
    }
}