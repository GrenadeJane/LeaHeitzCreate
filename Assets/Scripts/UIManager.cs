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
    [SerializeField] Text errorLabel;


    // Use this for initialization
    void Start ()
    {
        userInputContainer.SetActive(true);
        errorLabel.enabled = false;
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void SetCarouselActive()
    {
        userInputContainer.SetActive(false);
        carouselContainer.SetActive(true);
    }

    public void DisplayErrorMessage()
    {
        errorLabel.enabled = true;
    }

    public void HideErrorMessage()
    {
        errorLabel.enabled = false;
    }

    public void ShowInfoPanel(string title, string text, LocationDetails details = null)
    {
        titleInfoLabel.text = title;
        textInfoLabel.text = text + '\n';
        textInfoLabel.text += details.formatted_address + '\n';
        textInfoLabel.text += details.formatted_phone_number + '\n';
        textInfoLabel.text += details.website + '\n';
        textInfoLabel.text += details.rating + '\n';
        //textInfoLabel.text += details.types.ToString();

        infoPanelAnimator.Play("InfoPanel");
    }

    public void HideInfoPanel()
    {
        infoPanelAnimator.SetTrigger("Close");

    }
}
