using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using Google.Android.Material.BottomSheet;
using Agrosens.Models;
using Agrosens.Services;
using System;
using System.Threading.Tasks;

namespace Agrosens.Views
{
    /// <summary>
    /// Cadastro/edição de sensor em 2 passos: escolher o tipo e preencher
    /// apenas o nome + a porcentagem de alerta (porteira: minutos aberta).
    /// </summary>
    public class AddSensorBottomSheet : BottomSheetDialogFragment
    {
        public Action? OnSaved { get; set; }
        public Sensor? SensorEditando { get; set; }

        private View? _root;
        private string? _tipo;

        public override View OnCreateView(LayoutInflater inflater,
                                          ViewGroup container,
                                          Bundle savedInstanceState)
        {
            _root = inflater.Inflate(Resource.Layout.bottom_sheet_add_sensor,
                                     container, false);

            Dialog?.Window?.SetSoftInputMode(SoftInput.AdjustResize);

            var cardUmidade = _root.FindViewById<LinearLayout>(Resource.Id.cardTipoUmidade);
            var cardPorteira = _root.FindViewById<LinearLayout>(Resource.Id.cardTipoPorteira);
            var cardAgua = _root.FindViewById<LinearLayout>(Resource.Id.cardTipoAgua);
            var cardSilagem = _root.FindViewById<LinearLayout>(Resource.Id.cardTipoSilagem);

            void SelecionarTipo(string tipo, View cardSelecionado)
            {
                _tipo = tipo;
                var cards = new[]
                {
                    (card: cardUmidade, nameId: Resource.Id.txtTipoUmidade, checkId: Resource.Id.checkTipoUmidade),
                    (card: cardPorteira, nameId: Resource.Id.txtTipoPorteira, checkId: Resource.Id.checkTipoPorteira),
                    (card: cardAgua, nameId: Resource.Id.txtTipoAgua, checkId: Resource.Id.checkTipoAgua),
                    (card: cardSilagem, nameId: Resource.Id.txtTipoSilagem, checkId: Resource.Id.checkTipoSilagem)
                };

                foreach (var (card, nameId, checkId) in cards)
                {
                    bool selecionado = card == cardSelecionado;
                    card.SetBackgroundResource(selecionado
                        ? Resource.Drawable.bg_card_green_rounded
                        : Resource.Drawable.bg_card_light);

                    var grupo = (ViewGroup)card;
                    var txtNome = card.FindViewById<TextView>(nameId);
                    var txtDesc = grupo.GetChildAt(2) as TextView;
                    var check = card.FindViewById<TextView>(checkId);

                    if (selecionado)
                    {
                        txtNome.SetTextColor(Android.Graphics.Color.White);
                        txtDesc?.SetTextColor(Android.Graphics.Color.ParseColor("#E8F5E9"));
                        check.Visibility = ViewStates.Visible;
                    }
                    else
                    {
                        txtNome.SetTextColor(Android.Graphics.Color.ParseColor("#1B3A24"));
                        txtDesc?.SetTextColor(Android.Graphics.Color.ParseColor("#6B7A72"));
                        check.Visibility = ViewStates.Gone;
                    }
                }

                _root!.FindViewById<LinearLayout>(Resource.Id.stepEscolhaTipo).Visibility = ViewStates.Gone;
                _root.FindViewById<LinearLayout>(Resource.Id.stepFormulario).Visibility = ViewStates.Visible;

                MostrarSecaoEspecifica();
            }

            cardUmidade.Click += (s, e) => SelecionarTipo("Umidade", cardUmidade);
            cardPorteira.Click += (s, e) => SelecionarTipo("Porteira", cardPorteira);
            cardAgua.Click += (s, e) => SelecionarTipo("Agua", cardAgua);
            cardSilagem.Click += (s, e) => SelecionarTipo("Silagem", cardSilagem);

            // Voltar / fechar
            _root.FindViewById<ImageView>(Resource.Id.imgVoltarSensor).Click += (s, e) => Dismiss();
            _root.FindViewById<TextView>(Resource.Id.btnCancelar).Click += (s, e) => Dismiss();

            // Salvar
            _root.FindViewById<TextView>(Resource.Id.btnSalvarSensor).Click += async (s, e) =>
            {
                if (!Validar()) return;

                var etNome = _root!.FindViewById<EditText>(Resource.Id.etNomeSensor);

                var sensor = new Sensor
                {
                    Tipo = _tipo!,
                    Nome = etNome.Text!.Trim(),
                    Codigo = string.Empty,       // campos antigos, não são mais pedidos
                    Localizacao = string.Empty,
                    Notificacoes = true,
                    CreatedAt = DateTime.Now
                };

                if (SensorEditando != null)
                {
                    sensor.Id = SensorEditando.Id;
                    sensor.CreatedAt = SensorEditando.CreatedAt;
                    sensor.Codigo = SensorEditando.Codigo;
                    sensor.Localizacao = SensorEditando.Localizacao;
                    sensor.ValorAtual = SensorEditando.ValorAtual;
                    sensor.PorteiraAberta = SensorEditando.PorteiraAberta;
                }

                if (_tipo == "Umidade")
                {
                    sensor.LimiteAlerta = LerPercentual(Resource.Id.etLimiteUmidade, 30);
                }
                else if (_tipo == "Porteira")
                {
                    sensor.AlertaPorteiraAberta = true;
                    sensor.MinutosAberta = LerInteiro(_root.FindViewById<EditText>(Resource.Id.etMinutosPorteira)) ?? 10;
                }
                else if (_tipo == "Agua")
                {
                    sensor.LimiteAlerta = LerPercentual(Resource.Id.etLimiteAgua, 25);
                }
                else if (_tipo == "Silagem")
                {
                    sensor.LimiteAlerta = LerPercentual(Resource.Id.etLimiteSilagem, 30);
                }

                try
                {
                    await DatabaseService.Instance.SaveSensorAsync(sensor);
                    Toast.MakeText(Context, SensorEditando != null ? "Sensor atualizado" : "Sensor adicionado", ToastLength.Short)?.Show();
                    Dismiss();
                    OnSaved?.Invoke();
                }
                catch (Exception ex)
                {
                    Android.Util.Log.Error("Agrosens.Sensor", ex.ToString());
                    Toast.MakeText(Context, "Erro ao salvar sensor.", ToastLength.Short)?.Show();
                }
            };

            // Pré-enchimento ao editar um sensor existente
            if (SensorEditando != null)
            {
                var sensor = SensorEditando;

                _root!.FindViewById<EditText>(Resource.Id.etNomeSensor).Text = sensor.Nome;

                View cardTipo = sensor.Tipo switch
                {
                    "Umidade" => cardUmidade,
                    "Porteira" => cardPorteira,
                    "Agua" => cardAgua,
                    _ => cardSilagem
                };
                SelecionarTipo(sensor.Tipo, cardTipo);

                if (sensor.Tipo == "Umidade")
                    _root.FindViewById<EditText>(Resource.Id.etLimiteUmidade).Text = (sensor.LimiteAlerta ?? 30).ToString();
                else if (sensor.Tipo == "Porteira")
                    _root.FindViewById<EditText>(Resource.Id.etMinutosPorteira).Text = (sensor.MinutosAberta ?? 10).ToString();
                else if (sensor.Tipo == "Agua")
                    _root.FindViewById<EditText>(Resource.Id.etLimiteAgua).Text = (sensor.LimiteAlerta ?? 25).ToString();
                else if (sensor.Tipo == "Silagem")
                    _root.FindViewById<EditText>(Resource.Id.etLimiteSilagem).Text = (sensor.LimiteAlerta ?? 30).ToString();
            }

            return _root;
        }

