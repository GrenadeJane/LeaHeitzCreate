using System;
using System.Collections.Generic;
using UnityEngine;

public class Carousel : MonoBehaviour {

    #region Paramaters


    [SerializeField]
    private float _spaceX = 10.0f;
    public float SpaceX
    {
        get { return _spaceX; }
        set { _spaceX = value; SetPositions(); }
    }

    [SerializeField]
    private float _spaceY = 10.0f;
    public float SpaceY
    {
        get { return _spaceY; }
        set { _spaceY = value; SetPositions(); }
    }


    [SerializeField]
    [Range(-1f, 1f)]
    private float _inclinaison;
    public float Inclinaison
    {
        get { return _inclinaison; }
        set { _inclinaison = value; SetPositions(); }
    }

    [SerializeField]
    [Range(1f, 10f)]
    private float _inclinaisonIntensity;
    public float InclinaisonIntensity
    {
        get { return _inclinaisonIntensity; }
        set { _inclinaisonIntensity = value; SetPositions(); }
    }

    [SerializeField]
    [Range(-180f, 180)]
    private float rotationY;
    public float RotationY
    {
        get { return rotationY; }
        set { rotationY = value; SetRotations(); }

    }

    [SerializeField]
    [Range(-180f, 180)]
    private float rotationZ;
    public float RotationZ
    {
        get { return rotationZ; }
        set { rotationZ = value; SetRotations(); }

    }


    [SerializeField]
    [Range(1f, 360f)]
    private  float _modulo;
    public float Modulo
    {
        get { return _modulo; }
        set { _modulo = value; SetRotations(); }

    }


    void OnValidate()
    {
        SpaceX = _spaceX;
        SpaceY = _spaceY;
        Inclinaison = _inclinaison;
        InclinaisonIntensity = _inclinaisonIntensity;
        RotationY = rotationY;
        RotationZ = rotationZ;
        Modulo = _modulo;
    }


    #endregion

    #region RunTimeDatas


    List<GameObject> children = new List<GameObject>();


    #endregion


    #region MonoBehaviour Methods



    /// <summary>
    /// Implementation of the Start MonoBehaviour
    /// </summary>
    /// 
    void Start () {
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf)
                children.Add(child.gameObject);
        }

        SetPositions();
        SetRotations();
    }



    #endregion

    /// <summary>
    /// Set the positions one of the photos
    /// </summary>

    void SetPositions ()
    {
        float angle = Mathf.PI * 2/ children.Count;

        Vector2 DotVectorHeight = new Vector2( Mathf.Cos(_inclinaison ) * _inclinaisonIntensity, Mathf.Sin(_inclinaison) * _inclinaisonIntensity);
        for (int i = 0; i < children.Count; i++)
        {
            GameObject obj = children[i];

            obj.transform.position = new Vector3(Mathf.Cos(angle * i - Mathf.PI / 2) * _spaceX , 0 , Mathf.Sin(angle* i - Mathf.PI / 2 )*SpaceY ) ;

            float dot = Vector2.Dot(new Vector2(obj.transform.position.z, 0), DotVectorHeight);
            obj.transform.position += new Vector3(0, (DotVectorHeight * dot).y, 0);

            obj.transform.position += transform.position;
        }
	}


    /// <summary>
    /// Set the rotations  of the photos
    /// </summary>

    void SetRotations()
    {
        float angle = Mathf.PI * 2 / children.Count;

        for (int i = 0; i < children.Count; i++)
        {
            GameObject obj = children[i];
            float cos = Mathf.Cos(angle * i - Mathf.PI / 2);
            obj.transform.rotation = Quaternion.Euler(new Vector3(0, rotationY * Math.Sign(cos), rotationZ  * cos));
        }
    }
}
