using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using System;

public class LocationSearch : MonoBehaviour {

    #region Parameters

    [SerializeField] InputField locationInputField;

    #endregion


    #region RunTimeData

    UIManager uiManager;

    #endregion


    #region Events

    [Serializable] public class UnityEvent_Search : UnityEvent<string>{}

    [SerializeField] public UnityEvent_Search MakeGoogleResearch; 

    #endregion
    // Use this for initialization
    void Start ()
    {
        uiManager = GetComponent<UIManager>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnLocationSearchPressed()
    {
        string locationSearch = locationInputField.text;
        MakeGoogleResearch.Invoke(locationSearch);
        uiManager.HideErrorMessage();
    }
}
