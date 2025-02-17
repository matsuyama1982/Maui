﻿using System.ComponentModel;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Primitives;

namespace CommunityToolkit.Maui.Views;

public partial class CameraView : View, ICameraView
{
	static readonly WeakEventManager weakEventManager = new();

	public static readonly BindableProperty CameraFlashModeProperty =
		BindableProperty.Create(nameof(CameraFlashMode), typeof(CameraFlashMode), typeof(CameraView), CameraFlashMode.Off);

	public static readonly BindableProperty IsTorchOnProperty =
		BindableProperty.Create(nameof(CameraFlashMode), typeof(bool), typeof(CameraView), false);

	static readonly BindablePropertyKey isAvailablePropertyKey =
		BindableProperty.CreateReadOnly(nameof(IsAvailable), typeof(bool), typeof(CameraView), false);

	public static readonly BindableProperty IsAvailableProperty = isAvailablePropertyKey.BindableProperty;

	static readonly BindablePropertyKey isCameraBusyPropertyKey =
		BindableProperty.CreateReadOnly(nameof(IsCameraBusy), typeof(bool), typeof(CameraView), false);

	public static readonly BindableProperty IsCameraBusyProperty = isCameraBusyPropertyKey.BindableProperty;

	public CameraFlashMode CameraFlashMode
	{
		get => (CameraFlashMode)GetValue(CameraFlashModeProperty);
		set => SetValue(CameraFlashModeProperty, value);
	}

	public bool IsTorchOn
	{
		get => (bool)GetValue(IsTorchOnProperty);
		set => SetValue(IsTorchOnProperty, value);
	}

	public bool IsAvailable => (bool)GetValue(IsAvailableProperty);

	[EditorBrowsable(EditorBrowsableState.Never)]
	bool IAvailability.IsAvailable
	{
		get => IsAvailable;
		set => SetValue(isAvailablePropertyKey, value);
	}

	public bool IsCameraBusy => (bool)GetValue(IsCameraBusyProperty);

	[EditorBrowsable(EditorBrowsableState.Never)]
	bool IAvailability.IsBusy
	{
		get => IsCameraBusy;
		set => SetValue(isCameraBusyPropertyKey, value);
	}

	public event EventHandler<MediaCaptureFailedEventArgs> MediaCaptureFailed
	{
		add => weakEventManager.AddEventHandler(value);
		remove => weakEventManager.RemoveEventHandler(value);
	}

	public event EventHandler<MediaCapturedEventArgs> MediaCaptured
	{
		add => weakEventManager.AddEventHandler(value);
		remove => weakEventManager.RemoveEventHandler(value);
	}
	
	public void OnAvailable()
	{
	}
	
	public void OnMediaCaptured(Stream imageData)
	{
		weakEventManager.HandleEvent(this, new MediaCapturedEventArgs(imageData), nameof(MediaCaptured));
	}

	public void OnMediaCapturedFailed()
	{
		weakEventManager.HandleEvent(this, new MediaCaptureFailedEventArgs(), nameof(MediaCaptureFailed));
	}

	public void Shutter()
	{
		Handler?.Invoke(nameof(ICameraView.Shutter));
	}
}
