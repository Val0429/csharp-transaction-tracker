using System.Drawing;
using Constant;

namespace PanelBase
{
    public class StorageImage
    {
        private static Image _storageBg = Resources.GetResources(Properties.Resources.storageBG, Properties.Resources.IMGStorageBG);
        private static Image _storageBg2 = Resources.GetResources(Properties.Resources.storageBG2, Properties.Resources.IMGStorageBG2);
        private static Image _storageUsage = Resources.GetResources(Properties.Resources.storageBar, Properties.Resources.IMGStorageBar);
        private static Image _storageActivate = Resources.GetResources(Properties.Resources.storageBar_activate, Properties.Resources.IMGStorageBar_activate);
        private static Image _storageKeep = Resources.GetResources(Properties.Resources.keepSpaceBar, Properties.Resources.IMGKeepSpaceBar);
        public static Image StorageBg()
        {
            return _storageBg;
        }

        public static Image StorageBg2()
        {
            return _storageBg2;
        }

        public static Image StorageUsage()
        {
            return _storageUsage;
        }

        public static Image StorageActivate()
        {
            return _storageActivate;
        }

        public static Image StorageKeep()
        {
            return _storageKeep;
        }
    }
}
