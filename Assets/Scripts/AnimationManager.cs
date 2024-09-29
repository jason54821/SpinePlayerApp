using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Spine�A�j���[�V�����̐���ƘA�����������Đ�������s���N���X
/// </summary>
public class AnimationManager : MonoBehaviour
{

    // �A�j���[�V�����f�[�^��ݒ�
    // A, B, C, D�̊e�����ɑΉ����铮���̃A�j���[�V�����ƃZ���t�ƘA���������̃A�j���[�V�����𕪂��ĊǗ�
    // A, B�͍�i�ɂ���Đ����Ⴄ�̂ŁA���X�g�œ��I�ɊǗ�����
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

    // Spine�̃A�j���[�V�����Ǘ��p�I�u�W�F�N�g
    SkeletonAnimation skeletonAnimation;
    public Spine.AnimationState animationState;
    public Spine.Skeleton skeleton;

    public StatusIndexSO AStatus; // A�����̏�ԊǗ�
    public StatusIndexSO BStatus; // B�����̏�ԊǗ�

    // �T�E���h�Ǘ��p�̃I�u�W�F�N�g
    [SerializeField] SoundManager soundManager;
    [SerializeField] SoundManager trackingSoundManager;
    [SerializeField] AudioSource trackingAudioSource;

    // �Đ����̃A�j���[�V�������� (A, B, C, D) �ƃC���f�b�N�X��ǐ�
    public string currentPart = "";  // ���ݍĐ����̃A�j���[�V���������iA, B, C, D�j
    public int currentIndex = -1;    // ���݂̃A�j���[�V�����̃C���f�b�N�X

    void Start()
    {
        // �������F
        // �T�E���h�֘A�̐ݒ�
        trackingAudioSource = trackingSoundManager.GetComponent<AudioSource>();

        // SkeletonAnimation�̐ݒ�
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        animationState = skeletonAnimation.AnimationState;
        skeleton = skeletonAnimation.Skeleton;

        // �ŏ��̃A�j���[�V�����̍Đ�
        PlayA(1);
        AStatus.SetValue(1);
    }


    /// <summary>
    /// A�����̃A�j���[�V�������w�肵���C���f�b�N�X�ōĐ�
    /// </summary>
    public void PlayA(int index)
    {
        index--; //�C���f�b�N�X��0�x�[�X�ɒ���

        // �C���f�b�N�X���L�����m�F
        if (index >= 0 && index < AAnimations.Count)
        {
            // Spine�A�j���[�V�����̍Đ�
            animationState.SetAnimation(0, AAnimations[index], true);

            // AMouthAnimations���ݒ肳��Ă��邩�m�F���A���݂���ꍇ�̂ݍĐ�
            if (AMouthAnimations != null && index < AMouthAnimations.Count)
            {
                animationState.SetAnimation(1, AMouthAnimations[index], false);
            }
            else
            {
                Debug.LogWarning($"AMouthAnimations���ݒ肳��Ă��Ȃ��A�܂��̓C���f�b�N�X���͈͊O�ł�: {index}");
            }

            // �T�E���h���𓮓I�ɐ������A�Ή�����T�E���h���Đ�
            string soundName = $"A{index + 1}";
            PlaySoundSeq(soundName);

            // �Đ����̏�Ԃ��X�V
            currentPart = "A";
            currentIndex = index;

            // �f�o�b�O�p: ���݂̍Đ��A�j���[�V������\��
            Debug.Log($"�Đ����̃A�j���[�V����: {currentPart} �� {currentIndex + 1} �Ԗ�");
        }
        else
        {
            Debug.LogWarning("A�����̎w�肳�ꂽ�A�j���[�V���������X�g�ɑ��݂��܂���B");
        }
    }

    /// <summary>
    /// B�����̃A�j���[�V�������w�肵���C���f�b�N�X�ōĐ�
    /// ������A�����Ɠ��l
    /// </summary>
    /// <param name="index"></param>
    public void PlayB(int index)
    {
        index--; // �C���f�b�N�X��0�x�[�X�ɒ���

        if (index >= 0 && index < BAnimations.Count)
        {
            // Spine�A�j���[�V�����̍Đ�
            animationState.SetAnimation(0, BAnimations[index], true);

            // BMouthAnimations���ݒ肳��Ă��邩�m�F���A���݂���ꍇ�̂ݍĐ�
            if (BMouthAnimations != null && index < BMouthAnimations.Count)
            {
                animationState.SetAnimation(1, BMouthAnimations[index], false);
            }
            else
            {
                Debug.LogWarning($"BMouthAnimations���ݒ肳��Ă��Ȃ��A�܂��̓C���f�b�N�X���͈͊O�ł�: {index}");
            }

            // �T�E���h���𓮓I�ɐ������A�Ή�����T�E���h���Đ�
            string soundName = $"B{index + 1}";
            PlaySoundSeq(soundName);

            // �Đ����̏�Ԃ��X�V
            currentPart = "B";
            currentIndex = index;

            Debug.Log($"�Đ����̃A�j���[�V����: {currentPart} �� {currentIndex + 1} �Ԗ�");
        }
        else
        {
            Debug.LogWarning("B�����̎w�肳�ꂽ�A�j���[�V���������X�g�ɑ��݂��܂���B");
        }
    }

    /// <summary>
    /// C�����̃A�j���[�V�������Đ��i�Œ�̃A�j���[�V�����j
    /// </summary>
    public void PlayC()
    {
        // C�A�j���[�V�������Đ����A�������̏�����o�^
        TrackEntry C1Entry = animationState.SetAnimation(0, CAnimation, false);

        // CMouthAnimation���ݒ肳��Ă��邩�m�F���A���݂���ꍇ�̂ݍĐ�
        if (!string.IsNullOrEmpty(CMouthAnimation))
        {
            animationState.SetAnimation(1, CMouthAnimation, false);
        }
        else
        {
            Debug.LogWarning($"CMouthAnimation���ݒ肳��Ă��Ȃ�");
        }

        C1Entry.Complete += OnC1PlayComplete;

        PlaySoundSeq("C");

        currentPart = "C";
        currentIndex = 0;

        Debug.Log($"�Đ����̃A�j���[�V����: {currentPart} �� {currentIndex + 1} �Ԗ�");

    }

        // C1������������ēxC���Đ����āA�������̏�����o�^
        private void OnC1PlayComplete(TrackEntry C1Entry)
        {
            TrackEntry C2Entry = animationState.SetAnimation(0, CAnimation, false);
            C2Entry.Complete += OnC2PlayComplete;
        }

        // C2������������D�������Đ�
        private void OnC2PlayComplete(TrackEntry C2Entry)
        {
            PlayD();
        }

    /// <summary>
    /// D�����̃A�j���[�V�������Đ��i�Œ�̃A�j���[�V�����j
    /// </summary>
    public void PlayD()
    {
        animationState.SetAnimation(0, DAnimation, true);

        // DMouthAnimation���ݒ肳��Ă��邩�m�F���A���݂���ꍇ�̂ݍĐ�
        if (!string.IsNullOrEmpty(DMouthAnimation))
        {
            animationState.SetAnimation(1, DMouthAnimation, false);
        }
        else
        {
            Debug.LogWarning($"DMouthAnimation���ݒ肳��Ă��Ȃ�");
        }

        PlaySoundSeq("D");

        currentPart = "D";
        currentIndex = 0;

        Debug.Log($"�Đ����̃A�j���[�V����: {currentPart} �� {currentIndex + 1} �Ԗ�");

    }

    /// <summary>
    /// Mix�̍Đ��������������D���Đ�����o�[�W����
    /// </summary>
    public void PlayD_afterMix()
    {
        animationState.SetAnimation(0, DAnimation, true);

        PlaySoundSeq("D_mix");

        currentPart = "D_mix";
        currentIndex = 0;

        Debug.Log($"�Đ����̃A�j���[�V����: {currentPart} �� {currentIndex + 1} �Ԗ�");

    }

    /// <summary>
    /// �S�̂����ԍĐ�����A�j���[�V�������Đ�
    /// </summary>
    public void PlayMix()
    {
        currentPart = "Mix";
        TrackEntry MixEntry = animationState.SetAnimation(0, Mix, false);
        PlaySoundSeq("Mix");
        MixEntry.Complete += OnMixPlayComplete;
    }

        // Mix������������D�������Đ�
        private void OnMixPlayComplete(TrackEntry MixEntry)
        {
            PlayD_afterMix();
        }

    /// <summary>
    /// �A�j���[�V�����ɘA�����ăT�E���h���Đ�����
    /// </summary>
    private void PlaySoundSeq(string part)
    {
        StopAll();  // ���ׂẴT�E���h���~

        // �T�E���h���𓮓I�ɐ���
        string seSound = $"{part}se";
        string diaSound = $"{part}dia";
        string loopSound = $"{part}loop";


        // �A�j���[�V�������Ƃɓ���̏�����ǉ��������ꍇ�́A���ʂȃP�[�X������
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
            // �T�E���h�Đ�����
            soundManager.Play(seSound);
            trackingSoundManager.Play(diaSound);

            // �w�肳�ꂽ�T�E���h���Ɋ�Â��ă��[�v�������J�n
            StartCoroutine(CheckingAnimation(trackingAudioSource, part, currentIndex, () => {
                Debug.Log($"{part}{currentIndex + 1}loop");
                soundManager.Play(loopSound);
            }));
        }
    }

    /// <summary>
    /// �A�j���[�V�������Đ������ǂ������`�F�b�N���āA�I��������R�[���o�b�N�����s
    /// </summary>
    private IEnumerator CheckingAnimation(AudioSource audio, string part, int index, UnityAction callback)
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();

            // �Đ����̃A�j���[�V�������ύX���ꂽ�ꍇ�͏I��
            if (currentPart != part || currentIndex != index)
            {
                break;
            }

            // �I�[�f�B�I���Đ�����Ă��Ȃ��ꍇ�ɃR�[���o�b�N�����s
            if (!audio.isPlaying)
            {
                callback();
                break;
            }
        }
    }

    /// <summary>
    /// ���ׂẴT�E���h���~
    /// </summary>
    private void StopAll()
    {
        soundManager.StopAll();
        trackingSoundManager.StopAll();
    }

}
