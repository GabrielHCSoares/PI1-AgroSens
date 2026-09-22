package mono.com.google.android.material.slider;


public class Slider_OnChangeListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.google.android.material.slider.Slider.OnChangeListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onValueChange:(Lcom/google/android/material/slider/Slider;FZ)V:GetOnValueChange_Lcom_google_android_material_slider_Slider_FZHandler:Google.Android.Material.Slider.Slider+IOnChangeListenerInvoker, Xamarin.Google.Android.Material\n" +
			"";
		mono.android.Runtime.register ("Google.Android.Material.Slider.Slider+IOnChangeListenerImplementor, Xamarin.Google.Android.Material", Slider_OnChangeListenerImplementor.class, __md_methods);
	}

	public Slider_OnChangeListenerImplementor ()
	{
		super ();
		if (getClass () == Slider_OnChangeListenerImplementor.class) {
			mono.android.TypeManager.Activate ("Google.Android.Material.Slider.Slider+IOnChangeListenerImplementor, Xamarin.Google.Android.Material", "", this, new java.lang.Object[] {  });
		}
	}

	public void onValueChange (com.google.android.material.slider.Slider p0, float p1, boolean p2)
	{
		n_onValueChange (p0, p1, p2);
	}

	private native void n_onValueChange (com.google.android.material.slider.Slider p0, float p1, boolean p2);

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
