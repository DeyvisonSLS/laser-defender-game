using UnityEngine;

public class Player : MonoBehaviour
{
    #region FIELDS
    [SerializeField]
    private GameObject _laserPrefab = null;
    [SerializeField]
    private GameObject _targetPrefab = null;
    [SerializeField]
    private float _shipVelocity = 10.0f;
    [SerializeField]
    private GameObject _myLine = null;
    private LineRenderer _myLineRenderer;
    private Vector3 _shipOffset;
    private Vector3 newLaserPos
    {
        get
        {
            Vector3 newPos = new Vector3
            (
                transform.position.x, 
                transform.position.y + 0.7f, 
                transform.position.z
            );
            return newPos;
        }
    }
    #endregion

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
        _myLineRenderer = _myLine.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // DoMove();
        DoMouseMove();
        
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
        }

        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            Instantiate(_laserPrefab, newLaserPos, Quaternion.identity);
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

    // void OnDrawGizmos()
    // {
    //     Gizmos.DrawLine(transform.position, _targetPrefab.transform.position);
    // }

    private void DoMouseMove()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        _targetPrefab.transform.position = mousePos;

        _shipOffset = transform.position - _targetPrefab.transform.position;

        _myLineRenderer.SetPosition(0, transform.position);
        _myLineRenderer.SetPosition(1, _targetPrefab.transform.position);

        Ray ray = new Ray(transform.position, _targetPrefab.transform.position);

        Vector3 newPos = transform.position;

        newPos -= _shipOffset * Time.deltaTime * _shipVelocity;

        transform.position = LimitToScreen(newPos);
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