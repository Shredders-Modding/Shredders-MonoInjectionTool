using MelonLoader;
using UnityEngine;
using System.Linq;
using System.Reflection;
using System.IO;
using System;
using UnhollowerRuntimeLib;
using System.Collections.Generic;
using HarmonyLib;

namespace monoInjectionTool
{
    public class MonoInjectionTool : MelonMod
    {
        public List<AssetBundle> bundles;
        public Dictionary<string, Type> customMonobehaviours = new Dictionary<string, Type>();

        public override void OnApplicationStart()
        {
            try
            {
                bundles = new List<AssetBundle>();
                string path = $"{Application.dataPath}/../UserLibs/Inject/";
                string[] files = Directory.GetFiles(path);
                foreach (var file in files)
                {
                    if (file.Contains(".pdb")) continue;
                    if (file.Contains(".dll"))
                    {
                        var assembly = Assembly.LoadFile(file);
                        RegisterAllInAssembly(assembly);
                    }
                    else
                    {
                        bundles.Add(AssetBundle.LoadFromFile(file));
                    }
                }

            }
            catch (Exception ex)
            {
                LoggerInstance.Msg($"{ex.Message}");
            }
        }

        private void RegisterAllInAssembly(Assembly assembly)
        {
            bool harmonyPatchFlag = false;
            assembly.GetTypes().ToList().ForEach(T =>
            {
                if (T.IsSubclassOf(typeof(MonoBehaviour)))
                {
                    if (T.BaseType.IsSubclassOf(typeof(MonoBehaviour)))
                    {
                        RegisterMonoBehaviourInIl2Cpp(T.BaseType);
                    }
                    RegisterMonoBehaviourInIl2Cpp(T);
                }
                if (T.GetCustomAttribute<HarmonyPatch>() != null)
                {
                    PatchType(T, out harmonyPatchFlag);
                }
            });
            if (!harmonyPatchFlag)
            {
                LoggerInstance.Msg($"No Harmony patches found to patch");
            }
        }

        private void PatchType(Type T, out bool harmonyPatchFlag)
        {
            LoggerInstance.Msg($"Patching class: {T.Name} with Harmony");
            HarmonyLib.Harmony.CreateAndPatchAll(T);
            harmonyPatchFlag = true;
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (sceneName.Equals("mountain01"))
            {
                bundles.ForEach(bundle =>
                {
                    LoggerInstance.Msg($"Instantiating all assets in {bundle.name}"); foreach (var asset in bundle.LoadAllAssets())
                    {
                        LoggerInstance.Msg($"Instantiating {asset.name}"); var temp = UnityEngine.Object.Instantiate(asset); UnityEngine.Object.DontDestroyOnLoad(temp);
                    }
                });
            }
        }

        public void RegisterMonoBehaviourInIl2Cpp(Type type)
        {
            if (!customMonobehaviours.ContainsKey(type.FullName))
            {
                MethodInfo methodInfo = typeof(ClassInjector).GetMethod("RegisterTypeInIl2Cpp", AccessTools.all, null, new Type[] { typeof(bool) }, null).MakeGenericMethod(new Type[] { type });
                methodInfo.Invoke(null, new object[] { false });
                customMonobehaviours.Add(type.FullName, type);
                LoggerInstance.Msg("Registered " + type.FullName + " in IL2CPP");
            }
        }
    }
}
