using TMPro.Examples;
using UnityEngine;
public class CardDisplay : MonoBehaviour
{
    public enum CardType // 列舉 (單選清單)
    {
        Normal, //1-10
        Jack, // J
        Queen, // Q
        King //K
    }

    public enum CardSuit // 花色
    {
        Spades, // 黑桃
        Hearts, // 紅心
        Diamonds, // 方塊
        Clubs  // 梅花
    }

    private Camera mainCamera;
    private bool isDragging;  // 正在被拖移 // 私有或區域變數小大寫
    private float zCoordinate; // 卡牌和相機距離
    private bool isOnTable = false;
    private Vector3 tableCardPosition; // 手牌桌上位置
    private HandManager handManager;
    private int currentCardSpaceIndex = -1; // 卡牌放哪個洞(-1==未放置)
    private GameObject cardTextObject;
    private CardType cardType = CardType.Normal;
    private CardSuit cardSuit = CardSuit.Spades;
    public int cardValue = 0;
    public bool isFront = false; // 正面否(預設蓋牌)
    private float setupTimer = 0f; // 開局看牌計時
    private bool isPendingTime = true; // 開局看牌

    void Start()
    {
        mainCamera = Camera.main;

        // 系統函式：FindAnyObjectByType (在整個遊戲場景中搜尋掛載了 HandManager 腳本的物件)
        handManager = FindAnyObjectByType<HandManager>();
        
        //tableCardPosition = transform.position;初始卡牌位置安排
        currentCardSpaceIndex = handManager.RequestNearestEmptySpace(transform.position);
        transform.position = handManager.CardSpaces[currentCardSpaceIndex].position;

        cardTextObject = transform.Find("Text (TMP)").gameObject;

        Flop();
    }
    
    public void Flop()
    {
        if(isFront == true)
        {
            cardTextObject.SetActive(true);
        }
        else
        {
            cardTextObject.SetActive(false);
        }
    }

    void OnMouseDown() // 卡牌被滑鼠按下 // 公開函式或變數或檔名大大寫
    {
        if(isPendingTime == true)
        {
            if(currentCardSpaceIndex == 0 || currentCardSpaceIndex == 3)
            {
                isFront = !isFront;
                Flop();
            }
            return;
        }

        isDragging = true;

        Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position);
        zCoordinate = screenPos.z;
    }

    void Update()
    {
        if(isPendingTime == true)
        {
            setupTimer += Time.deltaTime;
            if(setupTimer >= 10f) // 10 秒
            {
                isPendingTime = false;
                isFront = false;
                Flop();
                Debug.Log("開局 10 秒結束");
            }
        }

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

        if (isOnTable == true) // 在桌上
        {
            if(currentCardSpaceIndex != -1)
            {
                handManager.isCardSpaceFull[currentCardSpaceIndex] = false;
            }
            currentCardSpaceIndex = -1;
            tableCardPosition = transform.position;

            isFront = true;
            Flop();
        }
        else
        {
            if(currentCardSpaceIndex != -1)
            {
                handManager.isCardSpaceFull[currentCardSpaceIndex] = false;
            }
            int allocatedSpace = handManager.RequestNearestEmptySpace(transform.position);

            if(allocatedSpace != -1)
            {
                currentCardSpaceIndex = allocatedSpace;
                transform.position = handManager.CardSpaces[allocatedSpace].position; // 換到最近的位置
            }
            else
            {
                //沒位置
                transform.position = tableCardPosition;
            }
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