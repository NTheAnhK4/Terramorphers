using System.Runtime.CompilerServices;
using UnityEngine;

namespace GameCore.Utility.Vibration
{
    public static class VibrationUti
    {
        private static bool canVibrate;
        public static void Init()
        {
            Vibration.Init();
        }

        public static void Vibrate(long milliseconds)
        {
            if (canVibrate)
            {
                Vibration.Vibrate(milliseconds);
            }
        }

        public static bool CanVibrate
        {
            get => canVibrate;
            set => canVibrate = value;
        }
    }
}