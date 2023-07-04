using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class KeyValue 
{
    public string key;
    public string value;

    public static string GetKey(string key, List<KeyValue> data)
    {
        for (int i = 0; i < data.Count; i++)
        {
            if (data[i].key == key)
            {
                return data[i].value;
            }
        }
        return string.Empty;
    }
}
