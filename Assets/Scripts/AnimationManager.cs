using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Spineアニメーションの制御と連動した音声再生制御を行うクラス
/// </summary>
public class AnimationManager : MonoBehaviour
{

    // アニメーションデータを設定
    // A, B, C, Dの各部分に対応する動きのアニメーションとセリフと連動した口のアニメーションを分けて管理
    // A, Bは作品によって数が違うので、リストで動的に管理する
    [Header("A Animations")]
    [SpineAnimation] public List<string> AAnimations;
    [SpineAnimation] public List<string> AMouthAnimations;

    [Header("B Animations")]
    [SpineAnimation] public List<string> BAnimations;
    [SpineAnimation] public List<string> BMouthAnimations;

    [Header("C Animations")]
    [SpineAnimation] public string CAnimation;
    [SpineAnimation] public string CMouthAnimation;

    [Header("D Animations")]
    [SpineAnimation] public string DAnimation;
    [SpineAnimation] public string DMouthAnimation;

    [SpineAnimation] public string Mix;

    // Spineのアニメーション管理用オブジェクト
    SkeletonAnimation skeletonAnimation;
    public Spine.AnimationState animationState;
    public Spine.Skeleton skeleton;

    public StatusIndexSO AStatus; // A部分の状態管理
    public StatusIndexSO BStatus; // B部分の状態管理

    // サウンド管理用のオブジェクト
    [SerializeField] SoundManager soundManager;
    [SerializeField] SoundManager trackingSoundManager;
    [SerializeField] AudioSource trackingAudioSource;

    // 再生中のアニメーション部分 (A, B, C, D) とインデックスを追跡
    public string currentPart = "";  // 現在再生中のアニメーション部分（A, B, C, D）
    public int currentIndex = -1;    // 現在のアニメーションのインデックス

    void Start()
    {
        // 初期化：
        // サウンド関連の設定
        trackingAudioSource = trackingSoundManager.GetComponent<AudioSource>();

        // SkeletonAnimationの設定
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        animationState = skeletonAnimation.AnimationState;
        skeleton = skeletonAnimation.Skeleton;

        // 最初のアニメーションの再生
        PlayA(1);
        AStatus.SetValue(1);
    }


    /// <summary>
    /// A部分のアニメーションを指定したインデックスで再生
    /// </summary>
    public void PlayA(int index)
    {
        index--; //インデックスを0ベースに調整

        // インデックスが有効か確認
        if (index >= 0 && index < AAnimations.Count)
        {
            // Spineアニメーションの再生
            animationState.SetAnimation(0, AAnimations[index], true);

            // AMouthAnimationsが設定されているか確認し、存在する場合のみ再生
            if (AMouthAnimations != null && index < AMouthAnimations.Count)
            {
                animationState.SetAnimation(1, AMouthAnimations[index], false);
            }
            else
            {
                Debug.LogWarning($"AMouthAnimationsが設定されていない、またはインデックスが範囲外です: {index}");
            }

            // サウンド名を動的に生成し、対応するサウンドを再生
            string soundName = $"A{index + 1}";
            PlaySoundSeq(soundName);

            // 再生中の状態を更新
            currentPart = "A";
            currentIndex = index;

            // デバッグ用: 現在の再生アニメーションを表示
            Debug.Log($"再生中のアニメーション: {currentPart} の {currentIndex + 1} 番目");
        }
        else
        {
            Debug.LogWarning("A部分の指定されたアニメーションがリストに存在しません。");
        }
    }

    /// <summary>
    /// B部分のアニメーションを指定したインデックスで再生
    /// 処理はA部分と同様
    /// </summary>
    /// <param name="index"></param>
    public void PlayB(int index)
    {
        index--; // インデックスを0ベースに調整

        if (index >= 0 && index < BAnimations.Count)
        {
            // Spineアニメーションの再生
            animationState.SetAnimation(0, BAnimations[index], true);

            // BMouthAnimationsが設定されているか確認し、存在する場合のみ再生
            if (BMouthAnimations != null && index < BMouthAnimations.Count)
            {
                animationState.SetAnimation(1, BMouthAnimations[index], false);
            }
            else
            {
                Debug.LogWarning($"BMouthAnimationsが設定されていない、またはインデックスが範囲外です: {index}");
            }

            // サウンド名を動的に生成し、対応するサウンドを再生
            string soundName = $"B{index + 1}";
            PlaySoundSeq(soundName);

            // 再生中の状態を更新
            currentPart = "B";
            currentIndex = index;

            Debug.Log($"再生中のアニメーション: {currentPart} の {currentIndex + 1} 番目");
        }
        else
        {
            Debug.LogWarning("B部分の指定されたアニメーションがリストに存在しません。");
        }
    }

