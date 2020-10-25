using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static public Main S;
    static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;
    public GameObject[] prefabEnemies;
    public float enemySpawnPerSecond = 0.5f;
    public float enemyDefaultPadding = 1.5f;
    public WeaponDefinition[] weaponDefinitions;
    public GameObject prefabPowerUp;
    public WeaponType[] powerUpFrequency = new WeaponType[]
    {
        WeaponType.blaster, WeaponType.blaster,
        WeaponType.spread, WeaponType.shield
    };
    private BoundsCheck bndCheck;

    public void ShipDestroyed(Enemy e)
    {
        //ewentualnie utwórz obiekt wspomagający
        if (Random.value <= e.powerUpDropChance)
        {
            //wybierz rodzaj obiektu wzmacniającego, wybierz jedną z opcji dostepnych w tablicy powerUpFrequency
            int ndx = Random.Range(0, powerUpFrequency.Length);
            WeaponType puType = powerUpFrequency[ndx];
            //wygeneruj obiekt wzmacniający
            GameObject go = Instantiate(prefabPowerUp) as GameObject;
            PowerUp pu = go.GetComponent<PowerUp>();
            //przypisz mu właściwy typ WeaponType
            pu.SetType(puType);
            //umieśc go w miejscu zniszczenia statku
            pu.transform.position = e.transform.position;
        }
    }
    void Awake()
    {
        S = this;
        bndCheck = GetComponent<BoundsCheck>();
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach (WeaponDefinition def in weaponDefinitions)
        {
            WEAP_DICT[def.type] = def;
        }
    }
    public void SpawnEnemy()
    {
        //Wybierz dowolny prefabrykat Enemy w celu jesgo skonkretyzowania
        int ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);
        //Umieść statek wroga powyżej górnej krawędzi ekranu w losowym położeniu osi x
        float enemyPadding = enemyDefaultPadding;
        if (go.GetComponent<BoundsCheck>() != null)
        {
            enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }
        //Zdefiniuj położenie generowanego statku Enemy
        Vector3 pos = Vector3.zero;
        float xMin = -bndCheck.camWidth + enemyPadding;
        float xMax = bndCheck.camWidth - enemyPadding;
        pos.x = Random.Range(xMin, xMax);
        pos.y = bndCheck.camHeight + enemyPadding;
        go.transform.position = pos;
        //Ponownia wywołuje funkcję SpawnEnemy
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
    }
    public void DelayedRestart(float delay)
    {
        //wywołuje funkcję Restart() z opuźnieniem delay
        Invoke("Restart", delay);
    }
    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }
    static public WeaponDefinition GetWeaponDefinition(WeaponType wt)
    {
        if (WEAP_DICT.ContainsKey(wt))
        {
            return (WEAP_DICT[wt]);
        }
        //zwracamy nowy obiekt WeaponDefinition z typem WeaponType.none -> nie znaleziono odpowiedniej definicji WeaponDefinition
        return (new WeaponDefinition());
    }
}
