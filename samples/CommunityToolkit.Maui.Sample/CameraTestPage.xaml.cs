using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Core.Handlers;


namespace CommunityToolkit.Maui.Sample;

public partial class CameraTestPage : ContentPage
{
	public CameraTestPage()
	{
		InitializeComponent();
		camera.CameraFlashMode = CameraFlashMode.On;
#if ANDROID
		CommunityToolkit.Maui.Core.Handlers.CameraViewHandler.Picture = (imgArr) =>
		{
			img.Dispatcher.Dispatch(() =>
			{
				img.Source = ImageSource.FromStream(() => new MemoryStream(imgArr));
			});
		};

#endif


	}

	void Button_Clicked(object sender, EventArgs e)
	{
		camera.Shutter();
	}
	
	int flashModeIndex = 0;
	void Button_Clicked_1(object sender, EventArgs e)
	{
		camera.CameraFlashMode = GenerateValue(flashModeIndex);
		flashModeIndex++;
		CameraFlashMode GenerateValue(int value)
		{
			return value switch
			{
				0 => CameraFlashMode.Auto,
				1 => CameraFlashMode.On,
				2 => CameraFlashMode.Off,
				_ => GenerateValue(0)
			};
		}
	}
}