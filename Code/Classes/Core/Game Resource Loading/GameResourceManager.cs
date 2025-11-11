using UnityEngine;
using System;
using System.IO;

//! This class is responsible for getting all the Stat instances and saving or loading them to/from a JSON file.
//! This allows for both resetting of values outside the objects themselves and opens them up for modding.
//! Reference this class as desired, menu items, player controls, whatever. Yay!

public class GameResourceManager
{
    //! Change this to change the location of the saved JSON object.
    //! Use forward slashs (/) for path.
    //! TO DO: Update to create folder if does not exist. Currently does not.
    private string saveFolder = "Modding";

    public bool SaveResourcesToFile()
    {
        string stringToSave;
        bool success = false;

        // Try loading the resources and also save them to file.
        // Catch any exception if any step fails, print out and return false.
        try
        {
            stringToSave = WrapperToJSONString(GetWrapper(LoadResourcesFromAssets()));

            string filePath = Path.Combine(Application.persistentDataPath, saveFolder + "/StartingValues.json");
            File.WriteAllText(filePath, stringToSave);
            Debug.Log("Data saved to: " + filePath);

            success = true;
        }
        catch(Exception e)
        {
            Debug.Log("Error saving resources to file: " + e.Message);
        }

        return success;
    }

    public bool LoadResourcesFromFile()
    {
        bool success = false;

        try
        {
            StatGameResource[] loadedValues = GetStatsGameResourcesFromJSON();
            UnityEngine.Object[] gameStats = Resources.LoadAll("Game Resources", typeof(Stat));
            foreach (StatGameResource sr in loadedValues)
            {
                foreach(Stat s in gameStats)
                {
                    if(s.name == sr.statName)
                    {
                        if (s.IsBoolValue)
                        {
                            s.DefaultValue = Convert.ToInt32(Convert.ToBoolean(sr.startValue));
                            Debug.Log("Boolean Stat loaded from JSON: " + s.name + " - value updated to: " + s.DefaultValue + "(float) / " + sr.startValue + "(bool)");
                        }
                        else
                        {
                            if (float.TryParse(sr.startValue, out float result))
                            {
                                s.DefaultValue = result;
                                Debug.Log("Stat loaded from JSON: " + s.name + " - value updated to: " + s.DefaultValue);
                            }
                            else
                            {
                                Debug.Log("Was unable to convert float on load for " + s.name + " - tried value: " + sr.startValue);
                            }
                        }

                        break;
                    }
                }
            }
        }
        catch(Exception e)
        {
            Debug.Log("Error loading resources from file: " + e.Message);
        }

        return success;
    }

    private ArrayWrapper GetWrapper(UnityEngine.Object[] objs)
    {
        return new ArrayWrapper(objs);
    }

    private UnityEngine.Object[] LoadResourcesFromAssets()
    {
        return Resources.LoadAll("Game Resources", typeof(Stat));
    }

    private string WrapperToJSONString(ArrayWrapper wrapper)
    {
        return JsonUtility.ToJson(wrapper, true);
    }

    private StatGameResource[] GetStatsGameResourcesFromJSON()
    {
        string filePath = Path.Combine(Application.persistentDataPath, saveFolder + "/StartingValues.json");
        string newJSONString = File.ReadAllText(filePath);

        ArrayWrapper loadedValues = JsonUtility.FromJson<ArrayWrapper>(newJSONString);

        return loadedValues.gameStartingValues;
    }
}

[Serializable]
public class ArrayWrapper
{
    public StatGameResource[] gameStartingValues;
    public ArrayWrapper(UnityEngine.Object[] objs) 
    {
        gameStartingValues = new StatGameResource[objs.Length];
        for (int i = 0; i < objs.Length; i++)
        {
            Stat s = objs[i] as Stat;
            StatGameResource sr = new StatGameResource();
            sr.statName = s.name;

            if (s.IsBoolValue)
            {
                sr.startValue = Convert.ToBoolean(s.DefaultValue).ToString();
            }
            else
            {
                sr.startValue = s.DefaultValue.ToString();
            }
            
            sr.moddingNote = s.ModdingNote;


            gameStartingValues[i] = sr;
        }
    }
}

[Serializable]
public class StatGameResource
{
    public string statName;
    public string startValue;
    public string moddingNote;
}