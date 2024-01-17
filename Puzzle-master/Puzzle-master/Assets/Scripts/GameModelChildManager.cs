using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModelChildManager : MonoBehaviour
{
    [HideInInspector]
    public GameModelChild selectedGameModel;
    public GameObject mainGameObject;
    [HideInInspector]
    public GameModelChild gameModelChild_1;
    [HideInInspector]
    public GameModelChild gameModelChild_2;
    [HideInInspector]
    public int parentID;
    GameModel gameModel;

    public void getSelectedModel(int index_of_image=-1)
    {
        parentID = mainGameObject.GetComponent<GameManager>().selectedId;
        gameModel = mainGameObject.GetComponent<GameModelManager>().getModelByID(parentID);//getting selected model to play

        if (index_of_image == -1)
        {
            if (!gameModel.gameModelChildren[0].completed)
            {
                selectedGameModel = gameModel.gameModelChildren[0];
            }
            else
            {
                selectedGameModel = gameModel.gameModelChildren[1];
            }
            gameModelChild_1 = gameModel.gameModelChildren[0];
            gameModelChild_2 = gameModel.gameModelChildren[1];
            //GetComponent<Slicer>().setProperModels(true);
        }
        else
        {
            selectedGameModel = gameModel.gameModelChildren[index_of_image];
        }

        GetComponent<SliceController>().startSplit();
        
    }
}