        private bool Validar()
        {
            var edit = _root!.FindViewById<EditText>(Resource.Id.etNomeSensor);
            var erro = _root.FindViewById<TextView>(Resource.Id.txtErroNome);
            if (string.IsNullOrWhiteSpace(edit.Text))
            {
                erro.Visibility = ViewStates.Visible;
                edit.RequestFocus();
                return false;
            }
            erro.Visibility = ViewStates.Gone;
            return true;
        }

        private int? LerInteiro(EditText edit)
        {
            string texto = edit.Text?.Trim() ?? "";
            if (int.TryParse(texto, out var v)) return v;
            return null;
        }

        /// <summary>Lê a porcentagem de alerta (0 a 100); vazio/inválido usa o padrão.</summary>
        private int LerPercentual(int idEdit, int padrao)
        {
            var v = LerInteiro(_root!.FindViewById<EditText>(idEdit)) ?? padrao;
            return Math.Clamp(v, 0, 100);
        }

        private void MostrarSecaoEspecifica()
        {
            var secaoUmidade = _root!.FindViewById<LinearLayout>(Resource.Id.secaoUmidade);
            var secaoPorteira = _root.FindViewById<LinearLayout>(Resource.Id.secaoPorteira);
            var secaoAgua = _root.FindViewById<LinearLayout>(Resource.Id.secaoAgua);
            var secaoSilagem = _root.FindViewById<LinearLayout>(Resource.Id.secaoSilagem);

            secaoUmidade.Visibility = ViewStates.Gone;
            secaoPorteira.Visibility = ViewStates.Gone;
            secaoAgua.Visibility = ViewStates.Gone;
            secaoSilagem.Visibility = ViewStates.Gone;

            if (_tipo == "Umidade") secaoUmidade.Visibility = ViewStates.Visible;
            else if (_tipo == "Porteira") secaoPorteira.Visibility = ViewStates.Visible;
            else if (_tipo == "Agua") secaoAgua.Visibility = ViewStates.Visible;
            else if (_tipo == "Silagem") secaoSilagem.Visibility = ViewStates.Visible;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            var behavior = (this.Dialog as BottomSheetDialog)?.Behavior;
            if (behavior != null)
            {
                behavior.PeekHeight = 900;
                behavior.SkipCollapsed = false;
                behavior.Hideable = true;
            }
        }
    }
}
