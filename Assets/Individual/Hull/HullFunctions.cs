using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class HullFunctions
{
    /// <summary>
    /// Creates a random payload of blocks constrained by the block manager. Payloads are by default
    /// created to be a size of 20 when randomly created
    /// </summary>
    /// <returns></returns>
    public static IndividualDatastructure createRandomHull(IndividualDatastructure hull, int hullSize)
    {
        int count = 0;
        ///Here i'm making sure that we don't fall into an infinite loop of trying to place blocks when it's not possible
        ///Normally this wouldn't be an issue, but because users can input their own blocks
        ///I need to safeguard against assholes.
        hullSize = Math.Min(hullSize, IndividualManager.Instance.startIndividualSize);
        ///While we have less blocks than desired, and less failed consecutive attempts than defined
        while (count < hullSize && count < IndividualManager.Instance.startIndividualSize)///This number may need to be tweaked. I feel 200 consecutive failed attempts is enough though
        {
            if (BlockFunctions.insertBlockAtRandom(hull))
            {
                count++;
            }
        }
        return hull;
    }

    internal static IndividualDatastructure copyPayload(IndividualDatastructure container)
    {
        IndividualDatastructure ret = new IndividualDatastructure();
        ret.contents = new Dictionary<double, Block>(container.contents);
        ret.openSpaces = new List<double>(container.openSpaces);
        return ret;
    }
}

