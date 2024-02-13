using BepInEx.Configuration;

namespace SpaceSunShine
{
    public class Configs
    {
        public static ConfigEntry<bool> UOS;
        public static ConfigEntry<bool> EFL;
        public static ConfigEntry<bool> SL;

        public Configs(ConfigFile config)
        {
            const string UOS_Key = "Use old sun lighting method";
            const string SL_Key = "Enable ladder in space";
            const string EFL_Key = "Enable flood light";
            ConfigDefinition UOS_Definition = new ConfigDefinition("Lighting", UOS_Key);
            ConfigDefinition EFL_Definition = new ConfigDefinition("Lighting", EFL_Key);
            ConfigDefinition SL_Definition = new ConfigDefinition("Space stuff", SL_Key);

            ConfigDescription UOS_Description = new ConfigDescription("Choose if " +
                "you want to use the old method for generating the light of the sun or the newer one thats like LE");
            ConfigDescription EFL_Description = new ConfigDescription("Choose if " +
                "you want to enable the flood light for a lil more light on the dark side of the ship");

            ConfigDescription SL_Description = new ConfigDescription("Choose if " +
                    "you want to enable the ladder while in space\n" +
                    "May conflict with mods untested i am only enabling it when needed to mitigate it effecting other mods if it does");
            UOS = config.Bind<bool>(UOS_Definition, false, UOS_Description);
            EFL = config.Bind<bool>(EFL_Definition, true, EFL_Description);
            SL = config.Bind<bool>(SL_Definition, true, SL_Description);
        }
    }
}