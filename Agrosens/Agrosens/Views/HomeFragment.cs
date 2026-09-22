using Android.OS;
using Android.Views;
using AndroidX.Fragment.App;

namespace Agrosens.Views
{
  public class HomeFragment : AndroidX.Fragment.App.Fragment
  {
    public override View OnCreateView(LayoutInflater inflater,
                                      ViewGroup container,
                                      Bundle savedInstanceState)
    {
      var view = inflater.Inflate(Resource.Layout.activity_home,
                                  container, false);

      return view;
    }
  }
}