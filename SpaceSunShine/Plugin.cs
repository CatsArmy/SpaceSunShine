using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceSunShine
{
    [BepInDependency("ainavt.lc.lethalconfig", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private const string SpaceShipScene = "SampleSceneRelay";
        private const string LethalConfigName = "LethalConfig";
        private const string PointLight = "Point Light(1)";
        public static Plugin Instance { get; private set; }
        public static ManualLogSource Log = new ManualLogSource(PluginInfo.PLUGIN_NAME);
        public static AssetBundle mainAssetBundle;
        private static Scene _scene;

        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            mainAssetBundle = AssetBundle.LoadFromFile(Assembly.GetExecutingAssembly().Location.Replace("SpaceSunShine.dll", "spacesunshine.lem"));
            SceneManager.sceneLoaded += OnSceneLoaded;
            //Potentialy move ladder out its container to allow use while ship is in space
        }
        private void Start()
        {
            string key = Key();
            string UPP_PrefKey = $"{key}-UPP";
            string UOS_PrefKey = $"{key}-UOS";
            string EPL_PrefKey = $"{key}-EPL";
            this.UsePlayerPrefs = GetBool(UPP_PrefKey);
            UPP = Config.Bind<bool>(UPP_Definition, this.UsePlayerPrefs, UPP_Description);
            this.UsePlayerPrefs = UPP.Value;
            UOS = Config.Bind<bool>(UOS_Definition, false, UOS_Description);
            UPP = Config.Bind<bool>(EPL_Definition, true, EPL_Description);
            if (!UsePlayerPrefs)
            {
                this.UseOldSun = GetBool(UOS_PrefKey, false);
                this.EnablePointLight = GetBool(EPL_PrefKey);
            }
            else
            {
                this.UseOldSun = UOS.Value;
                this.EnablePointLight = EPL.Value;
            }
            Log.LogInfo($"{nameof(UseOldSun)}{UseOldSun}");
            Log.LogInfo($"{nameof(EnablePointLight)}{EnablePointLight}");
            Log.LogInfo($"{nameof(UsePlayerPrefs)}{UsePlayerPrefs}");
        }
        public static string Key()
        {
            return $"{PluginInfo.PLUGIN_NAME}-";
        }
        public static void Update()
        {
            if (_scene.name != SpaceShipScene)
            {
                return;
            }
            GameObject[] SceneObjects = _scene.GetRootGameObjects();
            GameObject Environment = SceneObjects.FirstOrDefault(name => name.name == nameof(Environment));
            GameObject HangerShip = Environment.transform.Find(nameof(HangerShip)).gameObject;
            GameObject ShipModels2b = HangerShip.transform.Find(nameof(ShipModels2b)).gameObject;
            GameObject ShipLightsPost = ShipModels2b.transform.Find(nameof(ShipLightsPost)).gameObject;
            GameObject LightPostClone = Instantiate(ShipLightsPost, ShipModels2b.transform);
            ShipLightsPost.SetActive(true);
            //Plan:
            //Clone FloodLight insted of point light as done ^^^
            //FloodLight2
            //33.3982 220.3287 92.1077
            //Ladder Clone
            //
            //Configs fix please why no worky
            //let go of player prefs
            //add to the patches so that when they are called i also disable the newly added stuff
            //Thunderstore push
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _scene = scene;
            if (scene.name != SpaceShipScene)
            {
                return;
            }
            GameObject[] SceneObjects = scene.GetRootGameObjects();
            GameObject Environment = SceneObjects.FirstOrDefault(name => name.name == nameof(Environment));
            GameObject Lighting = Environment.transform.Find(nameof(Lighting)).gameObject;
            GameObject Sun;
            GameObject ZeekerSun = Lighting.transform.Find(nameof(Sun)).gameObject;
            if (EnablePointLight)
            {
                TogglePointLight(true);
            }
            if (UseOldSun)
            {
                Light SunLight = ZeekerSun.GetComponent<Light>();
                SunLight.enabled = true;
                return;
            }
            //using a method i took to copy a component
            GameObject _Sun = mainAssetBundle?.LoadAsset<GameObject>("Assets/Mods/SpaceSunShine/Prefabs/Sun.prefab");
            Sun = Instantiate(_Sun);
            var animator = ZeekerSun.GetComponent<Animator>();
            animator.enabled = false;
            var rotation = new Quaternion(0f, 0f, 0f, 0f);
            var pos = Vector3.zero;
            ZeekerSun.transform.localScale = Vector3.one;
            ZeekerSun.transform.position = pos;
            ZeekerSun.transform.localPosition = pos;
            ZeekerSun.transform.rotation = rotation;
            ZeekerSun.transform.localRotation = rotation;
            Sun.transform.SetParent(ZeekerSun.transform, true);
        }
        public static Quaternion Copy(Quaternion rot)
        {
            return new Quaternion(rot.x, rot.y, rot.z, rot.w);
        }
        public static Vector3 Copy(Vector3 vector)
        {
            return new Vector3(vector.x, vector.y, vector.z);
        }
        public static void TogglePointLight(bool value)
        {
            if (_scene.name != SpaceShipScene)
            {
                return;
            }
            GameObject[] SceneObjects = _scene.GetRootGameObjects();
            GameObject Environment = SceneObjects.FirstOrDefault(name => name.name == nameof(Environment));
            if (Environment == null)
            {
                return;
            }
            GameObject Lighting = Environment.transform.Find(nameof(Lighting)).gameObject;
            Lighting.transform.Find(PointLight).GetComponent<Light>().enabled = value;
        }
        public bool GetBool(string key, bool defualt = true)
        {
            return PlayerPrefs.GetInt(key, defualt ? 1 : 0) == 1;
        }
        public void SetBool(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }

        public bool UsePlayerPrefs;
        public bool UseOldSun;
        public bool EnablePointLight;

        private static ConfigEntry<bool> UPP;
        private static ConfigEntry<bool> UOS;
        private static ConfigEntry<bool> EPL;
        public const string UPP_Key = "Save localy";
        public const string UOS_Key = "Use old sun lighting method";
        public const string EPL_Key = "Enable point light";
        ConfigDefinition UPP_Definition = new ConfigDefinition("Saving", UPP_Key);
        ConfigDefinition UOS_Definition = new ConfigDefinition("Lighting", UOS_Key);
        ConfigDefinition EPL_Definition = new ConfigDefinition("Lighting", EPL_Key);

        ConfigDescription UPP_Description = new ConfigDescription("This setings lets you choose if " +
            "you want your configs saved across profiles(on by defualt) " +
            "or in your profile");
        ConfigDescription UOS_Description = new ConfigDescription("This settings lets you choose if " +
            "you want to use the old method for generating the light of the sun or the newer one thats like LE");
        ConfigDescription EPL_Description = new ConfigDescription("This settings lets you choose if " +
            "you want to enable the point light for a lil more light on the dark side of the ship");
    }

}