namespace KanKikuchi.AudioManager {

using System;
using System.Security.Claims;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// オーディオを管理するマネージャクラスの設定ファイル
/// </summary>
public class AudioManagerSetting : ScriptableObject {
  
  //外部からアクセスするようの実体、初アクセス時にLoadする
  private static AudioManagerSetting _entity = null;
  public  static AudioManagerSetting  Entity{
    get{
      if(_entity == null){
        _entity = Resources.Load<AudioManagerSetting>("AudioManagerSetting");
      }
      return _entity;
    }
  }
  
  //オーディオファイルへのパスを定数で管理するクラスを自動更新するか
  [SerializeField]
  private bool _isAutoUpdateAudioPath = true;
  public  bool  IsAutoUpdateAudioPath => _isAutoUpdateAudioPath;
  
  //同時再生可能数
  [SerializeField]
  private int _bgmAudioPlayerNum = 3, _seAudioPlayerNum = 10;
  public  int  BGMAudioPlayerNum => _bgmAudioPlayerNum;
  public  int  SEAudioPlayerNum  => _seAudioPlayerNum;
  
  //基準ボリューム
  [SerializeField]
  private float _bgmBaseVolume = 1f, _seBaseVolume = 1f;
  public  float  BGMBaseVolume => _bgmBaseVolume;
  public  float  SEBaseVolume  => _seBaseVolume;
  
  //SEのボリューム倍率調整をするか
  [SerializeField]
  private bool _shouldAdjustSEVolumeRate = true;
  public  bool  ShouldAdjustSeVolumeRate => _shouldAdjustSEVolumeRate;
  
  //BGMManager、SEManagerを自動生成するか
  [SerializeField]
  private bool _isAutoGenerateBGMManager = true, _isAutoGenerateSEManager = true;
  public  bool  IsAutoGenerateBGMManager => _isAutoGenerateBGMManager;
  public  bool  IsAutoGenerateSEManager  => _isAutoGenerateSEManager;
  
  //BGMManager、SEManagerを破棄するか
  [SerializeField]
  private bool _isDestroyBGMManager = false, _isDestroySEManager = false;
  public  bool  IsDestroyBGMManager => _isDestroyBGMManager;
  public  bool  IsDestroySEManager  => _isDestroySEManager;
  
  //AudioClipのキャッシュ設定
  [SerializeField]
  private AudioCacheType _bgmCacheType = AudioCacheType.All, _seCacheType = AudioCacheType.All;
  public  AudioCacheType  BGMCacheType => _bgmCacheType;
  public  AudioCacheType  SECacheType => _seCacheType;
  
  [SerializeField]
  private bool _isReleaseBGMCache = false, _isReleaseSECache = false;
  public  bool  IsReleaseBGMCache => _isReleaseBGMCache;
  public  bool  IsReleaseSECache => _isReleaseSECache;
  
  //オーディオファイルの自動設定
  [SerializeField]
  private bool _isAutoUpdateBGMSetting = true, _isAutoUpdateSESetting = true;
  public  bool  IsAutoUpdateBGMSetting => _isAutoUpdateBGMSetting;
  public  bool  IsAutoUpdateSESetting  => _isAutoUpdateSESetting;
  
  [SerializeField]
  private bool _forceToMonoForBGM = true, _forceToMonoForSE = true;
  public  bool  ForceToMonoForBGM => _forceToMonoForBGM;
  public  bool  ForceToMonoForSE  => _forceToMonoForSE;
  
  [SerializeField]
  private bool _normalizeForBGM = true, _normalizeForSE = true;
  public  bool  NormalizeForBGM => _normalizeForBGM;
  public  bool  NormalizeForSE  => _normalizeForSE;
  
  [SerializeField]
  private bool _ambisonicForBGM = false, _ambisonicForSE = false;
  public  bool  AmbisonicForBGM => _ambisonicForBGM;
  public  bool  AmbisonicForSE  => _ambisonicForSE;
  
  [SerializeField]
  private bool _loadInBackgroundForBGM = false, _loadInBackgroundForSE = false;
  public  bool  LoadInBackgroundForBGM => _loadInBackgroundForBGM;
  public  bool  LoadInBackgroundForSE  => _loadInBackgroundForSE;
  
  [SerializeField]
  private AudioClipLoadType _loadTypeForBGM = AudioClipLoadType.Streaming, _loadTypeForSE = AudioClipLoadType.CompressedInMemory;
  public  AudioClipLoadType  LoadTypeForBGM => _loadTypeForBGM;
  public  AudioClipLoadType  LoadTypeForSE  => _loadTypeForSE;

  [SerializeField]
  private float _qualityForBGM = 0.3f, _qualityForSE = 0.3f;
  public  float  QualityForBGM => _qualityForBGM;
  public  float  QualityForSE  => _qualityForSE;
  
  [SerializeField]
  private AudioCompressionFormat _compressionFormatForBGM = AudioCompressionFormat.Vorbis, _compressionFormatForSE = AudioCompressionFormat.Vorbis;
  public  AudioCompressionFormat  CompressionFormatForBGM => _compressionFormatForBGM;
  public  AudioCompressionFormat  CompressionFormatForSE  => _compressionFormatForSE;
  
  #if UNITY_EDITOR
  [SerializeField]
  private AudioSampleRateSetting _sampleRateSettingForBGM = AudioSampleRateSetting.OptimizeSampleRate, _sampleRateSettingForSE = AudioSampleRateSetting.OptimizeSampleRate;
  public  AudioSampleRateSetting  SampleRateSettingForBGM => _sampleRateSettingForBGM;
  public  AudioSampleRateSetting  SampleRateSettingForSE  => _sampleRateSettingForSE;
  #endif

}
}