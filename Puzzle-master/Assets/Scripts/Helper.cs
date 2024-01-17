using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper
{
    #region Saving and Getting unlocked ids of Models
    static string lockState = "modelStatesList";
    
    public static void setLockState(string id)
    {
        PlayerPrefs.SetString(lockState, getLockedState() + $"{id},");
    }

    public static string getLockedState()
    {
        return PlayerPrefs.GetString(lockState, "");
    }

    public static string[] getLockedStateSplitted()
    {
        return getLockedState().Split(',');
    }
    #endregion

    #region Saving and Getting completed models' ids

    static string completeState = "modelCompletedList";

    public static void setCompleteStatus(string id)
    {
        PlayerPrefs.SetString(completeState, getCompleteStatus() + $"{id},");
    }

    public static string getCompleteStatus()
    {
        return PlayerPrefs.GetString(completeState, "");
    }

    public static string[] getCompleteStatusSplitted()
    {
        return getCompleteStatus().Split(',');
    }
    #endregion


    #region Names of model tile positions and the latest (wPart and hPart) in local memory
    public static void saveTilePositions(string id,int wPart, int hPart, List< GameObject > listOFGameObjects)
    {
        deleteAllPrefs(id);//delete the previous saved data
        TileData[] tileDatas= new TileData[wPart * hPart];
        for (int i = 0; i < listOFGameObjects.Count; i++)
        {
            if(listOFGameObjects[i].activeSelf)
                tileDatas[i] = new TileData(listOFGameObjects[i].name, listOFGameObjects[i].transform.localPosition.x, listOFGameObjects[i].transform.localPosition.y);
        }
        PlayerPrefs.SetString(id,JsonUtility.ToJson(new Tile(wPart, hPart,tileDatas)));
        PlayerPrefs.Save();
        checkSavedData(id);
    }

    public static void deleteAllPrefs(string id)
    {
        PlayerPrefs.DeleteKey(id);
    }

    public static TileData[] getTileDatasArray(string id)
    {
        Tile tile = JsonUtility.FromJson<Tile>(PlayerPrefs.GetString(id));
        return tile.tiles;
    }
    public static TileData getTileDetailsByName(string name,string id)
    {
        TileData[] tiles = getTileDatasArray(id);
        foreach (TileData item in tiles)
        {
            if (item.name == name)
            {
                return item;
            }
        }

        return null;
    }
    public static string getTileCountOfSavedImage(string id)
    {
        Tile tile = JsonUtility.FromJson<Tile>(PlayerPrefs.GetString(id));
        return (tile.wPart * tile.hPart).ToString();
    }
    public static bool checkSavedData(string id)
    {
        return PlayerPrefs.GetString(id).Length==0 ? false : true;
    }

    #endregion
}
[System.Serializable]
public class Tile
{
    public int wPart;
    public int hPart;
    public TileData[] tiles;

    public Tile(int wPart, int hPart, TileData[] tiles)
    {
        this.wPart = wPart;
        this.hPart = hPart;
        this.tiles = tiles;
    }
}
[System.Serializable]
public class TileData
{
    public string name;
    public float x;
    public float y;

    public TileData(string name, float x, float y)
    {
        this.name = name;
        this.x = x;
        this.y = y;
    }
}