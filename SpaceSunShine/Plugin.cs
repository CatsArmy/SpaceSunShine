using System.Linq;
using BepInEx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceSunShine
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private const string SpaceShipScene = "SampleSceneRelay";
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
            GameObject[] SceneObjects = scene.GetRootGameObjects();
            GameObject Environment = SceneObjects.Where(name => name.name == nameof(Environment)).ToArray()[0];
            GameObject Lighting = Environment.transform.Find(nameof(Lighting)).gameObject;
            GameObject Sun = Lighting.transform.Find(nameof(Sun)).gameObject;
            Light SunLight = Sun.GetComponent<Light>();
            SunLight.enabled = true;
            SunLight.UseLethalExpansionLightSettings();
        }
    }
}