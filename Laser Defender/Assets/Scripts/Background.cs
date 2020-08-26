using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    #region FIELDS
    [Range(0f,10f)]
    [SerializeField]
    private float bgSpeed = 10.0f;
    [SerializeField]
    private GameObject bg1 = null;
    [SerializeField]
    private GameObject bg2 = null;
    private Vector3 _initialPosBg1;
    private Vector3 _initialPosBg2;
    private float _screenLimit;
    #endregion

    #region PROPERTIES
    public float BackgroundSpeed
    {
        get
        {
            return bgSpeed;
        } 
        set
        {
            bgSpeed = value;
        } 
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _initialPosBg1 = bg1.transform.position;
        _initialPosBg2 = bg2.transform.position;
        _screenLimit = bg1.GetComponent<SpriteRenderer>().sprite.bounds.max.y * (-1);
    }

    // Update is called once per frame
    void Update()
    {
        MoveObjects(bg1, bg2);
    }
    private void MoveObjects(GameObject object1, GameObject object2)
    {
        if(object1.transform.position.y <= _screenLimit)
        {
            object1.transform.position = _initialPosBg2;
            object2.transform.position = _initialPosBg1;
        }
        else
        {
            object1.transform.position -= new Vector3(0.0f, bgSpeed * Time.deltaTime, transform.position.z);
        }

        if(object2.transform.position.y <= _screenLimit)
        {
            object2.transform.position = _initialPosBg1;
            object1.transform.position = _initialPosBg2;
        }
        else
        {
            object2.transform.position -= new Vector3(0.0f, bgSpeed * Time.deltaTime, transform.position.z);
        }
    }
}
