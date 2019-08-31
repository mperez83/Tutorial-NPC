using UnityEngine;

public class SpinningObject : MonoBehaviour
{
    private Tile tile; 
    private int count = 0;
    private Vector3 rotationStartPosition;

    private void Awake() 
    {
        tile = GetComponent<Tile>();
        rotationStartPosition = transform.rotation.eulerAngles;
    }

    private void Update() 
    {
        RotateObject();    
    }

    private void RotateObject()
    { 
        Vector3 rotateTo = new Vector3(0, 0, -90f); 

        // check if the arrow has rotated bewtween 45 and 90 degrees from the starting position
        // to the next cardinal direction
        if ((Mathf.Abs(transform.rotation.eulerAngles.z) - Mathf.Abs(rotationStartPosition.z)) < 90f &&
            (Mathf.Abs(transform.rotation.eulerAngles.z) - Mathf.Abs(rotationStartPosition.z)) > 45f)
        {
            rotationStartPosition = transform.position;

            //ChangeArrowDirection();
        }

        transform.Rotate(rotateTo * Time.deltaTime, Space.Self);
    }

    // Change to the next cardinal direction in order of up, right, down, left
    private void ChangeArrowDirection()
    {

        if (count != 3)
            count++;
        else
            count = 0;

        switch(count)
        {
            case 0:
                tile.tileType = Tile.TileType.TurnUp; 
                Debug.Log("Count is " + count);
                Debug.Log("Tile type is " + tile.tileType);
                break; 
            case 1:
                tile.tileType = Tile.TileType.TurnRight; 
                Debug.Log("Count is " + count);
                Debug.Log("Tile type is " + tile.tileType);
                break;
            case 2: 
                tile.tileType = Tile.TileType.TurnDown; 
                Debug.Log("Count is " + count);
                Debug.Log("Tile type is " + tile.tileType);
                break;
            case 3:
                tile.tileType = Tile.TileType.TurnLeft;
                Debug.Log("Count is " + count);
                Debug.Log("Tile type is " + tile.tileType);
                break; 
        }
    }
}
