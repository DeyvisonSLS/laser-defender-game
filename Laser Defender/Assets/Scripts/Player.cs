using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region PROPERTIES
    public float Speed { get; set; } = 5.0f;
    public Vector2 ShipExtent 
    { 
        get
        {
            Vector2 extents = new Vector2
            (
                transform.GetComponent<SpriteRenderer>().bounds.extents.x, 
                transform.GetComponent<SpriteRenderer>().bounds.extents.y
            );
            return extents;
        }
    }
    #endregion 
    
    #region MONOBEHAVIOUR
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        DoMove();
        // DoMouseMove();
        
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
        }
    }
    #endregion

    #region PRIVATE_METHODS
    private void DoMove()
    {
        float deltaX = Input.GetAxis("Horizontal");
        float deltaY = Input.GetAxis("Vertical");

        float newXPos = transform.position.x + ((deltaX * Time.deltaTime) * Speed);
        float newYPos = transform.position.y + ((deltaY * Time.deltaTime) * Speed);

        transform.position = LimitToScreen(new Vector2(newXPos, newYPos));
    }
    private void DoMouseMove()
    {
        Vector2 mousePos = LimitToScreen(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        transform.position = mousePos;
    }
    private Vector3 LimitToScreen(Vector2 newPos)
    {
        Vector3 screenLimit = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, transform.position.z));

        float x = Mathf.Clamp(newPos.x, 0.0f + ShipExtent.x, screenLimit.x - ShipExtent.x);
        float y = Mathf.Clamp(newPos.y, 0.0f + ShipExtent.y, screenLimit.y - ShipExtent.y);
        
        return new Vector3(x, y, transform.position.z);
    }
    #endregion
}