using System.Collections;
using TMPro;
using UnityEngine;

public class NarrationBox : MonoBehaviour
{
    public float delay = 0.1f;
    [SerializeField] private TMP_Text textMesh;
    [SerializeField] private GameObject TextBox;
    [SerializeField] private string fullText;
    private string currentText = "";
    private bool alreadyActivated = false;

    private void Start()
    {
        textMesh.text = "";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(!alreadyActivated)
            {
                StartCoroutine(ShowText());
                TextBox.SetActive(true);
                alreadyActivated = true;
            }
        }
    }

    private IEnumerator ShowText()
    {
        for (int i = 0; i < fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i + 1);
            textMesh.text = currentText;
            yield return new WaitForSeconds(delay);
        }
    }
}