using BepInEx;
using HarmonyLib;

namespace CorporateRestructureWeather
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance { get; private set; }
        private readonly Harmony _harmony = new Harmony("CatsArmy.CorporateRestructureWeather");
        private void Awake()
        {
            // Plugin startup logic
            if (Plugin.Instance != null)
            {
                return;
            }
            Plugin.Instance = this;
            Plugin.Instance._harmony.PatchAll(typeof(WeatherPatch));

            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

        }
    }
}