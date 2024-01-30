using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using TMPro;

namespace CorporateRestructureWeather
{
    internal class WeatherPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.Instance.SetMapScreenInfoToCurrentLevel))]
        private static void ColorWeather(ref TextMeshProUGUI ___screenLevelDescription, ref SelectableLevel ___currentLevel)
        {
            LevelWeatherType currentWeather = ___currentLevel.currentWeather;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"Orbiting: {___currentLevel.PlanetName}\n");
            stringBuilder.Append($"Weather: <color=#{GetHexColor(currentWeather)}>{currentWeather}</color>\n");
            stringBuilder.Append(___currentLevel.LevelDescription ?? "");
            ___screenLevelDescription.text = stringBuilder.ToString();
        }
        
        private const string White = "FFFFFF";
        private const string Green = "69FF6B";
        private const string Yellow = "FFDC00";
        private const string Orange = "FF9300";
        private const string Red = "FF0000";

        private static string GetHexColor(LevelWeatherType currentWeather)
        {
            switch (currentWeather)
            {
                case (LevelWeatherType)(-1):
                    return Green;
                case (LevelWeatherType)0:
                    return Green;
                case (LevelWeatherType)1:
                    return Yellow;
                case (LevelWeatherType)3:
                    return Yellow;
                case (LevelWeatherType)2:
                    return Orange;
                case (LevelWeatherType)4:
                    return Orange;
                case (LevelWeatherType)5:
                    return Red;
                
                default: 
                    return White;
            }
        }
    }
}

