using UnityEngine;
// using UnityEngine.iOS;

public class Player : MonoBehaviour
{
    #region FIELDS
    private const string TAG = "LDefender:";
    [SerializeField]
    private GameObject _laserPrefab = null;
    [SerializeField]
    private GameObject _targetPrefab = null;
    [SerializeField]
    private float _shipVelocity = 10.0f;
    [SerializeField]
    private GameObject _myLine = null;
    private LineRenderer _myLineRenderer;
    private Vector2 _spaceBetween;
    private Animator _animator;
    public bool pressMouse = false;
    public bool playerHasClicked = false;
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
    void Update()
    {
        // DoMove();
        DoMouseMove();
        
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
        }

        #if UNITY_ANDROID && !UNITY_EDITOR
            if(Input.touchCount > 1)
            {
                Debug.Log(TAG + "touchCount: " + Input.touchCount);
                Touch touch = Input.GetTouch(1);

                if(touch.phase == TouchPhase.Began)
                {
                    Debug.Log(TAG + "Touch began.");
                    Fire();
                }
            }
        #elif UNITY_EDITOR
            if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
            {
                Fire();
            }
        #endif
    }
    #endregion

    #region PRIVATE_METHODS
    private void Fire()
    {
        Instantiate(_laserPrefab, newLaserPos, Quaternion.identity);
    }
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
        #if UNITY_ANDROID && !UNITY_EDITOR
            if(Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero);
                MoveShipToTarget(ShipCanMove(hit.collider));
            }
        #elif UNITY_EDITOR
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            MoveShipToTarget(ShipCanMove(hit.collider));
        #endif

        // pressMouse = false;
        // playerHasClicked = false;
    }

    private bool ShipCanMove(Collider2D collider = null)
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
            // if(Input.touchCount > 0)
            // {
            Touch moveTouch = Input.GetTouch(0);

            if(moveTouch.phase == TouchPhase.Began)
            {
                Debug.Log("Touch pressed once.");
                if(collider != null)
                {
                    if(collider.name == "Target")
                    {
                        // Debug.Log("You click in player");
                        playerHasClicked = true;
                    }
                    else
                    {
                        // Debug.Log("You have not clicked in the player yet.");
                    }
                }
                else
                {
                    // Debug.Log("You click in nothing.");
                    playerHasClicked = false;
                }
            }
            else if(moveTouch.phase == TouchPhase.Moved)
            {
                Debug.Log("Touch is pressing down.");
                pressMouse = true;
            }
            else if(moveTouch.phase == TouchPhase.Ended)
            {
                Debug.Log("Touch loose.");
                pressMouse = false;
                playerHasClicked = false;
            }
            // }
            // else
            // {
            //     Debug.Log(TAG + "No touch detected.");
            // }
        #elif UNITY_EDITOR
            if(Input.GetMouseButtonDown(0))
            {
                Debug.Log("Button pressed once.");
                if(collider != null)
                {
                    if(collider.name == "Target")
                    {
                        // Debug.Log("You click in player");
                        playerHasClicked = true;
                    }
                    else
                    {
                        // Debug.Log("You have not clicked in the player yet.");
                    }
                }
                else
                {
                    // Debug.Log("You click in nothing.");
                    playerHasClicked = false;
                }
            }
            else if(Input.GetMouseButton(0))
            {
                Debug.Log("Button is pressing down.");
                pressMouse = true;
            }
            else if(Input.GetMouseButtonUp(0))
            {
                Debug.Log("Button loose.");
                pressMouse = false;
                playerHasClicked = false;
            }
        #endif
        

        if(pressMouse && playerHasClicked)
        {
            Debug.Log("Ship can move");
            return true;
        }
        else
        {
            return false;
        }
    }

    private void MoveShipToTarget(bool canMove)
    {
        if(canMove)
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
                if(Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                    _targetPrefab.transform.position = touchPos;
                    _myLineRenderer.SetPosition(0, transform.position);
                    _myLineRenderer.SetPosition(1, _targetPrefab.transform.position);
                }
                else
                {
                    Debug.Log(TAG + "No touch detected.");
                }
            #elif UNITY_EDITOR
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _targetPrefab.transform.position = mousePos;
                _myLineRenderer.SetPosition(0, transform.position);
                _myLineRenderer.SetPosition(1, _targetPrefab.transform.position);
            #endif

            _animator.SetBool("MouseOver", true);

            _spaceBetween = transform.position - _targetPrefab.transform.position;

            Vector2 newPos = transform.position;

            newPos -= (_spaceBetween * _shipVelocity) * Time.deltaTime;

            transform.position = LimitToScreen(newPos);

            // if(transform.position == _targetPrefab.transform.position)
            // {
            //     _animator.SetBool("MouseOver", false);
            // }
        }
        else
        {
            _targetPrefab.transform.position = transform.position;

            _myLineRenderer.SetPosition(0, transform.position);
            _myLineRenderer.SetPosition(1, _targetPrefab.transform.position);

            _animator.SetBool("MouseOver", false);
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