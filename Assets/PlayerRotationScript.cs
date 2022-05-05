using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotationScript : MonoBehaviour
{
    private void Update()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePosition.x < transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}