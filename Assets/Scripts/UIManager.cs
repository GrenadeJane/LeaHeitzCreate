using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    #region Parameters

    [Header("GameObjects")]
    [SerializeField] GameObject userInputContainer;
    [SerializeField] GameObject carouselContainer;
    [SerializeField] GameObject carouselUIContainer;

    [Space(10)]

    [Header("Info panel ")]
    [SerializeField] Animator infoPanelAnimator;
    [SerializeField] Text titleInfoLabel;
    [SerializeField] Text textInfoLabel;

    [Space(10)]

    [Header("error Panel")]
    [SerializeField] Text errorLabel;
    #endregion

    #region Runtime Datas 
    bool infoPanelEnabled = false;
    bool swiping = false;
    #endregion

    // Use this for initialization
    void Start ()
    {
        userInputContainer.SetActive(true);
        carouselUIContainer.SetActive(false);
        errorLabel.enabled = false;
    }

    // Update is called once per frame
    public void OnSwipeBegin()
    {
        HideInfoPanel();
    }

    public void Swipe(Vector2 dir)
    {
        swiping = true;
    }

    public void OnSwipeEnd()
    {
        swiping = false;
    }

    public void SetCarouselActive()
    {
        userInputContainer.SetActive(false);
        carouselContainer.SetActive(true);
        carouselUIContainer.SetActive(true);
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
    public void ShowInfoPanel(string title, string opened, LocationDetails details = null)
    {
        if (swiping)
            return;

        titleInfoLabel.text = title;
        textInfoLabel.text = "Open :\t" + opened + '\n';

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
