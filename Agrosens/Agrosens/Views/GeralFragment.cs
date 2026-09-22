using Android.OS;
using Android.Views;

namespace Agrosens.Views
{
  public class GeralFragment : AndroidX.Fragment.App.Fragment
  {
    public override View OnCreateView(LayoutInflater inflater,
                                      ViewGroup container,
                                      Bundle savedInstanceState)
    {
      // Apenas infla o layout — nada funciona de fato por enquanto
      return inflater.Inflate(Resource.Layout.fragment_geral,
                              container, false);
    }
  }
}