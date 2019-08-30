using UnityEngine;

public class UIReloadCurrentSceneOnClick : MonoBehaviour
{
    public void RestartCurrentScene()
    {
        GameMaster.Instance.ReloadCurrentScene();
        AudioManager.Instance.Restart("Building Soundtrack");
    }
}
