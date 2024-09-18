[System.Serializable]
public class UserGameData
{
    /// <summary>
    /// Lobby Scene에서 보이는 플레이어 레
    /// /// </summary>
    public int level;
    /// <summary>
    /// Lobby Scene에서 보이는 플레이어 경험치 
    /// </summary>
    public float experience;
    /// <summary>
    /// 무료 재화 
    /// </summary>
    public int gold;
    /// <summary>
    /// 유료재화
    /// </summary>
    public int jewel;
    /// <summary>
    /// 게임 플레이에 소모되는 재화 
    /// </summary>
    public int heart;
    /// <summary>
    /// 일일최고 점수 
    /// </summary>
    public int dailyBestScore;

    /// <summary>
    /// 집사의 기본 체력
    /// </summary>
    public int jipsa_HP_Lv;

    /// <summary>
    /// 고양이의 체력 
    /// </summary>
    public int cat_HP_Lv;

    /// <summary>
    /// 고양이의 속도 
    /// </summary>
    public int cat_Speed_Lv;


    public void Reset()
    {
        level = 1;
        experience = 0;
        gold = 0;
        jewel = 0;
        heart = 30;
        dailyBestScore = 0;
        cat_Speed_Lv = 1;
        cat_HP_Lv = 1;
        jipsa_HP_Lv = 1;

    }
}
