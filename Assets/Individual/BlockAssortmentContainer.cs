using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Container class for holding an assortment of blocks.
/// Useful because rocket hulls and payloads need to be differentiated, but hold the same intrinsic structure.
/// </summary>
public abstract class BlockAssortmentContainer
{
    /// <summary>
    /// The container of block assortments
    /// </summary>
    public IndividualDatastructure container;

    /// <summary>
    /// Base Constructor
    /// </summary>
    public BlockAssortmentContainer()
    {
    }
    /// <summary>
    /// When you have a given assortment.
    /// </summary>
    /// <param name="payload"></param>
    public BlockAssortmentContainer(IndividualDatastructure conts)
    {
        container = conts;
    }



}

