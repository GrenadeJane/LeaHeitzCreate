using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    #region Parameters
    [SerializeField] GameObject userInputContainer;
    [SerializeField] GameObject carouselContainer;

    [SerializeField] Animator infoPanelAnimator;
    [SerializeField] Text titleInfoLabel;
    [SerializeField] Text textInfoLabel;
    [SerializeField] Text errorLabel;
    #endregion

    #region Runtime Datas 
    bool infoPanelEnabled = false;
    #endregion

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

    // display info but don't trigger the aniamtion
    public void ShowInfoPanel(string title, string text, LocationDetails details = null)
    {
        titleInfoLabel.text = title;
        textInfoLabel.text = text + '\n';

        if (details != null)
        {
            textInfoLabel.text += "Adress : \t" +  details.formatted_address + '\n';
            textInfoLabel.text += "Phone Number : \t" + details.formatted_phone_number + '\n';
            textInfoLabel.text += "Website : \t" + details.website + '\n';
            textInfoLabel.text += "Rating : \t" + details.rating + '\n';
        }

        if (!infoPanelEnabled)
            infoPanelAnimator.SetTrigger("Open");

        infoPanelEnabled = true;
    }

    public void HideInfoPanel()
    {
        if (infoPanelEnabled)
        {
            infoPanelAnimator.SetTrigger("Close");
            infoPanelEnabled = false;
        }
    }
}