    /// <summary>
    /// C部分のアニメーションを再生（固定のアニメーション）
    /// </summary>
    public void PlayC()
    {
        // Cアニメーションを再生し、完了時の処理を登録
        TrackEntry C1Entry = animationState.SetAnimation(0, CAnimation, false);

        // CMouthAnimationが設定されているか確認し、存在する場合のみ再生
        if (!string.IsNullOrEmpty(CMouthAnimation))
        {
            animationState.SetAnimation(1, CMouthAnimation, false);
        }
        else
        {
            Debug.LogWarning($"CMouthAnimationが設定されていない");
        }

        C1Entry.Complete += OnC1PlayComplete;

        PlaySoundSeq("C");

        currentPart = "C";
        currentIndex = 0;

        Debug.Log($"再生中のアニメーション: {currentPart} の {currentIndex + 1} 番目");

    }

        // C1が完了したら再度Cを再生して、完了時の処理を登録
        private void OnC1PlayComplete(TrackEntry C1Entry)
        {
            TrackEntry C2Entry = animationState.SetAnimation(0, CAnimation, false);
            C2Entry.Complete += OnC2PlayComplete;
        }

        // C2が完了したらD部分を再生
        private void OnC2PlayComplete(TrackEntry C2Entry)
        {
            PlayD();
        }

    /// <summary>
    /// D部分のアニメーションを再生（固定のアニメーション）
    /// </summary>
    public void PlayD()
    {
        animationState.SetAnimation(0, DAnimation, true);

        // DMouthAnimationが設定されているか確認し、存在する場合のみ再生
        if (!string.IsNullOrEmpty(DMouthAnimation))
        {
            animationState.SetAnimation(1, DMouthAnimation, false);
        }
        else
        {
            Debug.LogWarning($"DMouthAnimationが設定されていない");
        }

        PlaySoundSeq("D");

        currentPart = "D";
        currentIndex = 0;

        Debug.Log($"再生中のアニメーション: {currentPart} の {currentIndex + 1} 番目");

    }

    /// <summary>
    /// Mixの再生が完了した後にDを再生するバージョン
    /// </summary>
    public void PlayD_afterMix()
    {
        animationState.SetAnimation(0, DAnimation, true);

        PlaySoundSeq("D_mix");

        currentPart = "D_mix";
        currentIndex = 0;

        Debug.Log($"再生中のアニメーション: {currentPart} の {currentIndex + 1} 番目");

    }

    /// <summary>
    /// 全体が順番再生するアニメーションを再生
    /// </summary>
    public void PlayMix()
    {
        currentPart = "Mix";
        TrackEntry MixEntry = animationState.SetAnimation(0, Mix, false);
        PlaySoundSeq("Mix");
        MixEntry.Complete += OnMixPlayComplete;
    }

        // Mixが完了したらD部分を再生
        private void OnMixPlayComplete(TrackEntry MixEntry)
        {
            PlayD_afterMix();
        }

    /// <summary>
    /// アニメーションに連動してサウンドを再生する
    /// </summary>
    private void PlaySoundSeq(string part)
    {
        StopAll();  // すべてのサウンドを停止

        // サウンド名を動的に生成
        string seSound = $"{part}se";
        string diaSound = $"{part}dia";
        string loopSound = $"{part}loop";


        // アニメーションごとに特定の処理を追加したい場合は、特別なケースを処理
        if (part == "C")
        {
            trackingSoundManager.Play("Call");
        }
        else if (part == "Mix")
        {
            soundManager.Play("Mix");
        }
        else if (part == "D_mix")
        {
            soundManager.Play("Dloop");
        }
        else
        {
            // サウンド再生処理
            soundManager.Play(seSound);
            trackingSoundManager.Play(diaSound);

            // 指定されたサウンド名に基づいてループ処理を開始
            StartCoroutine(CheckingAnimation(trackingAudioSource, part, currentIndex, () => {
                Debug.Log($"{part}{currentIndex + 1}loop");
                soundManager.Play(loopSound);
            }));
        }
    }

    /// <summary>
    /// アニメーションが再生中かどうかをチェックして、終了したらコールバックを実行
    /// </summary>
    private IEnumerator CheckingAnimation(AudioSource audio, string part, int index, UnityAction callback)
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();

            // 再生中のアニメーションが変更された場合は終了
            if (currentPart != part || currentIndex != index)
            {
                break;
            }

            // オーディオが再生されていない場合にコールバックを実行
            if (!audio.isPlaying)
            {
                callback();
                break;
            }
        }
    }

    /// <summary>
    /// すべてのサウンドを停止
    /// </summary>
    private void StopAll()
    {
        soundManager.StopAll();
        trackingSoundManager.StopAll();
    }

}
