using UnityEngine;

namespace CWClient_Enchantment
{
    public class Loader
    {
        public static GameObject load_object;
        // smi.exe inject -p CWClient -a D:\CWClient-Enchantment.dll -n CWClient_Enchantment -c Loader -m Load
        public static void Load()
        {
            load_object = new GameObject();
            load_object.AddComponent<Dump>();
            Object.DontDestroyOnLoad(load_object);
        }
        public static void Unload()
        {
            Object.Destroy(load_object);
        }
    }
}
