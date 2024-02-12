//using config = SpaceSunShine.Configs;
/*namespace SpaceSunShine
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance { get; private set; }
        public static ManualLogSource Log = new ManualLogSource(PluginInfo.PLUGIN_NAME);

        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            if (!config.UseOldSunShineMethod)
            {
                SceneManager.sceneLoaded += OldOnSceneLoaded;
                return;
            }
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private const string SpaceShipScene = "SampleSceneRelay";
        private const string PointLight = "Point Light(1)";
        private static void InitConfigs()
        {
            config.InitConfigs(SteamClient.SteamId);
        }
        public Light Pointlight;
        private GameObject Lighting;
        public GameObject Sun;
        public AssetBundle mainAssetBundle = AssetBundle.LoadFromFile(Assembly.GetExecutingAssembly().Location.Replace("SpaceSunShine.dll", "spacesunshine.lem"));
        private void EnablePointLight()
        {
            try
            {
                if (!config.EnablePointLight)
                {
                    return;
                }
            }
            catch { }
            GameObject Pointlight = this.Lighting.transform.Find(PointLight).gameObject;
            this.Pointlight = Pointlight.GetComponent<Light>();
            this.Pointlight.enabled = true;
        }
        private void OldOnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != SpaceShipScene)
            {
                return;
            }

            try
            {
                InitConfigs();
            }
            catch { }
            GameObject[] SceneObjects = scene.GetRootGameObjects();
            GameObject Environment = SceneObjects.Where(name => name.name == nameof(Environment)).ToArray()[0];
            GameObject Lighting = Environment.transform.Find(nameof(Lighting)).gameObject;
            GameObject Sun = Lighting.transform.Find(nameof(Sun)).gameObject;
            this.Lighting = Lighting;
            this.Sun = Sun;
            try
            {
                EnablePointLight();
            }
            catch { }

            Light SunLight = Sun.GetComponent<Light>();
            SunLight.enabled = true;
        }
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != SpaceShipScene)
            {
                return;
            }

            try
            {
                InitConfigs();
            }
            catch { }
            GameObject[] SceneObjects = scene.GetRootGameObjects();
            GameObject Environment = SceneObjects.Where(name => name.name == nameof(Environment)).ToArray()[0];
            GameObject Lighting = Environment.transform.Find(nameof(Lighting)).gameObject;
            GameObject Sun = Lighting.transform.Find(nameof(Sun)).gameObject;
            this.Lighting = Lighting;
            this.Sun = Sun;
            try
            {
                EnablePointLight();
            }
            catch { }

            GameObject ZeekSun = Sun.gameObject;
            Sun = Instantiate(mainAssetBundle?.LoadAsset<GameObject>("Assets/Mods/SpaceSunShine/Prefabs/Sun.prefab"));
            Sun.name = nameof(Sun);
            //Sun.GetComponent<Animator>().rootRotation = ZeekSun.transform.rotation;
            //Sun.GetComponent<Animator>().bodyRotation = ZeekSun.transform.rotation;
            //Sun.transform.SetParent(Lighting.transform, true);
            //Component.Destroy(Sun.GetComponent<HDAdditionalLightData>());
            //Component.Destroy(Sun.GetComponent<Light>());
            return;

        }


    }
}*/

using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceSunShine
{
    //[BepInDependency("ainavt.lc.lethalconfig", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private const string SpaceShipScene = "SampleSceneRelay";
        private const string LethalConfigName = "LethalConfig";
        private const string PointLight = "Point Light(1)";
        public static Plugin Instance { get; private set; }
        public static ManualLogSource Log = new ManualLogSource(PluginInfo.PLUGIN_NAME);
        public static SpaceSunShine.Configs ConfigInstance = null;
        public static AssetBundle mainAssetBundle;
        private static Scene _scene;

        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            mainAssetBundle = AssetBundle.LoadFromFile(Assembly.GetExecutingAssembly().Location.Replace("SpaceSunShine.dll", "spacesunshine.lem"));
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != SpaceShipScene)
            {
                return;
            }
            if (ConfigInstance == null)
            {
                ConfigInstance = new SpaceSunShine.Configs(SteamClient.SteamId);
            }
            _scene = scene;
            GameObject[] SceneObjects = scene.GetRootGameObjects();
            GameObject Environment = SceneObjects.Where(name => name.name == nameof(Environment)).ToArray()[0];
            GameObject Lighting = Environment.transform.Find(nameof(Lighting)).gameObject;
            GameObject Sun;
            GameObject ZeekerSun = Lighting.transform.Find(nameof(Sun)).gameObject;
            if (ConfigInstance.EnablePointLight)
            {
                TogglePointLight(true);
            }
            if (ConfigInstance.UseOldSun)
            {
                Light SunLight = ZeekerSun.GetComponent<Light>();
                SunLight.enabled = true;
                return;
            }
            //using a method i took to copy a component
            Sun = Instantiate(mainAssetBundle?.LoadAsset<GameObject>("Assets/Mods/SpaceSunShine/Prefabs/Sun.prefab"), ZeekerSun.transform, true);
            //ZeekerSun.CopyComponent(Sun.GetComponent<Light>());
            //ZeekerSun.CopyComponent(Sun.GetComponent<HDAdditionalLightData>());
            var animator = ZeekerSun.GetComponent<Animator>();
            animator.rootRotation = Copy(Sun.transform.rotation);
            animator.bodyRotationInternal = Copy(Sun.transform.rotation);
            animator.bodyRotation = Copy(Sun.transform.rotation);
        }
        public static Quaternion Copy(Quaternion rot)
        {
            return new Quaternion(rot.x, rot.y, rot.z, rot.w);
        }
        public static void TogglePointLight(bool value)
        {
            GameObject[] SceneObjects = _scene.GetRootGameObjects();
            GameObject Environment = SceneObjects.Where(name => name.name == nameof(Environment)).ToArray()[0];
            GameObject Lighting = Environment.transform.Find(nameof(Lighting)).gameObject;
            Lighting.transform.Find(PointLight).GetComponent<Light>().enabled = value;
        }
    }

}