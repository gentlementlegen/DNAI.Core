using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Hardware;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using CorePluginMobile.Services;
using CorePluginMobile.Views;
using CustomRenderer.Droid;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CameraPage), typeof(CameraPageRenderer))]

namespace CustomRenderer.Droid
{
    public class CameraPageRenderer : PageRenderer, TextureView.ISurfaceTextureListener
    {
        private global::Android.Hardware.Camera camera;
        private global::Android.Widget.Button takePhotoButton;
        private global::Android.Widget.Button toggleFlashButton;
        private global::Android.Widget.Button switchCameraButton;
        private global::Android.Views.View view;

        private Activity activity;
        private CameraFacing cameraType;
        private TextureView textureView;
        private SurfaceTexture surfaceTexture;

        private bool flashOn;

        public CameraPageRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
            {
                return;
            }

            try
            {
                SetupUserInterface();
                SetupEventHandlers();
                AddView(view);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(@"			ERROR: ", ex.Message);
            }
        }

        private void SetupUserInterface()
        {
            activity = this.Context as Activity;
            view = activity.LayoutInflater.Inflate(CorePluginMobile.Droid.Resource.Layout.CameraLayout, this, false);
            cameraType = CameraFacing.Back;

            textureView = view.FindViewById<TextureView>(CorePluginMobile.Droid.Resource.Id.textureView);
            textureView.SurfaceTextureListener = this;
        }

        private void SetupEventHandlers()
        {
            takePhotoButton = view.FindViewById<global::Android.Widget.Button>(CorePluginMobile.Droid.Resource.Id.takePhotoButton);
            takePhotoButton.Click += TakePhotoButtonTapped;

            switchCameraButton = view.FindViewById<global::Android.Widget.Button>(CorePluginMobile.Droid.Resource.Id.switchCameraButton);
            switchCameraButton.Click += SwitchCameraButtonTapped;

            toggleFlashButton = view.FindViewById<global::Android.Widget.Button>(CorePluginMobile.Droid.Resource.Id.toggleFlashButton);
            toggleFlashButton.Click += ToggleFlashButtonTapped;
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);

            var msw = MeasureSpec.MakeMeasureSpec(r - l, MeasureSpecMode.Exactly);
            var msh = MeasureSpec.MakeMeasureSpec(b - t, MeasureSpecMode.Exactly);

