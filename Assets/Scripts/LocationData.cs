using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationData
{
    public string   icon,
                    id,
                    name;
    // opening hours
    public OpeningHours opening_hours;
    public Photos[] photos;
}

public struct OpeningHours
{
    public bool open_now;
}

public struct Photos
{
    public int   height,
                 width;

    public string[] html_attributions;
    public string photo_reference;
}
