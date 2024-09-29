using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// UI�̃{�^���E�����v�𐧌䂷��N���X
/// </summary>
public class ButtonManager : MonoBehaviour
{
    // A�̃{�^���Q
    [SerializeField] private Button foreplayA;
    [SerializeField] private Button actionsA;
    [SerializeField] private Button shootA;
    [SerializeField] private Button afterplayA;
    [SerializeField] private Button close;

    // B�̃{�^���Q
    [SerializeField] private Button foreplayB;
    [SerializeField] private Button actionsB;
    [SerializeField] private Button shootB;
    [SerializeField] private Button afterplayB;
    [SerializeField] private Button open;

    // ���̑��̃{�^��
    [SerializeField] private Button hide;
    [SerializeField] private Button auto;
    [SerializeField] private Button menu;

    // ���j���[�؂�ւ��p�̃{�^��
    [SerializeField] private GameObject viewA; 
    [SerializeField] private GameObject viewB;

    // A�X�e�[�^�X�����v (On/Off)
    [SerializeField] private GameObject[] AStatusLampOn;
    [SerializeField] private GameObject[] AStatusLampOff;

    // B�X�e�[�^�X�����v (On/Off)
    [SerializeField] private GameObject[] BStatusLampOn;
    [SerializeField] private GameObject[] BStatusLampOff;

    // A�X�e�[�^�X�����v (On/Off) ���j���[��
    [SerializeField] private GameObject[] AStatusLampOnc;
    [SerializeField] private GameObject[] AStatusLampOffc;

    // B�X�e�[�^�X�����v (On/Off) ���j���[��
    [SerializeField] private GameObject[] BStatusLampOnc;
    [SerializeField] private GameObject[] BStatusLampOffc;

    // UI�O���[�v hide�{�^���p
    [SerializeField] private CanvasGroup iconsGroup;

    // AnimationManager�ւ̎Q��
    private AnimationManager animationManager;

    // A, B�؂�ւ��̃X�e�[�^�X�Ǘ�
    public StatusIndexSO AStatus;
    public StatusIndexSO BStatus;

    // A�AB�����A�j���[�V�����̃C���f�b�N�X
    private int aIndex = 2;
    private int bIndex = 3;

    // ���j���[���J���Ă��邩�ǂ���
    [SerializeField] private bool isMenuOpen = true;

    // ����A�p�[�g�̃A�j���[�V�������Đ������ǂ����������t���O
    private bool isA = true;

    // ������
    void Awake()
    {
        // SpineBase�I�u�W�F�N�g����AnimationManager���擾
        GameObject obj = GameObject.Find("SpineBase");
        animationManager = obj.GetComponent<AnimationManager>();

        // A�{�^���Q�̃��X�i�[�ݒ�
        foreplayA.onClick.AddListener(OnClickA);
        actionsA.onClick.AddListener(OnClickB);
        shootA.onClick.AddListener(OnClickC);
        afterplayA.onClick.AddListener(OnClickD);

        // B�{�^���Q�̃��X�i�[�ݒ�
        foreplayB.onClick.AddListener(OnClickA);
        actionsB.onClick.AddListener(OnClickB);
        shootB.onClick.AddListener(OnClickC);
        afterplayB.onClick.AddListener(OnClickD);

        // ���̑��{�^���̃��X�i�[�ݒ�
        open.onClick.AddListener(OpenMenu);
        close.onClick.AddListener(CloseMenu);
        hide.onClick.AddListener(HideUI);
        auto.onClick.AddListener(Auto);
        menu.onClick.AddListener(ReturnMenu);

        // �X�e�[�^�X������
        AStatus.SetValue(0);
        BStatus.SetValue(0);

        // �A�j���[�V�������X�g����C���f�b�N�X�̏����l��ݒ�
        aIndex = animationManager.AAnimations.Count;
        bIndex = animationManager.BAnimations.Count;
    }

    void Update()
    {
        // �}�E�X�N���b�N�ŃA�C�R���\��
        if (Input.GetMouseButtonDown(0))
        {
            iconsGroup.alpha = 1;
        }

        // ���ݍĐ����̃A�j���[�V�����ɉ����ă����v�̏�Ԃ��X�V
        if (isMenuOpen)
        {
            UpdateLampsForOpenMenu(); // ���j���[���J���Ă��鎞�̃����v�X�V
        }
        else
        {
            UpdateLampsForClosedMenu(); // ���j���[�����Ă��鎞�̃����v�X�V
        }
    }

