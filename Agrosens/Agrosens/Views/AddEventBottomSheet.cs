using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using Google.Android.Material.BottomSheet;

namespace Agrosens.Views
{
  // BottomSheetDialogFragment: aparece por cima da tela atual
  // sem substituir o fragment do calendário, preservando o visual
  // do calendário visível atrás do sheet
  public class AddEventBottomSheet : BottomSheetDialogFragment
  {
    public override View OnCreateView(LayoutInflater inflater,
                                      ViewGroup container,
                                      Bundle savedInstanceState)
    {
      // Infla o layout bottom_sheet_add_event.xml
      var view = inflater.Inflate(Resource.Layout.bottom_sheet_add_event,
                                  container, false);

      var etEventName = view.FindViewById<EditText>(Resource.Id.etEventName);
      var etNotes = view.FindViewById<EditText>(Resource.Id.etNotes);
      var etDate = view.FindViewById<EditText>(Resource.Id.etDate);

      // Chips de categoria
      var chipReuniao = view.FindViewById<LinearLayout>(Resource.Id.chipReuniao);
      var chipManutencao = view.FindViewById<LinearLayout>(Resource.Id.chipManutencao);

      // Toggle de lembrete
      var switchReminder = view.FindViewById<Switch>(Resource.Id.switchReminder);

      // Botão "+ Add new"
      var btnAddCategory = view.FindViewById<TextView>(Resource.Id.btnAddCategory);

      // Botão "Criar Evento": fecha o sheet (sem salvar no banco)
      var btnCreateEvent = view.FindViewById<TextView>(Resource.Id.btnCreateEvent);
      if (btnCreateEvent != null)
      {
        btnCreateEvent.Click += (s, e) =>
        {
          string name = etEventName?.Text?.Trim() ?? "";
          string msg = string.IsNullOrEmpty(name)
              ? "Evento criado com sucesso! (Simulação)"
              : $"Evento '{name}' criado com sucesso! (Simulação)";
          Toast.MakeText(Context, msg, ToastLength.Short)?.Show();
          Dismiss();
        };
      }

      if (chipReuniao != null)
      {
        chipReuniao.Click += (s, e) =>
            Toast.MakeText(Context, "Categoria selecionada: Reunião", ToastLength.Short)?.Show();
      }

      if (chipManutencao != null)
      {
        chipManutencao.Click += (s, e) =>
            Toast.MakeText(Context, "Categoria selecionada: Manutenção", ToastLength.Short)?.Show();
      }

      if (btnAddCategory != null)
      {
        btnAddCategory.Click += (s, e) =>
            Toast.MakeText(Context, "Adicionar nova categoria", ToastLength.Short)?.Show();
      }

      return view;
    }

    // Permite que o sheet apareça em meia tela e expanda para tela cheia
    // ao arrastar para cima, exatamente como no Figma
    public override void OnViewCreated(View view, Bundle savedInstanceState)
    {
      base.OnViewCreated(view, savedInstanceState);

      var behavior = (this.Dialog as BottomSheetDialog)?.Behavior;

      if (behavior != null)
      {
        // começa em meia tela
        behavior.PeekHeight = 600;
        // pode expandir ao arrastar
        behavior.SkipCollapsed = false;
        behavior.Hideable = true;
      }
    }
  }
}