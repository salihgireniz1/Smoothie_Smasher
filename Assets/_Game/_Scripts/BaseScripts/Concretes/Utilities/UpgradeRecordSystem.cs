namespace PAG.UpgradeSystem
{
    public static class UpgradeRecordSystem
    {
        public static int GetLevel(string upgradeName)
        {
            return ES3.Load($"{upgradeName}_Upgrade_Level", 0);
        }
        public static void Upgrade(string upgradeName, int upgradeAmount)
        {
            int currentLevel = GetLevel(upgradeName);
            currentLevel += upgradeAmount;
            ES3.Save($"{upgradeName}_Upgrade_Level", currentLevel);
        }
        public static void Upgrade(string upgradeName)
        {
            Upgrade(upgradeName, 1);
        }
    }
}
