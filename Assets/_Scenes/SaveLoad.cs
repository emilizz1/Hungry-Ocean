using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    public void SaveInt(string path, int intToSave)
    {
        PlayerPrefs.SetInt(path, intToSave);
        PlayerPrefs.Save();
    }

    public int LoadInt(string path)
    {
        return PlayerPrefs.GetInt(path);
    }


}
