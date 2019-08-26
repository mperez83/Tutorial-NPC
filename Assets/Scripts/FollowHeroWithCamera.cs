using Cinemachine;
using UnityEngine;

public class FollowHeroWithCamera : MonoBehaviour
{
    private MapHandler mapHandler; 
    private PlayerHandler playerHandler; 
    private CinemachineVirtualCamera vcam; 

    private void Awake() 
    {
        mapHandler = FindObjectOfType<MapHandler>();
        mapHandler.OnLevelGenerated += AttachHeroToCamera;

        playerHandler = FindObjectOfType<PlayerHandler>(); 

        vcam = GetComponent<CinemachineVirtualCamera>(); 
    }

    private void AttachHeroToCamera()
    {
        vcam.Follow = playerHandler.player.transform; 
    }
    
}
