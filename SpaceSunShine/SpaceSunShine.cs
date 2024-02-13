using System.Linq;
using System.Reflection;
using UnityEngine;

using UnityEngine.SceneManagement;

namespace SpaceSunShine
{
    internal static class SpaceSunShine
    {
        private static GameObject ZeekerSun = null;//not null
        private static GameObject ShipModels2b = null;//not null
        private static GameObject OutsideShipRoom = null;//safe null or non null
        private static GameObject ShipLightsPost = null;//safe null or non null
        private static GameObject HangarShip = null;

        public static GameObject Sun = null;//non null
        public static GameObject OutsideShipRoomClone = null;//Ladder
        public static GameObject ShipLightsPostClone = null;//Flood Light
        //does not require LE or The LE SDK do not be confused lil snoopers i see you (:
        public static AssetBundle mainAssetBundle = AssetBundle.LoadFromFile(Assembly.GetExecutingAssembly().Location.Replace(DLL, LethalExpansionModule));
        private const string DLL = "SpaceSunShine.dll";
        private const string LethalExpansionModule = "spacesunshine.lem";
        private const string SpaceShipScene = "SampleSceneRelay";
        private const string SunPrefab = "Assets/Mods/SpaceSunShine/Prefabs/Sun.prefab";
        private const string Floodlight2 = nameof(Floodlight2);


        public static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != SpaceShipScene)
            {
                ZeekerSun = null;
                Sun = null;
                HangarShip = null;
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
            HangarShip = Environment.transform.Find(nameof(HangarShip)).gameObject;

            ZeekerSun = Lighting.transform.Find(nameof(Sun)).gameObject;
            ShipModels2b = HangarShip.transform.Find(nameof(ShipModels2b)).gameObject;
            if (Configs.UOS.Value)
            {
                Light SunLight = ZeekerSun.GetComponent<Light>();
                SunLight.enabled = true;
            }
            else
            {
                InitSun();
            }
            InitFloodLights();
            InitLadder();
        }

        private static void InitSun()
        {
            GameObject _Sun = mainAssetBundle?.LoadAsset<GameObject>(SunPrefab);
            Sun = GameObject.Instantiate(_Sun);
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

        private static void InitFloodLights()
        {
            if (!Configs.EFL.Value)
            {
                return;
            }
            ShipLightsPost = ShipModels2b.transform.Find(nameof(ShipLightsPost)).gameObject;
            ShipLightsPostClone = GameObject.Instantiate(ShipLightsPost, ShipModels2b.transform);
            ShipLightsPostClone.transform.position = ShipLightsPost.transform.position;
            GameObject FloodLight2 = ShipLightsPostClone.transform.Find(Floodlight2).gameObject;
            //Quaternion rotation = ShipLightsPost.transform.Find(Floodlight2).rotation;
            FloodLight2.transform.rotation = new Quaternion(0.5461f, 0.6892f, -0.4661f, -0.0976f);
            FloodLight2.transform.localEulerAngles = new Vector3(32.3982f, 229.6713f, 92.1077f);

            ShipLightsPostClone.SetActive(true);
        }

        private static void InitLadder()
        {
            if (!Configs.SL.Value)
            {
                return;
            }
            OutsideShipRoom = HangarShip.transform.Find(nameof(OutsideShipRoom)).gameObject;
            OutsideShipRoomClone = GameObject.Instantiate(OutsideShipRoom, HangarShip.transform);
            OutsideShipRoomClone.SetActive(true);
        }

        public static Quaternion Copy(this Quaternion quat, float xOffset = 0f, float yOffset = 0f, float zOffset = 0f, float wOffset = 0f)
        {
            return new Quaternion(quat.x + xOffset, quat.y + yOffset, quat.z + zOffset, quat.w + wOffset);
        }
    }
}
