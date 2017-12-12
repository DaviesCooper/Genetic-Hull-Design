using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CustomEventHandler : Singleton<CustomEventHandler>
{
    public static bool locked = false;
    #region event handling for when a signle simulation ends
    public delegate void SimulationEndEventHandler();

    public static event SimulationEndEventHandler onIndividualSimulationDone;

    public static void SelfDestruct(GameObject killing)
    {
        UnityEngine.Object.Destroy(killing);
        onIndividualSimulationDone();

    }
    #endregion

    public delegate void GenerationEndEventHandler();

    public static event GenerationEndEventHandler onGenerationEnd;

    public static void EndGeneration()
    {
        onGenerationEnd();
    }
}
