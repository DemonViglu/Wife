using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Text;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
/// <summary>
/// 脚本功能:ChatGTP发送消息
/// </summary>
public class GPT : MonoBehaviour
{
    #region <Singleton>
    public static GPT instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
            return;
        }
        instance = this;
    }
    #endregion


    [Header("ChatGPT_Api")]
    public string apiKey = string.Empty;
    private string apiUrl = "https://api.openai.com/v1/chat/completions";

    public List<string> messageWithHis = new List<string>();
    public InputField inputField;
    public bool isOnWork;


    public  string promot=string.Empty;
    private void Start()
    {
        if(promot!= string.Empty)
        {
            AddPromot(promot,true);
        }
    }
    public void ToJson()
    {
        ChatGPTJsonObjec chatGPTJsonObjec = new ChatGPTJsonObjec();
        string json = JsonUtility.ToJson(chatGPTJsonObjec, true);
        Debug.Log(json);
    }

    /// <summary>
    /// 携程收发消息
    /// </summary>
    /// <param name="message">发送的消息</param>
    /// 

    #region Private Function to deal with the message
    private string MessListToString()
    {
        string tmp;
        tmp = "[" + messageWithHis[0];
        for (int i = 1; i < messageWithHis.Count; i++)
        {
            tmp += "," + messageWithHis[i];
        }
        tmp += "]";
        return tmp;
    }
    private void SendMessageToChatGPT(string message)
    {
        isOnWork = true;
        // 构建请求体
        string requestBody = "{\"messages\": " + MessListToString() + ", \"max_tokens\": 1024, " +//max_tokens 是收到消息的字节长度 一个token是四个字节 maxtoken越长收到的消息就越长
                     "\"model\": \"gpt-3.5-turbo\"}";  // 设置使用的模型引擎

        StartCoroutine(AsyncSendMessageToChatGPT(requestBody));
        //Debug.Log("发送：" + message);
        Debug.Log("发送：" + requestBody);
    }
    private IEnumerator AsyncSendMessageToChatGPT(string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        using (UnityWebRequest www = new UnityWebRequest(apiUrl, "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(data);//设置上传的数据是字符数组
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", "Bearer " + apiKey);

            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Protocol Error: " + www.error);
                Debug.LogError("Error Response: " + www.downloadHandler.text);
                Debug.LogError("发送的消息失败，请重新创建链接，需要全局科学上网哦" + www.result);
            }
            else
            {
                string receive = www.downloadHandler.text;
                //Debug.Log(receive);//输出收到的所有Json信息
                ChatGPTJsonObjec chatGPTJsonObjec = JsonUtility.FromJson<ChatGPTJsonObjec>(receive);
                //chatGPTJsonObjec.choices[0].message.content  这个就是你收到的回复
                Debug.Log("收到的回复：" + chatGPTJsonObjec.choices[0].message.content);
                lastAnswer = chatGPTJsonObjec.choices[0].message.content;
                messageWithHis.Add("{\"role\": \"assistant\", \"content\": \"" + chatGPTJsonObjec.choices[0].message.content + "\"}");
            }
        }
        isOnWork = false;
    }
    #endregion

    #region public Function to use
    public string lastAnswer = "";
    public void SendMessageByInputField()
    {
        AddMessage(inputField.text);
        SendMessageToChatGPT(inputField.text);
    }
    public void SendMessageByString(string text)
    {
        AddMessage(text);
        SendMessageToChatGPT(text);
    }
    public string GetLastAnswer(bool cleanTheAnswer)
    {
        string tmp=lastAnswer;
        if (cleanTheAnswer&&lastAnswer!="") lastAnswer = "";
        return tmp;
    }
    public void ResetLastAnswer()
    {
        lastAnswer = "";
    }
    public bool IsOnWork()
    {
        return isOnWork;
    }
    public void AddPromot(string promot,bool sendThePrompt)
    {
        messageWithHis.Add("{\"role\": \"system\", \"content\": \"" + promot + "\"}");
        if(sendThePrompt)SendMessageToChatGPT(promot);
    }
    public void AddMessage(string message)
    {
        messageWithHis.Add("{\"role\": \"user\", \"content\": \"" + message + "\"}");
    }
    public void ClearHistoryMessage()
    {
        messageWithHis.Clear();
    }
    #endregion
}
#region JsonData
public class ChatGPTJsonObjec
{
    public string id;
    public string @object;
    public string created;
    public string model;
    public Choices[] choices;
    public Usage usage;
}
[System.Serializable]
public class Choices
{
    public float index;
    public Message message;
    public string finish_reason;
}
[System.Serializable]
public class Message
{
    public string role;
    public string content;
}
[System.Serializable]
public class Usage
{
    public int prompt_tokens;
    public int completion_tokens;
    public int total_tokens;
}
#endregion