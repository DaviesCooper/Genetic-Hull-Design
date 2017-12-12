using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Payload : BlockAssortmentContainer
{
    /// <summary>
    /// Generates a random payload of size 20
    /// </summary>
    public Payload()
    {
        container = PayloadFunctions.createRandomPayload(IndividualManager.Instance.payloadSize);
    }

    /// <summary>
    /// Generates a random payload of given size
    /// </summary>
    /// <param name="size"></param>
    public Payload(int size)
    {
        container = PayloadFunctions.createRandomPayload(size);
    }

    /// <summary>
    /// Creates a container surrounding the IndividualDataStructure passed in
    /// </summary>
    /// <param name="alreadyCreated"></param>
    public Payload(IndividualDatastructure alreadyCreated)
    {
        this.container = alreadyCreated;
    }
}
