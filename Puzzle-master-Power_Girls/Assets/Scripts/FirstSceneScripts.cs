using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstSceneScripts : MonoBehaviour
{
    public GameManager FirstGameManager;

    // Start is called before the first frame update
    public void select(int current_id)
    {
        FirstGameManager.selectedId = current_id;
    }

}
