using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationSearch : MonoBehaviour {

    #region Constants
    const string GOOGLE_API_KEY = "AIzaSyDxZDslnDL8yKkMv-kPTmgBiwedufQ_uHs";

    #endregion


    #region Parameters

    [SerializeField] InputField locationInputField;

    #endregion


    #region RunTimeData

    #endregion

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnLocationSearchPressed()
    {
        string locationSearch = locationInputField.text;
        string googleQuery = "https://maps.googleapis.com/maps/api/place/findplacefromtext/json?input=";
        googleQuery += locationSearch;
        googleQuery += " &inputtype=textquery&fields=photos,formatted_address,name,rating,opening_hours,geometry&key=";
        googleQuery += GOOGLE_API_KEY;

        StartCoroutine(CallGoogleApi(googleQuery));
    }

    IEnumerator CallGoogleApi(string googleQuery )
    {
        WWW googleResponse = new WWW(googleQuery);
        yield return googleResponse;
        string response = googleResponse.text;
        Debug.Log(response);
    }
}
