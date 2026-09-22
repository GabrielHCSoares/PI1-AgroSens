package mono.com.google.android.material.internal;


public class CheckableImageButton_OnFocusableChangedListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.google.android.material.internal.CheckableImageButton.OnFocusableChangedListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onFocusableChanged:(Landroid/view/View;Z)V:GetOnFocusableChanged_Landroid_view_View_ZHandler:Google.Android.Material.Internal.CheckableImageButton+IOnFocusableChangedListenerInvoker, Xamarin.Google.Android.Material\n" +
			"";
		mono.android.Runtime.register ("Google.Android.Material.Internal.CheckableImageButton+IOnFocusableChangedListenerImplementor, Xamarin.Google.Android.Material", CheckableImageButton_OnFocusableChangedListenerImplementor.class, __md_methods);
	}

	public CheckableImageButton_OnFocusableChangedListenerImplementor ()
	{
		super ();
		if (getClass () == CheckableImageButton_OnFocusableChangedListenerImplementor.class) {
			mono.android.TypeManager.Activate ("Google.Android.Material.Internal.CheckableImageButton+IOnFocusableChangedListenerImplementor, Xamarin.Google.Android.Material", "", this, new java.lang.Object[] {  });
		}
	}

	public void onFocusableChanged (android.view.View p0, boolean p1)
	{
		n_onFocusableChanged (p0, p1);
	}

	private native void n_onFocusableChanged (android.view.View p0, boolean p1);

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
