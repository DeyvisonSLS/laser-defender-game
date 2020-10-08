using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaser : MonoBehaviour
{
    private float speed = 10.0f;
    private float _screenTopLimit;
    private Vector2 _laserExtent
    {
        get
        {
            Vector2 laserExtent = new Vector2
            (
                transform.GetComponent<SpriteRenderer>().bounds.extents.x,
                transform.GetComponent<SpriteRenderer>().bounds.extents.y
            );
            return laserExtent;
        }
    }
    void Start()
    {
        _screenTopLimit = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f)).y;
    }
    void Update()
    {
        if(transform.position.y - _laserExtent.y > _screenTopLimit)
        {
            Debug.Log(transform.position.y);
            Debug.Log(_laserExtent.y);
            Destroy(this.gameObject);
        }
        else
        {
            transform.position += new Vector3(0.0f, speed * Time.deltaTime, 0.0f);
        }
    }
}
