using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class BlockManager : Singleton<BlockManager>
{
    /// <summary>
    /// This holds all the possible block types
    /// </summary>
    [Tooltip("The allowed blocks for any given individual. Here you can add your own game models")]
    public List<BlockType> blockTypes;

    
}

