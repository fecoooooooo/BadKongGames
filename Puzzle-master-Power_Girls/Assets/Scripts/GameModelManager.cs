using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class GameModelManager : MonoBehaviour
{
    public GameModel[] models;

    [SerializeField]
    string defaultID;// 1 model should always be unlocked

    [SerializeField]
    private int bonusModelID;

    public Transform modelsParent;
    
    
    
    private void Awake()
    {
        if (Helper.getLockedState().Equals(""))//if there is no any other unlocked model, set default 1 as unlocked always
            Helper.setLockState(defaultID);

        fetchAndSetModelsStates(); //fetching states of models from local Memory ( locked , unlocked )
        fetchAndSetModelsCompleteStatuses(); //fetching completed statuses of models from local Memory ( completed , uncompleted )
    }

    public GameModel getModelByID(int id)//getting model by its ID
    {
        return Array.Find(models, model => model.model_id == id);
    }


    private void fetchAndSetModelsStates() // to get all previously unlocked models and apply it into our array
    {
        string[] arrayOfIds = Helper.getLockedStateSplitted(); // get the unlocked models ids
        foreach (string id in arrayOfIds)
        {
            foreach (GameModel gameModel in models)//set the unlocked if id is in arrayOfIds
            {
                if (gameModel.gameModelChildren[0].id.Equals(id))//set child 1 if it is unlocked
                {
                    gameModel.gameModelChildren[0].unlocked = true;
                    break;
                }
                else if (gameModel.gameModelChildren[1].id.Equals(id))//set child 2 if it is unlocked
                {
                    gameModel.gameModelChildren[1].unlocked = true;
                    break;
                }
            }

        }
    }

    private void fetchAndSetModelsCompleteStatuses() // to get all previously unlocked models and apply it into our array
    {
        string[] arrayOfCompletedIds = Helper.getCompleteStatusSplitted();//get the completed models ids

        foreach (string id in arrayOfCompletedIds)
        {
            foreach (GameModel gameModel in models)//set the unlocked if id is in arrayOfIds
            {
                if (gameModel.gameModelChildren[0].id.Equals(id))//set child 1 if it is unlocked
                {
                    gameModel.gameModelChildren[0].completed = true;
                    break;
                }
                else if (gameModel.gameModelChildren[1].id.Equals(id))//set child 2 if it is unlocked
                {
                    gameModel.gameModelChildren[1].completed = true;
                    break;
                }
            }

        }
    }
    private void getElements()
    {
        Debug.Log(models[0].model_name);
    }

    public void checkIfAllCompleted()
    {
        var completed = true;

        for (int i = 0; i < models.Length; i++)
        {
            if (models[i].model_id == bonusModelID)//if it is akuma
            {
                continue;
            }

            for (int j = 0; j < models[i].gameModelChildren.Length; j++)
            {
                if (!models[i].gameModelChildren[j].completed)
                {
                    completed = false;
                    break;
                }
            }

            if (!completed)
            {
                break;
            }
        }

        if (completed)
        {
            modelsParent.GetChild(modelsParent.childCount - 1).GetComponent<Button>().interactable = true;

            modelsParent.GetChild(modelsParent.childCount - 1).GetChild(0).GetComponent<Image>().sprite = models[models.Length-1].model_general_image;
            modelsParent.GetChild(modelsParent.childCount - 1).GetChild(1).GetComponent<Image>().color = Color.white;
        }
    }
}
