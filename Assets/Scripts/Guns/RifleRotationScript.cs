using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleRotationScript : MonoBehaviour
{
    private void Update()
    {
        var mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        var angle = Vector2.Angle(Vector2.right, mousePosition - transform.position);
        if (transform.position.x < mousePosition.x)
        {
            transform.eulerAngles = new Vector3(0, 0, transform.position.y < mousePosition.y ? angle : -angle);
        }
        else
        {
            transform.eulerAngles = new Vector3(180, 0f, transform.position.y < mousePosition.y ? -angle : angle);
        }
    }
}