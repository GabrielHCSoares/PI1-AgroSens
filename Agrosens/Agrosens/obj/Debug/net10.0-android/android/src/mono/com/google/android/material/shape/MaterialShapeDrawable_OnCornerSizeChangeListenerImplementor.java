package mono.com.google.android.material.shape;


public class MaterialShapeDrawable_OnCornerSizeChangeListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.google.android.material.shape.MaterialShapeDrawable.OnCornerSizeChangeListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCornerSizeChange:(F)V:GetOnCornerSizeChange_FHandler:Google.Android.Material.Shape.MaterialShapeDrawable+IOnCornerSizeChangeListenerInvoker, Xamarin.Google.Android.Material\n" +
			"";
		mono.android.Runtime.register ("Google.Android.Material.Shape.MaterialShapeDrawable+IOnCornerSizeChangeListenerImplementor, Xamarin.Google.Android.Material", MaterialShapeDrawable_OnCornerSizeChangeListenerImplementor.class, __md_methods);
	}

	public MaterialShapeDrawable_OnCornerSizeChangeListenerImplementor ()
	{
		super ();
		if (getClass () == MaterialShapeDrawable_OnCornerSizeChangeListenerImplementor.class) {
			mono.android.TypeManager.Activate ("Google.Android.Material.Shape.MaterialShapeDrawable+IOnCornerSizeChangeListenerImplementor, Xamarin.Google.Android.Material", "", this, new java.lang.Object[] {  });
		}
	}

	public void onCornerSizeChange (float p0)
	{
		n_onCornerSizeChange (p0);
	}

	private native void n_onCornerSizeChange (float p0);

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
