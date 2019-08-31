using UnityEngine;

public class UIReloadCurrentSceneOnClick : MonoBehaviour
{
    public void RestartCurrentScene()
    {
        GameMaster.instance.ReloadCurrentScene();
        AudioManager.Instance.Restart("Building Soundtrack");
    }
}
