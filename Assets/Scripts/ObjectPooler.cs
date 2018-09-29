using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {

    [SerializeField] GameObject prefab;
    [SerializeField] int maxAmount;
    [SerializeField] bool canGrow;

    #region RuntimeData

    IList<GameObject> stackObjects = new List<GameObject>();
    IList<GameObject> pooledObjects = new List<GameObject>();

    #endregion

    #region MonoBehaviour Methods 
    void Start()
    {
        for (int i = 0; i < maxAmount; i++)
        {
            GameObject obj = Instantiate(prefab, transform, false) as GameObject;

            stackObjects.Add(obj);

            obj.SetActive(false);
        }
    }

    #endregion

    public void Clean()
    {
        foreach (GameObject obj in pooledObjects)
        {
            IObjectPooled script = obj.GetComponent<IObjectPooled>(); 
            script.Clean();
            obj.SetActive(false);
        }

        pooledObjects.Clear();
    }

    public GameObject GetPooledObject()
    {
        foreach (GameObject obj in stackObjects)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);

                pooledObjects.Add(obj);

                return obj;
            }
        }

        if (canGrow)
        {
            GameObject obj = Instantiate(prefab, transform, false) as GameObject;

            stackObjects.Add(obj);
            pooledObjects.Add(obj);

            obj.SetActive(true);
            return obj;
        }

        return null;
    }
}
