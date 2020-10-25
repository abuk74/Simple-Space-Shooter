using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : Enemy
{
    //liczba sekund potrzebna do wygenerowania pełnej sinusoidy
    public float waveFrequency = 2;
    //długość sinusoidy w metrach
    public float waveWidth = 4;
    public float waveRotY = 45;
    private float xO; //początkowa wartość położenia na osi x
    private float birthTime;
    //funkcja start działa poprawnie ponieważ nie jest używana przez klasę nadrzędna Enemy


    void Start()
    {
        //przypisz początkoqwe położenie Enemy_1
        xO = pos.x;
        birthTime = Time.time;
    }

    //zastępuje funkcję Move klasy Enemy
    public override void Move()
    {
        Vector3 tempPos = pos;
        //modyfikacja zmienne theta w oparciu o czas
        float age = Time.time - birthTime;
        float theta = Mathf.PI * 2 * age / waveFrequency;
        float sin = Mathf.Sin(theta);
        tempPos.x = xO + waveWidth * sin;
        pos = tempPos;
        //obróc trochę wokół osi Y
        Vector3 rot = new Vector3(0, sin * waveRotY, 0);
        this.transform.rotation = Quaternion.Euler(rot);
        base.Move();
    }

}
