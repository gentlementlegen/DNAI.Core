using System;
using Android;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Util;
using Xamarin.Forms;
using View = Android.Views.View;
using  CorePackageAndroid;

namespace CorePluginMobile.Droid
{
    [Activity(Label = "DNAI", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, ActivityCompat.IOnRequestPermissionsResultCallback

    {
        static readonly int REQUEST_CAMERA = 0;
        public string TAG
        {
            get
            {
                return "MainActivity";
            }
        }

        public static MainActivity Instance { get; private set; }

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            Instance = this;
            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
            Predictor.InitPredictor();
        }

        //protected override void OnStart()
        //{
        //    LoadApplication(new App()); vi
        //}


        //void RequestCameraPermission()
        //{
        //    Log.Info(TAG, "CAMERA permission has NOT been granted. Requesting permission.");

        //    if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.Camera))
        //    {
        //        // Provide an additional rationale to the user if the permission was not granted
        //        // and the user would benefit from additional context for the use of the permission.
        //        // For example if the user has previously denied the permission.
        //        Log.Info(TAG, "Displaying camera permission rationale to provide additional context.");

        //        Snackbar.Make(Window.DecorView.FindViewById(Android.Resource.Id.Content), "Camera permission is needed to show the camera preview.",
        //            Snackbar.LengthIndefinite).SetAction("OK", new Action<View>(delegate (View obj) {
        //            ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.Camera }, REQUEST_CAMERA);
        //        })).Show();
        //    }
        //    else
        //    {
        //        // Camera permission has not been granted yet. Request it directly.
        //        ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.Camera }, REQUEST_CAMERA);
        //    }
        //}

        //public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        //{
        //    if (requestCode == REQUEST_CAMERA)
        //    {
        //        // Received permission result for camera permission.
        //        Log.Info(TAG, "Received response for Camera permission request.");

        //        // Check if the only required permission has been granted
        //        if (grantResults.Length == 1 && grantResults[0] == Permission.Granted)
        //        {
        //            // Camera permission has been granted, preview can be displayed
        //            Log.Info(TAG, "CAMERA permission has now been granted. Showing preview.");
        //            Snackbar.Make(Window.DecorView.FindViewById(Android.Resource.Id.Content), "Camera Permission has been granted. Preview can now be opened.", Snackbar.LengthShort).Show();
        //        }
        //        else
        //        {
        //            Log.Info(TAG, "CAMERA permission was NOT granted.");
        //            Snackbar.Make(Window.DecorView.FindViewById(Android.Resource.Id.Content), "Permissions were not granted.", Snackbar.LengthShort).Show();
        //        }
        //    }
        //    else
        //    {
        //        base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        //    }
        //}
    }
}