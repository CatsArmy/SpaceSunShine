namespace SpaceSunShine
{
    //public class Configs
    //{


    //    public static ConfigFile configs;
    //    private static Configs Instance = null;
    //    private static Configs _Instance;
    //    public static Configs InitConfigs(ulong steamid)
    //    {
    //        return _Instance.Init(steamid);
    //    }
    //    private Configs Init(ulong steamid)
    //    {
    //        if (Instance != null)
    //        {
    //            return Instance;
    //        }
    //        //configs = Plugin.Instance.Config;
    //        //hasSteamid = SetSteamID(steamid);

    //        Instance = this;
    //        return Instance;
    //    }


    //}
    /*internal static class Configs
    {
        private static ConfigFile Config = Plugin.Instance.Config;
        private static string key = "SpaceSunShine-";
        public static bool UseOldSunShineMethod { get; private set; } = true;
        public static bool DefaultSavingType { get; private set; } = true;
        public static bool EnablePointLight { get; private set; } = true;

        private const string _LethalConfig = "LethalConfig";
        public static bool LethalConfigEnabled = false;
        private static ConfigEntry<bool> UOSSM;
        private static void _UOSSM()
        {
            string UseOldSunShineMethodKey = $"{key}OldSun";
            ConfigDefinition UOSSM_Definition = new ConfigDefinition("Sun Shine Method", "Use Old Sun Shine Method");
            UOSSM = Config.Bind<bool>(UOSSM_Definition, false);
            if (!DefaultSavingType)
            {
                UseOldSunShineMethod = UOSSM.Value;
                return;
            }
            if (!PlayerPrefs.HasKey(UseOldSunShineMethodKey))
            {
                SetBool(UseOldSunShineMethodKey, false);
            }
            UseOldSunShineMethod = GetBool(UseOldSunShineMethodKey, false);

        }
        private static ConfigEntry<bool> EPL;
        private static void _EPL()
        {
            string EnablePointlightKey = $"{key}EnablePointlight";
            ConfigDefinition EPL_Definition = new ConfigDefinition("Enables the point light", EnablePointlightKey);
            EPL = Config.Bind<bool>(EPL_Definition, true);
            if (!DefaultSavingType)
            {
                EnablePointLight = EPL.Value;
                return;
            }
            if (!PlayerPrefs.HasKey(EnablePointlightKey))
            {
                SetBool(EnablePointlightKey, true);
            }
            EnablePointLight = GetBool(EnablePointlightKey, true);
        }
        public static void InitConfigs(ulong? steamid)
        {
            if (steamid.HasValue)
            {
                key = $"SpaceSunShine{steamid}-";
            }
            InitConfigs();
            _UOSSM();
            _EPL();
        }
        private static ConfigEntry<bool> DST;
        private static void InitConfigs()
        {
            string DefaultSavingTypeKey = $"{key}Save";
            ConfigDefinition DST_Definition = new ConfigDefinition("Defualt saving method", nameof(DefaultSavingType));
            ConfigDescription DST_Description = new ConfigDescription(@"The defualt saving type method for saving the configuration of the mod
            true means that saves are saved across profiles
            false means that saves are saved on your profile
            to override the defualt value use LethalConfig");
            DST = Config.Bind<bool>(DST_Definition, true, DST_Description);

            if (!PlayerPrefs.HasKey(DefaultSavingTypeKey))
            {
                SetBool(DefaultSavingTypeKey, true);
            }
            if (GetBool(DefaultSavingTypeKey))
            {
                DefaultSavingType = true;
            }
            else
            {
                DefaultSavingType = DST.Value;
            }

            LethalConfigEnabled = IsLethalConfigInstalled();
            if (!LethalConfigEnabled)
            {
                return;
            }

            GenericButtonConfigItem Disable = new GenericButtonConfigItem("Defualt saving method", "Disable local saving",
                "Saves are saved on your profile", "Disable", () => SetBool(DefaultSavingTypeKey, false));
            GenericButtonConfigItem Enable = new GenericButtonConfigItem("Defualt saving method", "Enable local saving",
                "Saves are saved across profiles", "Enable", () => SetBool(DefaultSavingTypeKey, true));
        }
        public static bool IsLethalConfigInstalled()
        {
            foreach (BepInEx.PluginInfo mod in BepInEx.Bootstrap.Chainloader.PluginInfos.Values)
            {
                if (mod.Metadata.Name == _LethalConfig)
                {
                    return true;
                }
            }
            return false;
        }

        public static void SetBool(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }
        public static bool GetBool(string key, bool defualt = true)
        {
            return PlayerPrefs.GetInt(key, GetInt(defualt)) == 1;
        }
        public static int GetInt(bool value)
        {
            return value ? 1 : 0;
        }
    }*/
}
