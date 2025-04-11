using System.Collections.Generic;
using UnityEngine;


public static class ExtensionMethods
{

    public static List<GameObject> AddAllCollidersToList(this List<GameObject> list, Collider[] Colliders)
    {

        foreach (Collider collider in Colliders)
        {
            if (collider == null) continue;
            if(list.Contains(collider.gameObject)) continue;    
            list.Add(collider.gameObject);
        }
        return list;
    }
}