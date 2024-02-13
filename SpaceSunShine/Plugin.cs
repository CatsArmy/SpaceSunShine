using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceSunShine
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private const string SpaceShipScene = "SampleSceneRelay";
        public static Plugin Instance { get; private set; }
        public static ManualLogSource Log = new ManualLogSource(PluginInfo.PLUGIN_NAME);
        public static AssetBundle mainAssetBundle;

        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            const string DLL = "SpaceSunShine.dll";
            const string LethalExpansionModule = "spacesunshine.lem";
            //does not require LE or The LE SDK do not be confused lil snoopers i see you (:
            mainAssetBundle = AssetBundle.LoadFromFile(Assembly.GetExecutingAssembly().Location.Replace(DLL, LethalExpansionModule));
            BindConfigs();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        private void BindConfigs()
        {
            UOS = Config.Bind<bool>(UOS_Definition, false, UOS_Description);
            EFL = Config.Bind<bool>(EFL_Definition, true, EFL_Description);
            SL = Config.Bind<bool>(SL_Definition, true, SL_Description);
        }

        //Plan:
        //Clone FloodLight insted of point light as done ^^^
        //FloodLight2
        //32.3982 220.3287 92.1077
        //12.3982 = 20
        //Ladder Clone
        //
        //Configs fix please why no worky
        //let go of player prefs
        //add to the patches so that when they are called i also disable the newly added stuff
        //Thunderstore push


        private static GameObject ZeekerSun = null;
        public static GameObject Sun = null;
        private static GameObject ShipModels2b = null;
        private static GameObject OutsideShipRoom = null;
        public static GameObject OutsideShipRoomClone = null;
        private static GameObject ShipLightsPost = null;
        public static GameObject ShipLightsPostClone = null;
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != SpaceShipScene)
            {
                ZeekerSun = null;
                Sun = null;
                ShipModels2b = null;
                OutsideShipRoom = null;
                OutsideShipRoomClone = null;
                ShipLightsPost = null;
                ShipLightsPostClone = null;
                return;
            }
            GameObject[] SceneObjects = scene.GetRootGameObjects();
            GameObject Environment = SceneObjects.FirstOrDefault(name => name.name == nameof(Environment));
            GameObject Lighting = Environment.transform.Find(nameof(Lighting)).gameObject;
            GameObject HangerShip = Environment.transform.Find(nameof(HangerShip)).gameObject;
            ZeekerSun = Lighting.transform.Find(nameof(Sun)).gameObject;
            ShipModels2b = HangerShip.transform.Find(nameof(ShipModels2b)).gameObject;
            InitSun();
            InitFloodLights();
            InitLadder();
        }
        private void InitSun()
        {
            if (UOS.Value)
            {
                Light SunLight = ZeekerSun.GetComponent<Light>();
                SunLight.enabled = true;
                return;
            }
            const string SunPrefab = "Assets/Mods/SpaceSunShine/Prefabs/Sun.prefab";
            GameObject _Sun = mainAssetBundle?.LoadAsset<GameObject>(SunPrefab);
            Sun = Instantiate(_Sun);
            Animator animator = ZeekerSun.GetComponent<Animator>();
            animator.enabled = false;
            Quaternion rotation = new Quaternion(0f, 0f, 0f, 0f);
            Vector3 pos = Vector3.zero;
            ZeekerSun.transform.localScale = Vector3.one;
            ZeekerSun.transform.position = pos;
            ZeekerSun.transform.localPosition = pos;
            ZeekerSun.transform.rotation = rotation;
            ZeekerSun.transform.localRotation = rotation;
            Sun.transform.SetParent(ZeekerSun.transform, true);
        }
        private void InitFloodLights()
        {
            if (!EFL.Value)
            {
                return;
            }
            const string Floodlight2 = nameof(Floodlight2);
            ShipLightsPost = ShipModels2b.transform.Find(nameof(ShipLightsPost)).gameObject;
            ShipLightsPostClone = Instantiate(ShipLightsPost, ShipModels2b.transform);
            ShipLightsPostClone.transform.position = ShipLightsPost.transform.position;
            GameObject FloodLight2 = ShipLightsPostClone.transform.Find(Floodlight2).gameObject;
            FloodLight2.transform.rotation = Copy(FloodLight2.transform.rotation, 20f);
            ShipLightsPostClone.SetActive(EFL.Value);
        }
        private void InitLadder()
        {
            if (!SL.Value)
            {
                return;
            }
            OutsideShipRoom = ShipModels2b.transform.Find(nameof(OutsideShipRoom)).gameObject;
            OutsideShipRoomClone = Instantiate(OutsideShipRoom, ShipModels2b.transform);
            OutsideShipRoomClone.SetActive(SL.Value);
        }
        public static Quaternion Copy(Quaternion rot, float xO = 0f, float yO = 0f, float zO = 0f, float wO = 0f)
        {
            return new Quaternion(rot.x + xO, rot.y + yO, rot.z + zO, rot.w + wO);
        }
        //public static Vector3 Copy(Vector3 vector)
        //{
        //    return new Vector3(vector.x, vector.y, vector.z);
        //}

        public static ConfigEntry<bool> UOS;
        public static ConfigEntry<bool> EFL;
        public static ConfigEntry<bool> SL;
        public const string UOS_Key = "Use old sun lighting method";
        public const string SL_Key = "enable ladder in space";
        public const string EFL_Key = "Enable flood light";
        ConfigDefinition UOS_Definition = new ConfigDefinition("Lighting", UOS_Key);
        ConfigDefinition EFL_Definition = new ConfigDefinition("Lighting", EFL_Key);
        ConfigDefinition SL_Definition = new ConfigDefinition("Space stuff", SL_Key);

        ConfigDescription UOS_Description = new ConfigDescription("Choose if " +
            "you want to use the old method for generating the light of the sun or the newer one thats like LE");
        ConfigDescription EFL_Description = new ConfigDescription("Choose if " +
            "you want to enable the flood light for a lil more light on the dark side of the ship");

        ConfigDescription SL_Description = new ConfigDescription("Choose if " +
                @"you want to enable the ladder while in space
May conflict with mods untested i am only enabling it when needed to mitigate it effecting other mods if it does");
    }
}
