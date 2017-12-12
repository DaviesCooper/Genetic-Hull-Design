using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Generation
{
    public int genNum;
    public List<Individual> currentGen;
    public List<Individual> bestMap;
    public Individual currentBest;

    public Generation(Payload payload)
    {
        genNum = 0;
        currentGen = new List<Individual>();
        bestMap = new List<Individual>();
        currentBest = null;
        for(int i = 0; i < MutationManager.Instance.generationSize; i++)
        {
            currentGen.Add(new Individual(payload));
        }
    }

    public Generation() : this(new Payload())
    {
    }
}

