using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class IndividualTest : MonoBehaviour
{

    public GameObject marker;
    public Material mat;

    Individual indiv;
    GameObject follow;
    void Start()
    {
        ///This is just to measure distance
        
        /*for(int i = -100; i < 100; i+=2)
        {
            GameObject mark = GameObject.Instantiate(marker);
            mark.transform.position = new Vector3(0, i, 20);
            if(i%10 == 0)
            {
                mark.GetComponent<MeshRenderer>().material = mat;
            }

        }
        */
        CustomEventHandler.onIndividualSimulationDone += this.endSim;
        Randomizer.newSeed();
        indiv = new Individual();
        /*
        follow = IndividualFunctions.Simulate(indiv);
        */
        follow = IndividualDatastructureFunctions.instantiate(indiv);
        follow.GetComponent<Rigidbody>().useGravity = false;
    }

    void endSim()
    {
        Debug.Log(indiv.score);
    }

    private void Update()
    {
        if (follow != null)
        {
            Camera.main.transform.position = follow.transform.position + new Vector3(0, 0, -20);
            Camera.main.transform.LookAt(follow.transform);
        }
    }


    
}

