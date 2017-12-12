using System.Runtime.Serialization;
using UnityEngine;

public class MutationManager : Singleton<MutationManager>
{
    [Tooltip("The probability of any single block being created")]
    [Range(0,1)]
    public float additionProbability;

    [Tooltip("The probability of any single block being left alone")]
    [Range(0, 1)]
    public float doNothingProbability;

    [Tooltip("The probability that once a block has been selected to be mutated, we mutate it, or remove it entirely. The higher the number, the greater the likelihood of altering a block")]
    [Range(0,1)]
    public float alterOrDeleteProbability;

    [Tooltip("The maximum size of each generation of individuals")]
    [Range(1, 5000)]
    public int generationSize;

    [Range(1, 10)]
    [Tooltip("The number of times we iterate over an individual between generations. The greater this number, the more mutations occure.")]
    public int degreeToMutate;

    [Range(1, 20)]
    [Tooltip("The number of generations of nothing happening before the algorithm decides to start muating on a previous individual")]
    public int numOfGenerationsWithoutImprovement;
}

