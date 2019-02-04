using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsSystem : MonoBehaviour {


    protected Transform t;
    [HideInInspector] public MovementSystem ms;

    [Space(10)]

    [SerializeField] private AudioSource hitAud;
    [SerializeField] private AudioSource deathAud;


    [Space(10)]


    [HideInInspector] public bool isDead;
    [HideInInspector] public bool isOnAcid;

    public float maxHealth;
    public float currentHealth;
    private Mesh mesh;


    public float kills = 0f;
    public float deaths = 0f;

    [Space(10)]

    [SerializeField] private ObjectPooler pool;
    [SerializeField] private string[] prefabsToSpawnOnDeath;

    




    // Use this for initialization
    void Start() {

        t = transform;
        ms = GetComponent<MovementSystem>();
        pool = ObjectPooler.instance;

        currentHealth = maxHealth;
    }








    public void TakeDmg(int dmg, int bulletID)
    {
        if (isDead || ms.joueurID == bulletID)
            return;


        hitAud.Play();

        ScoreManager.instance.UpdateHealthUI(ms.joueurID);

        currentHealth -= dmg;

        if (currentHealth <= 0)
        {
            currentHealth = 0;

            //PlayerManager.instance.GainExpForPlayer(bulletID);
            ScoreManager.instance.UpdateScore(ms.joueurID, bulletID);
            Die();
        }
    }

    public void KillPlayer(int bulletID)
    {
        if (isDead || ms.joueurID == bulletID)
            return;
        

        ScoreManager.instance.UpdateHealthUI(ms.joueurID);

        currentHealth = 0;

        //PlayerManager.instance.GainExpForPlayer(bulletID);
        ScoreManager.instance.UpdateScore(ms.joueurID, bulletID);
        Die();
    }









    public IEnumerator ApplyAcidDmg(int acidID, float acidDuration, int dmg)
    {
        isOnAcid = true;
        float t = 0f;

        while (t < acidDuration)
        {
            t += .3f;
            yield return new WaitForSeconds(.3f);
            TakeDmg(dmg, acidID);
        } 

        isOnAcid = false;
    }





    private void Die()
    {


        deathAud.Play();


        //print(gameObject.name + " is dead");
        isDead = true;
        mesh = GetComponent<MeshFilter>().mesh;
        GetComponent<MeshFilter>().mesh = null;
        

        ms.lastWeapon = ms.currentWeapon;

        ms.currentWeapon = ms.weaponPos.GetChild(0).GetComponent<Weapon>();
        ms.DropLastWeapon();



        for (int i = 0; i < prefabsToSpawnOnDeath.Length; i++)
        {
            pool.SpawnFromPool(prefabsToSpawnOnDeath[i], t.position, Quaternion.identity);
        }


        ScoreManager.instance.UpdateHealthUI(ms.joueurID);

    }


    public void Reset()
    {
        ms.currentWeapon.gameObject.SetActive(true);
        

        isDead = false;
        currentHealth = maxHealth;
        GetComponent<MeshFilter>().mesh = mesh;
    }


    public float GetKDR()
    {
        if(deaths == 0)
        {
            return kills;
        }
        else
        {
            if(deaths > kills)
            {
                if(kills == 0)
                {
                    return -1f;
                }
                else
                {
                    return (Mathf.Round((kills / deaths) * 100f)) / 100f;
                }
            }
            else if(Mathf.Approximately(deaths, kills))
            {
                return 0f;
            }
            else
            {
                return (Mathf.Round((kills / deaths) * 100f)) / 100f;
                // return 1 - (Mathf.Round(((1 / kills) * (kills-deaths)) * 100f)) / 100f;
            }
        }
    }
}

