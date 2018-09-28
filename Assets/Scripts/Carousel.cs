using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Carousel : MonoBehaviour {

    #region Paramaters

    [SerializeField] GameObject carouselContainer;
    [SerializeField] GameObject prefabLocation;

    [Space (20)]
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
    private float _inclinaisonIntensity;
    public float InclinaisonIntensity
    {
        get { return _inclinaisonIntensity; }
        set { _inclinaisonIntensity = value; SetPositions(); }
    }

    [SerializeField]
    [Range(-180f, 180)]
    private float rotationY;
    public float RotationY
    {
        get { return rotationY; }
        set { rotationY = value; SetRotations(); }

    }

    [SerializeField]
    [Range(-180f, 180)]
    private float rotationZ;
    public float RotationZ
    {
        get { return rotationZ; }
        set { rotationZ = value; SetRotations(); }

    }


    [SerializeField]
    [Range(1f, 360f)]
    private float _modulo;
    public float Modulo
    {
        get { return _modulo; }
        set { _modulo = value; SetRotations(); }

    }


    void OnValidate()
    {
        SpaceX = _spaceX;
        SpaceY = _spaceY;
        Inclinaison = _inclinaison;
        InclinaisonIntensity = _inclinaisonIntensity;
        RotationY = rotationY;
        RotationZ = rotationZ;
        Modulo = _modulo;
    }


    #endregion

    #region RunTimeDatas


    List<GameObject> children = new List<GameObject>();
    List<PlaceContent> locationList = new List<PlaceContent>();
    UIManager uiManager;
    int currentHightLight = 0;

    #endregion


    #region MonoBehaviour Methods



    /// <summary>
    /// Implementation of the Start MonoBehaviour
    /// </summary>
    /// 
    void Start() {
        uiManager = GetComponent<UIManager>();
        locationList.Clear();
    }



    #endregion

    void Clear()
    {

    }

    public void CreateCarousel(ResultsGooglePlace res)
    {
        foreach (LocationData location in res.results)
        {
            if (location.photos.Count > 0)
            {
                CreateLocationInCarousel(location);
            }
        }

        StartCoroutine(LoadAllLocations());
    }
    public void CreateLocationInCarousel(LocationData locationData)
    {
        GameObject obj = Instantiate(prefabLocation, carouselContainer.transform, false);
        PlaceContent placeContent = obj.GetComponent<PlaceContent>();
        placeContent.Init(locationData);
        locationList.Add(placeContent);
    }

    IEnumerator LoadAllLocations()
    {
        for (int i = 0; i < locationList.Count; i++)
        {
            string query = "https://maps.googleapis.com/maps/api/place/photo?maxwidth=1200&photoreference=" + locationList[i].GetFirstPhotoRef();
            yield return StartCoroutine(GoogleResultsReader.CallGoogleApiImage(query, locationList[i].SetPicture));
        }

        DisplayPhotos();
        uiManager.SetCarouselActive();
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

    public void DisplayPhotos()
    {
        angle = Mathf.PI * 2 / locationList.Count;

        SetPositions();
        SetRotations();

        locationList[currentHightLight].SetAsActivePlace();
    }
    /// <summary>
    /// Set the positions one of the photos
    /// </summary>

    void SetPositions()
    {
        Vector2 DotVectorHeight = new Vector2(Mathf.Cos(_inclinaison) * _inclinaisonIntensity, Mathf.Sin(_inclinaison) * _inclinaisonIntensity);
        float masterdot = Vector2.Dot(new Vector2(SpaceY, 0), DotVectorHeight);
        for (int i = 0; i < locationList.Count; i++)
        {
            GameObject obj = locationList[i].gameObject;

            obj.transform.position = new Vector3(Mathf.Cos(angle * i - currentAngle) * _spaceX, 0, Mathf.Sin(angle * i - currentAngle) * SpaceY);

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
            float cos = Mathf.Cos(angle * i - currentAngle);
            obj.transform.rotation = Quaternion.Euler(new Vector3(-90, rotationY * Math.Sign(cos), rotationZ * cos));
        }
    }

    public void OnLeftTurn()
    {
        if (Rotating)
            return;
        locationList[currentHightLight].SetAsBackgroundPlace();
        currentHightLight = (currentHightLight == 0) ? locationList.Count - 1 : currentHightLight - 1;

        locationList[currentHightLight].SetAsActivePlace();
        Rotating = true;
        newAngle = currentAngle - angle;
    }

    public void OnRightTurn()
    {
        if (Rotating)
            return;

        locationList[currentHightLight].SetAsBackgroundPlace();
        currentHightLight = (currentHightLight == locationList.Count - 1) ? 0 : currentHightLight + 1;

        newAngle = currentAngle+ angle;

        locationList[currentHightLight].SetAsActivePlace();

        Rotating = true;
    }

    bool Rotating = false;
    float currentAngle = Mathf.PI / 2;
    float newAngle;
    float angle;

    private void Update()
    {
        if (Rotating)
        {
            currentAngle = Mathf.Lerp(currentAngle, newAngle, 0.2f);

            if (Mathf.Abs(currentAngle - newAngle) < 0.1f)
            {
                Rotating = false;
                currentAngle = newAngle;
            }

            SetPositions();
            SetRotations();
        }
    }
}
