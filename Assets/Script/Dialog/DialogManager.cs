using System.Collections;
using System.Collections.Generic;
//using System.Data.Common;
//using System.Diagnostics;
//using System.Runtime.CompilerServices;
//using Unity.Collections;
//using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

/// <summary>
/// Instrution:
/// there are several things to init; textLable and faceImage to ensure the location;
///                                   textSpeed to controll the chars flowing speed;
///                                   Sprite to indifferent the image;
///                                   Don't FORGET to change the name in switch, which should be consistent to your NPC's Name ;
/// </summary>
public class DialogManager : MonoBehaviour
{
    static public DialogManager instance;
    public GameObject panel;
    public DialogSetting setting;

    [Header("UI_Component")]
    public Text textLabel;
    //public Image faceImage;

    [Header("TextFile")]
    //public TextAsset textFile;
    public int index;
    public float textSpeed;

    public int textIndex;
    public List<TextAsset> assetList=new List<TextAsset>();
    public int loadFileIndex;

    [Header("Image")]
    //public Sprite npc1, npc2;
    List<string> textList=new List<string>();
    #region Singleton
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(instance);
            return;
        }
        instance = this;
    }
    #endregion
    private void Start()
    {
        //GetTextFromFile(textFile);
        loadFileIndex = -1;
        index = 0;
        StartCoroutine(LoadFuck());
    }
    private void Update()
    {
        if (loadFileIndex == -1 && fileToBeRead.Count == 0 && JustFinishedPlay())
        {
            isBusy = false;
        }
        //TmpControllor();
        if (edgeTime > 0)
        {
            edgeTime -= Time.deltaTime;
        }
    }
    //��ǰ�Ƿ��������ַ���ѡ���������
    private bool isOnCoroutine = false;
    private bool cancelTyping = false;
    public void TextCharge()
    {
        if (index == textList.Count)
        {
            panel.SetActive(false);
            isOnPlay = false;
            index = 0;
            loadFileIndex = -1;
            StopCoroutine(SetTextUI());
            StopCoroutine("AutoPlayAllTheFile_");
            edgeTime = edgeTimeSet;
            return;
        }
        if (isOnCoroutine)
        {
            if (Input.GetKeyDown(KeyCode.R)) { cancelTyping = !cancelTyping; }
            return;
        }
            StartCoroutine(SetTextUI());
    }
    public void UICharge()
    {
        if (!panel.activeSelf) panel.SetActive(true);
    }

    private void GetTextFromFile(TextAsset file)
    {
        textList.Clear();
        index = 0;

        var lineData= file.text.Split('\n');
        foreach(var line in lineData)
        {
            textList.Add(line);
        }
    }

    IEnumerator SetTextUI()
    {
        isOnCoroutine = true;
        textLabel.text = "";
        switch (textList[index])
        {
            case "A":
                //faceImage.sprite = npc1;
                ++index;
                break;
            case "B":
                //faceImage.sprite = npc2;
                ++index;
                break;
        }
        for (int i = 0; i < textList[index].Length; i++)
        {
            textLabel.text += textList[index][i];
            if (cancelTyping) break;
            yield return new WaitForSeconds(textSpeed);
        }
        textLabel.text = textList[index];
        ++index;
        isOnCoroutine = false;
        cancelTyping = false;
    }

    //��ǰ�Ƿ����ڲ��ŶԻ�,mua�ģ������߼�д��һ�磬����ܴ��
    public bool isOnPlay=false;

    //��¶������Ƿ�ǰ��ռ�ã��Լ��Զ������б�Ի�������б�
    public bool isBusy;
    IEnumerator LoadFuck()
    {
        while (true)
        {
            if (fileToBeRead.Count > 0 && !isOnPlay && !JustFinishedPlay())
            {
                AutoPlayAllTheFile(preloadTimeGap[0], fileToBeRead[0]);
                preloadTimeGap.RemoveAt(0);
                fileToBeRead.RemoveAt(0);
            }
            yield return new WaitForSeconds(1);
        }

    }


    //pre load ����籩¶�����������Ҫ���ŵĶԻ�
    public List<int> fileToBeRead = new List<int>();
    private List<float> preloadTimeGap = new List<float>();
    public void PreLoadTheFile(float timeGap,int fileIndex)
    {
        //if (fileToBeRead.Contains(fileIndex)||fileIndex==loadFileIndex) return;
        isBusy = true;
        fileToBeRead.Add(fileIndex);
        preloadTimeGap.Add(timeGap);
    }
    private void AutoPlayAllTheFile(float timeGap, int fileIndex)
    {
        if (isOnPlay || JustFinishedPlay())
        {
            PreLoadTheFile(timeGap, fileIndex);
            return;
        }
        isOnPlay = true;
        GetTextFromFile(assetList[fileIndex]);
        loadFileIndex = fileIndex;
        StartCoroutine("AutoPlayAllTheFile_", timeGap);
    }
    IEnumerator AutoPlayAllTheFile_(float timeGap)
    {
        while(true)
        {
            if (isOnCoroutine)yield return new WaitForSeconds(1);
            //UnityEngine. Debug.Log(Time.time);
            PlayNextSentence();
            yield return new WaitForSeconds(timeGap);
        }
    }
    public void PlayNextSentence()
    {
        if (isOnCoroutine) return;
        UICharge();
        TextCharge();
    }

    //�����ĵ��ú���������Ƿ�ող�����һ�ζԻ�������նԻ�Ԥ���ļ�����ǿ���жϵ�ǰ�Ի�
    public float edgeTimeSet;
    private float edgeTime;
    public bool JustFinishedPlay()
    {
        if (edgeTime > 0) return true;
        return false;
    }

    public void ReFreshTheLoadList()
    {
        fileToBeRead.Clear();
        preloadTimeGap.Clear();
    }

    public void BreakThrough()
    {
        panel.SetActive(false);
        isOnPlay = false;
        loadFileIndex = -1;
        StopCoroutine(SetTextUI());
        StopCoroutine("AutoPlayAllTheFile_");
        edgeTime = edgeTimeSet;
        index = 0;
        return;
    }
}
