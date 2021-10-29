using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieGenerator : MonoBehaviour
{
    NavMeshAgent agent;

    public GameObject player;
    public GameObject zombiePrefabA;
    public GameObject zombiePrefabB;
    float span =1.0f;
    float delta = 0;
    int ratio = 5;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        this.delta += Time.deltaTime;
        if(this.delta>this.span)
        {
            this.delta = 0;
            GameObject aimer;
            int dice = Random.Range(1, 11);
            if (dice <= this.ratio)
            {
                aimer = Instantiate(zombiePrefabA);
            }
            else
            {
                aimer = Instantiate(zombiePrefabB);
            }
            float x = Random.Range(3,100);
            float z = Random.Range(2,100);
            aimer.transform.position = new Vector3(x, 0, z);
            if (aimer.transform.position =NavMesh.AllAreas)

            {

            }
        }


    }
}
