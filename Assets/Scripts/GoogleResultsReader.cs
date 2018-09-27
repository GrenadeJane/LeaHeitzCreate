using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GoogleResultsReader : MonoBehaviour
{
    #region Constants
    const string GOOGLE_API_KEY = "AIzaSyDxZDslnDL8yKkMv-kPTmgBiwedufQ_uHs";

    const float NIVELLES_LOCATION_LATITUDE = 50.597000f;
    const float NIVELLES_LOCATION_LONGITUDE = 4.323420f;

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

        StartCoroutine(CallGoogleApi(googleQuery));
    }


    public void ReadGoogleResults(string response)
    {
        ResultsGooglePlace res = JsonUtility.FromJson<ResultsGooglePlace>(response);
        Debug.Log(res);
    }

    public string AskNextPage(string idNextRound)
    {
        return "";
    }

    IEnumerator CallGoogleApi(string googleQuery)
    {
        WWW googleResponse = new WWW(googleQuery);
        yield return googleResponse;
        string response = googleResponse.text;
        Debug.Log(response);

        ReadGoogleResults(response);
    }

}
