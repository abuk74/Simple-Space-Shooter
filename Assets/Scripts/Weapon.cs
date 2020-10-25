using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

///<summary>
///typ wyliczeniowy dla wszystkich możliwych typów bronii
/// </summary>

public enum WeaponType
{
    none,       //stan domyślny - brak broni
    blaster,    //prosty blaster
    spread,     //możliwość oddania dwóch strzałów jednocześnie
    phaser,     //nie implementowane
    missile,    //nie implementowane
    laser,      //nie implementowane
    shield      //zwiększenie poziomu ochrony dla tarczy
}
///<Summary>
///klasa WeaponDefinition pozwala na ustawiebie w panelu inspekcyjnym parametrów określonego typu broni
///klasa Main zawiera tablicę obiektów WeaponDefinition, dzięki czemu powyższa funkcjonalnośc jest dostępna
/// </summary>
[System.Serializable]
public class WeaponDefinition
{
    public WeaponType type = WeaponType.none;
    public string letter; //litera, która pojawi się na obiekcie wzmacniającym
    public Color color = Color.white; //kolor ramki obiektu wzmacniającego
    public GameObject projectilePrefab;
    public Color projectileColor = Color.white;
    public float damageOnHit = 0; //liczba spowodowanych uszkodzeń
    public float continuosDamage = 0; //liczba uszkodzeń na sekundę (Laser)
    public float delayBetweenShots = 0;
    public float velocity = 20; //prędkośc pocisków
}
public class Weapon : MonoBehaviour
{
    static public Transform PROJECTILE_ANCHOR;
    [Header("Definiowane dynamicznie")]
    [SerializeField]
    private WeaponType _type = WeaponType.none;
    public WeaponDefinition def;
    public GameObject collar;
    public float lastShotTime; //czas, w którym oddano ostatni strzał
    private Renderer collarRend;

    void Start()
    {
        collar = transform.Find("Collar").gameObject;
        collarRend = collar.GetComponent<Renderer>();
        //wywałaj funkcję SetType() dla dowolnego dla dowolnego typu _type lub WeaponType.none 
        SetType(_type);
        //utwórz dynamicznie punkt mocowania dla wszystkich typów pocisków
        if (PROJECTILE_ANCHOR == null)
        {
            GameObject go = new GameObject("_ProjectileAnchor");
            PROJECTILE_ANCHOR = go.transform;
        }
        //znajdź delegata fireDelegate dla głównego obiektu gry
        GameObject rootGO = transform.root.gameObject;
        if (rootGO.GetComponent<Hero>() != null)
        {
            rootGO.GetComponent<Hero>().fireDelegate += Fire;
        }
    }
    public WeaponType type
    {
        get { return (_type); }
        set { SetType(value); }
    }
    public void SetType(WeaponType wt)
    {
        _type = wt;
        if (type == WeaponType.none)
        {
            this.gameObject.SetActive(false);
            return;
        }
        else
        {
            this.gameObject.SetActive(true);
        }
        def = Main.GetWeaponDefinition(_type);
        collarRend.material.color = def.color;
        lastShotTime = 0; //możesz strzelać, gdy tylko pole _type zostanie zainicjowane
    }
    public void Fire()
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }
        if (Time.time - lastShotTime < def.delayBetweenShots)
        {
            return;
        }
        Projectile p;
        Vector3 vel = Vector3.up * def.velocity;
        if (transform.up.y < 0)
        {
            vel.y = -vel.y;
        }
        switch (type)
        {
            case WeaponType.blaster:
                p = MakeProjectile();
                p.rigid.velocity = vel;
                break;
            case WeaponType.spread:
                p = MakeProjectile();
                p.rigid.velocity = vel;
                p = MakeProjectile();
                p.transform.rotation = Quaternion.AngleAxis(10, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                p = MakeProjectile();
                p.transform.rotation = Quaternion.AngleAxis(-10, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                break;
            default:
                break;
        }
    }
    public Projectile MakeProjectile()
    {
        GameObject go = Instantiate<GameObject>(def.projectilePrefab);
        if (transform.parent.gameObject.tag == "Hero")
        {
            go.tag = "ProjectileHero";
            go.layer = LayerMask.NameToLayer("ProjectileHero");
        }
        else
        {
            go.tag = "ProjectileEnemy";
            go.layer = LayerMask.NameToLayer("ProjectileEnemy");
        }
        go.transform.position = collar.transform.position;
        go.transform.SetParent(PROJECTILE_ANCHOR, true);
        Projectile p = go.GetComponent<Projectile>();
        p.type = type;
        lastShotTime = Time.time;
        return (p);
    }
}
