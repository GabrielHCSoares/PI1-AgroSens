using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;

namespace Agrosens.Views
{
  public class CalendarFragment : AndroidX.Fragment.App.Fragment
  {
    public override View OnCreateView(LayoutInflater inflater,
                                      ViewGroup container,
                                      Bundle savedInstanceState)
    {
      var view = inflater.Inflate(Resource.Layout.activity_calendar,
                                  container, false);

      // Botão "+" abre o bottom sheet de adicionar evento
      var btnAddEvent = view.FindViewById<FrameLayout>(Resource.Id.btnAddEvent);
      btnAddEvent.Click += (s, e) =>
      {
        var sheet = new AddEventBottomSheet();
        sheet.Show(ChildFragmentManager, sheet.Tag);
      };

      return view;
    }
  }
}