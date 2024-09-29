using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// サウンド管理クラス。複数の音源を管理し、指定された数だけ同時に再生可能。
/// 再生中の音源が上限に達した場合、最も古い音源を上書きして新しい音を再生する。
/// </summary>
public class SoundManager : MonoBehaviour
{
    // シーン間で破棄されないようにするフラグ
    [SerializeField]
    private bool dontDestroyOnLoad = false;

    // 同時に再生できるAudioSourceの数
    [SerializeField]
    private int audioSourceNum = 1;

    /// <summary>
    /// サウンドデータクラス。サウンドの名前、AudioClip、ループ設定を含む。
    /// </summary>
    [System.Serializable]
    public class SoundData
    {
        public string name;
        public AudioClip audioClip;
        public bool loopSw;
    }

    // 再生可能なサウンドデータリスト
    [SerializeField]
    private SoundData[] soundDatas;

    // 音源リストと再生開始時間を記録する配列
    private AudioSource[] audioSourceList;
    private float[] audioSourcePlayTimes;

    // サウンド名をキーにしてSoundDataを格納するDictionary
    private Dictionary<string, SoundData> soundDictionary = new Dictionary<string, SoundData>();

    private void Awake()
    {
        if (dontDestroyOnLoad)
        {
            // シーンをまたいでも破棄されないようにする
            DontDestroyOnLoad(gameObject);
        }

        InitializeAudioSources();      // AudioSourceの初期化
        InitializeSoundDictionary();   // サウンドデータのDictionaryを初期化
    }

    /// <summary>
    /// AudioSourceとその再生時間を初期化する。
    /// </summary>
    private void InitializeAudioSources()
    {
        audioSourceList = new AudioSource[audioSourceNum];
        audioSourcePlayTimes = new float[audioSourceNum];

        // 指定された数のAudioSourceをコンポーネントに追加し、再生時間を初期化
        for (int i = 0; i < audioSourceList.Length; ++i)
        {
            audioSourceList[i] = gameObject.AddComponent<AudioSource>();
            audioSourcePlayTimes[i] = -1f; // 再生していない状態を示す
        }
    }

    /// <summary>
    /// サウンドデータをDictionaryに格納。サウンド名をキーにしてアクセス可能にする。
    /// </summary>
    private void InitializeSoundDictionary()
    {
        foreach (var soundData in soundDatas)
        {
            if (!soundDictionary.ContainsKey(soundData.name))
            {
                soundDictionary.Add(soundData.name, soundData); // サウンドデータをDictionaryに追加
            }
            else
            {
                Debug.LogWarning($"サウンドデータ '{soundData.name}' は既に存在しています。");
            }
        }
    }

    /// <summary>
    /// 使用されていないAudioSource、または最も古い音源を探して返す。
    /// </summary>
    private AudioSource GetOldestOrUnusedAudioSource()
    {
        int oldestIndex = -1;
        float oldestTime = float.MaxValue;

        // 既存のAudioSourceを確認
        for (int i = 0; i < audioSourceList.Length; ++i)
        {
            if (!audioSourceList[i].isPlaying)
            {
                // 未使用のAudioSourceを見つけたら即座に返す
                return audioSourceList[i];
            }

            // 最も古い再生開始時間を持つAudioSourceを探す
            if (audioSourcePlayTimes[i] < oldestTime)
            {
                oldestTime = audioSourcePlayTimes[i];
                oldestIndex = i;
            }
        }

        // 全て使用中なら、最も古いAudioSourceを返す
        return audioSourceList[oldestIndex];
    }

    /// <summary>
    /// サウンドを再生する。ボリュームやピッチも指定可能。
    /// </summary>
    public void Play(string name, float volume = 1.0f, float pitch = 1.0f)
    {
        if (soundDictionary.TryGetValue(name, out var soundData))
        {
            var audioSource = GetOldestOrUnusedAudioSource();
            audioSource.clip = soundData.audioClip;
            audioSource.loop = soundData.loopSw;
            audioSource.volume = volume;
            audioSource.pitch = pitch;
            audioSource.Play();

            // 再生時間を記録
            int index = System.Array.IndexOf(audioSourceList, audioSource);
            audioSourcePlayTimes[index] = Time.time;
        }
        else
        {
            Debug.LogWarning($"サウンド名 '{name}' が見つかりません。");
        }
    }

    /// <summary>
    /// デフォルト設定でサウンドを再生（ボリュームやピッチを指定しない）。
    /// </summary>
    public void Play(string name)
    {
        if (soundDictionary.TryGetValue(name, out var soundData))
        {
            var audioSource = GetOldestOrUnusedAudioSource();
            audioSource.clip = soundData.audioClip;
            audioSource.loop = soundData.loopSw;
            audioSource.Play();

            // 再生時間を記録
            int index = System.Array.IndexOf(audioSourceList, audioSource);
            audioSourcePlayTimes[index] = Time.time;
        }
        else
        {
            Debug.LogWarning($"サウンド名 '{name}' が見つかりません。");
        }
    }

    /// <summary>
    /// サウンド名で特定のサウンドを停止する。
    /// </summary>
    public void Stop(string name)
    {
        foreach (var source in audioSourceList)
        {
            if (source.isPlaying && source.clip != null && source.clip.name == name)
            {
                source.Stop();
                return;
            }
        }

        Debug.LogWarning($"'{name}' という名前のサウンドは再生されていません。");
    }

    /// <summary>
    /// 全ての再生中のサウンドを停止する。
    /// </summary>
    public void StopAll()
    {
        foreach (var source in audioSourceList)
        {
            if (source.isPlaying)
            {
                source.Stop();
            }
        }
    }

}
