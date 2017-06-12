using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerModel
{
    public struct LegacyModel
    {
        public static double foods = 0;
        public static double ran = 0;
        public static double defended = 0;
        public static double attacked = 0;
    }

    public struct CurrentModel
    {
        public static double foods = 0;
        public static double ran = 0;
        public static double defended = 0;
        public static double attacked = 0;
    }

    public static void triggerBreed()
    {
        LegacyModel.foods += CurrentModel.foods;
        LegacyModel.ran += CurrentModel.ran;
        LegacyModel.defended += CurrentModel.defended;
        LegacyModel.attacked += CurrentModel.attacked;
        CurrentModel.foods = 0;
        CurrentModel.ran = 0;
        CurrentModel.defended = 0;
        CurrentModel.attacked = 0;
    }
}
