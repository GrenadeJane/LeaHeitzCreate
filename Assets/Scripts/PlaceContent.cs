using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceContent : MonoBehaviour {

    [SerializeField] static   float heightActive =2;
    [SerializeField] static float widthActive = 2;
    [SerializeField] static float heightBackground = 1;
    [SerializeField] static  float widthBackground = 1;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetAsActivePlace()
    {
        transform.localScale = new Vector2(widthActive, heightActive);
    }

    public void SetAsBackgroundPlace()
    {
        transform.localScale = new Vector2(widthBackground, heightBackground);
    }
}
