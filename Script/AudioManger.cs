using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManger
{

    private static AudioManger instance;
    public static AudioManger Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AudioManger();
            }
            return instance;
        }
    }
    private string path = "musicFile/";
    public Dictionary<string, AudioClip> clips;
    public Dictionary<int, GameObject> sourseObjs;

    public AudioManger()
    {
        clips = new Dictionary<string, AudioClip>();
        sourseObjs = new Dictionary<int, GameObject>();
    }

    public void PlayAudio(string clipName)
    {
        AudioClip tempPlayClip = CheckPlayClip(clipName);

        AudioSource.PlayClipAtPoint(tempPlayClip, Camera.main.transform.position);
    }

    public void PlayAudio(string clipName, Vector3 playPoint)
    {
        AudioClip tempPlayClip = CheckPlayClip(clipName);
        AudioSource.PlayClipAtPoint(tempPlayClip, playPoint);
    }

    public AudioSource PlayAudio(string clipName, Transform parent, int index)
    {
        if (sourseObjs.ContainsKey(index))
        {
            sourseObjs.Remove(index);
        }
        AudioClip tempPlayClip = CheckPlayClip(clipName);
        GameObject tempObj = new GameObject();
        AudioSource source = tempObj.AddComponent<AudioSource>();
        tempObj.transform.SetParent(parent);
        tempObj.transform.localPosition = Vector3.zero;
        source.clip = tempPlayClip;
        source.loop = false;
        source.Play();
        sourseObjs.Add(index, tempObj);
        return source;
    }

    public void DeleteClipObj(int index)
    {
        if (sourseObjs.ContainsKey(index))
        {
            GameObject tempObj = sourseObjs[index];
            sourseObjs.Remove(index);
            GameObject.Destroy(tempObj);
        }
    }

    private AudioClip LoadClip(string path)
    {
        AudioClip tempClip = Resources.Load<AudioClip>(path);
        if (tempClip != null)
        {
            return tempClip;
        }
        else
        {
            Debug.Log("读取音频文件失败");
            return null;
        }
    }

    private AudioClip CheckPlayClip(string clipName)
    {
        AudioClip tempPlayClip = null;
        if (!clips.ContainsKey(clipName))
        {
            string tempPath = path + clipName;
            tempPlayClip = LoadClip(tempPath);
            clips.Add(clipName, tempPlayClip);
        }
        else
        {
            tempPlayClip = clips[clipName];
        }
        return tempPlayClip;
    }
}
