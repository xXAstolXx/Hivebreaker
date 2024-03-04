using System.Collections;
using TMPro;
using UnityEngine;

public class DialogBox : MonoBehaviour
{
    [SerializeField]
    private GameObject speechXBone;

    private float speechXcorrection = 1f;

    [SerializeField] 
    private GameObject speechYBone;

    [SerializeField]
    private SkinnedMeshRenderer meshRenderer;

    private float speechYcorrection = 0.5f;

    private TMP_Text textBox;


    private void Awake()
    {
        textBox = GetComponentInChildren<TMP_Text>();
    }

    public void SetTextSize(float size)
    {
        textBox.fontSize = size; 
    }

    public void WriteTextToDialogBox(string text, int maxChar)
    {
        if (maxChar > 0 && text.Length > maxChar) 
        {
            for (int i = maxChar; i <= text.Length; i += maxChar) 
            {
                text = text.Insert(maxChar * (i / maxChar), "<br>");
            }
            
        }
        textBox.text = text;
        StartCoroutine(AutoSizeDialogBox(text, maxChar));

    }

    private IEnumerator AutoSizeDialogBox(string text, int maxChar)
    {
        textBox.GetComponent<MeshRenderer>().enabled = false;
        meshRenderer.enabled = false;

        ResetSizeDialogBox();
        yield return new WaitForEndOfFrame();

        textBox.GetComponent<MeshRenderer>().enabled = true;
        meshRenderer.enabled = true;

        Vector2 size = Vector2.zero;
        size.x = textBox.bounds.size.x;
        size.y = textBox.bounds.size.y;
      
        
        SetDialogBoxSize(size);
    }

    private void ResetSizeDialogBox()
    {
        speechXBone.transform.localPosition = new Vector3(0, speechXcorrection, 0);
        speechYBone.transform.localPosition = new Vector3(0, 0, speechYcorrection);
    }

    public void SetDialogBoxSize(Vector2 size)
    {
        float extendValueX;
        extendValueX = size.x - 1.5f;

        float extendValueY;
        extendValueY = size.y - 3f;

        if (extendValueX > -1)
        {
            speechXBone.transform.localPosition = new Vector3(0, extendValueX, 0);
        }
        if (extendValueY > -1.5) 
        {
            speechYBone.transform.localPosition = new Vector3(0, 0, extendValueY);
        }
       
    }


    private void Update()
    {
        transform.rotation = Quaternion.Euler(0.0f, -45.0f, 0.0f);
    }
}
