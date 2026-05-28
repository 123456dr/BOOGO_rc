using UnityEngine;

public class HandManager : MonoBehaviour
{
    public Transform[] CardSpaces;
    public bool[] isCardSpaceFull;

    public int RequestNearestEmptySpace(Vector3 cardPosition)
    {
        int nearestIndex = -1;
        float shortestDistance = Mathf.Infinity; // 無限大

        for(int i = 0; i < CardSpaces.Length; i++)
        {
            if(isCardSpaceFull[i] == false)
            {
                float distance = Vector3.Distance(cardPosition, CardSpaces[i].position); // 兩點之間的 3D 距離
                
                if(distance < shortestDistance)
                {
                    shortestDistance = distance;
                    nearestIndex = i;
                }
            }
        }

        if(nearestIndex != -1)
        {
            isCardSpaceFull[nearestIndex] = true;
        }

        return nearestIndex;
    }
}
