using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogWithGPT : MonoBehaviour
{
    public InputField inputField;
    public Text outputText;
    public KeyCode keyCode=KeyCode.KeypadEnter;
    public float timeBetweenWord;
    public float holdTime;
    private void Update()
    {
        Chat();
    }

    private bool chat = false;
    public void Chat()
    {
        if(Input.GetKeyDown(keyCode))
        {
            chat = true;
            inputField.readOnly = true;
            GPT.instance.SendMessageByString(inputField.text);
            inputField.text = "";
            inputField.gameObject.SetActive(false);
        }
        string text = GPT.instance.GetLastAnswer(true);
        if (text != ""&&chat)
        {
            StartCoroutine(LoadChat(text));
        }
        
    }

    IEnumerator LoadChat(string text)
    {
        outputText.text = "";
        outputText.gameObject.SetActive(true);
        foreach(char ch in text)
        {
            outputText.text += ch;
            yield return new WaitForSeconds(timeBetweenWord);
        }
        yield return new WaitForSeconds(holdTime);
        outputText.text = "";
        inputField.readOnly = false;
        outputText.gameObject.SetActive(false);
        inputField.gameObject.SetActive(true);
        chat = false;
    }
}
