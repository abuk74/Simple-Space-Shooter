using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    //===========Funkcje związane z materiałami================\\

    //zwraca listę wszystkich materiałów dla danego obiektu gry oraz jego potomków
    static public Material[] GetAllMaterials(GameObject go)
    {
        Renderer[] rends = go.GetComponentsInChildren<Renderer>();
        List<Material> mats = new List<Material>();
        foreach (Renderer rend in rends)
        {
            mats.Add(rend.material);
        }
        return (mats.ToArray());
    }

}
