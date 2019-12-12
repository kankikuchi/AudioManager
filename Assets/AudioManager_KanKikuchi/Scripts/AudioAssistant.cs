using System;
using KanKikuchi.AudioManager;
using UnityEngine;

/// <summary>
/// Audioを再生するための補助クラス
/// </summary>
public abstract class AudioAssistant : MonoBehaviour {

  //再生する対象
  [SerializeField]
  protected AudioClip _audioClip = null;
  public AudioClip AudioClip {
    get { return _audioClip; }
    set { _audioClip = value; }
  }

  //自動で再生するか
  [SerializeField]
  protected bool _isAutoPlay = false;
  public bool IsAutoPlay {
    get { return _isAutoPlay; }
    set { _isAutoPlay = value; }
  }

  //ボリューム倍率、再生開始の遅延時間、ピッチ、フェードインの時間(0ならフェードインしない)
  [SerializeField]
  protected float _volumeRate = 1, _delay = 0, _pitch = 1, _fadeInDuration = 0;
  public float VolumeRate {
    get { return _volumeRate; }
    set { _volumeRate = value; }
  }

  public float Delay {
    get { return _delay; }
    set { _delay = value; }
  }

  public float Pitch {
    get { return _pitch; }
    set { _pitch = value; }
  }

  public float FadeInDuration {
    get { return _fadeInDuration; }
    set { _fadeInDuration = value; }
  }

  //=================================================================================
  //初期化
  //=================================================================================

  protected virtual void Start() {
    if (_isAutoPlay) {
      Play();
    }
  }
  
  //=================================================================================
  //再生
  //=================================================================================

  //再生
  public abstract void Play();

}