using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GameModel", order = 1)]
public class GameModel : ScriptableObject
{
    #region General details for model

    public string model_name;
    public int model_id;
    public Sprite model_general_image;
    public Sprite model_view_text;
    public String model_voice_name;

    #endregion

    #region Details about children of model

    public GameModelChild[] gameModelChildren;
    
    #endregion

}
[System.Serializable]
public class GameModelChild
{
    public string id; //model_id + some value to be unique

    public Texture2D model_picture_1200;//for game area

    public Sprite model_sprite_1200;

    public Sprite model_picture_960;//for gallery

    public Sprite model_picture_160;//for left right side of pages
    public Sprite model_picture_160_blurred;// for left right side of pages blurred version

    public bool unlocked = false;//whenever user unlocked this will be true for good
    public bool completed = false;//whenever user completed this will be true for good
    public Sprite getSmallIcon()
    {
        return unlocked ? model_picture_160 : model_picture_160_blurred;// this function will return blurred picture of model,
        // if user did not completed this,yet.
    }
    public Sprite getOriginalSmallIcon()
    {
        return model_picture_160;
    }
    public Sprite getModelSprite1200()
    {
        return model_sprite_1200;
    }
}
