using BepInEx;
using BepInEx.Logging;
using UnityEngine.SceneManagement;

namespace SpaceSunShine
{
    [BepInPlugin(PluginMetadata.PLUGIN_GUID, PluginMetadata.PLUGIN_NAME, PluginMetadata.PLUGIN_VERSION)]
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
    }
}