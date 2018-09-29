using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class GoogleResultsReader : MonoBehaviour
{
    #region Constants

    const string GOOGLE_API_KEY = "AIzaSyDxZDslnDL8yKkMv-kPTmgBiwedufQ_uHs";

    const float NIVELLES_LOCATION_LATITUDE = 50.597000f;
    const float NIVELLES_LOCATION_LONGITUDE = 4.323420f;

    #endregion


    #region Parameters
    [Header("NearbySearch Parameters")]
    [SerializeField] uint radiusNearbySearch = 1500;
    [SerializeField] float latitudeNearbySearch = NIVELLES_LOCATION_LATITUDE;
    [SerializeField] float longitudeNearbySearch = NIVELLES_LOCATION_LONGITUDE;

    #endregion




    #region Events

    [Serializable] public class UnityEvent_CreateLocation : UnityEvent<ResultsGooglePlace> { }

    [SerializeField] public UnityEvent_CreateLocation CreateCarousel;

    #endregion


    public void GoogleApiNearbySearch(string input)
    {
        string googleQuery = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?keyword=";
        googleQuery += input;
        googleQuery +=  "&location=" + latitudeNearbySearch + "," + longitudeNearbySearch;
        googleQuery += "&radius=" + radiusNearbySearch;
        StartCoroutine(CallGoogleApi(googleQuery, ReadGoogleResults));
    }


    public void ReadGoogleResults(string response)
    {
        ResultsGooglePlace res = JsonUtility.FromJson<ResultsGooglePlace>(response);
        CreateCarousel.Invoke(res);
    }


    public void GetNextPage(string next_page_token)
    {
        string googleQuery = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?pagetoken=" + next_page_token;
        StartCoroutine(CallGoogleApi(googleQuery, ReadGoogleResults));
    }

    public static IEnumerator CallGoogleApi(string googleQuery, UnityAction<string> actionDone )
    {
        googleQuery += "&key=";
        googleQuery += GOOGLE_API_KEY;

        WWW googleResponse = new WWW(googleQuery);
        yield return googleResponse;
        string response = googleResponse.text;
        Debug.Log(response);

        actionDone(response);
    }

    public static IEnumerator CallGoogleApiImage(string googleQuery, UnityAction<Texture2D> actionDone)
    {
        googleQuery += "&key=";
        googleQuery += GOOGLE_API_KEY;

        WWW googleResponse = new WWW(googleQuery);
        yield return googleResponse;

        actionDone(googleResponse.texture);
    }
}
