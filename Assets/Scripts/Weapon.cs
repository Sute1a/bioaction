using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public AudioSource weapon;
    public AudioClip reloadingSE, fireSE, triggerSE;


    public static Weapon instance;

    public Transform shotDirection;

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
        //Debug.DrawRay(shotDirection.position, shotDirection.transform.forward * 10, Color.green);
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

    public void Shooting()
    {
        RaycastHit hitInfo;

        if(Physics.Raycast(shotDirection.transform.position,transform.forward,out hitInfo, 300))
        {

            GameObject hitGameobject = hitInfo.collider.gameObject;

            if(hitInfo.collider.gameObject.GetComponent<ZombieController>() != null)
            {
                ZombieController hitZombie = hitInfo.collider.gameObject.GetComponent<ZombieController>();


                if (Random.Range(0, 10) < 11)
                {
                    hitZombie.ZombieDeath();

                    GameObject rdPrefab = hitZombie.ragdoll;

                    GameObject NewRD = Instantiate(rdPrefab, hitGameobject.transform.position, hitGameobject.transform.rotation);

                    NewRD.transform.Find("Hips").GetComponent<Rigidbody>().AddForce(shotDirection.transform.forward * 1000);

                    Destroy(hitGameobject);
                }
                else
                {
                    hitZombie.ZombieDeath();
                }
            }
        }
    }
}
