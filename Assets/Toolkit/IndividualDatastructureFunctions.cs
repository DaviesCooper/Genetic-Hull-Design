using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

static class  IndividualDatastructureFunctions
{
    /// <summary>
    /// Places the blocks within this assortment into 3D space
    /// Uses the optimized block creation for deciding whether or not to render a block/ give it a collider
    /// This will also alter the individual by locating the centre of gravity, 
    /// creating a block there with a mass of all the un-rendered blocks,
    /// then jointing the centre of gravity with all the rendered blocks.
    /// This also semi-optimizes the addition of the thrusters to the thruster
    /// index in the individual by performing that within the loop we are already doing.
    /// It does the same with the fuel amount.
    /// </summary>
    /// <param name="indiv"></param>
    /// </summary>
    public static GameObject instantiate(Individual passedIN)
    {
        ///Temporary variables for instantiating the Block structure as a unity 3-D object
        BlockManager blockMan = BlockManager.Instance;
        List<BlockType> blockTypes = blockMan.blockTypes;
        int centre = IndividualManager.Instance.maxIndividualSize / 2;
        float centreWeight = 0;
        Vector3 COGPosition = Vector3.zero;
        int COGWeight = 0;
        Dictionary<double, Block> contents = passedIN.hull.container.contents;

        GameObject parent = new GameObject();
        parent.transform.position = new Vector3(centre, centre, centre);

        ///Iterate through all our positions/weights to find the centre of gravity
        foreach (double index in contents.Keys)
        {
            int blockWeight = blockTypes[contents[index].type].weight;
            int blockType = contents[index].type;
            int blockCost = blockTypes[contents[index].type].weight;

            ///Adding our values to the individual values
            passedIN.weight += blockWeight;
            passedIN.cost += blockCost;
            if (blockType == BlockFunctions.findBlockID("fuel"))
            {
                passedIN.fuelVolume++;
            }

            ///Create the 3D block
            GameObject created = BlockFunctions.createBlock(index, contents);


            ///Calculate the centre of gravity location
            COGPosition += (created.transform.position * blockWeight);
            centreWeight += blockWeight;

            ///If creation was allowed, we;
            if (created.GetComponent<MeshRenderer>().enabled)
            {
                ///Additionally, I optimize setting the thruster index here because we are already iterating over each block anyway
                if (contents[index].type == BlockFunctions.findBlockID("thruster"))
                {
                    passedIN.thrusterIndex.Add(created);
                }
                else
                {
                    passedIN.exteriorIndex.Add(created);
                }
                created.transform.SetParent(parent.transform);
            }
            ///If we do not render physically;
            else
            {
                ///Increment the COGWeight
                COGWeight += blockTypes[contents[index].type].weight;
                ///And destroy the block for it
                UnityEngine.Object.Destroy(created);
            }

        }

        ///After doing the physics for each block
        ///make sure our weight is non-zero
        if (passedIN.weight <= 0)
        {
            passedIN.weight = 1;
        }


        ///This is our centre of gravity. Now I need to create a block there, and set it's weight;
        COGPosition = COGPosition / centreWeight;
        parent.transform.position = Vector3.zero;

        Rigidbody rigid = parent.AddComponent<Rigidbody>();
        rigid.centerOfMass = COGPosition;
        rigid.mass = passedIN.weight;
        rigid.drag = 0;


        return parent;
    }
}

