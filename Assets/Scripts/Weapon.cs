using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public AudioSource weapon;
    public AudioClip reloadingSE, fireSE, triggerSE;


    public static Weapon instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CanShoot()
    {
        GameState.canShoot = true;
    }

    public void FireSE()
    {
        weapon.clip = fireSE;
        weapon.Play();
    }

    public void ReloadingSE()
    {
        weapon.clip = reloadingSE;
        weapon.Play();

    }

    public void TriggerSE()
    {
        if (!weapon.isPlaying)
        {
            weapon.clip = triggerSE;
            weapon.Play();
        }
    }
}
