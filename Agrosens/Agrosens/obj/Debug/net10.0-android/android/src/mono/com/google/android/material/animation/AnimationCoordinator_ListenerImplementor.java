package mono.com.google.android.material.animation;


public class AnimationCoordinator_ListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.google.android.material.animation.AnimationCoordinator.Listener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onAnimationsEnd:()V:GetOnAnimationsEndHandler:Google.Android.Material.Animation.AnimationCoordinator+IListenerInvoker, Xamarin.Google.Android.Material\n" +
			"n_onAnimationsStart:()V:GetOnAnimationsStartHandler:Google.Android.Material.Animation.AnimationCoordinator+IListenerInvoker, Xamarin.Google.Android.Material\n" +
			"";
		mono.android.Runtime.register ("Google.Android.Material.Animation.AnimationCoordinator+IListenerImplementor, Xamarin.Google.Android.Material", AnimationCoordinator_ListenerImplementor.class, __md_methods);
	}

	public AnimationCoordinator_ListenerImplementor ()
	{
		super ();
		if (getClass () == AnimationCoordinator_ListenerImplementor.class) {
			mono.android.TypeManager.Activate ("Google.Android.Material.Animation.AnimationCoordinator+IListenerImplementor, Xamarin.Google.Android.Material", "", this, new java.lang.Object[] {  });
		}
	}

	public void onAnimationsEnd ()
	{
		n_onAnimationsEnd ();
	}

	private native void n_onAnimationsEnd ();

	public void onAnimationsStart ()
	{
		n_onAnimationsStart ();
	}

	private native void n_onAnimationsStart ();

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
