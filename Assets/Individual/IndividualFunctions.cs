using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// TODO
/// </summary>
public static class IndividualFunctions
{
    #region file manipulation
    public static void saveToFile(string fileName)
    {

    }

    public static Individual loadFromFile(string fileName)
    {
        return null;
    }
    #endregion

    #region individual simulation
    /// <summary>
    /// This will create the rocket.
    /// The returned game object is what we need to apply physics to
    /// </summary>
    /// <returns></returns>
    public static GameObject Instantiate(Individual indiv)
    {
        return IndividualDatastructureFunctions.instantiate(indiv);
    }

    /// <summary>
    /// Call this after an individual is created, and it will instantiate, then score this individual for you
    /// </summary>
    public static GameObject Simulate(Individual indiv)
    {
        GameObject instanced = Instantiate(indiv);
        SimulationPhysics so = instanced.AddComponent<SimulationPhysics>();
        so.Init(indiv);
        return instanced;
    }


    /// <summary>
    /// Will alter the score of the individual passed in
    /// </summary>
    /// <param name="indiv"></param>
    /// <returns></returns>
    public static void ScoreIndividual(Individual indiv)
    {
        indiv.score = ((indiv.thrusterIndex.Count * (indiv.maxHeight + 1)) + indiv.timeInAir) / (indiv.weight * indiv.cost) * 1000;
    }
    #endregion
}
