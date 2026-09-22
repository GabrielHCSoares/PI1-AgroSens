package mono.com.google.android.material.slider;


public class RangeSlider_OnSliderTouchListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.google.android.material.slider.RangeSlider.OnSliderTouchListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onStartTrackingTouch:(Lcom/google/android/material/slider/RangeSlider;)V:GetOnStartTrackingTouch_Lcom_google_android_material_slider_RangeSlider_Handler:Google.Android.Material.Slider.RangeSlider+IOnSliderTouchListenerInvoker, Xamarin.Google.Android.Material\n" +
			"n_onStopTrackingTouch:(Lcom/google/android/material/slider/RangeSlider;)V:GetOnStopTrackingTouch_Lcom_google_android_material_slider_RangeSlider_Handler:Google.Android.Material.Slider.RangeSlider+IOnSliderTouchListenerInvoker, Xamarin.Google.Android.Material\n" +
			"";
		mono.android.Runtime.register ("Google.Android.Material.Slider.RangeSlider+IOnSliderTouchListenerImplementor, Xamarin.Google.Android.Material", RangeSlider_OnSliderTouchListenerImplementor.class, __md_methods);
	}

	public RangeSlider_OnSliderTouchListenerImplementor ()
	{
		super ();
		if (getClass () == RangeSlider_OnSliderTouchListenerImplementor.class) {
			mono.android.TypeManager.Activate ("Google.Android.Material.Slider.RangeSlider+IOnSliderTouchListenerImplementor, Xamarin.Google.Android.Material", "", this, new java.lang.Object[] {  });
		}
	}

	public void onStartTrackingTouch (com.google.android.material.slider.RangeSlider p0)
	{
		n_onStartTrackingTouch (p0);
	}

	private native void n_onStartTrackingTouch (com.google.android.material.slider.RangeSlider p0);

	public void onStopTrackingTouch (com.google.android.material.slider.RangeSlider p0)
	{
		n_onStopTrackingTouch (p0);
	}

	private native void n_onStopTrackingTouch (com.google.android.material.slider.RangeSlider p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