            view.Measure(msw, msh);
            view.Layout(0, 0, r - l, b - t);
        }

        public void OnSurfaceTextureUpdated(SurfaceTexture surface)
        {
            //var bitmap = textureView.Bitmap
            //var pixel = bitmap.GetPixel(0, 0);
            //Android.Util.Log.Debug("Camera", pixel.ToString());
        }

        public void OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
        {
            if (ActivityCompat.CheckSelfPermission(Context, Manifest.Permission.Camera) != (int)Permission.Granted)
            {
                // Camera permission has not been granted
                RequestCameraPermission();
            }
            camera = global::Android.Hardware.Camera.Open((int)cameraType);
            textureView.LayoutParameters = new FrameLayout.LayoutParams(width, height);
            surfaceTexture = surface;

            camera.SetPreviewTexture(surface);
            PrepareAndStartCamera();
        }

        void RequestCameraPermission()
        {
            ActivityCompat.RequestPermissions(activity, new String[] { Manifest.Permission.Camera }, 0);
        }

        public bool OnSurfaceTextureDestroyed(SurfaceTexture surface)
        {
            camera.StopPreview();
            camera.Release();
            return true;
        }

        public void OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height)
        {
            PrepareAndStartCamera();
        }

        private void PrepareAndStartCamera()
        {
            camera.StopPreview();

            var display = activity.WindowManager.DefaultDisplay;
            if (display.Rotation == SurfaceOrientation.Rotation0)
            {
                camera.SetDisplayOrientation(90);
            }

            if (display.Rotation == SurfaceOrientation.Rotation270)
            {
                camera.SetDisplayOrientation(180);
            }

            camera.StartPreview();
        }

        private void ToggleFlashButtonTapped(object sender, EventArgs e)
        {
            flashOn = !flashOn;
            if (flashOn)
            {
                if (cameraType == CameraFacing.Back)
                {
                    toggleFlashButton.SetBackgroundResource(CorePluginMobile.Droid.Resource.Drawable.FlashButton);
                    cameraType = CameraFacing.Back;

                    camera.StopPreview();
                    camera.Release();
                    camera = global::Android.Hardware.Camera.Open((int)cameraType);
                    var parameters = camera.GetParameters();
                    parameters.FlashMode = global::Android.Hardware.Camera.Parameters.FlashModeTorch;
                    camera.SetParameters(parameters);
                    camera.SetPreviewTexture(surfaceTexture);
                    PrepareAndStartCamera();
                }
            }
            else
            {
                toggleFlashButton.SetBackgroundResource(CorePluginMobile.Droid.Resource.Drawable.NoFlashButton);
                camera.StopPreview();
                camera.Release();

                camera = global::Android.Hardware.Camera.Open((int)cameraType);
                var parameters = camera.GetParameters();
                parameters.FlashMode = global::Android.Hardware.Camera.Parameters.FlashModeOff;
                camera.SetParameters(parameters);
                camera.SetPreviewTexture(surfaceTexture);
                PrepareAndStartCamera();
            }
        }

        private void SwitchCameraButtonTapped(object sender, EventArgs e)
        {
            if (cameraType == CameraFacing.Front)
            {
                cameraType = CameraFacing.Back;

                camera.StopPreview();
                camera.Release();
                camera = global::Android.Hardware.Camera.Open((int)cameraType);
                camera.SetPreviewTexture(surfaceTexture);
                PrepareAndStartCamera();
            }
            else
            {
                cameraType = CameraFacing.Front;

                camera.StopPreview();
                camera.Release();
                camera = global::Android.Hardware.Camera.Open((int)cameraType);
                camera.SetPreviewTexture(surfaceTexture);
                PrepareAndStartCamera();
            }
        }

        private async void TakePhotoButtonTapped(object sender, EventArgs e)
        {
            camera.StopPreview();
            FetchPixels();

            var image = textureView.Bitmap;

            //try
            //{
            //    var absolutePath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim).AbsolutePath;
            //    var folderPath = absolutePath + "/Camera";
            //    var filePath = System.IO.Path.Combine(folderPath, string.Format("photo_{0}.jpg", Guid.NewGuid()));

            //    var fileStream = new FileStream(filePath, FileMode.Create);
            //    await image.CompressAsync(Bitmap.CompressFormat.Jpeg, 50, fileStream);
            //    fileStream.Close();
            //    image.Recycle();

            //    var intent = new Android.Content.Intent(Android.Content.Intent.ActionMediaScannerScanFile);
            //    var file = new Java.IO.File(filePath);
            //    var uri = Android.Net.Uri.FromFile(file);
            //    intent.SetData(uri);
            //    CorePluginMobile.Droid.MainActivity.Instance.SendBroadcast(intent);

            //    FetchPixels();
            //}
            //catch (Exception ex)
            //{
            //    System.Diagnostics.Debug.WriteLine(@"				", ex.Message);
            //}

            camera.StartPreview();
        }

        private void FetchPixels()
        {
            var bitmapScalled = Bitmap.CreateScaledBitmap(textureView.Bitmap, 28, 28, true);
            //MemoryStream stream = new MemoryStream();
            //bitmapScalled.Compress(Bitmap.CompressFormat.Jpeg, 0, stream);
            //byte[] bitmapData = stream.ToArray();
            var mat = MathNet.Numerics.LinearAlgebra.Matrix<double>.Build.Dense(28, 28);
            for (int y = 0; y < bitmapScalled.Height; ++y)
            {
                for (int x = 0; x < bitmapScalled.Width; ++x)
                {
                    var pixel = bitmapScalled.GetPixel(x, y);
                    byte[] values = BitConverter.GetBytes(pixel);
                    if (!BitConverter.IsLittleEndian) Array.Reverse(values);
                    var med = (values[0] + values[1] + values[2]) / 3.0;
                    mat[y, x] = med / 255.0;
                }
            }
            (DependencyService.Get<ICamera>() as CorePluginMobile.Droid.Camera)?.SetImage(mat);
        }
    }
}