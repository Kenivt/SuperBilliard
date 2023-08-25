using UnityEngine;
using UnityGameFramework;
namespace SuperBilliard
{
    public static class TimeUtility
    {

        public static string TimeConvert(float time)
        {
            int mid = (int)(time / 60f);
            int sec = (int)(time % 60f);
            string timeStr;
            if (mid < 10)
            {
                if (sec < 10)
                    timeStr = $"0{mid}:0{sec}";
                else
                    timeStr = $"0{mid}:{sec}";
            }
            else
            {
                if (sec < 10)
                    timeStr = $"{mid}:0{sec}";
                else
                    timeStr = $"{mid}:{sec}";
            }
            return timeStr;
        }
    }
}