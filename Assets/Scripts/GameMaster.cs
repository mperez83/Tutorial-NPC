using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    public static GameMaster instance;
    public LevelData[] levels;
    public int levelIndex;

    public bool seenCutscene;
    [HideInInspector]
    public bool TileMapInventoryItemSelected;
    [HideInInspector]
    public bool VictoryScreenActivated;
    [HideInInspector]
    public bool placingUIItem;
    public event Action<GameObject> OnInventoryItemSelected;
    public event Action OnInventoryItemDeselected;
    public event Action<GameObject> OnInventoryItemAdded;



    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else instance = this;
        DontDestroyOnLoad(this);
    }

    public void AdvanceLevel()
    {
        levelIndex++;
        if (levelIndex > levels.Length - 1) levelIndex = 0;
        SetLevel(levels[levelIndex].levelName);
    }

    public void SetLevel(string levelName)
    {
        //Update the levelIndex
        for (int i = 0; i < levels.Length; i++)
        {
            if (levelName == levels[i].levelName)
                levelIndex = i;
        }

        //Actually start loading the scene
        StartCoroutine(LoadTheAsyncScene(levelName));
    }

    public void SetCurrentLevelBeaten()
    {
        if (levels[levelIndex].beaten == false) levels[levelIndex].beaten = true;
    }

    public void SetNextLevelAvailable()
    {
        //Make sure we're not on the last level
        if (levelIndex < levels.Length - 1)
            if (levels[levelIndex + 1].isAvailable == false)
                levels[levelIndex + 1].isAvailable = true;
    }

    public void LoadSceneRaw(string sceneToLoad)
    {
        StartCoroutine(LoadTheAsyncScene(sceneToLoad));
    }

    IEnumerator LoadTheAsyncScene(string levelToLoad)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelToLoad);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    internal void InventoryItemSelected(GameObject inventoryItem)
    {
        if (OnInventoryItemSelected != null)
            OnInventoryItemSelected(inventoryItem);
    }

    internal void InventoryItemDeselected()
    {
        if (OnInventoryItemSelected != null)
            OnInventoryItemDeselected();

        TileMapInventoryItemSelected = false;
    }

    internal void AddInventoryItemToMap(GameObject inventoryItemController)
    {
        if (OnInventoryItemAdded != null)
            OnInventoryItemAdded(inventoryItemController);
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    public float GetCamTopEdge()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, -(Camera.main.transform.position.z))).y;
    }

    public float GetCamBottomEdge()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(0, 0, -(Camera.main.transform.position.z))).y;
    }

    public float GetCamLeftEdge()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(0, 0, -(Camera.main.transform.position.z))).x;
    }

    public float GetCamRightEdge()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, -(Camera.main.transform.position.z))).x;
    }

    public bool GetIfCoordsAreInsideCam(float x, float y)
    {
        if (y < GetCamTopEdge() && y > GetCamBottomEdge() && x > GetCamLeftEdge() && x < GetCamRightEdge())
            return true;
        else
            return false;
    }



    [System.Serializable]
    public struct LevelData
    {
        public string levelName;
        public bool isAvailable;
        public bool beaten;
    }
}