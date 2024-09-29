using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// UIのボタン・ランプを制御するクラス
/// </summary>
public class ButtonManager : MonoBehaviour
{
    // Aのボタン群
    [SerializeField] private Button foreplayA;
    [SerializeField] private Button actionsA;
    [SerializeField] private Button shootA;
    [SerializeField] private Button afterplayA;
    [SerializeField] private Button close;

    // Bのボタン群
    [SerializeField] private Button foreplayB;
    [SerializeField] private Button actionsB;
    [SerializeField] private Button shootB;
    [SerializeField] private Button afterplayB;
    [SerializeField] private Button open;

    // その他のボタン
    [SerializeField] private Button hide;
    [SerializeField] private Button auto;
    [SerializeField] private Button menu;

    // メニュー切り替え用のボタン
    [SerializeField] private GameObject viewA; 
    [SerializeField] private GameObject viewB;

    // Aステータスランプ (On/Off)
    [SerializeField] private GameObject[] AStatusLampOn;
    [SerializeField] private GameObject[] AStatusLampOff;

    // Bステータスランプ (On/Off)
    [SerializeField] private GameObject[] BStatusLampOn;
    [SerializeField] private GameObject[] BStatusLampOff;

    // Aステータスランプ (On/Off) メニュー閉時
    [SerializeField] private GameObject[] AStatusLampOnc;
    [SerializeField] private GameObject[] AStatusLampOffc;

    // Bステータスランプ (On/Off) メニュー閉時
    [SerializeField] private GameObject[] BStatusLampOnc;
    [SerializeField] private GameObject[] BStatusLampOffc;

    // UIグループ hideボタン用
    [SerializeField] private CanvasGroup iconsGroup;

    // AnimationManagerへの参照
    private AnimationManager animationManager;

    // A, B切り替えのステータス管理
    public StatusIndexSO AStatus;
    public StatusIndexSO BStatus;

    // A、B部分アニメーションのインデックス
    private int aIndex = 2;
    private int bIndex = 3;

    // メニューが開いているかどうか
    [SerializeField] private bool isMenuOpen = true;

    // 現在Aパートのアニメーションが再生中かどうかを示すフラグ
    private bool isA = true;

    // 初期化
    void Awake()
    {
        // SpineBaseオブジェクトからAnimationManagerを取得
        GameObject obj = GameObject.Find("SpineBase");
        animationManager = obj.GetComponent<AnimationManager>();

        // Aボタン群のリスナー設定
        foreplayA.onClick.AddListener(OnClickA);
        actionsA.onClick.AddListener(OnClickB);
        shootA.onClick.AddListener(OnClickC);
        afterplayA.onClick.AddListener(OnClickD);

        // Bボタン群のリスナー設定
        foreplayB.onClick.AddListener(OnClickA);
        actionsB.onClick.AddListener(OnClickB);
        shootB.onClick.AddListener(OnClickC);
        afterplayB.onClick.AddListener(OnClickD);

        // その他ボタンのリスナー設定
        open.onClick.AddListener(OpenMenu);
        close.onClick.AddListener(CloseMenu);
        hide.onClick.AddListener(HideUI);
        auto.onClick.AddListener(Auto);
        menu.onClick.AddListener(ReturnMenu);

        // ステータス初期化
        AStatus.SetValue(0);
        BStatus.SetValue(0);

        // アニメーションリストからインデックスの初期値を設定
        aIndex = animationManager.AAnimations.Count;
        bIndex = animationManager.BAnimations.Count;
    }

    void Update()
    {
        // マウスクリックでアイコン表示
        if (Input.GetMouseButtonDown(0))
        {
            iconsGroup.alpha = 1;
        }

        // 現在再生中のアニメーションに応じてランプの状態を更新
        if (isMenuOpen)
        {
            UpdateLampsForOpenMenu(); // メニューが開いている時のランプ更新
        }
        else
        {
            UpdateLampsForClosedMenu(); // メニューが閉じている時のランプ更新
        }
    }

