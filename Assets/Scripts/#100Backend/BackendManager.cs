using UnityEngine;
using BackEnd;              // �ڳ� SDK
using System.Threading.Tasks;
using UnityEngine.UI;

public class BackendManager : MonoBehaviour
{


    public InputField InputField;
    private string coupon;

    public GameObject couponPenal;



    private void Awake()
    {
        // update() �޼ҵ��� Backend.AsyncPoll(); ȣ���� ���� ������Ʈ�� �ı����� �ʴ´�
        DontDestroyOnLoad(gameObject);

        // �ڳ� ���� �ʱ�ȭ
        //BackendSetup();
    }


    void Start()
    {
        var bro = Backend.Initialize(true); // 뒤끝 초기화

        // 뒤끝 초기화에 대한 응답값
        if (bro.IsSuccess())
        {
            Debug.Log("초기화 성공 : " + bro); // 성공일 경우 statusCode 204 Success
        }
        else
        {
            Debug.LogError("초기화 실패 : " + bro); // 실패일 경우 statusCode 400대 에러 발생
        }

        //Test();
        //Coupon();
    }

    // 동기 함수를 비동기에서 호출하게 해주는 함수(유니티 UI 접근 불가)
    async void Test()
    {
        await Task.Run(() => {
            BackendLogin.Instance.CustomLogin("user1", "1234");

            // 게임데이터를 불러와 로컬에 저장합니다.(캐싱)
            BackendGameData.Instance.GameDataGet();

            // 우편 리스트를 불러와 우편의 정보와 inDate값들을 로컬에 저장합니다.  
            BackendPost.Instance.PostListGet(PostType.Admin);

            // 저장된 우편의 위치를 읽어 우편을 수령합니다. 여기서 index는 우편의 순서. 0이면 제일 윗 우편, 1이면 그 다음 우편
            BackendPost.Instance.PostReceive(PostType.Admin, 0);

            Debug.Log("테스트를 종료합니다.");
        });
    }

    private void Update()
    {
        if (Backend.IsInitialized)
        {
            Backend.AsyncPoll();
        }
    }

    async void Coupon()
    {
        await Task.Run(() =>
        {
            
            coupon = "뒤끝게임잼";

            // [변경 필요] 쿠폰 코드를 뒤끝 콘솔 > 쿠폰 관리 > 테스트 쿠폰에서 생성된 쿠폰코드 값으로 변경해주세요.  
            BackendCoupon.Instance.CouponUse(coupon);  // 예시 : "쿠폰 코드" -> "f9ffeed2c882bc1418"

            BackendGameData.Instance.UserGameData.gold = +5000;

            Debug.Log("쿠폰 사용 완료 ");

            couponPenal.SetActive(false);

        });
    }
    public void useCoupon()
    {

        Coupon();
        Debug.Log("여기 타나 ??");
    }


    private void BackendSetup()
    {
        // �ڳ� �ʱ�ȭ
        var bro = Backend.Initialize(true);

        // �ڳ� �ʱ�ȭ�� ���� ���䰪
        if (bro.IsSuccess())
        {
            // �ʱ�ȭ ���� �� statusCode 204 Success
            Debug.Log($"�ʱ�ȭ ���� : {bro}");
        }
        else 
        {
            // �ʱ�ȭ ���� �� statusCode 400�� ���� �߻�
            Debug.LogError($"�ʱ�ȭ ���� : {bro}");
        }
    }
}
