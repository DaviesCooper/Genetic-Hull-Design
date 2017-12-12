using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

///Only want this to exist as a static class
public class IndividualManager : Singleton<IndividualManager>
{
    /// <summary>
    /// The force the fuel outputs per 1m^3
    /// </summary>
    [Tooltip("The amount of force your fuel outputs at each time step delta")]
    [Range(1, 1000000)]
    public float fuelForce;

    /// <summary>
    /// The amount of fuel consumed per force added (optimally occurs per frame)
    /// </summary>
    [Tooltip("The amount of fuel that is consumed per time step delta")]
    [Range(0, 100)]
    public float fuelConsumption;

    /// <summary>
    /// The maximum number of blocks allowed in an individual
    /// </summary>
    [Tooltip("How large you want the rockets to become")]
    [Range(1, 2000)]
    public int maxIndividualSize;

    [Tooltip("The size of the originally random payload")]
    public int payloadSize;

    [Tooltip("The number of blocks contained within any newly created individual")]
    [Range(1, 500)]
    public int startIndividualSize;

}

