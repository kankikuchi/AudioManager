using System;
using UnityEngine;
using KanKikuchi.AudioManager;
using UnityEngine.UI;

/// <summary>
/// 基本サンプルのボタン
/// </summary>
public class BasicSampleButton : MonoBehaviour {

  private InputField _bgmDelayInputField, _seDelayInputField;
  
  //=================================================================================
  //BGM
  //=================================================================================

  private void Awake() {
    _bgmDelayInputField = GameObject.Find("BGMDelayInputField").GetComponent<InputField>();
    _seDelayInputField  = GameObject.Find("SEDelayInputField").GetComponent<InputField>();
  }

  //=================================================================================
  //BGM
  //=================================================================================
  
  /// <summary>
  /// BGM1を再生
  /// </summary>
  public void PlayBGM1() {
    PlayBGM(BGMPath.FANTASY14);
  }
  
  /// <summary>
  /// BGM1を再生
  /// </summary>
  public void PlayBGM2() {
    PlayBGM(BGMPath.BATTLE27);
  }
  
  /// <summary>
  /// 重複するBGMを再生
  /// </summary>
  public void PlayDuplicateBGM() {
    PlayBGM(BGMPath.HEARTBEAT01, 0.5f, true);
  }

  //BGMを再生
  private void PlayBGM(string bgmPath, float volumeRate = 1, bool allowsDuplicate = false) {
    Debug.Log(bgmPath + "再生開始");
    
    float delay = 0;
    float.TryParse(_bgmDelayInputField.text, out delay);
    
    BGMManager.Instance.Play(bgmPath, volumeRate:volumeRate, delay:delay, allowsDuplicate:allowsDuplicate);
  }
  
  /// <summary>
  /// BGMを再生していたら停止する
  /// </summary>
  public void StopBGM() {
    Debug.Log("BGM停止");
    BGMManager.Instance.Stop();
  }
  
  /// <summary>
  /// BGMを再生していたら一時停止する
  /// </summary>
  public void PauseBGM() {
    Debug.Log("BGM一時停止");
    BGMManager.Instance.Pause();
  }
  
  /// <summary>
  /// BGMを一時停止していたら再開する
  /// </summary>
  public void UnPauseBGM() {
    Debug.Log("BGM再開");
    BGMManager.Instance.UnPause();
  }
  
  /// <summary>
  /// BGMを再生していたらフェードアウトする
  /// </summary>
  public void FadeOutBGM() {
    Debug.Log("BGMフェードアウト開始");
    BGMManager.Instance.FadeOut(callback: () => {
      Debug.Log("BGMフェードアウト終了");
    });
  }
  
  /// <summary>
  /// BGMを再生していたらフェードインする
  /// </summary>
  public void FadeInBGM() {
    Debug.Log("BGMフェードイン開始");
    BGMManager.Instance.FadeIn(callback: () => {
      Debug.Log("BGMフェードイン終了");
    });
  }
  
  //=================================================================================
  //SE
  //=================================================================================
  
  /// <summary>
  /// SE1を再生
  /// </summary>
  public void PlaySE1() {
    PlaySE(SEPath.SYSTEM20);
  }
  
  /// <summary>
  /// SE1を再生
  /// </summary>
  public void PlaySE2() {
    PlaySE(SEPath.JINGLE10);
  }
  
  //BGMを再生
  private void PlaySE(string sePath) {
    Debug.Log(sePath + "再生開始");
    
    float delay = 0;
    float.TryParse(_seDelayInputField.text, out delay);
    
    SEManager.Instance.Play(sePath, delay:delay, callback: () => {
      Debug.Log(sePath + "再生終了");
    });
  }
  
  /// <summary>
  /// SEを再生していたら停止する
  /// </summary>
  public void StopSE() {
    Debug.Log("SE停止");
    SEManager.Instance.Stop();
  }
  
  /// <summary>
  /// SEを再生していたら一時停止する
  /// </summary>
  public void PauseSE() {
    Debug.Log("SE一時停止");
    SEManager.Instance.Pause();
  }
  
  /// <summary>
  /// SEを一時停止していたら再開する
  /// </summary>
  public void UnPauseSE() {
    Debug.Log("SE再開");
    SEManager.Instance.UnPause();
  }
  
}