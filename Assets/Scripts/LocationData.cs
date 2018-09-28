using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ResultsGooglePlace
{
    public string next_page_token;
    [SerializeField]
    public List<LocationData> results;
}

[Serializable]
public class LocationData
{
    public string   icon,
                    id,
                    name;
    // opening hours
    [SerializeField]
    public OpeningHours opening_hours;
    [SerializeField]
    public List<Photos> photos;
}

[Serializable]
public class OpeningHours
{
    public bool open_now;
}

[Serializable]
public class Photos
{
    public int   height,
                 width;

    public string[] html_attributions;
    public string photo_reference;
}
