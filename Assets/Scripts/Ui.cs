using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.IO;

public class Ui : MonoBehaviour
{
    [SerializeField]private TextAsset doc;
    public void house()
    {
        WriteString("test");
        SceneManager.LoadScene("House");
    }
    [MenuItem("Tools/Write file")]
    public static void WriteString(string str)
    {
        string path = Application.persistentDataPath + "/test.json";
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(str);
        writer.Close();
    }
   public static string ReadString()
   {
       string path = Application.persistentDataPath + "/test.json";
       StreamReader reader = new StreamReader(path);
       return reader.ReadToEnd();
       reader.Close();
   }
    public void outside()
    {
        Debug.Log(ReadString());
        SceneManager.LoadScene("Game");
    }
}
