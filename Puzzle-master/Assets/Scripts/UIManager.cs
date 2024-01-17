using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public GameModelManager gameModelManager;
    public GameObject modelMenuImagePrefab;
    public Transform modelMenuParent;
    public FirstSceneScripts firstSceneScripts;
    public Image[] galleryLittleImages;
    public Image galleryMainImage;
    public Material shaderMaterial;

    public GameManager gameManager;
    public GameObject page_2, page_3, page_5, page_6;


    public Button page_3PlayButton;
    public Sprite lockedModel;

    public AudioManagerr audioManagerr;

    public Text galleryImageInfo;

    private void Start()
    {
        placeTheImages();
        gameModelManager.checkIfAllCompleted();
    }

    void placeTheImages()
    {
        var models = gameModelManager.models;

        for(var i = 0; i < models.Length; i++)
        {
            var newModel = Instantiate(modelMenuImagePrefab, modelMenuParent);
            var localId = models[i].model_id;
            string localClipName = models[i].model_voice_name;

            newModel.transform.GetChild(0).GetComponent<Image>().sprite = models[i].model_general_image;
            newModel.transform.GetChild(1).GetComponent<Image>().sprite = models[i].model_view_text;
            if (i == models.Length - 1)
            {
                newModel.transform.GetChild(0).GetComponent<Image>().sprite = lockedModel;
                //newModel.transform.GetChild(1).GetComponent<Image>().color = Color.black;
                newModel.GetComponent<Button>().interactable = false;
            }


            newModel.GetComponent<Button>().onClick.AddListener(delegate () { firstSceneScripts.select(localId); });
            newModel.GetComponent<Button>().onClick.AddListener(delegate () { selectAModel(localId); });
            newModel.GetComponent<Button>().onClick.AddListener(delegate () {
                hoverAModel(newModel.transform);
                audioManagerr.Play(localClipName);
            });
            

            var LocalI = i;
            galleryLittleImages[2*i].sprite = models[i].gameModelChildren[0].getSmallIcon();
            galleryLittleImages[2*i+1].sprite = models[i].gameModelChildren[1].getSmallIcon();
            
            galleryLittleImages[2*i].GetComponent<Button>().onClick.AddListener(delegate() { changeMainPictureToThis(models[LocalI].gameModelChildren[0].id); });
            galleryLittleImages[2*i+1].GetComponent<Button>().onClick.AddListener(delegate () { changeMainPictureToThis(models[LocalI].gameModelChildren[1].id); });

            galleryLittleImages[2*i].GetComponent<Button>().interactable = models[i].gameModelChildren[0].unlocked;
            galleryLittleImages[2*i+1].GetComponent<Button>().interactable = models[i].gameModelChildren[1].unlocked;
        }

    }


    public void updateGalleryImages()
    {
        var models = gameModelManager.models;
        for (var i = 0; i < models.Length; i++)
        {
            galleryLittleImages[2 * i].sprite = models[i].gameModelChildren[0].getSmallIcon();
            galleryLittleImages[2 * i + 1].sprite = models[i].gameModelChildren[1].getSmallIcon();

            galleryLittleImages[2 * i].GetComponent<Button>().interactable = models[i].gameModelChildren[0].unlocked;
            galleryLittleImages[2 * i + 1].GetComponent<Button>().interactable = models[i].gameModelChildren[1].unlocked;
        }
    }


    public void activateTheGiftModel()
    {

    }
    public void hoverAModel(Transform Obj)
    {
        Transform ObjNew = Obj.GetChild(0); // this is Prefab child called "Up"
        var n = Obj.GetSiblingIndex(); // this is child index (Current Prefab index)
        ObjNew.gameObject.GetComponent<Image>().material = shaderMaterial;
        
        for (int i = 0; i < Obj.transform.parent.childCount; i++)
        {
            if (i != n)
            {
                //Debug.Log("For id= " + i);
                //Debug.Log("MY id=" + n);
                Obj.transform.parent.GetChild(i).transform.GetChild(0).gameObject.GetComponent<Image>().material = null;
            }
        }
    }


    public void changeMainPictureToThis(string id)
    {
        galleryImageInfo.gameObject.SetActive(false);

        galleryMainImage.gameObject.SetActive(true);

        galleryMainImage.sprite = getMainImageWithChildId(id);
    }


    Sprite getMainImageWithChildId(string id)
    {
        for (int i = 0; i < gameModelManager.models.Length; i++)
        {
            for (int j = 0; j < gameModelManager.models[i].gameModelChildren.Length; j++)
            {
                if (gameModelManager.models[i].gameModelChildren[j].id.Equals(id))
                {
                    return gameModelManager.models[i].gameModelChildren[j].model_picture_960;
                }
            }   
        }

        return null;
    }




    public void backButton()
    {
        if (page_6.activeSelf)
        {
            panelLoader(page_6, page_3);
        }
        else if (page_5.activeSelf)
        {
            panelLoader(page_5, page_3);
        }
    }



    public void selectAModel(int modelId)
    {
        gameManager.selectedId = modelId;
        page_3PlayButton.interactable = true;
    }


    public void playForPage_3() //opens the game page
    {
        panelLoader(page_3, page_5);  
    }


    public void openPage_6_gallery() //opens the gallery
    {
        if (page_3.activeSelf)
        {
            panelLoader(page_3, page_6);
        }
        else if(page_5.activeSelf)
        {
            panelLoader(page_5, page_6);
        }
    }

    public void playForPage_2() // opens the model selection page
    {
        page_3PlayButton.interactable = false;
        panelLoader(page_2, page_3);
    }


    public void quit()
    {
        Application.Quit();
    }



    void panelLoader(GameObject panelToBeCLosed, GameObject panelToBeOpened)
    {
        panelToBeCLosed.SetActive(false);
        panelToBeOpened.SetActive(true);
    }
}