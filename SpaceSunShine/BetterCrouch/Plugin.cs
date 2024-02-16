using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine.SceneManagement;

namespace BetterCrouch
{

    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginMetadata.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance { get; private set; }
        public static Harmony harmony = new Harmony("CatsArmy.BetterCrouch");
        private const string IngameScene = "SampleSceneRelay";
        public static ManualLogSource Log;

        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginMetadata.PLUGIN_GUID} is loaded!");
            Log = Logger;
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            //SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            //Patches.IsIngame = scene.name == IngameScene;
        }
    }
}
