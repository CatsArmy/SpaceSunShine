using BepInEx;
using BepInEx.Logging;
using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine.SceneManagement;

namespace BetterCrouch
{

    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance { get; private set; }
        public static ManualLogSource Log = new ManualLogSource(PluginInfo.PLUGIN_NAME);
        public static Harmony harmony = new Harmony("CatsArmy.BetterCrouch");
        private const string IngameScene = "SampleSceneRelay";
        //public static bool

        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            SceneManager.sceneLoaded += OnSceneLoaded;
            harmony.PatchAll(typeof(PlayerControllerB));
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Patches.Initialized = scene.name == IngameScene;
        }
    }
}
