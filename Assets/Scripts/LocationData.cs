using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ResultsGooglePlace
{
    public string next_page_token;
    [SerializeField]
    public List<LocationData> results;
    public string status;
}

public class ResultDetailGooglePlace
{
    [SerializeField]
    public LocationDetails result;
}

[Serializable]
public class LocationDetails
{
    public string formatted_address;
    public string formatted_phone_number;
    public float rating;
    public string[] types;
    public string website;
}

[Serializable]
public class LocationData
{
    public string icon,
                    id,
                    name,
        place_id;
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
