using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// This class exists solely so that we can use the same seed to study certain effects at runtime
/// </summary>
public static class Randomizer
{
    public static Random random;
    public static int seed;

    public static void setSeed(int newSeed)
    {
        seed = newSeed;
        random = new Random(seed);
    }

    public static int getSeed()
    {
        return seed;
    }

    public static void newSeed()
    {
        int seed = (int)DateTime.Now.ToBinary();
        random = new Random(seed);
    }

    /// <summary>
    /// Gets a random enum
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T RandomEnumValue<T>()
    {
        var v = Enum.GetValues(typeof(T));
        return (T)v.GetValue(random.Next(v.Length));
    }
}
