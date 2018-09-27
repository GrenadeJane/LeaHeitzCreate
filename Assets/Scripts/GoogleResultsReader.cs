using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GoogleResultsReader : MonoBehaviour
{
    #region Constants
    const string GOOGLE_API_KEY = "AIzaSyDxZDslnDL8yKkMv-kPTmgBiwedufQ_uHs";

    const float NIVELLES_LOCATION_LATITUDE = 50.597000f;
    const float NIVELLES_LOCATION_LONGITUDE = 4.323420f;

    #endregion

    #region Parameters 
    [SerializeField] GameObject prefabPhoto;

    #endregion

    #region Events

    [Serializable] public class UnityEvent_AddPlace : UnityEvent<PlaceContent> { }

    [SerializeField] public UnityEvent_AddPlace AddPlaceToCarousel;

    #endregion

    public void SearchGoogleApi(string input)
    {
        string googleQuery = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?keyword=";
        googleQuery += input;
        googleQuery +=
             // "&inputtype=textquery&fields=photos,formatted_address,name,rating,opening_hours,geometry" +
             "&location=" + NIVELLES_LOCATION_LATITUDE + "," + NIVELLES_LOCATION_LONGITUDE + "&radius=1500" +
            "&key=";
        googleQuery += GOOGLE_API_KEY;

        StartCoroutine(CallGoogleApi(googleQuery, ReadGoogleResults));
    }


    public void ReadGoogleResults(string response)
    {
        ResultsGooglePlace res = JsonUtility.FromJson<ResultsGooglePlace>(response);
        foreach (LocationData location in res.results)
        {
            if (location.photos.Count > 0)
            {
                ReadPhotos(location.photos.First().photo_reference);
            }
        }
        Debug.Log(res);
    }

    void ReadPhotos(string photoRef)
    {
        string query = "https://maps.googleapis.com/maps/api/place/photo?maxwidth=750&photoreference=" + photoRef + "&key=" + GOOGLE_API_KEY;
        StartCoroutine(CallGoogleApiImage(query, CreatePrefabPhoto));
    }

    void CreatePrefabPhoto(Texture2D tex)
    {
        GameObject obj = Instantiate(prefabPhoto);
        obj.GetComponent<SpriteRenderer>().sprite = Sprite.Create(tex, new Rect(0,0,200,200),  Vector2.zero);
        AddPlaceToCarousel.Invoke(obj.GetComponent<PlaceContent>());
    }

    public string AskNextPage(string idNextRound)
    {
        return "";
    }

    IEnumerator CallGoogleApi(string googleQuery, UnityAction<string> actionDone )
    {
        WWW googleResponse = new WWW(googleQuery);
        yield return googleResponse;
        string response = googleResponse.text;
        Debug.Log(response);

        actionDone(response);
    }

    IEnumerator CallGoogleApiImage(string googleQuery, UnityAction<Texture2D> actionDone)
    {
        WWW googleResponse = new WWW(googleQuery);
        yield return googleResponse;

        actionDone(googleResponse.texture);
    }
}
