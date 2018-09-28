using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    [SerializeField] GameObject userInputContainer;
    [SerializeField] GameObject carouselContainer;

    [SerializeField] Animator infoPanelAnimator;
    [SerializeField] Text titleInfoLabel;
    [SerializeField] Text textInfoLabel;


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

    public void ShowInfoPanel(string title, string text)
    {
        titleInfoLabel.text = title;
        textInfoLabel.text = text;

        infoPanelAnimator.SetTrigger("Open");
    }

    public void HideInfoPanel()
    {
        infoPanelAnimator.SetTrigger("Close");

    }
}
