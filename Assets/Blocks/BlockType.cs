using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
///Class dictating what attributes each type of block can fulfill without being instantiated
[System.Serializable]
public class BlockType
{
    /// <summary>
    /// The unique id for this type of block
    /// </summary>
    public int UID;
    /// <summary>
    /// The name of this type of block
    /// </summary>
    public string Name;
    /// <summary>
    /// The cost this type of block induces per m^3
    /// </summary>
    [Range(0,9999999)]
    public int cost;
    /// <summary>
    /// the weight this type of block creates per m^3
    /// </summary>
    /// [Range(0,int.MaxValue)]
    [Range(0,9999999)]
    public int weight;
    /// <summary>
    /// The model this block uses (REQUIRED FOR PERFORMING THE PHYSICS
    /// </summary>
    public UnityEngine.GameObject model;
}

