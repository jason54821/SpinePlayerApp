using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ԃ�ێ����邽�߂�ScriptableObject�N���X�B
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/StatusIndexSO")]
public class StatusIndexSO : ScriptableObject
{

    public int Value;

    /// <summary>
    /// ��Ԃ̒l��V�����l�ɐݒ肷��B
    /// </summary>
    public void SetValue(int value)
    {
        Value = value;
    }

    /// <summary>
    /// ����StatusIndexSO����l��ݒ肷��B
    /// </summary>
    public void SetValue(StatusIndexSO value)
    {
        Value = value.Value;
    }

    /// <summary>
    /// ���݂̒l�Ɏw�肳�ꂽ���l�����Z����B
    /// </summary>
    public void ApplyChange(int amount)
    {
        Value += amount;
    }

    /// <summary>
    /// ����StatusIndexSO�̒l�����Z����B
    /// </summary>
    public void ApplyChange(StatusIndexSO amount)
    {
        Value += amount.Value;
    }

    /// <summary>
    /// ���݂̏�Ԃ̒l���擾����B
    /// </summary>
    public int GetValue()
    {
        return Value;
    }
    
}
