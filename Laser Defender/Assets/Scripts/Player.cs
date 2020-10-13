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
    [SerializeField]
    private bool _mouseDown;
    [SerializeField]
    private bool _mouseUp;
    private Animator _animator;
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
        // Cursor.visible = false;
        _myLineRenderer = _myLine.GetComponent<LineRenderer>();
        _animator = GameObject.Find("Target").GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
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

    private void DoMouseMove()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        bool colliderNotNull = true;

        if(Input.GetMouseButtonDown(0))
        {
            if(hit.collider != null)
            {
                if(hit.collider.transform.name == "Player")
                {
                    colliderNotNull = true;
                    _mouseDown = true;
                    _mouseUp = false;
                }
                else
                {
                    Debug.Log("You have not clicked in the player yet.");
                }
            }
            else
            {
                Debug.Log("You click in nothing.");
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            colliderNotNull = false;
            _mouseUp = true;
            _mouseDown = false;
        }

        if(colliderNotNull)
        {
            if(_mouseDown)
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _targetPrefab.transform.position = mousePos;

                _animator.SetBool("MouseOver", true);

                _shipOffset = transform.position - _targetPrefab.transform.position;

                _myLineRenderer.SetPosition(0, transform.position);
                _myLineRenderer.SetPosition(1, _targetPrefab.transform.position);

                Vector3 newPos = transform.position;

                newPos -= (_shipOffset * Time.deltaTime) * _shipVelocity;

                transform.position = LimitToScreen(newPos);

                // if(transform.position == _targetPrefab.transform.position)
                // {
                //     _animator.SetBool("MouseOver", false);
                // }
            }
            else if(_mouseUp)
            {
                _targetPrefab.transform.position = transform.position;

                _myLineRenderer.SetPosition(0, transform.position);
                _myLineRenderer.SetPosition(1, _targetPrefab.transform.position);

                _animator.SetBool("MouseOver", false);
            }
        }

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