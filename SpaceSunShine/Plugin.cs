using BepInEx;
using BepInEx.Logging;
using UnityEngine.SceneManagement;

namespace SpaceSunShine
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource Log = new ManualLogSource("SpaceSunShine");
        public static new Configs Configs { get; internal set; }

        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            Configs = new Configs(base.Config);
            SceneManager.sceneLoaded += SpaceSunShine.OnSceneLoaded;
        }
        //Thunderstore push:no fuck u
    }
}