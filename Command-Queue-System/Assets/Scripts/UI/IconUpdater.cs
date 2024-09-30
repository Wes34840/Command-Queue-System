using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class IconUpdater : MonoBehaviour
{
    public Sprite[] imageArray;
    public List<GameObject> iconQueue;
    public GameObject iconPrefab;

    public Dictionary<Vector2, Sprite> spritesDictionary;

    private void Start()
    {
        PlayerMove.addIcon = AddIcon;
        PlayerMove.removeIcon = RemoveIcon;
        iconQueue = new List<GameObject>();
        spritesDictionary = new Dictionary<Vector2, Sprite>
        {
            { new Vector2(0, 1), imageArray[0] },   // Up arrow
            { new Vector2(0, -1), imageArray[1] },  // Down arrow
            { new Vector2(1, 0), imageArray[2] },   // Right arrow
            { new Vector2(-1, 0), imageArray[3] },  // Left arrow
            { new Vector2(0, 0), imageArray[4] }    // Attack symbol
        };
    }

    public void AddIcon(Vector2 direction)
    {
        // Add a sprite of the direction or action of the player onto the screen
        GameObject newIcon = Instantiate(iconPrefab, Vector3.zero, Quaternion.identity);
        RectTransform parentTransform = GetComponent<RectTransform>();
        newIcon.transform.SetParent(transform);
        newIcon.GetComponent<RectTransform>().localPosition = new Vector3(parentTransform.localPosition.x - 700 - (iconQueue.Count * -150), parentTransform.localPosition.y, parentTransform.localPosition.z);
        newIcon.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        newIcon.GetComponent<Image>().sprite = DetermineSprite(direction);
        iconQueue.Add(newIcon);
    }

    public void RemoveIcon()
    {
        // Remove the latest added icon from the screen
        GameObject icon = iconQueue.Last();
        iconQueue.Remove(icon);
        Destroy(icon);
    }

    public Sprite DetermineSprite(Vector2 direction)
    {
        // return the sprite of the direction, if there is no direction, it's an attack command
        return spritesDictionary[direction];
    }

}
