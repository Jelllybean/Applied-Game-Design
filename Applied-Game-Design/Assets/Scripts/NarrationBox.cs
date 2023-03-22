using System.Collections;
using TMPro;
using UnityEngine;

public class NarrationBox : MonoBehaviour
{
    public float delay = 0.1f;
    [SerializeField] private TMP_Text textMesh;
    [SerializeField] public GameObject TextBox;
    [SerializeField] private string fullText;
    private string currentText = "";
    public bool alreadyActivated = false;

    public static NarrationBox narrationBoxSingleton;

    private void Awake()
    {
        narrationBoxSingleton = this;
    }

    private void Start()
    {
        textMesh.text = "";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!alreadyActivated)
            {
                StartCoroutine(ShowText(null));
                TextBox.SetActive(true);
                alreadyActivated = true;
            }
        }
    }

    public IEnumerator ShowText(string _textToSay)
    {

        if (_textToSay == null)
        {
            for (int i = 0; i < fullText.Length; i++)
            {
                currentText = fullText.Substring(0, i + 1);
                currentText = _textToSay.Substring(0, i + 1);
                textMesh.text = currentText;
                yield return new WaitForSeconds(delay);
            }
        }
        else
        {
            for (int i = 0; i < _textToSay.Length; i++)
            {

                currentText = _textToSay.Substring(0, i + 1);
                textMesh.text = currentText;
                yield return new WaitForSeconds(delay);
            }
        }

        Invoke("DisableBox", 8f);
    }

    public void DisableBox()
    {
        TextBox.SetActive(false);
    }
}