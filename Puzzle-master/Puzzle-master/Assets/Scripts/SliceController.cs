using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SliceController : MonoBehaviour
{
    Dictionary<string, SliceSizes> dictionary= new Dictionary<string, SliceSizes>();

    public string selectedModelID;

    public void sliceByButton(string size){
        selectedModelID = GetComponent<GameModelChildManager>().selectedGameModel.id;
        if (Helper.checkSavedData(selectedModelID))
            Helper.deleteAllPrefs(selectedModelID);//delete all previous data

        settingWeightAndHeight(size);
        GetComponent<Slicer>().shuffle();
    } 

    public void startSplit(){
        string size;

        setProperDictionary();

        if (Helper.checkSavedData(GetComponent<GameModelChildManager>().selectedGameModel.id.ToString()))
        {
            size = Helper.getTileCountOfSavedImage(GetComponent<GameModelChildManager>().selectedGameModel.id.ToString());
            //print($"size : {size}");
            selectedModelID = GetComponent<GameModelChildManager>().selectedGameModel.id;
            //print($"{selectedModelID} {size}");
        }
        else
        {
            size = "12";
        }

        initialSet(size);
    }

    void settingWeightAndHeight(string name){
        GetComponent<Slicer>().setWPart(dictionary[name].getWeight());
        GetComponent<Slicer>().setHPart(dictionary[name].getHeight());
        
    }
    void initialSet(string size)
    {
        settingWeightAndHeight(size);
        GetComponent<Slicer>().setProperModels(true);
    }
    void setProperDictionary()
    {
        if (dictionary.Count != 0)
            return;
        dictionary.Add("9", new SliceSizes(3, 3));
        dictionary.Add("12", new SliceSizes(3, 4));
        dictionary.Add("16", new SliceSizes(4, 4));
    }
}

[System.Serializable]
public class SliceSizes{
    int weight;
    int height;

    public SliceSizes(int weight, int height)
    {
        this.weight = weight;
        this.height = height;
    }

    public int getWeight(){
        return weight;
    }

    public int getHeight(){
        return height;
    }
}
