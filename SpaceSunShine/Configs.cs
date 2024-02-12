namespace SpaceSunShine
{
    using BepInEx.Configuration;
    using UnityEngine;

    public class Configs
    {
        private static ulong steamid;
        private static bool hasSteamid = false;
        public bool UsePlayerPrefs;
        public bool UseOldSun;
        public bool EnablePointLight;

        public ConfigEntry<bool> UPP;
        public ConfigEntry<bool> UOS;
        public ConfigEntry<bool> EPL;
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
        public static string Key()
        {
            string str = $"{SpaceSunShine.PluginInfo.PLUGIN_NAME}-";
            if (hasSteamid)
            {
                str += steamid.ToString();
            }
            return str;
        }
        public static bool SetSteamID(ulong _steamid)
        {
            try
            {
                steamid = _steamid;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public ConfigFile configs;
        public Configs(ulong _steamid)
        {
            configs = Plugin.Instance.Config;
            hasSteamid = SetSteamID(steamid);
            string key = Key();
            string UPP_PrefKey = $"{key}-UPP";
            string UOS_PrefKey = $"{key}-UOS";
            string EPL_PrefKey = $"{key}-EPL";
            this.UsePlayerPrefs = GetBool(UPP_PrefKey);
            UPP = configs.Bind<bool>(UPP_Definition, this.UsePlayerPrefs, UPP_Description);
            this.UsePlayerPrefs = UPP.Value;
            UOS = configs.Bind<bool>(UOS_Definition, false, UOS_Description);
            UPP = configs.Bind<bool>(EPL_Definition, true, EPL_Description);
            if (UsePlayerPrefs)
            {
                this.UseOldSun = GetBool(UOS_PrefKey, false);
                this.EnablePointLight = GetBool(EPL_PrefKey);
            }
            else
            {
                this.UseOldSun = UOS.Value;
                this.EnablePointLight = EPL.Value;
            }
        }

        public static bool GetBool(string key, bool defualt = true)
        {
            return PlayerPrefs.GetInt(key, defualt ? 1 : 0) == 1;
        }
        public static void SetBool(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }
    }
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
