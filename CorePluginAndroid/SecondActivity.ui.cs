using Android.Widget;
using GalaSoft.MvvmLight.Views;

namespace CorePluginAndroid
{
    public partial class SecondActivity : ActivityBase
    {
        private TextView _navigationParameterText;

        public TextView NavigationParameterText
        {
            get
            {
                return _navigationParameterText
                       ?? (_navigationParameterText = FindViewById<TextView>(Resource.Id.NavigationParameterText));
            }
        }

        private Button _goBackButton;

        public Button GoBackButton
        {
            get
            {
                return _goBackButton
                       ?? (_goBackButton = FindViewById<Button>(Resource.Id.GoBackButton));
            }
        }
    }
}