using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private BoundsCheck bndCheck;
    private Renderer rend;
    public Rigidbody rigid;
    [SerializeField]
    private WeaponType _type;
    //ta publiczna właściwość zastępuje pole _type i wykonuje odpowiednie działanie, gdy jest ono modyfikowane
    public WeaponType type
    {
        get
        {
            return (_type);
        }
        set
        {
            SetType(value);
        }
    }
    void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        rend = GetComponent<Renderer>();
        rigid = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (bndCheck.offUp)
        {
            Destroy(gameObject);
        }
    }
    ///<summary>
    ///przypisuje wartość do prywatnego pola _type oraz zmienia kolor pocisku,
    ///aby odpowiadał wpisowi w WeaponDefinition
    /// </summary>
    public void SetType(WeaponType eType)
    {
        //ustaw pole _type
        _type = eType;
        WeaponDefinition def = Main.GetWeaponDefinition(_type);
        rend.material.color = def.projectileColor;
    }
}
