using TMPro.Examples;
using UnityEngine;
public class CardDisplay : MonoBehaviour
{
    private Camera mainCamera;
    private bool isDragging;  // 正在被拖移 // 私有或區域變數小大寫
    private float zCoordinate; // 卡牌和相機距離
    private bool isOnTable = false;
    private Vector3 handCardPosition; // 手牌原本位置
    private HandManager handManager;
    private int currentCardSpaceIndex = -1; // 卡牌放哪個洞(-1==未放置)

    void Start()
    {
        mainCamera = Camera.main;
        handCardPosition = transform.position;

        // 系統函式：FindAnyObjectByType (在整個遊戲場景中搜尋掛載了 HandManager 腳本的物件)
        handManager = FindAnyObjectByType<HandManager>();
    }
    
    void OnMouseDown() // 卡牌被滑鼠按下 // 公開函式或變數或檔名大大寫
    {
        isDragging = true;

        Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position);
        zCoordinate = screenPos.z;
    }

    void Update()
    {
        if(isDragging == true)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = zCoordinate;

            Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);
            transform.position = worldPos;
        }
    }

    void OnMouseUp()
    {
        isDragging = false;

        if (isOnTable) // 不在桌上
        {
            if(currentCardSpaceIndex!=-1)handManager.isCardSpaceFull[currentCardSpaceIndex] = false;
            currentCardSpaceIndex = -1;
        }
        else
        {
            int allocatedSpace = handManager.RequestEmptySpace(gameObject);
            if(allocatedSpace != -1)
            {
                currentCardSpaceIndex = allocatedSpace;
            }
            else
            {
                //沒位置 == 手不能拿新牌
            }
        }
    void OnTriggerEnter(Collider other) // 進入 maybe table 的觸發區 // other 置物櫃存放進入觸發區的物件
    {
        if(other.gameObject.name == "Table")
        {
            isOnTable = true; Debug.Log("牌上桌");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.name == "Table")
        {
            isOnTable= false; Debug.Log("牌移");
        }
    }
    }

    

}