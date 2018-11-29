//using Android.App;
//using Android.Content;
//using Android.Views;
//using CorePluginMobile.Droid;
//using CorePluginMobile.ViewModels;
//using Xamarin.Forms;
//using Xamarin.Forms.Platform.Android;

//[assembly: ExportRenderer(typeof(CustomPage),
//                          typeof(CustomRenderer))]

//namespace CorePluginMobile.Droid
//{
//    public class CustomRenderer : PageRenderer
//    {
//        private Android.Views.View _view;

//        public CustomRenderer(Context context) : base(context)
//        {
//        }

//        //protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Page> e)
//        //{
//        //    base.OnElementChanged(e);

//        //    var activity = this.Context as Activity;
//        //    _view = activity.LayoutInflater.Inflate(Resource.Layout.camera, this, false);

//        //    AddView(_view);
//        //}

//        //protected override void OnLayout(bool changed, int l, int t, int r, int b)
//        //{
//        //    base.OnLayout(changed, l, t, r, b);
//        //    var msw = MeasureSpec.MakeMeasureSpec(r - l, MeasureSpecMode.Exactly);
//        //    var msh = MeasureSpec.MakeMeasureSpec(b - t, MeasureSpecMode.Exactly);
//        //    _view.Measure(msw, msh);
//        //    _view.Layout(0, 0, r - l, b - t);
//        //}
//    }
//}