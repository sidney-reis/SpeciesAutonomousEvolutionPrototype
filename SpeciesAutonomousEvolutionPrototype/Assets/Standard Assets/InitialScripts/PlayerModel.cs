using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerModel
{
    public struct LegacyModel
    {
        public static double steps = 0;
        public static double foods = 0;
    }

    public struct CurrentModel
    {
        public static double steps = 0;
        public static double foods = 0;
    }

    public static void triggerBreed()
    {
        LegacyModel.steps += CurrentModel.steps;
        LegacyModel.foods += CurrentModel.foods;
        CurrentModel.steps = 0;
        CurrentModel.foods = 0;
    }
}
