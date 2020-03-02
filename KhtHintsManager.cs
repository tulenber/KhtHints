using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace KhtHints
{
    enum KhtHintsStrategy {KhtHintsRandom, KhtHintsSequential};
    
    [Serializable] 
    class KhtHintConfig
    {
        public string name = "";
        public string groupName = "";
        public KhtHintsStrategy strategy = 0;
        public int updateTimeout = 0;
    }
    
    [Serializable] 
    class KhtHintsData
    {
        public string name = null;
        public List<string> hints = null;
    }

    [Serializable] 
    class KhtHintsListObject
    {
        public List<KhtHintConfig> khtHintsConfigs = null;
        public List<KhtHintsData> khtHintsData = null;
    }

    public class KhtHintsManager : KhtSingleton<KhtHintsManager>
    {
        private const string DefaultGroupName = "default";
        private const KhtHintsStrategy DefaultStrategy = KhtHintsStrategy.KhtHintsRandom;
        private const int DefaultUpdateTimeout = 0;

        private readonly Dictionary<string, List<string>> _khtHintsData = new Dictionary<string, List<string>>();
        private readonly Dictionary<string, KhtHintConfig> _khtHintsConfigs = new Dictionary<string, KhtHintConfig>();
        
        // Start is called before the first frame update
        void Start()
        {
            DontDestroyOnLoad(gameObject);
            ReadHintsConfig();
        }
        
        void ReadHintsConfig()
        {
            Debug.Log("[KhtHintsManager] Init");

            TextAsset mytxtData = (TextAsset)Resources.Load("KhtHintsData");
            if (mytxtData == null)
            {
                Debug.LogError("[KhtHintsManager] Can not load config data");
                return;
            }
            
            string jsonObject = mytxtData.text;
            KhtHintsListObject khtHintsObject = JsonUtility.FromJson<KhtHintsListObject>(jsonObject);
            
            foreach (KhtHintsData hintsData in khtHintsObject.khtHintsData)
            {
                _khtHintsData[hintsData.name] = hintsData.hints;
            }
            
            foreach (KhtHintConfig hintsConfig in khtHintsObject.khtHintsConfigs)
            {
                _khtHintsConfigs[hintsConfig.name] = hintsConfig;
            }
        }

        int GetIndexByRandom(int groupCount)
        {
            return Random.Range(0, groupCount);
        }

        int GetIndexBySequential(string groupName, int groupCount)
        {
            int counter = PlayerPrefs.GetInt("KhtHints_"+groupName);
            if (counter >= groupCount)
            {
                counter = 0;
            }
            PlayerPrefs.SetInt("KhtHints_"+groupName, counter + 1);
            return counter;
        }

        int GetNextHintIndexForGroup(string groupName, int groupCount, KhtHintsStrategy strategy)
        {
            switch (strategy)
            {
                case KhtHintsStrategy.KhtHintsRandom:
                    return GetIndexByRandom(groupCount);
                case KhtHintsStrategy.KhtHintsSequential:
                    return GetIndexBySequential(groupName, groupCount);
                default:
                    return GetIndexByRandom(groupCount);
            }
        }

        string GetHint(string groupName, KhtHintsStrategy strategy)
        {
            if (_khtHintsData.ContainsKey(groupName))
            {
                List<string> groupHints = _khtHintsData[groupName];
                return groupHints[GetNextHintIndexForGroup(groupName, groupHints.Count, strategy)];
            }

            if (groupName.Equals(DefaultGroupName))
            {
                Debug.LogWarning("[KhtHintsManager] Can't find default group");
                return "";
            }

            Debug.LogWarning("[KhtHintsManager] Can't find group with name: "+groupName);
            Debug.LogWarning("[KhtHintsManager] Return text from default group");
            return GetHint(DefaultGroupName, strategy);
        }

        public string GetHint(string hintName)
        {
            if (_khtHintsConfigs.ContainsKey(hintName))
            {
                KhtHintConfig config = _khtHintsConfigs[hintName];
                return GetHint(config.groupName, config.strategy);
            }
            
            Debug.Log("[KhtHintsManager] No config for hint field: " + hintName);
            return GetHint(DefaultGroupName, DefaultStrategy);
        }

        public int GetUpdateTimeout(string hintName)
        {
            return _khtHintsConfigs.ContainsKey(hintName) ? _khtHintsConfigs[hintName].updateTimeout : DefaultUpdateTimeout;
        }
    }
}
