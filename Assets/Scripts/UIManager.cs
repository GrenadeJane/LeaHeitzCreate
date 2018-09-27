using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    [SerializeField] GameObject userInputContainer;
    [SerializeField] GameObject carouselContainer;

    // Use this for initialization
    void Start () {
        userInputContainer.SetActive(true);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetCarouselActive()
    {
        userInputContainer.SetActive(false);
        carouselContainer.SetActive(true);
    }
}
