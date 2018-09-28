using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PlaceContent : MonoBehaviour {

    [SerializeField] static   float heightActive =2;
    [SerializeField] static float widthActive = 2;
    [SerializeField] static float heightBackground = 1;
    [SerializeField] static  float widthBackground = 1;

     LocationData locationData;

    public void Init(LocationData locationData)
    {
        this.locationData = locationData;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
      //  transform.localScale = new Vector2(widthActive, heightActive);
    }

    public void SetAsBackgroundPlace()
    {
        //transform.localScale = new Vector2(widthBackground, heightBackground);
    }
}
