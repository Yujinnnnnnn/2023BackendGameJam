using System.Collections;
using System.Collections.Generic;
using Project9;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CustomManager : MonoBehaviour
{
    private int catSkinID = 1110;

    private int currentBodyID = 1000;  // 기본값 설정
    private int currentEyesID = 100;   // 기본값 설정
    private int currentTailID = 10;    // 기본값 설정

    public SpriteRenderer targetImage; // Inspector에서 UI Image를 연결할 변수
    public string resourceImagePath = "Custommizing/"; // Resources 폴더 내 이미지 파일 경로 (확장자 제외)


    // 기본값으로 캐릭터 생성
    private void Start()
    {
        UpdateCharacter();
    }

    // 몸통을 변경할 때 호출할 메서드
    public void ChangeBody(int bodyIndex)
    {
        currentBodyID = bodyIndex * 1000;

        UpdateCharacter();
    }

    // 눈을 변경할 때 호출할 메서드
    public void ChangeEyes(int eyesIndex)
    {
        currentEyesID = eyesIndex * 100;
        UpdateCharacter();
    }

    // 꼬리를 변경할 때 호출할 메서드
    public void ChangeTail(int tailIndex)
    {
        currentTailID = tailIndex * 10;
        UpdateCharacter();
    }

    // 캐릭터 업데이트
    private void UpdateCharacter()
    {

        catSkinID = currentBodyID + currentEyesID + currentTailID + 1;
        
        string img = catSkinID.ToString();

        

        targetImage.sprite = Resources.Load<Sprite>(resourceImagePath + img);
        //GameManager.instance.catSkinID = catSkinID;


        //catSkinImg = Resources.Load("Custommizing/" + img, typeof(Sprite)) as Sprite;

        if (targetImage != null)
        {
            Debug.Log(img);

            LobbyData.CatSkinID = img;
        }
        else
        {
            Debug.LogError("스프라이트를 찾을 수 없습니다 : " + img);
        }
    }
}