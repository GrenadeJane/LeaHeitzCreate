using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Events;
using System;


public class PlaceContent : MonoBehaviour
{

    [SerializeField] static   float heightActive =2;
    [SerializeField] static float widthActive = 2;
    [SerializeField] static float heightBackground = 1;
    [SerializeField] static  float widthBackground = 1;


    [Serializable] public class UnityEvent_PlaceInfo : UnityEvent<string,string> { }
    [SerializeField] public UnityEvent_PlaceInfo SendPlaceInfo;


    public static event Action<string, string> OnPressed;

     LocationData locationData;

    bool isActive;

    public void Init(LocationData locationData)
    {
        this.locationData = locationData;
    }

    // Use this for initialization
    void Start () {
        SetAsBackgroundPlace();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OpenInfos()
    {
        if (isActive)
        {
            OnPressed.Invoke(locationData.name, "Open" + locationData.opening_hours.open_now.ToString());
        }
        //if (isActive)
        //{
        //    SendPlaceInfo.Invoke(locationData.name, "Open" + locationData.opening_hours.open_now.ToString());
        //}
        Debug.Log(locationData);
    }

    public void SetPicture(Texture2D tex)
    {
        GetComponent<Renderer>().material.mainTexture = tex;
       // GetComponent<RawImage>().texture = tex;
        //GetComponent<RectTransform>().sizeDelta = new Vector2(tex.width, tex.height);
    }

    public string GetFirstPhotoRef()
    {
        return locationData.photos.First().photo_reference;
    }

    public void SetAsActivePlace()
    {
        isActive = true;
      //  transform.localScale = new Vector2(widthActive, heightActive);
    }

    public void SetAsBackgroundPlace()
    {
        isActive = false;
        //transform.localScale = new Vector2(widthBackground, heightBackground);
    }
}
