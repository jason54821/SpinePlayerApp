using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 状態を保持するためのScriptableObjectクラス。
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/StatusIndexSO")]
public class StatusIndexSO : ScriptableObject
{

    public int Value;

    /// <summary>
    /// 状態の値を新しい値に設定する。
    /// </summary>
    public void SetValue(int value)
    {
        Value = value;
    }

    /// <summary>
    /// 他のStatusIndexSOから値を設定する。
    /// </summary>
    public void SetValue(StatusIndexSO value)
    {
        Value = value.Value;
    }

    /// <summary>
    /// 現在の値に指定された数値を加算する。
    /// </summary>
    public void ApplyChange(int amount)
    {
        Value += amount;
    }

    /// <summary>
    /// 他のStatusIndexSOの値を加算する。
    /// </summary>
    public void ApplyChange(StatusIndexSO amount)
    {
        Value += amount.Value;
    }

    /// <summary>
    /// 現在の状態の値を取得する。
    /// </summary>
    public int GetValue()
    {
        return Value;
    }
    
}
