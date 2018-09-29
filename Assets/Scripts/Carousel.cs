using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Carousel : MonoBehaviour {

    #region Paramaters
    [Header("Pooler")]
    [SerializeField] ObjectPooler objectPoolerPlace;

    [Space(10)]

    [Header("GameObjects")]
    [SerializeField] GameObject carouselContainer;
    [SerializeField] GameObject prefabLocation;
    [Space(10)]

    [Header("Swipe")]
    [SerializeField] float swipeIntensity = 2f;
    [SerializeField] float velocityAmountlost = 0.5f;

    [Space (10)]

    [Header("Carousel's display parameters")]
    [SerializeField]
    private uint _numberItem = 10;
    public uint NumberItem
    {
        get { return _numberItem; }
        set { _numberItem = value; Clear();  }
    }

    [SerializeField]
    private float _spaceX = 10.0f;
    public float SpaceX
    {
        get { return _spaceX; }
        set { _spaceX = value; SetPositions(); }
    }

    [SerializeField]
    private float _spaceY = 10.0f;
    public float SpaceY
    {
        get { return _spaceY; }
        set { _spaceY = value; SetPositions(); }
    }


    [SerializeField]
    [Range(-1f, 1f)]
    private float _inclinaison;
    public float Inclinaison
    {
        get { return _inclinaison; }
        set { _inclinaison = value; SetPositions(); }
    }

    [SerializeField]
    [Range(1f, 10f)]
    private float _inclinaisonMultiplier;
    public float InclinaisonMultiplier
    {
        get { return _inclinaisonMultiplier; }
        set { _inclinaisonMultiplier = value; SetPositions(); }
    }

    [SerializeField]
    [Range(-180f, 180)]
    private float rotationY;
    public float RotationY
    {
        get { return rotationY; }
        set { rotationY = value; SetRotations(); }

    }

    void OnValidate()
    {
        SpaceX = _spaceX;
        SpaceY = _spaceY;
        Inclinaison = _inclinaison;
        InclinaisonMultiplier = _inclinaisonMultiplier;
        RotationY = rotationY;
    }


    #endregion



    #region Events

    [Serializable] public class UnityEvent_Search : UnityEvent<string> { }
    [SerializeField] public UnityEvent_Search GetNextPageGoogleNearbySearch;

    #endregion


    #region RunTimeDatas


    List<PlaceContent> locationList = new List<PlaceContent>();
    List<float> listPositionAngle = new List<float>();

    UIManager uiManager;

    Vector2 lastPosition = Vector2.zero;
    Vector2 lastDirection = Vector2.zero;

    float partitionAngle;
    float startAngle = Mathf.PI / 2;
    float currentAngle;
    float goalAngle;
    float velocity = 0;

    int currentLocation = 0;

    bool isRotating = false;
    bool isSwiping = false;

    #endregion


    #region MonoBehaviour Methods



    /// <summary>
    /// Implementation of the Start MonoBehaviour
    /// </summary>
    /// 
    void Start() {
        uiManager = GetComponent<UIManager>();
        locationList.Clear();

        currentAngle = startAngle; 
    }

    private void Update()
    {
        if (isRotating)
        {
            currentAngle = Mathf.Lerp(currentAngle, goalAngle, 0.2f);

            if (Mathf.Abs(currentAngle - goalAngle) < 0.1f)
            {
                isRotating = false;
                currentAngle = goalAngle;

            }

            SetPositions();
            SetRotations();

        }

        if (Mathf.Abs(velocity) > 0.5f)
        {
            goalAngle = currentAngle + velocity * swipeIntensity;
            velocity = (velocity > 0) ? velocity - velocityAmountlost : velocity + velocityAmountlost;

            if ((Mathf.Abs(velocity) < velocityAmountlost))
            {
                FindClosestPlace();
                velocity = 0f;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Plane plane = new Plane(Vector3.up, carouselContainer.transform.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            float distance;

            if (Physics.Raycast(ray, out hit) && !isSwiping)
            {
                hit.collider.GetComponent<PlaceContent>().OpenInfos();
            }
        }
    }


    #endregion

    void Clear()
    {
        objectPoolerPlace.Clean();
    
        locationList.Clear();
        currentAngle = startAngle;
    }

    /// <summary>
    /// Create Carousel from the information in the res parameters
    /// </summary>
    /// <param name="res"> The results from the google query </param>
    public void CreateCarousel(ResultsGooglePlace res)
    {
        PlaceContent.OnPressed += uiManager.ShowInfoPanel;

        foreach (LocationData location in res.results)
        {
            // :: enough item in the carousel
            if (_numberItem == locationList.Count - 1)
                break;

            if (location.photos.Count > 0)
            {
                CreateLocationInCarousel(location);
            }
        }


        if (res.next_page_token != null && _numberItem > locationList.Count)
        {
            GetNextPageGoogleNearbySearch.Invoke(res.next_page_token);
        }
        else
            StartCoroutine(LoadAllLocations());

    }


    /// <summary>
    /// Instantiate and Init the place content of the places
    /// </summary>
    /// <param name="locationData"> The current locationData </param>
    public void CreateLocationInCarousel(LocationData locationData)
    {
        GameObject obj = objectPoolerPlace.GetPooledObject();

        PlaceContent placeContent = obj.GetComponent<PlaceContent>();
        placeContent.Init(locationData);
        locationList.Add(placeContent);
    }


    /// <summary>
    /// Get The Photos from all the places in one point and show the photos after that
    /// </summary>
    IEnumerator LoadAllLocations()
    {
        for (int i = 0; i < locationList.Count; i++)
        {
            string query = "https://maps.googleapis.com/maps/api/place/photo?maxwidth=1200&photoreference=" + locationList[i].GetFirstPhotoRef();
            yield return StartCoroutine(GoogleResultsReader.CallGoogleApiImage(query, locationList[i].SetPicture));
        }

        DisplayPhotos();
        uiManager.SetCarouselActive();

        for (int i = 0; i < locationList.Count; i++)
        {
            string query = "https://maps.googleapis.com/maps/api/place/details/json?placeid=" + locationList[i].GetID() + "&fields=rating,formatted_phone_number,formatted_address";
            yield return StartCoroutine(GoogleResultsReader.CallGoogleApi(query, locationList[i].SaveDetails));
        }

    }


    [ContextMenu("Displau Photos")]
    void Test()
    {
        locationList.Clear();
        foreach (Transform child in carouselContainer.transform)
        {
            GameObject obj = child.gameObject;
            PlaceContent placeContent = obj.GetComponent<PlaceContent>();
            locationList.Add(placeContent);
        }

        DisplayPhotos();
    }

    /// <summary>
    /// Display the photos for the first time and save the angle position of the photos
    /// </summary>
    /// 
    public void DisplayPhotos()
    {
        partitionAngle = Mathf.PI * 2 / locationList.Count;

        SetPositions();
        SetRotations();

        for (int i = 0; i < locationList.Count; i++)
        {
            float anglePosition = Utility.ClampAngle(startAngle + partitionAngle * i);
            listPositionAngle.Add(anglePosition);
        }

        locationList[currentLocation].SetAsActivePlace();
    }


    /// <summary>
    /// Set the positions one of the photos
    /// </summary>

    void SetPositions()
    {
        Vector2 DotVectorHeight = new Vector2(Mathf.Cos(_inclinaison) * _inclinaisonMultiplier, Mathf.Sin(_inclinaison) * _inclinaisonMultiplier);
        float masterdot = Vector2.Dot(new Vector2(SpaceY, 0), DotVectorHeight);
        for (int i = 0; i < locationList.Count; i++)
        {
            GameObject obj = locationList[i].gameObject;

            obj.transform.position = new Vector3(Mathf.Cos(partitionAngle * i - currentAngle) * _spaceX, 0, Mathf.Sin(partitionAngle * i - currentAngle) * SpaceY);

            float dot = Vector2.Dot(new Vector2(obj.transform.position.z, 0), DotVectorHeight);
            obj.transform.position += new Vector3(0, (DotVectorHeight * dot).y  + DotVectorHeight.y * SpaceY, _spaceY );
            obj.transform.position += carouselContainer.transform.position;

        }
    }


    /// <summary>
    /// Set the rotations  of the photos
    /// </summary>

    void SetRotations()
    {
        for (int i = 0; i < locationList.Count; i++)
        {
            GameObject obj = locationList[i].gameObject;
            float cos = Mathf.Cos(partitionAngle * i - currentAngle);
            obj.transform.localRotation = Quaternion.Euler(new Vector3(90, rotationY * Math.Sign(cos) + 180,0));
        }
    }


    /// <summary>
    /// Moves the carousel to the left 
    /// </summary>
    /// 
    public void OnLeftTurn()
    {
        if (isRotating)
            return;
        locationList[currentLocation].SetAsBackgroundPlace();
        currentLocation = (currentLocation == 0) ? locationList.Count - 1 : currentLocation - 1;

        locationList[currentLocation].SetAsActivePlace();
        isRotating = true;
        goalAngle = currentAngle - partitionAngle;
    }


    /// <summary>
    /// Moves the carousel to the right 
    /// </summary>
    /// 
    public void OnRightTurn()
    {
        if (isRotating)
            return;

        locationList[currentLocation].SetAsBackgroundPlace();
        currentLocation = (currentLocation == locationList.Count - 1) ? 0 : currentLocation + 1;

        goalAngle = currentAngle+ partitionAngle;

        locationList[currentLocation].SetAsActivePlace();

        isRotating = true;
    }




    /// <summary>
    /// Moves the carousel according to the input of the swipe
    /// </summary>
    /// 
    public void Swipe(Vector2 direction)
    {
        int sign = (direction.x > 0) ? -1 : 1;

        isSwiping = true;
        lastDirection = direction;
        currentAngle = Mathf.Lerp(currentAngle, (currentAngle + direction.magnitude * sign)  , swipeIntensity);

        SetPositions();
        SetRotations();
    }


    /// <summary>
    /// The swipe is released
    /// </summary>
    /// 
    public void OnSwipeEnd()
    {
        velocity = lastDirection.magnitude  ;
        velocity *= (lastDirection.x > 0) ? -1 : 1;

        isRotating = true;
        isSwiping = false;
    }


    /// <summary>
    /// Find the closest Location from the camera and go to it
    /// </summary>
    /// 
    void FindClosestPlace()
    {
        float closestDistance = 900000000;
        int indexClosest = -1;

        for (int i = 0; i < locationList.Count; i++)
        {
            if (locationList[i].gameObject.transform.localPosition.z < closestDistance)
            {
                closestDistance = locationList[i].gameObject.transform.localPosition.z;
                indexClosest = i;
            }
        }

        if (indexClosest == -1)
            Debug.LogError("no index found for closest");

        currentAngle = Utility.ClampAngle(currentAngle);

        goalAngle = listPositionAngle[indexClosest];
        currentLocation = indexClosest;
        locationList[currentLocation].SetAsActivePlace();

        isRotating = true;
    }
}
