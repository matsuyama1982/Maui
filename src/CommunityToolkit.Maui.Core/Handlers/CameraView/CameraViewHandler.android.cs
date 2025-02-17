﻿using Android.Views;
using Microsoft.Maui.Handlers;
using AndroidX.Camera.View;
using CommunityToolkit.Maui.Core.Views.CameraView;

namespace CommunityToolkit.Maui.Core.Handlers;

public partial class CameraViewHandler : ViewHandler<ICameraView, PreviewView>, IDisposable
{
	public static Action<byte[]>? Picture { get; set; }
	
	CameraManager? cameraManager;

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			cameraManager?.Dispose();
		}
	}

	public static IPropertyMapper<ICameraView, CameraViewHandler> Propertymapper = new PropertyMapper<ICameraView, CameraViewHandler>(ViewMapper)
	{
		[nameof(ICameraView.CameraFlashMode)] = MapCameraFlashMode,
		[nameof(IAvailability.IsAvailable)] = MapIsAvailable
	};

	public static void MapIsAvailable(CameraViewHandler handler, ICameraView view)
	{
		var cameraAvailability = (IAvailability)handler.VirtualView;
		cameraAvailability.UpdateAvailability(handler.Context);
	}

	public static CommandMapper<ICameraView, CameraViewHandler> Commandmapper = new(ViewCommandMapper)
	{
		[nameof(ICameraView.Shutter)] = MapShutter,
	};

	public static void MapShutter(CameraViewHandler handler, ICameraView view, object? arg3)
	{
		handler.cameraManager?.TakePicture();
	}

	public static void MapCameraFlashMode(CameraViewHandler handler, ICameraView view)
	{
		handler.cameraManager?.UpdateFlashMode(view.CameraFlashMode);
	}

	protected override PreviewView CreatePlatformView()
	{
		ArgumentNullException.ThrowIfNull(MauiContext);
		cameraManager = new(MauiContext, CameraLocation.Rear, VirtualView)
		{
			Loaded = () => Init(VirtualView)
		};
		return cameraManager.CreatePlatformView();

		void Init(ICameraView view)
		{
			MapCameraFlashMode(this, view);
		}
	}

	private protected override async void OnConnectHandler(View platformView)
	{
		base.OnConnectHandler(platformView);
		await cameraManager!.CheckPermissions();
		cameraManager?.Connect();
	}

	public CameraViewHandler() : base(Propertymapper, Commandmapper)
	{
	}
}

