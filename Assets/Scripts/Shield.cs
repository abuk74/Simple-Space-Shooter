using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public float rotationsPerSecond = 0.1f;
    public int levelShown = 0;
    Material mat;
    
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    
    void Update()
    {
        //Odczytaj bieżący poziom tarczy z singletona obiektu hero
        int currLevel = Mathf.FloorToInt(Hero.S.shieldLevel);
        if (levelShown != currLevel)
        {
            levelShown = currLevel;
            //wybierz fragment tekstury odpowiadający innemu fragmentowi tarczy
            mat.mainTextureOffset = new Vector2(0.2f * levelShown, 0);
        }
        //obracanie tarczy
        float rZ = -(rotationsPerSecond * Time.time * 360) % 360f;
        transform.rotation = Quaternion.Euler(0,0,rZ);
    }
}
