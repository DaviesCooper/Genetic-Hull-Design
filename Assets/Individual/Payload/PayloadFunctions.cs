using System;
using System.Collections.Generic;
using UnityEngine;

public static class PayloadFunctions
{
    /// <summary>
    /// Creates a random payload of blocks constrained by the block manager. Payloads are by default
    /// created to be a size of 20 when randomly created
    /// </summary>
    /// <returns></returns>
    public static IndividualDatastructure createRandomPayload(int payloadsize)
    {
        ///So now that we've added our open space, we attempt to insert random blocks
        int count = 0;

        IndividualDatastructure payload = new IndividualDatastructure();
        ///Here we need to init an open space. I want this to be at the centre of the 3D space
        int centre = IndividualManager.Instance.maxIndividualSize / 2;

        ///Calculating the offset of the centre
        double offset = BlockFunctions.calculateOffset(centre, centre, centre);
        payload.openSpaces.Add(offset);

        ///Here we grab the payload type that is tagged as such
        BlockType type = null;
        type = BlockManager.Instance.blockTypes[BlockFunctions.findBlockID("payload")];
        if (type == null)
        {
            throw new Exception("There was no block of type payload defined");
        }

        payloadsize = Math.Min(payloadsize, IndividualManager.Instance.startIndividualSize);
        ///While we have less blocks than desired, and less failed consecutive attempts than defined
        while (count < payloadsize && count < IndividualManager.Instance.startIndividualSize)///This number may need to be tweaked. I feel 200 consecutive failed attempts is enough though
        {
            if(BlockFunctions.insertBlockTypeAtRandom(payload, type))
            {
                count++;
            }            
        }
        Debug.Log("Payload created with size of " + count);
        return payload;
    }
}

