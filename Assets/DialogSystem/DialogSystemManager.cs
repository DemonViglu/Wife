using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystemManager : MonoBehaviour {
    public static DialogSystemManager instance;

    #region Singleton
    /// <summary>
    /// Singleton the DialogSystemManager
    /// </summary>
    private void Awake() {
        if (instance != null) {
            Destroy(instance);
            Debug.LogError("Find another DialogSystem!");
        }
        instance = this;
    }
    #endregion

    #region parameter
    [Header("UI_Component")]
    [SerializeField] private GameObject panel;
    [SerializeField] private Text textLabel;

    [Header("Image")]
    [SerializeField] private Sprite npc1;
    [SerializeField] private Sprite npc2;
    [SerializeField] private Image faceImage;

    [Header("Player Setting")]
    ///                                     MISSION
    ///_________________________________________________________________________________________________|                       |___________________________________________________
    ///        wordTimeGap        sentenceTimeGap                                    sentenceTimeGap             missiontTimeGap
    ///|- - - - - - - - - - -|                       |- - - - - - - - - - -|                            |                       |- - - - - - - - - - -|
    ///                                                                     (close the panel here)----->
    ///
    [SerializeField] private float wordTimeGap;
    [SerializeField] private float sentenceTimeGap;
    [SerializeField] private float missionTimeGap;
    [SerializeField] private float missionEdgeTime;
    [SerializeField] private KeyCode keyToPassTheSentence = KeyCode.P;

    [SerializeField]
    List<Mission> missionList = new List<Mission>();
    List<string> textList = new List<string>();
    private int sentenceIndex;
    private bool onMission = false;
    [SerializeField] private bool justFinishMission = false;

    [Header("MissionSO")]
    [SerializeField] private MissionSOManager missionSOManager;
    private bool hasOption;

    [Header("MissionEventHandler")]
    [SerializeField] private MissionEventHandler missionEventHandler;
    #endregion

    #region Two Charge
    //当前是否在输入字符，选择快速码
    private bool isOnCoroutine = false;
    private bool cancelTyping = false;
    private void TextCharge() {
        if (sentenceIndex == textList.Count) {
            SetUpButton();
            if (hasOption) return;

            OnMissionEnd();
            return;
        }
        if (isOnCoroutine) {
            if (Input.GetKeyDown(keyToPassTheSentence)) { cancelTyping = true; }
            return;
        }
        StartCoroutine("SetTextUI");
    }
    private void UICharge() {
        if (!panel.activeSelf) panel.SetActive(true);
    }
    IEnumerator SetTextUI() {
        isOnCoroutine = true;
        textLabel.text = "";
        switch (textList[sentenceIndex]) {
            case "A":
                if (npc1 != null) faceImage.sprite = npc1;
                ++sentenceIndex;
                break;
            case "B":
                if (npc2 != null) faceImage.sprite = npc2;
                ++sentenceIndex;
                break;
        }
        for (int i = 0; i < textList[sentenceIndex].Length; i++) {
            // IF YOU WANT TO PASS THE SENTENCE, PRESS THE KEY THAT
            // SET IN TEXTCHARGE
            if (cancelTyping) break;
            textLabel.text += textList[sentenceIndex][i];
            yield return new WaitForSeconds(wordTimeGap);
        }
        textLabel.text = textList[sentenceIndex];
        ++sentenceIndex;
        isOnCoroutine = false;
        cancelTyping = false;
    }
    #endregion
    private void Update() {
        AutoLoadMission();
    }

    #region MissionLogic
    private float _SentenceTimeGap;
    private bool missionLock = false;
    /// <summary>
    /// Auto play the logic every frame
    /// </summary>
    private void AutoLoadMission() {
        //when is typing words, press the key to pass
        if (isOnCoroutine) {
            if (Input.GetKeyDown(keyToPassTheSentence)) { cancelTyping = true; }
            return;
        }
        //if the mission is lock return;
        if (missionLock) {
            return;
        }
        if (onMission) {
            if (isOnCoroutine) {
                return;
            }
            if (_SentenceTimeGap > 0) {
                _SentenceTimeGap -= Time.deltaTime;
            }
            else {
                _SentenceTimeGap = sentenceTimeGap;
                UICharge();
                TextCharge();
            }
            return;
        }
        else {
            if (missionList.Count > 0) {
                LoadMissionAtFirst();
                missionList.RemoveAt(0);
                onMission = true;
            }
        }
    }

    private void LoadMissionAtFirst() {
        currentMission = missionList[0];
        if (currentMission.optionMissionIndex != null) {
            hasOption = currentMission.optionMissionIndex.Count > 0;
        }
        else {
            hasOption = false;
        }

        bool hasEvent = TryLoadMissionEvent(missionList[0].eventIndex);
        if (missionList[0].textString != "") {
            var lineData = missionList[0].textString.Split('\n');
            foreach (var line in lineData) {
                textList.Add(line);
            }
        }
        else if (missionList[0].textAsset != null) {
            TextAsset file = missionList[0].textAsset;
            textList.Clear();
            sentenceIndex = 0;
            var lineData = file.text.Split('\n');
            foreach (var line in lineData) {
                textList.Add(line);
            }
        }
        else if (hasEvent) {
            return;
        }
        else {
            Debug.LogError("There is nothing in the mission!");
            return;
        }


    }
    private void OnMissionEnd() {
        Debug.Log("mission complete");
        panel.SetActive(false);
        sentenceIndex = 0;
        textList.Clear();
        textLabel.text = "";
        _SentenceTimeGap = 0;
        missionLock = true;
        justFinishMission = true;

        currentMission = null;
        Invoke("ResetJustFinishMission", missionEdgeTime);
        Invoke("SetMissionAvalible", missionTimeGap);
    }

    private void ResetJustFinishMission() {
        justFinishMission = false;
    }
    private void SetMissionAvalible() {
        onMission = false;
        missionLock = false;
    }
    #endregion 
    // The PUBLIC API that expose to outside
    public void AddMission(Mission mission) {
        missionList.Add(mission);
    }
    /// <summary>
    /// Refresh the mission state or jump to next mission;
    /// </summary>
    public void ClearMissionRightNow() {
        missionLock = true;
        StopCoroutine("SetTextUI");
        isOnCoroutine = false;
        {
            panel.SetActive(false);
            sentenceIndex = 0;
            textList.Clear();
            textLabel.text = "";
            _SentenceTimeGap = 0;
        }
        //Debug.Log("ClearMissionSuccess");
        CancelInvoke("SetMissionAvalible");
        Invoke("SetMissionAvalible", 0.1f);
    }
    public void ClearAllMission() {
        ClearMissionRightNow();
        missionList.Clear();
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////                          Dialog Tree                   ////////////////////////////////////////////////////////////////
    /// <summary>
    /// It will clean all the mission exsited, so you better user AddMissionSO at last or AddMissionSOAtFirst for priority
    /// </summary>
    /// <param name="index"> call for the missionSO in the managerList </param>
    public void StartMissionSO(int index) {
        ClearAllMission();
        hasOption = missionSOManager.missionList[index].optionMissionIndex.Count > 0;
        if (index >= missionSOManager.missionList.Count || index < 0) {
            Debug.LogError("Wrong index, it's out of the missionSO range!");
            return;
        }
        currentMission = new Mission(missionSOManager.missionList[index].textString, missionSOManager.missionList[index].textAsset, missionSOManager.missionList[index].optionMissionIndex, missionSOManager.missionList[index].optionDescription, missionSOManager.missionList[index].eventIndex);
        bool hasEvent = TryLoadMissionEvent(currentMission.eventIndex);

        if (currentMission.textString != "" || currentMission.textAsset != null) {
            instance.AddMission(currentMission);
            return;
        }
        else if (hasEvent) {
            return;
        }
        else {
            Debug.LogError("There is nothing in the missionSO!");
            return;
        }
    }
    public void AddMissionSO(int index) {
        if (index >= missionSOManager.missionList.Count || index < 0) {
            Debug.LogError("Wrong index, it's out of the missionSO range!");
            return;
        }
        missionList.Add(new Mission(missionSOManager.missionList[index].textString, missionSOManager.missionList[index].textAsset, missionSOManager.missionList[index].optionMissionIndex, missionSOManager.missionList[index].optionDescription, missionSOManager.missionList[index].eventIndex));
    }
    public void AddMissionSOAtFirst(int index) {
        if (index >= missionSOManager.missionList.Count || index < 0) {
            Debug.LogError("Wrong index, it's out of the missionSO range!");
            return;
        }
        missionList.Insert(0, new Mission(missionSOManager.missionList[index].textString, missionSOManager.missionList[index].textAsset, missionSOManager.missionList[index].optionMissionIndex, missionSOManager.missionList[index].optionDescription, missionSOManager.missionList[index].eventIndex));
    }

    [Header("DialogTree")]
    private Mission currentMission;
    [SerializeField] private Button Option_1;
    [SerializeField] private Button Option_2;
    private void SetUpButton() {
        if (!hasOption) { return; }
        if (currentMission.optionMissionIndex.Count == 1) {
            Option_1.gameObject.GetComponentInChildren<Text>().text = currentMission.optionDescription[0];
            Option_1.gameObject.SetActive(true);
        }
        else if (currentMission.optionMissionIndex.Count == 2) {
            Option_1.gameObject.GetComponentInChildren<Text>().text = currentMission.optionDescription[0];
            Option_2.gameObject.GetComponentInChildren<Text>().text = currentMission.optionDescription[1];
            Option_1.gameObject.SetActive(true);
            Option_2.gameObject.SetActive(true);
        }
        return;
    }
    private void CloseButton() {
        Option_1.gameObject.SetActive(false);
        Option_2.gameObject.SetActive(false);
    }
    public void PlayMissionOption_1() {
        CloseButton();
        ClearMissionRightNow();
        //StartMissionSO(currentMission.optionMissionIndex[0]);
        AddMissionSOAtFirst(currentMission.optionMissionIndex[0]);
    }
    public void PlayMissionOption_2() {
        CloseButton();
        ClearMissionRightNow();
        //StartMissionSO(currentMission.optionMissionIndex[1]);
        AddMissionSOAtFirst(currentMission.optionMissionIndex[1]);
    }
    public bool IsOnMission() {
        return onMission;
    }
    public bool IsOutPutWord() {
        return isOnCoroutine;
    }
    public bool IsOnMissionGap() {
        return missionLock;
    }
    public bool JustFinishPlay() {
        return justFinishMission;
    }
    private bool TryLoadMissionEvent(int index) {
        if(index<0||index>missionEventHandler.missionEvents.Count) {
            return false;
        }
        else {
            missionEventHandler.missionEvents[index].optionEvent?.Invoke();
            return true;
        }
    }
}
