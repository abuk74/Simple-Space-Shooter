using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Enemy_2 : Enemy
{
    //określa poziom wpływu sinusoidy na ruch statku
    public float sinEccentricity = 0.6f;
    public float lifeTime = 10;
    public Vector3 p0;
    public Vector3 p1;
    public float birthTime;

    void Start()
    {
        //zdefiniuj jakieś miejsce po lewej stronie ekranu
        p0 = Vector3.zero;
        p0.x = -bndCheck.camWidth - bndCheck.radius;
        p0.y = Random.Range(-bndCheck.camHeight, bndCheck.camHeight);
        //zdefiniuj jakieś miejsce po prawej stronie ekranu
        p1 = Vector3.zero;
        p1.x = bndCheck.camWidth + bndCheck.camWidth;
        p1.y = Random.Range(-bndCheck.camHeight, bndCheck.camHeight);
        //w losowych przypadkach przenieś punkt na drugą stronę ekranu
        if (Random.value > 0.5f)
        {
            p0.x *= -1;
            p1.x *= -1;
        }
        birthTime = Time.time;
    }
    public override void Move()
    {
        float u = (Time.time - birthTime) / lifeTime;
        if (u > 1)
        {
            Destroy(this.gameObject);
            return;
        }
        u = u + sinEccentricity * (Mathf.Sin(u * Mathf.PI * 2)); //modyfikuje parametr "u" przez dodanie krzywej w kształcie U opartej na wykresie sin
        //oblicz interpolacje liniową
        pos = (1 - u) * p0 + u * p1;
    }
}
