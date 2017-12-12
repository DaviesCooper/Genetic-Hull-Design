using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class GenerationFunctions
{
    static Stack<Individual> toBeDone;
    static int numberSimulated;
    static Generation currentlyWorking;
    public static bool continueSimulation = true;
    static float lastBest = 0;
    static int genNumberWithSame = 0;


    public static void init(Generation g)
    {
        currentlyWorking = g;
        CustomEventHandler.onIndividualSimulationDone += doNextSim;

        ///Creating a stack of individuals we still need to score;
        toBeDone = new Stack<Individual>();
        foreach (Individual indi in currentlyWorking.currentGen)
        {
            toBeDone.Push(indi);
        }
    }

    public static void performSearch()
    {
        if (continueSimulation)
        {
            foreach (Individual indi in currentlyWorking.currentGen)
            {
                toBeDone.Push(indi);
            }
            createNextGen();
        }
        else
        {
            GameObject.FindObjectOfType<GenerationTest>().finish(currentlyWorking);         
        }
    }


    /// <summary>
    /// We want to simulate each individual in this generation
    /// </summary>
    /// <param name="g"></param>
    public static void createNextGen()
    {  
        IndividualFunctions.Simulate(toBeDone.Pop());
    }

    /// <summary>
    /// As long as we have individuals we need to simulate, we need to simulate them
    /// otherwise we signal the event handler that all the simulations have finished
    /// </summary>
    private static void doNextSim()
    {
        if (numberSimulated < MutationManager.Instance.generationSize - 1)
        {
            if(toBeDone.Count < 1)
            {
                return;
            }
            numberSimulated++;
            IndividualFunctions.Simulate(toBeDone.Pop());
        }
        else
        {
            numberSimulated = 0;
            finishGenerationSimulation();
        }
    }
    
    /// <summary>
    /// We are done simulating so we need to score, and move on to culling/replacing
    /// </summary>
    private static void finishGenerationSimulation()
    {
        foreach (Individual i in currentlyWorking.currentGen)
        {
            if (currentlyWorking.currentBest == null || currentlyWorking.currentBest.score < i.score)
            {
                currentlyWorking.currentBest = i;
                Debug.Log("Score :" + currentlyWorking.currentBest.score);
                currentlyWorking.bestMap.Add(i);
            }
        }
        cullGeneration();
        mutateGeneration();
        ///We are now a new generation
        currentlyWorking.genNum++;
        Debug.Log("Generation " + currentlyWorking.genNum);
        performSearch();
    }


    /// <summary>
    /// Replaces all the individuals in the generation with the current best
    /// </summary>
    /// <param name="g"></param>
    private static void cullGeneration()
    {
        if(lastBest == currentlyWorking.currentBest.score)
        {
            genNumberWithSame++;
        }
        else
        {
            genNumberWithSame = 0;
            lastBest = currentlyWorking.currentBest.score;
        }

        if(genNumberWithSame > MutationManager.Instance.numOfGenerationsWithoutImprovement)
        {
            if(currentlyWorking.bestMap.Count != 1)
            {
                //Increases the chance of mutation
                MutationManager.Instance.doNothingProbability *= 0.8f;
                currentlyWorking.bestMap.RemoveAt(currentlyWorking.bestMap.Count - 1);
                currentlyWorking.currentBest = currentlyWorking.bestMap[currentlyWorking.bestMap.Count - 1];
                lastBest = 0;
                genNumberWithSame = 0;
            }
            
        }
        currentlyWorking.currentGen = new List<Individual>();
        for (int i = 0; i < MutationManager.Instance.generationSize; i++)
        {
            currentlyWorking.currentGen.Add(new Individual(currentlyWorking.currentBest));
        }
    }


    /// <summary>
    /// Now we need to mutate all the individuals a bit
    /// </summary>
    private static void mutateGeneration()
    {
        foreach (Individual indi in currentlyWorking.currentGen)
        {
            mutateIndividual(indi);
        }
    }


    private static void mutateIndividual(Individual indi)
    {
        alterPieces(indi);
        addPieces(indi);
    }

    private static void alterPieces(Individual indiv)
    {
        ///For each pass defined by degree of mutation
        for (int i = 0; i < MutationManager.Instance.degreeToMutate; i++)
        {
            List<double> toDelete = new List<double>();
            Dictionary<double, Block> replaceAtWith = new Dictionary<double, Block>();

            ///for each block
            foreach (double b in indiv.hull.container.contents.Keys)
            {
                double rand = Randomizer.random.NextDouble();
                ///WE DO NOT ALTER THE PAYLOAD NOR IGNORED BLOCKS
                if (indiv.payload.container.contents.Keys.Contains(b))
                {
                    continue;
                }
                if(rand < MutationManager.Instance.doNothingProbability)
                {
                    continue;
                }
                rand = Randomizer.random.NextDouble();
                ///If we should alter it
                if (Randomizer.random.NextDouble() < MutationManager.Instance.alterOrDeleteProbability)
                {
                    ///RANDOMIZE BLOCK
                    Block newBlock = BlockFunctions.generateRandomBlock();
                    replaceAtWith.Add(b, newBlock);
                }
                ///or delete it
                else
                {
                    toDelete.Add(b);
                }
            }
            foreach (double b in toDelete)
            {
                indiv.hull.container.openSpaces.Add(b);
                indiv.hull.container.contents.Remove(b);
            }
            foreach(double b in replaceAtWith.Keys)
            {
                indiv.payload.container.contents[b] = replaceAtWith[b];
            }
        }

    }

    private static void addPieces(Individual indiv)
    {
        List<double> insert = new List<double>();
        foreach(double b in indiv.hull.container.openSpaces)
        {
            ///We have already added enough blocks
            if (indiv.hull.container.contents.Count >= IndividualManager.Instance.maxIndividualSize)
            {
                continue;
            }
            if(Randomizer.random.NextDouble() < MutationManager.Instance.additionProbability)
            {
                insert.Add(b);
            }
        }
        ///Doing it this way because we cannot alter a structure while iterating through it
        foreach(double inserting in insert)
        {
            BlockFunctions.insertBlockAtRandom(indiv.hull.container);
        }
    }


}

