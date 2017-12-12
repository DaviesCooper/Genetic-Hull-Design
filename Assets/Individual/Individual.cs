using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

/// <summary>
/// This class is the definition for what constitutes an "individual" of our problem.
/// Each individual is an attempt at our A.I to create a viable solution for an efficient
/// rocket. This means that generations are bags of individuals, and individuals are what
/// get scored on for fitness. This is the framework of the project.
/// </summary> 
public class Individual{

    #region variables
    /// <summary>
    /// The Whole ship of this individual.
    /// </summary>   
    public RocketHull hull;
    /// <summary>
    /// The payload of this individual. This exists solely for remembering the payload when saving
    /// </summary> 
    public Payload payload;
    /// <summary>
    /// The index of the thrusters on this individual.
    /// This exists so that we can find their locations, and apply the force
    /// amongst them, at the location, in the direction they are facing
    /// </summary>
    public List<GameObject> thrusterIndex;
    /// <summary>
    /// The index of the displayed models
    /// of this individual. This exists so we can find them and apply drag to them
    /// </summary>
    public List<GameObject> exteriorIndex;
    /// <summary>
    /// The cost of the whole individual. Useful for quickly scoring an individual
    /// </summary>
    public int cost;
    /// <summary>
    /// The weight of the whole individual. Useful for quickly scoring an individual
    /// </summary>
    public int weight;
    /// <summary>
    /// The cost of the whole individual. Useful for quickly culling/performing generation
    /// calculations
    /// </summary>    
    public float score;
    /// <summary>
    /// I have decided to use this after all
    /// </summary>
    public float fuelVolume;
    /// <summary>
    /// Used for calculating score
    /// </summary>
    public float maxHeight;
    public float timeInAir;
    #endregion


    /// <summary>
    /// Creates an individual with a passed in hull and payload.
    /// </summary>
    /// <param name="hull"></param>
    /// <param name="payload"></param>
    public Individual(RocketHull hull, Payload payload)
    {
        this.hull = hull;
        this.payload = payload;
        this.cost = 0;
        this.score = 0;
        this.thrusterIndex = new List<GameObject>();
        this.exteriorIndex = new List<GameObject>();
        this.fuelVolume = 0;
        this.timeInAir = 0;

    }

    /// <summary>
    /// This will generate an individual with a random rocket hull.
    /// NOTE that this will not return you a payload so it will be impossible
    /// to recover it at a later time.
    /// </summary>
    public Individual()
    {

        this.payload = new Payload();
        this.hull = new RocketHull(payload);
        this.cost = 0;
        this.score = 0;
        this.thrusterIndex = new List<GameObject>();
        this.exteriorIndex = new List<GameObject>();
        this.fuelVolume = 0;
        this.timeInAir = 0;
    }

    /// <summary>
    /// This will generate an individual with a random rocket hull.
    /// NOTE that this will not return you a payload so it will be impossible
    /// to recover it at a later time.
    /// </summary>
    public Individual(Payload p)
    {

        this.payload = p;
        this.hull = new RocketHull(p);
        this.cost = 0;
        this.score = 0;
        this.thrusterIndex = new List<GameObject>();
        this.exteriorIndex = new List<GameObject>();
        this.fuelVolume = 0;
        this.timeInAir = 0;
    }

    public Individual(Individual copy)
    {
        this.payload = new Payload(copy.payload.container);
        this.hull = new RocketHull(copy.hull.container);
        this.cost = copy.cost;
        this.score = copy.score;
        this.thrusterIndex = new List<GameObject>();
        this.exteriorIndex = new List<GameObject>();
        this.fuelVolume = copy.fuelVolume;
        this.timeInAir = copy.timeInAir;

    }




}