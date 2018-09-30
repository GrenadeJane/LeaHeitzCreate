using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.IO;
using System.Linq;



public class GoogleResultsReader : MonoBehaviour
{
    enum resultGoogle
    {
        ZERO_RESULTS,
    }
    #region Constants

    const string GOOGLE_API_KEY = "AIzaSyDxZDslnDL8yKkMv-kPTmgBiwedufQ_uHs";

    const float NIVELLES_LOCATION_LATITUDE = 50.597000f;
    const float NIVELLES_LOCATION_LONGITUDE = 4.323420f;

    const string SAVE_PATH_JSON = "/google_results_json.json";
    #endregion


    #region Parameters
    [Header("NearbySearch Parameters")]
    [SerializeField] uint radiusNearbySearch = 1500;
    [SerializeField] float latitudeNearbySearch = NIVELLES_LOCATION_LATITUDE;
    [SerializeField] float longitudeNearbySearch = NIVELLES_LOCATION_LONGITUDE;

    #endregion


    #region Events

    [Serializable] public class UnityEvent_CreateLocation : UnityEvent<ResultsGooglePlace, bool> { }

    [SerializeField] public UnityEvent_CreateLocation CreateCarousel;
    [SerializeField] public UnityEvent DisplayErrorNoResults;

    #endregion

    #region RuntimeDatas 

    ResultsGooglePlace currentPlaceResult; // the one which gonna be saved

    #endregion

    #region MonoBahaviour Methods

    void OnDestroy()
    {
        SaveGoogleResult();
    }

    #endregion

    #region Public Methods 
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
        bool isLocal = false;
        ResultsGooglePlace res = JsonUtility.FromJson<ResultsGooglePlace>(response);//
        if (res == null)
        {
            res = GetLastGoogleResult(); isLocal = true;
        }

        // no correct response - no internet connection - no saved result
        if (res == null || res.status == "ZERO_RESULTS"  )
            DisplayErrorNoResults.Invoke();
        else
        {
            currentPlaceResult = res;
            CreateCarousel.Invoke(res, isLocal);
        }
    }

    public void GetNextPage(string next_page_token)
    {
        string googleQuery = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?pagetoken=" + next_page_token;
        StartCoroutine(CallGoogleApi(googleQuery, ReadGoogleResults));
    }

    public void SavePhoto(PlaceContent place, Texture2D tex)
    {
        int index = currentPlaceResult.results.FindIndex(x => x.place_id == place.GetID());

        currentPlaceResult.results[index].savedPhoto = new SavedPhoto();
        currentPlaceResult.results[index].savedPhoto.texture = tex;

        currentPlaceResult.results[index].savedPhoto.path = place.GetID();
    }

    public void SaveDetails(PlaceContent place, LocationDetails details)
    {
        int index = currentPlaceResult.results.FindIndex(x => x.place_id == place.GetID());
        currentPlaceResult.results[index].details = details;
    }

    public static IEnumerator CallGoogleApi(string googleQuery, UnityAction<string> actionDone )
    {
        googleQuery += "&key=";
        googleQuery += GOOGLE_API_KEY;

        WWW googleResponse = new WWW(googleQuery);
        yield return googleResponse;
        string response = googleResponse.text;

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
    #endregion

    #region Private Methods

    void SaveGoogleResult()
    {
        if (currentPlaceResult == null)
            return;

        foreach (LocationData location in currentPlaceResult.results)
        {
            if ( location.savedPhoto != null && location.savedPhoto.texture )
                EncodeTexture2D(location.savedPhoto.texture, location.savedPhoto.path);
        }

        string jsonSave = JsonUtility.ToJson(currentPlaceResult);
        File.WriteAllText(Application.persistentDataPath + SAVE_PATH_JSON, jsonSave);
    }

    ResultsGooglePlace GetLastGoogleResult()
    {
        ResultsGooglePlace placesResults = null;

        if (File.Exists(Application.persistentDataPath + SAVE_PATH_JSON))
        {
            string jsonSave = File.ReadAllText(Application.persistentDataPath + SAVE_PATH_JSON);
            placesResults =  JsonUtility.FromJson<ResultsGooglePlace>(jsonSave);

            foreach (LocationData location in placesResults.results)
            {
                if (location.savedPhoto != null )
                    location.savedPhoto.texture = GetTexture2DFromPath(location.place_id);
            }
        }

        return placesResults;
    }

    void EncodeTexture2D(Texture2D tex, string path)
    {
        byte[] bytes = tex.EncodeToPNG();
        File.WriteAllBytes(Application.persistentDataPath + "/placesScreens-" + path + ".png", bytes);
    }

    Texture2D GetTexture2DFromPath(string path)
    {
        if (File.Exists(Application.persistentDataPath + "/placesScreens-" + path + ".png"))
        {
            byte[] bytes = File.ReadAllBytes(Application.persistentDataPath + "/placesScreens-" + path + ".png");
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);

            return texture;
        }
        return null;
    }



    #endregion
}
