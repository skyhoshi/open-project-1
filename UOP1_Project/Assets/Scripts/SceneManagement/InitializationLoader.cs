﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.AddressableAssets;
using System;

/// <summary>
/// This class is responsible for starting the game by loading the persistent managers scene 
/// and raising the event to load the Main Menu
/// </summary>

public class InitializationLoader : MonoBehaviour
{
	[Header("Persistent managers Scene")]
	[SerializeField] private GameSceneSO _persistentManagersScene = default;

	[Header("Loading settings")]
	[SerializeField] private GameSceneSO[] _menuToLoad = default;
	[SerializeField] private bool _showLoadScreen = default;

	[Header("Broadcasting on")]
	[SerializeField] private AssetReference _menuLoadChannel = default;

	private void Start()
	{
		//Load the persistent managers scene
		_persistentManagersScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed += LoadEventChannel;
	}

	private void LoadEventChannel(AsyncOperationHandle<SceneInstance> obj)
	{
		_menuLoadChannel.LoadAssetAsync<LoadEventChannelSO>().Completed += LoadMainMenu;
	}

	private void LoadMainMenu(AsyncOperationHandle<LoadEventChannelSO> obj)
	{
		LoadEventChannelSO loadEventChannelSO = (LoadEventChannelSO)_menuLoadChannel.Asset;
		loadEventChannelSO.RaiseEvent(_menuToLoad, _showLoadScreen);

		SceneManager.UnloadSceneAsync(0); //Initialization is the only scene in BuildSettings, thus it has index 0
	}
}
