using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EmotionUIManageer : MonoBehaviour
{
    // Start is called before the first frame update

    public  Slider emotionBar;
    public float slideSpeed;
    public float delayClose;

    public int secondTodecreaseXP;

    bool isChange;
    float changeLast;
    float targetValue;
    public GameObject emotionUI;

    private void Start()
    {
        isChange = false;
        StartCoroutine(DecreaseXP());
        changeLast = 0;
    }
    private void Update()
    {
        if (isChange)
        {
            ChangeValueSmoothly();
        }
        CloseItself();
    }
    public void ChangeEmotionXP(float change)
    {
        isChange=true;
        changeLast = change;
        emotionUI.SetActive(true);
        if (emotionBar.value + change > 100)
        {
            targetValue = 100;
        }
        else if(emotionBar.value+change < 0) 
        {
            targetValue = 0;
        }
        else
        {
            targetValue = emotionBar.value+change;
        }
    }
    public float GetEmotionXP()
    {
        return targetValue;
    }

    private void ChangeValueSmoothly()
    {
        if (Mathf.Abs(changeLast) <= 1)
        {
            emotionBar.value = targetValue;
            isChange = false;
            delayTimeTmp = delayClose;
            return;
        }
        emotionBar.value += changeLast * slideSpeed * Time.deltaTime;
        changeLast-=changeLast*slideSpeed*Time.deltaTime;
    }

    private float delayTimeTmp;
    private void CloseItself()
    {
        if (delayTimeTmp < -100) return;
        if (delayTimeTmp <= 0)
        {
            emotionUI.SetActive(false);
            delayTimeTmp = -200;
        }
        else
        {
            delayTimeTmp-=Time.deltaTime;
        }
    }
    public void OpenEmotionTemply()
    {
        delayTimeTmp = delayClose;
        emotionUI.SetActive(true);
    }
    IEnumerator DecreaseXP()
    {
        while (true)
        {
            emotionBar.value=emotionBar.value>1?emotionBar.value-1:0;
            yield return new WaitForSeconds(secondTodecreaseXP);

        }
    }
}
