using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace KhtHints
{
    public class KhtHint : MonoBehaviour
    {
        [SerializeField] public string hintName = "";
        private int _updateTimeout = 0;

        private Text _text = null;
        // Start is called before the first frame update
        void Start()
        {
            _text = GetComponent<Text>();
            
            _updateTimeout = KhtHintsManager.Instance.GetUpdateTimeout(hintName);

            if (_updateTimeout > 0)
            {
                StartCoroutine(nameof(UpdateText));                
            }
            else
            {
                _text.text = KhtHintsManager.Instance.GetHint(hintName);                
            }
        }

        private IEnumerator UpdateText() {
            while(true) {
                _text.text = KhtHintsManager.Instance.GetHint(hintName);
                yield return new WaitForSeconds(_updateTimeout);
            }
        }
    }
}
