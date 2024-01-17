using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Slicer : MonoBehaviour
{   
    [SerializeField]
    int wPart = 3;
    [SerializeField]
    int hPart = 3;
    public GameObject mainGameObject;

    public Texture2D exampleImage;
    [SerializeField]
    private Image image_1;
    [SerializeField]
    private Image image_2;

    [SerializeField]
    private Image fullImage;

    private List<TileDetails> imageTiles=new List<TileDetails>();

    private List<List<double>> exactPositions = new List<List<double>>();

    List<Vector3> listOfPositions= new List<Vector3>();

    private Vector2[,] vector2s;

    private List<GameObject> listOFGameObjects=new List<GameObject>();

    public GameObject parentGM;
    [SerializeField]
    private Transform helperGM;

    [SerializeField]
    private Vector2Int vectorParts;
    [SerializeField]
    private string selectedModelID;
    private GameModelChild gameModelChild;

    [SerializeField]
    private AudioManagerr audioManagerr;

    public bool isCompleted = false;

    public void setProperModels(bool ff=false)//true means it is the first time object is creating, false means user uses left side buttons to choose images
    {

        gameModelChild = GetComponent<GameModelChildManager>().selectedGameModel;

        fullImage.sprite = gameModelChild.getModelSprite1200();//this part will be shown whenever puzzle completed for full UI image

        //gameModelChild.unlocked = true;
        selectedModelID = gameModelChild.id;
        

        exampleImage = gameModelChild.model_picture_1200;

        if (ff)
        {
            image_1.sprite = GetComponent<GameModelChildManager>().gameModelChild_1.getOriginalSmallIcon();
            image_2.sprite = GetComponent<GameModelChildManager>().gameModelChild_2.getOriginalSmallIcon();

            image_1.GetComponent<Button>().onClick.RemoveAllListeners();
            image_2.GetComponent<Button>().onClick.RemoveAllListeners();

            image_2.GetComponent<Button>().onClick.AddListener(() => GetComponent<GameModelChildManager>().getSelectedModel(1));
            image_1.GetComponent<Button>().onClick.AddListener(() => GetComponent<GameModelChildManager>().getSelectedModel(0));
        }

        splitInStart();
    }



    void splitInStart()
    {
        Split(exampleImage, vectorParts.x/hPart, vectorParts.y/wPart);
    }
    
    public void Split(Texture2D image, int width, int height)
    {
        activateFullImageOfPuzzle(false);//disable full Image of Puzzle when the first time puzzle split creation

        imageTiles.Clear();
        imageTiles = new List<TileDetails>();
        listOfPositions.Clear();

        bool perfectWidth = image.width % width == 0;
        bool perfectHeight = image.height % height == 0;
 
        int lastWidth = width;
        if(!perfectWidth)
        {
            lastWidth = image.width - ((image.width / width) * width);
        }
 
        int lastHeight = height;
        if(!perfectHeight)
        {
            lastHeight = image.height - ((image.height / height) * height);
        }
 
        int widthPartsCount = image.width / width + (perfectWidth ? 0 : 1);
        int heightPartsCount = image.height / height + (perfectHeight ? 0 : 1);

        int tmpIndex=0;

        for (int i = 0; i < widthPartsCount; i++)
        {
            for(int j = 0; j < heightPartsCount; j++)
            {
                int tileWidth = i == widthPartsCount - 1 ? lastWidth : width;
                int tileHeight = j == heightPartsCount - 1 ? lastHeight : height;
 
                Texture2D g = new Texture2D(tileWidth, tileHeight);
                
                g.SetPixels(image.GetPixels(i * width, j * height, tileWidth, tileHeight));
                g.name=$"imgTile-{tmpIndex}";
                g.Apply();
                imageTiles.Add(new TileDetails(tmpIndex,g));
                tmpIndex++;
                listOfPositions.Add(Vector3.zero);//just fill list for future use
            }
        }
        shuffle();
    }

    void createImagesByTiles(){
        int tmpIndexImg=0;
        float bX=-(exampleImage.width/2-(imageTiles[0].getTexture().width/2)),bY=-(exampleImage.height/2-(imageTiles[0].getTexture().height/2));

        for (int k = listOFGameObjects.Count; k < wPart*hPart; k++)
        {
            GameObject game=new GameObject();
            game.AddComponent<Image>();
            game.AddComponent<DragHandler>();
            game.AddComponent<CanvasGroup>();
            game.AddComponent<DropHandler>();
            listOFGameObjects.Add(game);
        }
        bool f = Helper.checkSavedData(selectedModelID) && Helper.getTileCountOfSavedImage(selectedModelID)==(wPart*hPart).ToString();
        for (int i = 0; i < hPart; i++)
        {
            int tmpInc=vectorParts.y/wPart;
            for (int j = 0; j < wPart; j++)
            {
                GameObject game=listOFGameObjects[tmpIndexImg];
                if(!game.gameObject.activeInHierarchy){
                    game.SetActive(true);
                }
                Sprite sprite=Sprite.Create(imageTiles[tmpIndexImg].getTexture(), new Rect(0, 0, imageTiles[tmpIndexImg].getTexture().width, imageTiles[tmpIndexImg].getTexture().height),Vector2.zero);
                sprite.name=$"{imageTiles[tmpIndexImg].getTexture().name}";
                game.transform.SetParent(parentGM.transform,false);
                game.GetComponent<Image>().sprite=sprite;
                game.GetComponent<RectTransform>().sizeDelta=new Vector2(imageTiles[tmpIndexImg].getTexture().width, imageTiles[tmpIndexImg].getTexture().height);

                if (f)
                {
                    TileData tileData = Helper.getTileDetailsByName(imageTiles[tmpIndexImg].getIndex().ToString(),selectedModelID);

                    if (tileData == null || game == null) return;

                    game.transform.localPosition = new Vector2(tileData.x, tileData.y);
                }
                else
                {
                    game.transform.localPosition = new Vector2(bX, bY + tmpInc * j);
                }


                game.name=$"{imageTiles[tmpIndexImg].getIndex()}";
                listOfPositions.Insert(imageTiles[tmpIndexImg].getIndex(),game.transform.localPosition);
                
                tmpIndexImg++;
            }
            bY=-(exampleImage.height/2-(imageTiles[0].getTexture().height/2));
            bX+=vectorParts.x/hPart;
        }

        for (int k = tmpIndexImg; k < listOFGameObjects.Count; k++)
        {
            listOFGameObjects[k].SetActive(false);
        }
        checkIfPuzzleCompleted(true);
        // saveTilePositions();
    }
    
    public void shuffle(){
        activateFullImageOfPuzzle(false);
        //if next tile count if differ than current one to prevent unwilling GC
        if (imageTiles.Count!=wPart*hPart){
            Split(exampleImage, vectorParts.x/hPart, vectorParts.y/wPart); 
            return;
        }
        //randomize tiles
        for (int t = 0; t < imageTiles.Count; t++ )
        {
            TileDetails tmp = imageTiles[t];
            int r = Random.Range(t, imageTiles.Count);
            imageTiles[t] = imageTiles[r];
            imageTiles[r] = tmp;
        }
        createImagesByTiles();

    }

 

    public void saveTilePositionsProperly()
    {
        Helper.saveTilePositions(selectedModelID, wPart, hPart, listOFGameObjects);
    }
    public void setWPart(int weight){
        wPart=weight;
    }

    public void setHPart(int height){
        hPart=height;
    }

    #region To define if puzzle completion

    public void checkIfPuzzleCompleted(bool isFirstCheckOfModel=false)
    {
        

        defineFinalPlacesOfTiles();

        for (int i = 0; i < listOFGameObjects.Count; i++)
        {
            if (listOFGameObjects[i].activeSelf)
            {
                int name = int.Parse(listOFGameObjects[i].name);

                if(Vector2.Distance(vector2s[name / wPart, name % wPart], listOFGameObjects[i].transform.localPosition)>1)
                {
                    return;
                }
            }
        }

        callWhenThePuzzleCompleted();

        if (!isFirstCheckOfModel)
            audioManagerr.Play("SetSuccess");

        if (!gameModelChild.unlocked && !isFirstCheckOfModel)// this code unlock model after the first swap, if model was not locked beforehand
        {
            gameModelChild.unlocked = true;
            Helper.setLockState(gameModelChild.id);
        }

        int pieceCount = listOFGameObjects.Count(x => x.activeSelf);
        AchievementsHandler.UnlockByModelId(gameModelChild.id, pieceCount);
        
        Debug.Log("PUZZLE COMPLETED!");
        activateFullImageOfPuzzle(true);
        mainGameObject.GetComponent<GameModelManager>().checkIfAllCompleted();

    }

    public void callWhenThePuzzleCompleted()
    {
        gameModelChild.completed = true;
        Helper.setCompleteStatus(gameModelChild.id);
    }

    private void defineFinalPlacesOfTiles()
    {
        float bX = -(exampleImage.width / 2 - (imageTiles[0].getTexture().width / 2)), bY = -(exampleImage.height / 2 - (imageTiles[0].getTexture().height / 2));
        vector2s = new Vector2[hPart, wPart];

        for (int i = 0; i < hPart; i++)
        {
            int tmpInc = vectorParts.y / wPart;
            for (int j = 0; j < wPart; j++)
            {
                vector2s[i, j] = new Vector2(bX, bY + tmpInc * j);
            }
            bY = -(exampleImage.height / 2 - (imageTiles[0].getTexture().height / 2));
            bX += vectorParts.x / hPart;
        }
    }
    #endregion

    public void activateFullImageOfPuzzle(bool fActive)
    {
        fullImage.enabled = fActive;
        isCompleted = fActive;
        for (int i = 0; i < listOFGameObjects.Count; i++)
        {
            listOFGameObjects[i].GetComponent<Image>().enabled = !fActive;
        }
        
    }
}

class TileDetails{
    int index;
    Texture2D texture;
    Positions tilePosition;
    public TileDetails(int index, Texture2D texture)
    {
        this.index = index;
        this.texture = texture;
    }

    public Texture2D getTexture(){
        return texture;
    }

    public int getIndex(){
        return index;
    }

}

public class Positions{
    Vector2 vector;
    public Vector2 Vector { get => vector; set => vector = value; }
}