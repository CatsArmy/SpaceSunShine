using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceSunShine
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private const string SpaceShipScene = "SampleSceneRelay";
        public static ManualLogSource Log = new ManualLogSource(PluginInfo.PLUGIN_NAME);
        public AssetBundle mainAssetBundle = AssetBundle.LoadFromFile(Assembly.GetExecutingAssembly().Location.Replace("SpaceSunShine.dll", "spacesunshine.lem"));
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            SceneManager.sceneLoaded += OnSceneLoaded;

        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != SpaceShipScene)
            {
                return;
            }
            GameObject Sun = Instantiate(mainAssetBundle?.LoadAsset<GameObject>("Assets/Mods/SpaceSunShine/Prefabs/Sun.prefab"));
        }
    }
}
