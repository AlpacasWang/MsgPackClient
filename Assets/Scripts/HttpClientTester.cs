using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using MsgPack;

public class TestStruct
{
    public int TestInt;
    public Int16 TestInt16;
    public Int32 TestInt32;
    public Int64 TestInt64;
    public uint TestUint;
    public UInt16 TestUint16;
    public UInt32 TestUint32;
    public UInt64 TestUint64;
}

public class HttpClientTester : MonoBehaviour
{
    private readonly string serverHost = "http://127.0.0.1:1212/test";
    private bool isProcessing = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnSendMinData()
    {
        if (isProcessing)
        {
            return;
        }

        var testData = new TestStruct()
        {
            TestInt = int.MinValue,
            TestInt16 = Int16.MinValue,
            TestInt32 = Int32.MinValue,
            TestInt64 = Int64.MinValue,
            TestUint = uint.MinValue,
            TestUint16 = UInt16.MinValue,
            TestUint32 = UInt32.MinValue,
            TestUint64 = UInt64.MinValue,
        };

        StartCoroutine(requestByUnityWebRequest(serverHost, serialize(testData)));
    }

    public void OnSendMaxData()
    {
        if (isProcessing)
        {
            return;
        }
        var testData = new TestStruct()
        {
            TestInt = int.MaxValue,
            TestInt16 = Int16.MaxValue,
            TestInt32 = Int32.MaxValue,
            TestInt64 = Int64.MaxValue,
            TestUint = uint.MaxValue,
            TestUint16 = UInt16.MaxValue,
            TestUint32 = UInt32.MaxValue,
            TestUint64 = UInt64.MaxValue,
        };

        StartCoroutine(requestByUnityWebRequest(serverHost, serialize(testData)));
    }


    private byte[] serialize(TestStruct data)
    {
        ObjectPacker packer = new MsgPack.ObjectPacker();
        var packedData = packer.Pack(data);
        return packedData;
    }

    private IEnumerator requestByUnityWebRequest(string url, byte[] data)
    {
        isProcessing = true;
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            // ハンドラの生成、及び送信データの設定
            request.uploadHandler = new UploadHandlerRaw(data);
            request.downloadHandler = new DownloadHandlerBuffer();

            // 送信
            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
            }
        }
        isProcessing = false;
    }
}
