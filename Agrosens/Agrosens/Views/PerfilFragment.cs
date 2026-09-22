using Android.OS;
using Android.Views;
using Android.Widget;

namespace Agrosens.Views
{
  public class PerfilFragment : AndroidX.Fragment.App.Fragment
  {
    public override View OnCreateView(LayoutInflater inflater,
                                      ViewGroup container,
                                      Bundle savedInstanceState)
    {
      var view = inflater.Inflate(Resource.Layout.fragment_perfil,
                                  container, false);

      // Itens do menu (apenas visual por enquanto)
      view.FindViewById<LinearLayout>(Resource.Id.menuProfile).Click += (s, e) => { };
      view.FindViewById<LinearLayout>(Resource.Id.menuPrivacy).Click += (s, e) => { };
      view.FindViewById<LinearLayout>(Resource.Id.menuSettings).Click += (s, e) => { };
      view.FindViewById<LinearLayout>(Resource.Id.menuHelp).Click += (s, e) => { };
      view.FindViewById<LinearLayout>(Resource.Id.menuLogout).Click += (s, e) => { };

      return view;
    }
  }
}