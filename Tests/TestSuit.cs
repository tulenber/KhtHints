using System.Collections;
using KhtHints;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tests
{
    public class TestSuit
    {
        private string _testHintName = "LoadingSceneHintTimeOut";
        private int _counterBeforeTest = 0;

        [OneTimeSetUp]
        public void Setup()
        {
            new GameObject().AddComponent<KhtHintsManager> ();
            _counterBeforeTest = PlayerPrefs.GetInt("KhtHints_"+_testHintName);
        }
        
        [OneTimeTearDown]
        public void Teardown()
        {
            Object.Destroy(KhtHintsManager.Instance);
            PlayerPrefs.SetInt("KhtHints_"+_testHintName, _counterBeforeTest);
        }
        
        [UnityTest]
        public IEnumerator TestKhtHintsManager()
        {
            Assert.NotNull(KhtHintsManager.Instance);
            yield return null;
        }
        
        [UnityTest]
        public IEnumerator TestKhtHintDefaultResultNotEmpty()
        {
            Assert.IsNotEmpty(KhtHintsManager.Instance.GetHint(""));
            yield return null;
        }
        
        [UnityTest]
        public IEnumerator TestKhtHintEverySequentialNewHint()
        {
            string text1 = KhtHintsManager.Instance.GetHint("LoadingSceneHint");
            string text2 = KhtHintsManager.Instance.GetHint("LoadingSceneHint");
            Assert.AreNotEqual(text1, text2);
            yield return null;
        }
        
        [UnityTest]
        public IEnumerator TestKhtHintHintTimeOut()
        {
            GameObject text = new GameObject ();
            text.AddComponent<Text>();
            text.AddComponent<KhtHint>();
            text.GetComponent<KhtHint>().hintName = _testHintName;

            string text1 = text.GetComponent<Text>().text;
            
            yield return 2;
            
            string text2 = text.GetComponent<Text>().text;
            
            Assert.AreNotEqual(text1, text2);
            
            Object.Destroy(text);
            
            yield return null;
        }
    }
}
