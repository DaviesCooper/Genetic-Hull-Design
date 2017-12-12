using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class GenerationTest : MonoBehaviour
{

    Generation gen;
    GameObject follow;


    bool finished = false;

    void Start()
    {
        Randomizer.newSeed();
        gen = new Generation();
        GenerationFunctions.init(gen);
        GenerationFunctions.createNextGen();
        Camera.main.transform.position = new Vector3(25, 25, 25);
        Camera.main.transform.LookAt(Vector3.zero);

    }

    public void finish(Generation gen)
    {

        Individual best = gen.currentBest;
        Debug.Log("Cost : "+best.cost);
        Debug.Log("Fuel (m^3) : " + best.fuelVolume);
        Debug.Log("Weight : "+best.weight);
        Debug.Log("Score : " + best.score);
        Debug.Log("Height : " + best.maxHeight);
        Debug.Log("Time in Air : " + best.timeInAir);

        Individual copy = new Individual(best);

        GameObject bestObject = IndividualFunctions.Instantiate(copy);
        UnityEngine.Object.Destroy(bestObject.GetComponent<Rigidbody>());
        follow = bestObject;

    }

    private void Update()
    {
        if (follow != null)
        {
            Camera.main.transform.position = follow.transform.position + new Vector3(0, 0, -20);
            Camera.main.transform.LookAt(follow.transform);
        }
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (GenerationFunctions.continueSimulation)
            {
                Debug.Log("Simulation ending. Please wait for the current generation to finish simulating in order to receive the results");
                
            }
            else
            {
                Debug.Log("Resuming Simulation"); 
            }
            GenerationFunctions.continueSimulation = !GenerationFunctions.continueSimulation;
        }
    }



}