    /// <summary>
    /// ���j���[���J���Ă��鎞�̃����v�X�V����
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
    /// ���j���[�����Ă��鎞�̃����v�X�V����
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
    /// �ėp�I�Ƀ����v�̏�Ԃ����Z�b�g���郁�\�b�h
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
    /// �����v�̃��Z�b�g�����������ɉ����ČĂяo��
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
    /// A��B�̃X�e�[�^�X�����Z�b�g
    /// </summary>
    private void ResetStatus()
    {
        AStatus.SetValue(0);
        BStatus.SetValue(0);
    }

    /// <summary>
    /// �ėp�I�ȃA�j���[�V�����؂�ւ�����
    /// </summary>
    private void SwitchAnimation(string part, StatusIndexSO status, int maxIndex, UnityAction<int> playMethod)
    {
        EventSystem.current.SetSelectedGameObject(null);

        // ���݂̃C���f�b�N�X�Ɋ�Â��ăA�j���[�V�������Đ�
        for (int i = 0; i <= maxIndex; i++)
        {
            if (status.GetValue() == i)
            {
                status.ApplyChange(1);
                i++;
                playMethod.Invoke(i); // �w�肳�ꂽ�A�j���[�V�������Đ�
                //Debug.Log($"{part} > {part}:" + i);

                if (status.GetValue() >= maxIndex)
                {
                    status.SetValue(0); // �C���f�b�N�X���ő�ɒB�����烊�Z�b�g
                }
                break;
            }
        }
    }

    /// <summary>
    /// A�{�^���������ꂽ�Ƃ��̏���
    /// </summary>
    void OnClickA()
    {
        if (!isA)
        {
            ResetStatus(); // �ʂ̃A�j���[�V�������Đ�����Ă����烊�Z�b�g
        }
        SwitchAnimation("A", AStatus, aIndex, animationManager.PlayA);
        isA = true; // A���Đ����ł��邱�Ƃ��L�^
    }

    /// <summary>
    /// B�{�^���������ꂽ�Ƃ��̏���
    /// </summary>
    void OnClickB()
    {
        if (isA)
        {
            ResetStatus(); // �ʂ̃A�j���[�V�������Đ�����Ă����烊�Z�b�g
        }
        SwitchAnimation("B", BStatus, bIndex, animationManager.PlayB);
        isA = false; // B���Đ����ł��邱�Ƃ��L�^
    }

    /// <summary>
    /// C�{�^���������ꂽ�Ƃ��̏���
    /// </summary>
    void OnClickC()
    {
        ResetStatus();
        ResetLamp();
        animationManager.PlayC(); // C�̃A�j���[�V�������Đ�
    }

    /// <summary>
    /// D�{�^���������ꂽ�Ƃ��̏���
    /// </summary>
    void OnClickD()
    {
        ResetStatus();
        ResetLamp();
        animationManager.PlayD(); // D�̃A�j���[�V�������Đ�
    }

    /// <summary>
    /// ���j���[���J������
    /// </summary>
    void OpenMenu()
    {
        isMenuOpen = true;
        ResetLamp(); // �����v�����Z�b�g
        viewB.SetActive(false); // B�r���[���\��
        viewA.SetActive(true);  // A�r���[��\��
    }

    /// <summary>
    /// ���j���[����鏈��
    /// </summary>
    void CloseMenu()
    {
        isMenuOpen = false;
        ResetLamp(); // �����v�����Z�b�g
        viewA.SetActive(false); // A�r���[���\��
        viewB.SetActive(true);  // B�r���[��\��
    }

    /// <summary>
    /// UI���B������
    /// </summary>
    void HideUI()
    {
        iconsGroup.alpha = 0; // UI���\���ɂ���
    }

    /// <summary>
    /// �����Đ����[�h
    /// </summary>
    void Auto()
    {
        ResetLamp(); // �����v�����Z�b�g
        animationManager.PlayMix(); // Mix�A�j���[�V�������Đ�
    }

    /// <summary>
    /// ���j���[�V�[���ɖ߂�
    /// </summary>
    void ReturnMenu()
    {
        SceneManager.LoadScene("Menu"); // ���j���[�V�[�������[�h
    }
}
