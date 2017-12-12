using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class IndividualDatastructure
{
    public Dictionary<double, Block> contents;
    public List<double> openSpaces;

    public IndividualDatastructure()
    {
        contents = new Dictionary<double, Block>();
        openSpaces = new List<double>();
    }
}

