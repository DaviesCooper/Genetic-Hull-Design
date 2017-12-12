using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class RocketHull: BlockAssortmentContainer
{
    /// <summary>
    /// Creates a rocket hull of given size around a given payload
    /// </summary>
    /// <param name="p"></param>
    /// <param name="size"></param>
    public RocketHull(Payload p, int size)
    {
        container = HullFunctions.copyPayload(p.container);
        container = HullFunctions.createRandomHull(container, size);
    }

    /// <summary>
    /// Creates a rocket of max individual size around a given payload
    /// </summary>
    /// <param name="p"></param>
    public RocketHull(Payload p)
    {
        container = HullFunctions.copyPayload(p.container);
        container = HullFunctions.createRandomHull(container, IndividualManager.Instance.maxIndividualSize);
    }

    /// <summary>
    /// Creates the rocket hull container around the passed in IndividualDataStructure
    /// </summary>
    /// <param name="alreadyCreated"></param>
    public RocketHull(IndividualDatastructure alreadyCreated)
    {
        container = HullFunctions.copyPayload(alreadyCreated);
    }

}
