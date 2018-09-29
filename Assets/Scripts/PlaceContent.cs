using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Events;
using System;


public class PlaceContent : MonoBehaviour, IObjectPooled
{
    #region Parameters
    [SerializeField] static   float heightActive =2;
    [SerializeField] static float widthActive = 2;
    [SerializeField] static float heightBackground = 1;
    [SerializeField] static  float widthBackground = 1;

    #endregion

    #region Events 
    public static event Action<string, string, LocationDetails> OnPressed;

    [Serializable] public class UnityEvent_PlaceInfo : UnityEvent<string,string> { }
    [SerializeField] public UnityEvent_PlaceInfo SendPlaceInfo;


    #endregion

    #region RuntimeDatas

    LocationData locationData;
    LocationDetails locationDetails;

    bool isActive;
    
    #endregion


    public void Init(LocationData locationData)
    {
        this.locationData = locationData;
    }

    // Use this for initialization
    void Start () {
        SetAsBackgroundPlace();
    }

    public void OpenInfos()
    {
        if (isActive)
        {
            OnPressed.Invoke(locationData.name, "Open" + locationData.opening_hours.open_now.ToString(), locationDetails);
        }
    }

    public void SetPicture(Texture2D tex)
    {
        GetComponent<Renderer>().material.mainTexture = tex;
    }

    public void SaveDetails(string jsonResultDetail)
    {
        locationDetails = JsonUtility.FromJson<ResultDetailGooglePlace>(jsonResultDetail).result;
        OpenInfos();
    }

    public string GetFirstPhotoRef()
    {
        return locationData.photos.First().photo_reference;
    }

    public string GetID()
    {
        return locationData.place_id;
    }

    public void SetAsActivePlace()
    {
        isActive = true;
    }

    public void SetAsBackgroundPlace()
    {
        isActive = false;
    }

    public void Clean()
    {
        locationData = null;
        locationDetails = null;
    }
}