    /// <summary>
    /// メニューが開いている時のランプ更新処理
    /// </summary>
    private void UpdateLampsForOpenMenu()
    {
        switch (animationManager.currentPart)
        {
            case "A":
                ResetLamp();
                if (animationManager.currentIndex < AStatusLampOn.Length)
                {
                    AStatusLampOff[animationManager.currentIndex].SetActive(false);
                    AStatusLampOn[animationManager.currentIndex].SetActive(true);
                }
                break;

            case "B":
                ResetLamp();
                if (animationManager.currentIndex < BStatusLampOn.Length)
                {
                    BStatusLampOff[animationManager.currentIndex].SetActive(false);
                    BStatusLampOn[animationManager.currentIndex].SetActive(true);
                }
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// メニューが閉じている時のランプ更新処理
    /// </summary>
    private void UpdateLampsForClosedMenu()
    {
        switch (animationManager.currentPart)
        {
            case "A":
                ResetLamp();
                if (animationManager.currentIndex < AStatusLampOnc.Length)
                {
                    AStatusLampOffc[animationManager.currentIndex].SetActive(false);
                    AStatusLampOnc[animationManager.currentIndex].SetActive(true);
                }
                break;

            case "B":
                ResetLamp();
                if (animationManager.currentIndex < BStatusLampOnc.Length)
                {
                    BStatusLampOffc[animationManager.currentIndex].SetActive(false);
                    BStatusLampOnc[animationManager.currentIndex].SetActive(true);
                }
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// 汎用的にランプの状態をリセットするメソッド
    /// </summary>
    private void ResetLamp(GameObject[] lampOn, GameObject[] lampOff)
    {
        for (int i = 0; i < lampOn.Length; i++)
        {
            lampOn[i].SetActive(false);
            lampOff[i].SetActive(true);
        }
    }

    /// <summary>
    /// ランプのリセット処理を条件に応じて呼び出す
    /// </summary>
    private void ResetLamp()
    {
        if (isMenuOpen)
        {
            ResetLamp(AStatusLampOn, AStatusLampOff);
            ResetLamp(BStatusLampOn, BStatusLampOff);
        }
        else
        {
            ResetLamp(AStatusLampOnc, AStatusLampOffc);
            ResetLamp(BStatusLampOnc, BStatusLampOffc);
        }
    }

    /// <summary>
    /// AとBのステータスをリセット
    /// </summary>
    private void ResetStatus()
    {
        AStatus.SetValue(0);
        BStatus.SetValue(0);
    }

    /// <summary>
    /// 汎用的なアニメーション切り替え処理
    /// </summary>
    private void SwitchAnimation(string part, StatusIndexSO status, int maxIndex, UnityAction<int> playMethod)
    {
        EventSystem.current.SetSelectedGameObject(null);

        // 現在のインデックスに基づいてアニメーションを再生
        for (int i = 0; i <= maxIndex; i++)
        {
            if (status.GetValue() == i)
            {
                status.ApplyChange(1);
                i++;
                playMethod.Invoke(i); // 指定されたアニメーションを再生
                //Debug.Log($"{part} > {part}:" + i);

                if (status.GetValue() >= maxIndex)
                {
                    status.SetValue(0); // インデックスが最大に達したらリセット
                }
                break;
            }
        }
    }

    /// <summary>
    /// Aボタンが押されたときの処理
    /// </summary>
    void OnClickA()
    {
        if (!isA)
        {
            ResetStatus(); // 別のアニメーションが再生されていたらリセット
        }
        SwitchAnimation("A", AStatus, aIndex, animationManager.PlayA);
        isA = true; // Aが再生中であることを記録
    }

    /// <summary>
    /// Bボタンが押されたときの処理
    /// </summary>
    void OnClickB()
    {
        if (isA)
        {
            ResetStatus(); // 別のアニメーションが再生されていたらリセット
        }
        SwitchAnimation("B", BStatus, bIndex, animationManager.PlayB);
        isA = false; // Bが再生中であることを記録
    }

    /// <summary>
    /// Cボタンが押されたときの処理
    /// </summary>
    void OnClickC()
    {
        ResetStatus();
        ResetLamp();
        animationManager.PlayC(); // Cのアニメーションを再生
    }

    /// <summary>
    /// Dボタンが押されたときの処理
    /// </summary>
    void OnClickD()
    {
        ResetStatus();
        ResetLamp();
        animationManager.PlayD(); // Dのアニメーションを再生
    }

    /// <summary>
    /// メニューを開く処理
    /// </summary>
    void OpenMenu()
    {
        isMenuOpen = true;
        ResetLamp(); // ランプをリセット
        viewB.SetActive(false); // Bビューを非表示
        viewA.SetActive(true);  // Aビューを表示
    }

    /// <summary>
    /// メニューを閉じる処理
    /// </summary>
    void CloseMenu()
    {
        isMenuOpen = false;
        ResetLamp(); // ランプをリセット
        viewA.SetActive(false); // Aビューを非表示
        viewB.SetActive(true);  // Bビューを表示
    }

    /// <summary>
    /// UIを隠す処理
    /// </summary>
    void HideUI()
    {
        iconsGroup.alpha = 0; // UIを非表示にする
    }

    /// <summary>
    /// 自動再生モード
    /// </summary>
    void Auto()
    {
        ResetLamp(); // ランプをリセット
        animationManager.PlayMix(); // Mixアニメーションを再生
    }

    /// <summary>
    /// メニューシーンに戻る
    /// </summary>
    void ReturnMenu()
    {
        SceneManager.LoadScene("Menu"); // メニューシーンをロード
    }
}
