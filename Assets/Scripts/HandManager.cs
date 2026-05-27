using UnityEngine;

public class HandManager : MonoBehaviour
{
    public Transform[] CardSpaces;
    public bool[] isCardSpaceFull;

    public int RequestEmptySpace(GameObject card)
    {
        for(int i = 0; i < CardSpaces.Length; i++)
        {
            if (isCardSpaceFull[i] == false)
            {
                isCardSpaceFull[i] = true;
                card.transform.position = CardSpaces[i].position;
                return i;
            }
        }
        return -1;
    }
}
