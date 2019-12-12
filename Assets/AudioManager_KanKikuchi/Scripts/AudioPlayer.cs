namespace KanKikuchi.AudioManager {

using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オーディオを再生するクラス
/// </summary>
public class AudioPlayer {

  //再生用のソース
  private readonly AudioSource _audioSource;

  //再生した時間
  public float PlayedTime => _audioSource.time;

  //再生中のオーディオの名前
  public string CurrentAudioName => _audioSource.clip == null ? "" : _audioSource.clip.name;

  //再生終了後の処理
  private Action _callback;

  //状態
  public enum State {
    Wait, Delay, Playing, Pause, Fading
  }

  private State _currentState = State.Wait;
  public State CurrentState => _currentState;

  //ボリュームの基準と倍率
  private float _baseVolume, _volumeRate;
  public float CurrentVolume => _baseVolume * _volumeRate;
  
  //再生までの待ち時間
  private float _initialDelay, _currentDelay;
  public float ElapsedDelay => _initialDelay - _currentDelay;
  
  //フェード関係
  private float _fadeProgress, _fadeDuration, _fadeFrom, _fadeTo;
  private Action _fadeCallback;

  //=================================================================================
  //初期化
  //=================================================================================

  public AudioPlayer(AudioSource audioSource) {
    _audioSource = audioSource;
    _audioSource.playOnAwake = false;
  }

  //=================================================================================
  //更新
  //=================================================================================

  public void Update() {
    //実行中の終了判定
    if (_currentState == State.Playing && !_audioSource.isPlaying && Mathf.Approximately(_audioSource.time, 0)) {
      Finish();
    }
    //再生前の待機
    else if (_currentState == State.Delay) {
      Delay();
    }
    //フェード
    else if (_currentState == State.Fading) {
      Fade();
    }
  }

  private void Delay() {
    _currentDelay -= Time.deltaTime;
    if (_currentDelay > 0) {
      return;
    }

    _audioSource.Play();
    
    if (_fadeDuration > 0) {
      _currentState = State.Fading;
      Update();
    }
    else {
      _currentState = State.Playing;
    }
  }

  private void Fade() {
    _fadeProgress += Time.deltaTime;
    float timeRate = Mathf.Min(_fadeProgress / _fadeDuration, 1);

    _audioSource.volume = GetVolume() * (_fadeFrom * (1 - timeRate) + _fadeTo * timeRate);

    if (timeRate < 1) {
      return;
    }

    if (_fadeTo <= 0) {
      Finish();
    }
    else {
      _currentState = State.Playing;
    }
    _fadeCallback?.Invoke();
  }

  //=================================================================================
  //設定、変更
  //=================================================================================

  /// <summary>
  /// ボリュームを変更する(再生中のボリュームも変更する)
  /// </summary>
  public void ChangeVolume(float baseVolume) {
    _baseVolume = baseVolume;
    _audioSource.volume = GetVolume();
  }

  /// <summary>
  /// volumeRate変更,(主にミュート切り替え時に使う)
  /// </summary>
  public void ChangeVolumeRate(float volumeRate) {
    _volumeRate = volumeRate;
    _audioSource.volume = GetVolume();
  }

  //ボリュームを取得
  private float GetVolume() {
    return _baseVolume * _volumeRate;
  }

  //=================================================================================
  //再生開始
  //=================================================================================

  /// <summary>
  /// 再生開始
  /// </summary>
  public void Play(AudioClip audioClip, float baseVolume, float volumeRate, float delay, float pitch, bool isLoop, Action callback = null) {
    //停止中でなければ停止させる
    if (_currentState != AudioPlayer.State.Wait) {
      Stop();
    }
    _audioSource.Stop();

    _volumeRate = volumeRate;
    ChangeVolume(baseVolume);
    
    _initialDelay = delay;
    _currentDelay = _initialDelay;
    
    _audioSource.pitch = pitch;
    _audioSource.loop  = isLoop;
    _callback = callback;
    
    _audioSource.clip = audioClip;
    
    _currentState = _currentDelay > 0 ? State.Delay : State.Playing;
    if (_currentState == State.Playing) {
      _audioSource.Play();
    }
    
    //ループ再生でなければ、再生終了のチェックをする
    if (_audioSource.loop) {
      return;
    }

    //ポーズされていたらすぐに止める
    if (_currentState == State.Pause) {
      Pause();
    }
  }

  //=================================================================================
  //再生終了
  //=================================================================================

  /// <summary>
  /// 指定された名前のものを再生していたら停止
  /// </summary>
  public void Stop(string audioName) {
    if (audioName == CurrentAudioName) {
      Stop();
    }
  }

  /// <summary>
  /// 再生を停止する
  /// </summary>
  public void Stop() {
    _callback = null;
    Finish();
  }

  //再生終了
  private void Finish() {
    _currentState = State.Wait;

    _audioSource.Stop();
    _audioSource.clip = null;

    _initialDelay = 0;
    _currentDelay = 0;
    _fadeDuration = 0;

    _callback?.Invoke();
  }

  //=================================================================================
  //一時停止、再開
  //=================================================================================

  /// <summary>
  /// 指定された名前のものを再生していたら一時停止
  /// </summary>
  public void Pause(string audioName) {
    if (audioName == CurrentAudioName) {
      Pause();
    }
  }

  /// <summary>
  /// 再生していたら一時停止
  /// </summary>
  public void Pause() {
    if (_currentState == State.Playing || _currentState == State.Fading) {
      _audioSource.Pause();
    }

    _currentState = State.Pause;
  }

  /// <summary>
  /// 指定された名前のものを一時停止していたら再開
  /// </summary>
  public void UnPause(string audioName) {
    if (audioName == CurrentAudioName) {
      UnPause();
    }
  }

  /// <summary>
  /// 一時停止していたら再開
  /// </summary>
  public void UnPause() {
    if (_currentState != State.Pause) {
      return;
    }

    if (_audioSource.clip == null) {
      _currentState = State.Wait;
    }
    else if (_currentDelay > 0) {
      _currentState = State.Delay;
    }
    else {
      _audioSource.UnPause();
      _currentState = _fadeDuration > 0 ? State.Fading : State.Playing;
    }
  }

  //=================================================================================
  //フェード
  //=================================================================================

  /// <summary>
  /// 指定された名前のものを再生していたらフェード
  /// </summary>
  public void Fade(string audioName, float duration, float from, float to, Action callback = null) {
    if (audioName == CurrentAudioName) {
      Fade(duration, from, to, callback);
    }
  }
  
  /// <summary>
  /// フェード
  /// </summary>
  public void Fade(float duration, float from, float to, Action callback = null) {
    if (_currentState != State.Playing && _currentState != State.Delay && _currentState != State.Fading) {
      return;
    }
    
    _fadeProgress = 0;
    _fadeDuration = duration;
    _fadeFrom     = from;
    _fadeTo       = to;
    _fadeCallback = callback;

    if (_currentState == State.Playing) {
      _currentState = State.Fading;
    }
    if (_currentState == State.Fading) {
      Update();
    }
  }

}

}