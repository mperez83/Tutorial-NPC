using UnityEngine;

public class UIReloadCurrentSceneOnClick : MonoBehaviour
{
    public void RestartCurrentScene()
    {
        GameMaster.instance.ReloadCurrentScene();
        AudioManager.instance.Play("Building Soundtrack");
    }
}
