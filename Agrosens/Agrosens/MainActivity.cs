using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Text;
using Android.Graphics;
using Android.Graphics.Drawables;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;
using PopupMenu = AndroidX.AppCompat.Widget.PopupMenu;
using Google.Android.Material.BottomSheet;
using Android.Webkit;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Agrosens.Models;
using Agrosens.Services;
using Agrosens.Views;
using AlertDialog = AndroidX.AppCompat.App.AlertDialog;

namespace Agrosens
{
    public enum AppTab
    {
        Home,
        Calendario,
        Geral,
        Mapa,
        Perfil,
        AuthWelcome,
        LoginForm,
        SignUpForm,
        AboutUs,
        PrivacyPolicy,
        Settings,
        ChangePassword,
        EditProfile,
        DayDetails
    }

    [Activity(Name = "com.companyname.agrosens.MainActivity", Label = "Agrosens", MainLauncher = true, Icon = "@mipmap/appicon", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class MainActivity : AppCompatActivity
    {
        private FrameLayout? _contentContainer;
        private LinearLayout? _navCalendario;
        private LinearLayout? _navGeral;
        private LinearLayout? _navMapa;
        private LinearLayout? _navPerfil;
        private FrameLayout? _btnCentral;
        private FrameLayout? _centralCircleBg;
        private WebView? _mapWebView;
        private ImageView? _mapCenterPin;
        private TextView? _mapTipBanner;
        private TextView? _mapLocalizacaoSalvaText;

        private bool _mapSelecao = false;
        private AppCompatButton? _mapBtnSalvar;
        private AppCompatButton? _mapBtnCancelar;
        private LinearLayout? _mapHeaderActions;

        private double _farmLat = -15.7801;
        private double _farmLng = -47.9292;
        private int _farmZoom = 13;
        private bool _hasFarmLocation = false;

        // Google Maps Platform (Maps JavaScript API dentro do WebView).
        // "hybrid" = imagem de satélite + ruas/nomes; use "satellite" para só a imagem.
        // Dica: restrinja a chave no Google Cloud Console (referenciadores HTTP + só a Maps JavaScript API).
        private const string GoogleMapsApiKey = "AIzaSyCKY85Ra4s4jrJ4o59xLLV4xwyM11286_0";
        private const string GoogleMapType = "hybrid";
        private const string PrefGarage = "garage_polygon";

        // Página do mapa (Google Maps JavaScript API). Placeholders __X__ são trocados em C#.
        // Funções expostas ao app: agroResize, agroSetView, agroGetView e agroGarage* (área da garagem).
        private const string MapHtmlTemplate = """
            <html>
            <head>
            <meta charset='utf-8'/>
            <meta name='viewport' content='width=device-width,initial-scale=1,maximum-scale=1,user-scalable=no'/>
            <style>
              html,body,#map{height:100%;margin:0}
              .erro{font-family:sans-serif;color:#555;padding:24px;text-align:center}
            </style>
            </head>
            <body>
            <div id='map'></div>
            <script>
            var G = { pts: [], poly: null, marks: [], edit: false };

            function gRedraw() {
              if (G.poly) { G.poly.setMap(null); G.poly = null; }
              for (var i = 0; i < G.marks.length; i++) G.marks[i].setMap(null);
              G.marks = [];
              var map = window.mapObj;
              if (!map) return;

              if (G.pts.length >= 2) {
                G.poly = new google.maps.Polygon({
                  paths: G.pts.map(function (p) { return { lat: p[0], lng: p[1] }; }),
                  strokeColor: '#E65100', strokeWeight: 2,
                  fillColor: '#FF9800', fillOpacity: 0.35,
                  clickable: false, map: map
                });
              }
              if (G.edit) {
                G.pts.forEach(function (p, i) {
                  var mk = new google.maps.Marker({
                    position: { lat: p[0], lng: p[1] }, map: map, draggable: true,
                    icon: { path: google.maps.SymbolPath.CIRCLE, scale: 8, fillColor: '#FFFFFF',
                            fillOpacity: 1, strokeColor: '#E65100', strokeWeight: 3 }
                  });
                  mk.addListener('dragend', function (e) {
                    G.pts[i] = [e.latLng.lat(), e.latLng.lng()];
                    gRedraw();
                  });
                  G.marks.push(mk);
                });
              }
            }

            function initMap() {
              var map = new google.maps.Map(document.getElementById('map'), {
                center: { lat: __LAT__, lng: __LNG__ }, zoom: __ZOOM__, mapTypeId: '__TYPE__',
                gestureHandling: 'greedy', streetViewControl: false, fullscreenControl: false,
                mapTypeControl: true, mapTypeControlOptions: { mapTypeIds: ['hybrid', 'satellite', 'roadmap'] }
              });
              window.mapObj = map;

              window.agroResize = function () { google.maps.event.trigger(map, 'resize'); };
              window.agroSetView = function (la, ln, z) { map.setCenter({ lat: la, lng: ln }); map.setZoom(z); };
              window.agroGetView = function () {
                var c = map.getCenter();
                return JSON.stringify({ lat: c.lat(), lng: c.lng(), zoom: Math.round(map.getZoom()) });
              };

              // ----- Área da garagem (polígono marcado por toques) -----
              window.agroGarageLoad  = function (a) { G.pts = a || []; gRedraw(); };
              window.agroGarageEdit  = function (b) { G.edit = !!b; gRedraw(); };
              window.agroGarageUndo  = function () { G.pts.pop(); gRedraw(); };
              window.agroGarageClear = function () { G.pts = []; gRedraw(); };
              window.agroGarageGet   = function () { return G.pts; };
              window.agroGarageFit   = function () {
                if (G.pts.length < 2) return;
                var b = new google.maps.LatLngBounds();
                G.pts.forEach(function (p) { b.extend({ lat: p[0], lng: p[1] }); });
                map.fitBounds(b, 80);
              };
              map.addListener('click', function (e) {
                if (!G.edit) return;
                G.pts.push([e.latLng.lat(), e.latLng.lng()]);
                gRedraw();
              });

              window.agroGarageLoad(__GARAGE__);
            }
            window.gm_authFailure = function () {
              document.body.innerHTML = "<div class='erro'>Chave do Google Maps recusada.<br/>Verifique se a Maps JavaScript API está ativada e se o faturamento está configurado.</div>";
              console.error('gm_authFailure');
            };
            </script>
            <script async src='https://maps.googleapis.com/maps/api/js?key=__KEY__&callback=initMap&language=pt-BR&loading=async'></script>
            </body>
            </html>
            """;

        private const string PrefFarmLat = "farm_lat";
        private const string PrefFarmLng = "farm_lng";
        private const string PrefFarmZoom = "farm_zoom";

        private AppTab _currentTab = AppTab.Home;
        private bool _isLoggedOut = false;

        private TextView? _txtMonth;
        private TextView? _txtYear;
        private Android.Views.View? _geralView;
        private TextView? _homeTxtUserName;

        private LinearLayout? _calendarDayEventsContainer;

        private LinearLayout? _homeWeekBand;
        private ScrollView? _homeScroll;
        private HorizontalScrollView? _homeWeekScroll;
        private TextView? _homeWeatherToggle;
        private DateTime? _homeReturnDay; // dia a restaurar ao voltar da tela de detalhes
        private DateTime _detailDay = DateTime.Today;
        private DateTime _homeWeekStart = DateTime.Today;
        private Dictionary<DateTime, DayWeather> _homeWeather = new();
        private readonly Dictionary<DateTime, View> _homeDaySections = new();
        private LinearLayout? _homeEventsContainer;
        private TextView? _homeWeekCounter;
        private TextView? _txtEmptyEvents;
        private DateTime _homeSelectedDay = DateTime.Today;

        private static readonly string[] MonthNamesPt = new string[]
        {
            "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho",
            "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"
        };

        private static readonly string[] DayNamesPtShort = new string[]
        {
            "DOM", "SEG", "TER", "QUA", "QUI", "SEX", "SÁB"
        };

        private Task _dbInitTask = Task.CompletedTask;

        private DateTime _currentMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

        private int _selectedDay = 1;

        private DateTime _selectedDate = DateTime.Today;
        private TimeSpan _startTime = new TimeSpan(10, 0, 0);
        private TimeSpan _endTime = new TimeSpan(11, 0, 0);

        private string _selectedCategory = string.Empty;

        private readonly Dictionary<int, LinearLayout> _dayCells = new();

        private readonly Dictionary<LinearLayout, DateTime> _cellDates = new();

        private TextView? _txtStartTime;
        private TextView? _txtEndTime;

        private RelativeLayout? _rootLayout;
        private RelativeLayout? _bottomBarContainer;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);

                // Log de exceções não tratadas para diagnóstico de tela branca
                AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                {
                    try { Android.Util.Log.Error("AgroSens", "UnhandledException: " + e.ExceptionObject?.ToString()); } catch { }
                };
                TaskScheduler.UnobservedTaskException += (s, e) =>
                {
                    try { Android.Util.Log.Error("AgroSens", "UnobservedTaskException: " + e.Exception?.ToString()); } catch { }
                    e.SetObserved();
                };

                LogStep("OnCreate start");
                SetContentView(Resource.Layout.activity_main);
                LogStep("SetContentView ok");

                SetupStatusBar();

                _rootLayout = FindViewById<RelativeLayout>(Resource.Id.rootLayout);
                _contentContainer = FindViewById<FrameLayout>(Resource.Id.contentContainer);
                _bottomBarContainer = FindViewById<RelativeLayout>(Resource.Id.bottomBarContainer);
                _navCalendario = FindViewById<LinearLayout>(Resource.Id.navCalendario);
                _navGeral = FindViewById<LinearLayout>(Resource.Id.navGeral);
                _navMapa = FindViewById<LinearLayout>(Resource.Id.navMapa);
                _navPerfil = FindViewById<LinearLayout>(Resource.Id.navPerfil);
                _btnCentral = FindViewById<FrameLayout>(Resource.Id.btnCentral);
                _centralCircleBg = FindViewById<FrameLayout>(Resource.Id.centralCircleBg);

                // Barra de status: em Android 15+ o app desenha atrás dela (borda a borda) e a cor definida
                // em SetupStatusBar é ignorada. Então medimos a altura da barra e pintamos uma faixa preta
                // com essa altura no topo, empurrando o conteúdo para baixo.
                var statusSpacer = FindViewById<Android.Views.View>(Resource.Id.statusBarSpacer);
                if (_rootLayout != null && statusSpacer != null)
                {
                    _rootLayout.SetOnApplyWindowInsetsListener(new StatusBarInsetsListener(statusSpacer));
                    _rootLayout.RequestApplyInsets();
                }

                LogStep("views encontradas");

                if (_navCalendario != null)
                    _navCalendario.Click += (s, e) => SwitchTab(AppTab.Calendario);

                if (_navGeral != null)
                    _navGeral.Click += (s, e) => SwitchTab(AppTab.Geral);

                if (_navMapa != null)
                    _navMapa.Click += (s, e) => SwitchTab(AppTab.Mapa);

                if (_navPerfil != null)
                    _navPerfil.Click += (s, e) => SwitchTab(AppTab.Perfil);

                if (_btnCentral != null)
                    _btnCentral.Click += (s, e) => SwitchTab(AppTab.Home);

                _dbInitTask = DatabaseService.Instance.InitializeAsync();
                LogStep("banco inicializado (assíncrono)");

                var flags = GetSharedPreferences("agrosens_flags", FileCreationMode.Private);
                bool demoSeedado = flags.GetBoolean("demo_user_seeded", false);
                if (!demoSeedado)
                {
                    var edFlags = flags.Edit();
                    edFlags.PutBoolean("demo_user_seeded", true);
                    edFlags.Apply();
                }

                var prefsBoot = new PreferenceService(this);
                bool primeiraInstalacao = !demoSeedado;
                if (primeiraInstalacao)
                {
                    // Primeira instalação: já abre logado, com o perfil padrão do UserProfileStore.
                    prefsBoot.IsLoggedIn = true;
                }
                LogStep($"primeiraInstalacao={primeiraInstalacao} is_logged_in={prefsBoot.IsLoggedIn}");

                SwitchTab(prefsBoot.IsLoggedIn ? AppTab.Geral : AppTab.AuthWelcome);
                LogStep($"tab selecionada={_currentTab}");
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error("AgroSens", "OnCreate falhou: " + ex);
                MostrarErroNaTela(ex, "OnCreate");
            }
        }

        private void SetupStatusBar()
        {
            try
            {
                Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                Window.ClearFlags(WindowManagerFlags.TranslucentStatus);
                Window.ClearFlags(WindowManagerFlags.Fullscreen);
                Window.ClearFlags(WindowManagerFlags.LayoutNoLimits);
                Window.SetStatusBarColor(Android.Graphics.Color.Black);
                // windowLightStatusBar=false (padrão) => ícones claros (brancos) sobre a barra preta
            }
            catch { }
        }

        private void LogStep(string mensagem)
        {
            try { Android.Util.Log.Info("AgroSens.Boot", mensagem); } catch { }
        }

        private void MostrarErroNaTela(Exception ex, string contexto)
        {
            try
            {
                Android.Util.Log.Error("AgroSens", $"{contexto}: {ex}");
                if (_contentContainer == null) return;
                _contentContainer.RemoveAllViews();

                var tv = new TextView(this)
                {
                    Text = "Não foi possível carregar esta tela."
                };
                tv.SetTextSize(Android.Util.ComplexUnitType.Sp, 14);
                tv.SetTextColor(Android.Graphics.Color.ParseColor("#111111"));
                tv.SetBackgroundColor(Android.Graphics.Color.ParseColor("#FFFFFF"));
                tv.SetPadding(16, 16, 16, 16);
                tv.Gravity = GravityFlags.Center;
                var tlp = new FrameLayout.LayoutParams(
                    ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
                tv.LayoutParameters = tlp;

                _contentContainer.AddView(tv);
            }
            catch { }
        }

        private void SwitchTab(AppTab tab)
        {
            try
            {
                _currentTab = tab;

                if (_contentContainer == null || LayoutInflater == null)
                    return;

            _contentContainer.RemoveAllViews();

            ResetTabHighlights();

            if (_rootLayout != null)
            {
                if (tab == AppTab.AuthWelcome)
                    _rootLayout.SetBackgroundColor(Android.Graphics.Color.ParseColor("#00B050"));
                else
                    _rootLayout.SetBackgroundColor(Android.Graphics.Color.ParseColor("#FFFFFF"));
            }

            if (_bottomBarContainer != null)
            {
                bool hideBar = (tab == AppTab.AuthWelcome || tab == AppTab.LoginForm || tab == AppTab.SignUpForm);
                _bottomBarContainer.Visibility = hideBar ? ViewStates.Gone : ViewStates.Visible;
            }

            switch (tab)
            {
                case AppTab.Home:
                    _currentTab = tab;
                    _centralCircleBg?.SetBackgroundResource(Resource.Drawable.bg_circle_green);
                    View homeView = LayoutInflater.Inflate(Resource.Layout.fragment_home, _contentContainer, false)!;
                    _contentContainer.AddView(homeView);
                    SetupHomeEvents(homeView);
                    break;

                case AppTab.DayDetails:
                    _currentTab = tab;
                    _centralCircleBg?.SetBackgroundResource(Resource.Drawable.bg_circle_green);
                    View detailView = LayoutInflater.Inflate(Resource.Layout.fragment_day_details, _contentContainer, false)!;
                    _contentContainer.AddView(detailView);
                    SetupDayDetails(detailView);
                    break;

                case AppTab.Calendario:
                    _currentTab = tab;
                    _navCalendario?.SetBackgroundResource(Resource.Drawable.bg_tab_active_pill);
                    _centralCircleBg?.SetBackgroundResource(Resource.Drawable.bg_circle_black);
                    var _calToday = DateTime.Today;
                    _currentMonth = new DateTime(_calToday.Year, _calToday.Month, 1);
                    _selectedDate = _calToday;
                    _selectedDay = _calToday.Day;
                    View calendarView = LayoutInflater.Inflate(Resource.Layout.fragment_calendar, _contentContainer, false)!;
                    _contentContainer.AddView(calendarView);
                    SetupCalendarEvents(calendarView);
                    break;

                case AppTab.Geral:
                    _navGeral?.SetBackgroundResource(Resource.Drawable.bg_tab_active_pill);
                    _centralCircleBg?.SetBackgroundResource(Resource.Drawable.bg_circle_black);
                    View geralView = LayoutInflater.Inflate(Resource.Layout.fragment_geral, _contentContainer, false)!;
                    _contentContainer.AddView(geralView);
                    SetupGeralEvents(geralView);
                    break;

                case AppTab.Mapa:
                    try
                    {
                        _navMapa?.SetBackgroundResource(Resource.Drawable.bg_tab_active_pill);
                        _centralCircleBg?.SetBackgroundResource(Resource.Drawable.bg_circle_black);
                        View mapaView = LayoutInflater.Inflate(Resource.Layout.fragment_map, _contentContainer, false)!;
                        mapaView.LayoutParameters = new FrameLayout.LayoutParams(
                            ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
                        _contentContainer.AddView(mapaView);

                        var webView = mapaView.FindViewById<WebView>(Resource.Id.mapWebView);
                        var loading = mapaView.FindViewById<ProgressBar>(Resource.Id.mapLoading);
                        var errorView = mapaView.FindViewById<TextView>(Resource.Id.mapError);
                        var mapContainer = mapaView.FindViewById<FrameLayout>(Resource.Id.mapContainer);
                        _mapCenterPin = mapaView.FindViewById<ImageView>(Resource.Id.mapCenterPin);
                        _mapTipBanner = mapaView.FindViewById<TextView>(Resource.Id.mapTipBanner);
                        _mapLocalizacaoSalvaText = mapaView.FindViewById<TextView>(Resource.Id.mapLocalizacaoSalva);
                        _mapHeaderActions = mapaView.FindViewById<LinearLayout>(Resource.Id.mapHeaderActions);
                        SetupMapMenu(mapaView); // menu lateral "Mais opções" + garagem

                        if (webView != null)
                        {
                            _mapWebView = webView;
                            _mapWebView.Settings.JavaScriptEnabled = true;
                            _mapWebView.Settings.DomStorageEnabled = true;
                            _mapWebView.Settings.UserAgentString =
                                "Agrosens/1.0 (com.companyname.agrosens)";
                            Android.Webkit.WebView.SetWebContentsDebuggingEnabled(true);
                            webView.SetWebChromeClient(new MapWebChromeClient());

                            // Lê localização salva (numeros serializados como strings com cultura invariante)
                            var prefs = GetSharedPreferences("agrosens_map", FileCreationMode.Private);
                            _hasFarmLocation = prefs.Contains(PrefFarmLat);
                            if (_hasFarmLocation)
                            {
                                _farmLat = double.Parse(prefs.GetString(PrefFarmLat, "-15.7801")!, CultureInfo.InvariantCulture);
                                _farmLng = double.Parse(prefs.GetString(PrefFarmLng, "-47.9292")!, CultureInfo.InvariantCulture);
                                _farmZoom = int.Parse(prefs.GetString(PrefFarmZoom, "13")!, CultureInfo.InvariantCulture);
                                if (_mapLocalizacaoSalvaText != null)
                                    _mapLocalizacaoSalvaText.Text =
                                        $"Localização salva: {_farmLat.ToString("F4", CultureInfo.InvariantCulture)}, {_farmLng.ToString("F4", CultureInfo.InvariantCulture)}";
                            }
                            else
                            {
                                _farmLat = -15.7801;
                                _farmLng = -47.9292;
                                _farmZoom = 13;
                                if (_mapLocalizacaoSalvaText != null)
                                    _mapLocalizacaoSalvaText.Text = "Localização salva: não definida";
                            }

                            string latS = _farmLat.ToString(CultureInfo.InvariantCulture);
                            string lngS = _farmLng.ToString(CultureInfo.InvariantCulture);

                            // Área da garagem salva (lista de [lat,lng]); embutida no HTML para já aparecer no mapa
                            _garageJson = GetSharedPreferences("agrosens_map", FileCreationMode.Private)?.GetString(PrefGarage, "[]") ?? "[]";
                            if (string.IsNullOrWhiteSpace(_garageJson)) _garageJson = "[]";
                            AtualizarTextoGaragem();

                            string html = MapHtmlTemplate
                                .Replace("__KEY__", GoogleMapsApiKey)
                                .Replace("__TYPE__", GoogleMapType)
                                .Replace("__LAT__", latS)
                                .Replace("__LNG__", lngS)
                                .Replace("__ZOOM__", _farmZoom.ToString(CultureInfo.InvariantCulture))
                                .Replace("__GARAGE__", _garageJson);

                            void InvalidateMapSize()
                            {
                                try
                                {
                                    webView.EvaluateJavascript("if(window.agroResize) window.agroResize();", null);
                                }
                                catch { }
                            }

                            void ApplySavedView()
                            {
                                if (!_hasFarmLocation || _mapWebView == null) return;
                                try
                                {
                                    _mapWebView.EvaluateJavascript(
                                        $"if(window.agroSetView) window.agroSetView({latS},{lngS},{_farmZoom})", null);
                                }
                                catch { }
                            }

                            // Timeout de segurança: se o mapa não carregar em 8s, mostra erro com retry
                            System.Threading.Tasks.Task.Delay(8000).ContinueWith(_ =>
                            {
                                try
                                {
                                    RunOnUiThread(() =>
                                    {
                                        if (loading != null && loading.Visibility == ViewStates.Visible)
                                        {
                                            loading.Visibility = ViewStates.Gone;
                                            if (errorView != null)
                                            {
                                                errorView.Text = "Mapa não carregou. Verifique a internet.";
                                                errorView.Visibility = ViewStates.Visible;
                                            }
                                            var btnRetry = mapaView.FindViewById<TextView>(Resource.Id.mapBtnRetry);
                                            if (btnRetry != null) btnRetry.Visibility = ViewStates.Visible;
                                        }
                                    });
                                }
                                catch { }
                            });

                            // Chama quando o layout estabilizar e sempre que mudar de tamanho
                            if (mapContainer != null)
                            {
                                mapContainer.ViewTreeObserver.AddOnGlobalLayoutListener(new LayoutListener(InvalidateMapSize));
                            }

                            // Botoes do cabecalho (recriados sempre que a aba Mapa e exibida)
                            CriarBotoesCabecalhoMapa();

                            // Botão Tentar novamente (erro de carregamento)
                            var btnRetry = mapaView.FindViewById<TextView>(Resource.Id.mapBtnRetry);
                            if (btnRetry != null)
                            {
                                btnRetry.Visibility = ViewStates.Gone;
                                btnRetry.Click += (s, e) =>
                                {
                                    btnRetry.Visibility = ViewStates.Gone;
                                    if (errorView != null) errorView.Visibility = ViewStates.Gone;
                                    if (loading != null) loading.Visibility = ViewStates.Visible;
                                    webView.Reload();
                                };
                            }

                            // Chamada inicial após o carregamento do conteúdo
                            webView.SetWebViewClient(new MapWebViewClient(webView, loading, errorView, () =>
                            {
                                InvalidateMapSize();
                                ApplySavedView();
                            }));

                            webView.LoadDataWithBaseURL("https://localhost/", html, "text/html", "UTF-8", null);
                        }
                    }
                    catch (Exception ex)
                    {
                        Android.Util.Log.Error("Agrosens.Mapa", ex.ToString());
                        _contentContainer.RemoveAllViews();
                        var error = new TextView(this)
                        {
                            Text = "Mapa indisponível no momento.",
                            TextSize = 16,
                            Gravity = GravityFlags.Center
                        };
                        error.SetTextColor(Android.Graphics.Color.ParseColor("#8A968D"));
                        error.LayoutParameters = new FrameLayout.LayoutParams(
                            ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
                        _contentContainer.AddView(error);
                    }
                    break;

                case AppTab.Perfil:
                    _navPerfil?.SetBackgroundResource(Resource.Drawable.bg_tab_active_pill);
                    _centralCircleBg?.SetBackgroundResource(Resource.Drawable.bg_circle_black);
                    View perfilView = LayoutInflater.Inflate(Resource.Layout.fragment_perfil, _contentContainer, false)!;
                    _contentContainer.AddView(perfilView);
                    SetupPerfilEvents(perfilView);
                    break;

                case AppTab.AuthWelcome:
                    _centralCircleBg?.SetBackgroundResource(Resource.Drawable.bg_circle_black);
                    View authView = LayoutInflater.Inflate(Resource.Layout.fragment_auth_welcome, _contentContainer, false)!;
                    _contentContainer.AddView(authView);
                    SetupAuthWelcomeEvents(authView);
                    break;

                case AppTab.LoginForm:
                    _centralCircleBg?.SetBackgroundResource(Resource.Drawable.bg_circle_black);
                    View loginFormView = LayoutInflater.Inflate(Resource.Layout.fragment_login_form, _contentContainer, false)!;
                    _contentContainer.AddView(loginFormView);
                    SetupLoginFormEvents(loginFormView);
                    break;

                case AppTab.SignUpForm:
                    _centralCircleBg?.SetBackgroundResource(Resource.Drawable.bg_circle_black);
                    View signUpFormView = LayoutInflater.Inflate(Resource.Layout.fragment_signup_form, _contentContainer, false)!;
                    _contentContainer.AddView(signUpFormView);
                    SetupSignUpFormEvents(signUpFormView);
                    break;

                case AppTab.AboutUs:
                    _navPerfil?.SetBackgroundResource(Resource.Drawable.bg_tab_active_pill);
                    _centralCircleBg?.SetBackgroundResource(Resource.Drawable.bg_circle_black);
                    View aboutUsView = LayoutInflater.Inflate(Resource.Layout.fragment_about_us, _contentContainer, false)!;
                    _contentContainer.AddView(aboutUsView);
                    SetupAboutUsEvents(aboutUsView);
                    break;

                case AppTab.PrivacyPolicy:
                    _navPerfil?.SetBackgroundResource(Resource.Drawable.bg_tab_active_pill);
                    _centralCircleBg?.SetBackgroundResource(Resource.Drawable.bg_circle_black);
                    View privacyView = LayoutInflater.Inflate(Resource.Layout.fragment_privacy, _contentContainer, false)!;
                    _contentContainer.AddView(privacyView);
                    SetupPrivacyEvents(privacyView);
                    break;

                case AppTab.Settings:
                    _navPerfil?.SetBackgroundResource(Resource.Drawable.bg_tab_active_pill);
                    _centralCircleBg?.SetBackgroundResource(Resource.Drawable.bg_circle_black);
                    View settingsView = LayoutInflater.Inflate(Resource.Layout.fragment_settings, _contentContainer, false)!;
                    _contentContainer.AddView(settingsView);
                    SetupSettingsEvents(settingsView);
                    break;

                case AppTab.ChangePassword:
                    _navPerfil?.SetBackgroundResource(Resource.Drawable.bg_tab_active_pill);
                    _centralCircleBg?.SetBackgroundResource(Resource.Drawable.bg_circle_black);
                    View changePasswordView = LayoutInflater.Inflate(Resource.Layout.fragment_change_password, _contentContainer, false)!;
                    _contentContainer.AddView(changePasswordView);
                    SetupChangePasswordEvents(changePasswordView);
                    break;

                case AppTab.EditProfile:
                    _navPerfil?.SetBackgroundResource(Resource.Drawable.bg_tab_active_pill);
                    _centralCircleBg?.SetBackgroundResource(Resource.Drawable.bg_circle_black);
                    View editProfileView = LayoutInflater.Inflate(Resource.Layout.fragment_edit_profile, _contentContainer, false)!;
                    _contentContainer.AddView(editProfileView);
                    SetupEditProfileEvents(editProfileView);
                    break;
                }
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error("AgroSens", $"SwitchTab({tab}) falhou: {ex}");
                MostrarErroNaTela(ex, $"SwitchTab({tab})");
            }
        }

        private void ResetTabHighlights()
        {
            _navCalendario?.SetBackgroundResource(0);
            _navGeral?.SetBackgroundResource(0);
            _navMapa?.SetBackgroundResource(0);
            _navPerfil?.SetBackgroundResource(0);
        }

        private async void SetupHomeEvents(View view)
        {
            try
            {
                var imgUser = view.FindViewById<ImageView>(Resource.Id.imgUser);
                var txtUserName = view.FindViewById<TextView>(Resource.Id.txtUserName);
                _homeTxtUserName = txtUserName;

                _homeScroll = view.FindViewById<ScrollView>(Resource.Id.homeScroll);
                _homeWeekScroll = view.FindViewById<HorizontalScrollView>(Resource.Id.homeWeekScroll);
                _homeWeekBand = view.FindViewById<LinearLayout>(Resource.Id.homeWeekBand);
                _homeEventsContainer = view.FindViewById<LinearLayout>(Resource.Id.homeEventsContainer);
                _homeWeekCounter = view.FindViewById<TextView>(Resource.Id.homeWeekCounter);
                _txtEmptyEvents = view.FindViewById<TextView>(Resource.Id.txtEmptyEvents);
                _homeWeatherToggle = view.FindViewById<TextView>(Resource.Id.homeWeatherToggle);

                // Abre na semana atual (segunda a domingo) com hoje selecionado,
                // ou no dia que estava aberto ao voltar da tela de detalhes
                bool voltando = _homeReturnDay != null;
                _homeSelectedDay = _homeReturnDay ?? DateTime.Today;
                _homeReturnDay = null;
                _homeWeekStart = StartOfWeek(_homeSelectedDay);

                if (_isLoggedOut)
                {
                    imgUser?.SetImageResource(Resource.Drawable.sem_usuario);
                    if (txtUserName != null) txtUserName.Text = "Usuario";
                }
                else
                {
                    imgUser?.SetImageResource(Resource.Drawable.user_photo);
                    if (txtUserName != null) txtUserName.Text = NomeUsuarioLogado();
                }

                if (_homeWeatherToggle != null)
                {
                    _homeWeatherToggle.Click += (s, e) =>
                    {
                        _detailDay = _homeSelectedDay;
                        SwitchTab(AppTab.DayDetails);
                    };
                }

                BuildHomeWeekDays();
                _ = LoadHomeWeatherAsync();
                await LoadHomeEventsAsync();
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error("Agrosens.Home", ex.ToString());
                MostrarErroNaTela(ex, "SetupHomeEvents");
            }
        }

        private string NomeUsuarioLogado()
        {
            try
            {
                var prefs = new PreferenceService(this);
                var nome = prefs.UserName;
                if (!string.IsNullOrWhiteSpace(nome)) return nome;

                var store = new UserProfileStore(this);
                nome = store.Name;
                if (!string.IsNullOrWhiteSpace(nome)) return nome;
            }
            catch { }
            return "Usuário";
        }

        /// <summary>Segunda-feira da semana de <paramref name="d"/>.</summary>
        private static DateTime StartOfWeek(DateTime d)
        {
            int diff = ((int)d.DayOfWeek + 6) % 7;
            return d.Date.AddDays(-diff);
        }

        private static readonly string[] WeekAbbrPt = { "SEG", "TER", "QUA", "QUI", "SEX", "SÁB", "DOM" };

        // ---------- Clima (Open-Meteo) ----------

        private bool _weatherOffline = false;

        /// <summary>
        /// 1) mostra na hora o clima salvo no banco local (funciona offline);
        /// 2) busca na API, grava no banco e atualiza a tela.
        /// <paramref name="onUpdate"/> roda na thread de UI após cada etapa.
        /// </summary>
        private async Task RefreshWeatherAsync(Action onUpdate)
        {
            try
            {
                await _dbInitTask;

                // 1) cache local
                var salvo = await DatabaseService.Instance.GetWeatherAsync().ConfigureAwait(false);
                if (salvo.Count > 0)
                {
                    RunOnUiThread(() =>
                    {
                        _homeWeather = salvo;
                        onUpdate();
                    });
                }

                // 2) API -> banco
                var (lat, lng) = FarmLatLng();
                var data = await WeatherService.GetDailyAsync(lat, lng).ConfigureAwait(false);
                await DatabaseService.Instance.SaveWeatherAsync(data.Values, lat, lng).ConfigureAwait(false);

                // relê do banco para manter os dias antigos já salvos + os novos
                var atualizado = await DatabaseService.Instance.GetWeatherAsync().ConfigureAwait(false);
                RunOnUiThread(() =>
                {
                    _weatherOffline = false;
                    _homeWeather = atualizado;
                    onUpdate();
                });
            }
            catch (Exception ex)
            {
                // Sem internet / API indisponível: continua com o que já está salvo no banco
                Android.Util.Log.Warn("Agrosens.Weather", ex.Message);
                RunOnUiThread(() =>
                {
                    _weatherOffline = true;
                    onUpdate();
                });
            }
        }

        private Task LoadHomeWeatherAsync() => RefreshWeatherAsync(() =>
        {
            if (_homeWeekBand == null || _currentTab != AppTab.Home) return;
            BuildHomeWeekDays();
        });

        // ---------- Tela "Mais detalhes do dia" ----------

        private (double lat, double lng) FarmLatLng()
        {
            double lat = -15.7801, lng = -47.9292; // padrão: Brasília
            var prefs = GetSharedPreferences("agrosens_map", FileCreationMode.Private);
            if (prefs != null && prefs.Contains(PrefFarmLat))
            {
                double.TryParse(prefs.GetString(PrefFarmLat, "-15.7801"), NumberStyles.Float, CultureInfo.InvariantCulture, out lat);
                double.TryParse(prefs.GetString(PrefFarmLng, "-47.9292"), NumberStyles.Float, CultureInfo.InvariantCulture, out lng);
            }
            return (lat, lng);
        }

        private static string DirecaoVento(double graus)
        {
            string[] pontos = { "N", "NE", "L", "SE", "S", "SO", "O", "NO" };
            return pontos[(int)Math.Round(((graus % 360) + 360) % 360 / 45.0) % 8];
        }

        private void SetupDayDetails(View view)
        {
            try
            {
                var imgUser = view.FindViewById<ImageView>(Resource.Id.detailImgUser);
                var txtUser = view.FindViewById<TextView>(Resource.Id.detailTxtUserName);
                var btnBack = view.FindViewById<TextView>(Resource.Id.detailBtnBack);
                var txtTitle = view.FindViewById<TextView>(Resource.Id.detailDateTitle);

                if (_isLoggedOut)
                {
                    imgUser?.SetImageResource(Resource.Drawable.sem_usuario);
                    if (txtUser != null) txtUser.Text = "Usuario";
                }
                else
                {
                    imgUser?.SetImageResource(Resource.Drawable.user_photo);
                    if (txtUser != null) txtUser.Text = NomeUsuarioLogado();
                }

                var dia = _detailDay.Date;
                string titulo = $"{dia.Day}/{dia.Month} - {NomeDiaSemanaPt(dia.DayOfWeek)}";
                if (dia == DateTime.Today) titulo += " - Hoje";
                if (txtTitle != null) txtTitle.Text = titulo;

                if (btnBack != null)
                {
                    btnBack.Click += (s, e) =>
                    {
                        _homeReturnDay = dia;
                        SwitchTab(AppTab.Home);
                    };
                }

                FillDayWeather(view, dia);
                if (!_homeWeather.ContainsKey(dia))
                    _ = FetchDetailWeatherAsync(view, dia);

                _ = LoadDayDetailEventsAsync(view, dia);
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error("Agrosens.Detalhes", ex.ToString());
                MostrarErroNaTela(ex, "SetupDayDetails");
            }
        }

        private Task FetchDetailWeatherAsync(View view, DateTime dia) => RefreshWeatherAsync(() =>
        {
            if (_currentTab != AppTab.DayDetails) return;
            FillDayWeather(view, dia);
        });

        private void FillDayWeather(View view, DateTime dia)
        {
            var rain = view.FindViewById<TextView>(Resource.Id.detailRainValue);
            var rainSub = view.FindViewById<TextView>(Resource.Id.detailRainSub);
            var wind = view.FindViewById<TextView>(Resource.Id.detailWindValue);
            var arrow = view.FindViewById<FrameLayout>(Resource.Id.detailWindArrow);
            var feels = view.FindViewById<TextView>(Resource.Id.detailFeelsValue);
            var feelsSub = view.FindViewById<TextView>(Resource.Id.detailFeelsSub);
            var hum = view.FindViewById<TextView>(Resource.Id.detailHumidityValue);
            var humSub = view.FindViewById<TextView>(Resource.Id.detailHumiditySub);
            var summary = view.FindViewById<TextView>(Resource.Id.detailSummary);

            if (!_homeWeather.TryGetValue(dia.Date, out var w))
            {
                if (summary != null) summary.Text = "Previsão indisponível. Verifique a conexão com a internet.";
                return;
            }

            var pt = CultureInfo.GetCultureInfo("pt-BR");

            if (rain != null) rain.Text = w.RainSum != null ? $"{w.RainSum.Value.ToString("0.#", pt)} mm" : "--";
            if (rainSub != null)
                rainSub.Text = w.RainProbability != null
                    ? $"Probabilidade de chuva: {w.RainProbability}%"
                    : string.Empty;

            if (wind != null) wind.Text = w.WindMax != null ? $"{Math.Round(w.WindMax.Value)}" : "--";
            if (arrow != null && w.WindDirection != null) arrow.Rotation = (float)w.WindDirection.Value;

            if (feels != null)
            {
                double? media = (w.FeelsMax != null && w.FeelsMin != null) ? (w.FeelsMax + w.FeelsMin) / 2 : w.FeelsMax;
                feels.Text = media != null ? $"{Math.Round(media.Value)}°" : "--";
            }
            if (feelsSub != null && w.TempMax != null && w.TempMin != null)
                feelsSub.Text = $"Temperatura real: máx {Math.Round(w.TempMax.Value)}° / mín {Math.Round(w.TempMin.Value)}°.";

            if (hum != null) hum.Text = w.HumidityMean != null ? $"{Math.Round(w.HumidityMean.Value)}%" : "--";
            if (humSub != null && w.DewPointMean != null)
                humSub.Text = $"O ponto de orvalho médio é {Math.Round(w.DewPointMean.Value)}°.";

            if (summary != null)
            {
                string txt = $"{w.Emoji} {w.Description}";
                if (w.WindDirection != null && w.WindMax != null)
                    txt += $"  •  Vento vindo de {DirecaoVento(w.WindDirection.Value)}";
                if (_weatherOffline && w.FetchedAt != null)
                    txt += $"\n\U0001F4F4 Sem conexão — dados salvos em {w.FetchedAt.Value:dd/MM HH:mm}";
                summary.Text = txt;
            }
        }

        private async Task LoadDayDetailEventsAsync(View view, DateTime dia)
        {
            var container = view.FindViewById<LinearLayout>(Resource.Id.detailEventsContainer);
            var counter = view.FindViewById<TextView>(Resource.Id.detailCounter);
            var empty = view.FindViewById<TextView>(Resource.Id.detailEmpty);
            if (container == null) return;

            await _dbInitTask;

            try
            {
                var eventos = await DatabaseService.Instance.GetEventsForDayAsync(dia).ConfigureAwait(false); // só pendentes
                eventos = eventos.OrderBy(ev => ev.StartTime).ToList();

                RunOnUiThread(() =>
                {
                    container.RemoveAllViews();
                    float density = Resources.DisplayMetrics.Density;

                    if (counter != null)
                    {
                        string quando = dia == DateTime.Today ? "HOJE" : "NESTE DIA";
                        counter.Text = eventos.Count == 1
                            ? $"VOCÊ TEM 1 EVENTO {quando}!"
                            : $"VOCÊ TEM {eventos.Count} EVENTOS {quando}!";
                    }

                    if (empty != null) empty.Visibility = eventos.Count == 0 ? ViewStates.Visible : ViewStates.Gone;

                    for (int i = 0; i < eventos.Count; i++)
                    {
                        container.AddView(BuildHomeEventCard(eventos[i], density,
                            () => LoadDayDetailEventsAsync(view, dia)));

                        if (i < eventos.Count - 1)
                        {
                            var divider = new View(this)
                            {
                                LayoutParameters = new LinearLayout.LayoutParams(
                                    ViewGroup.LayoutParams.MatchParent, (int)(1 * density))
                            };
                            divider.SetBackgroundResource(Resource.Drawable.bg_divider_dashed);
                            divider.SetLayerType(LayerType.Software, null);
                            var lpDiv = (LinearLayout.LayoutParams)divider.LayoutParameters;
                            lpDiv.SetMargins(0, (int)(8 * density), 0, (int)(8 * density));
                            divider.LayoutParameters = lpDiv;
                            container.AddView(divider);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error("Agrosens.Detalhes", ex.ToString());
            }
        }

        // ---------- Seletor semanal (segunda → domingo) ----------

        private void BuildHomeWeekDays()
        {
            if (_homeWeekBand == null) return;
            var today = DateTime.Today;
            _homeWeekBand.RemoveAllViews();
            float density = Resources.DisplayMetrics.Density;
            View? selectedPill = null;

            for (int i = 0; i < 7; i++)
            {
                var dayDate = _homeWeekStart.AddDays(i);
                bool isSelected = dayDate.Date == _homeSelectedDay.Date;
                bool isToday = dayDate.Date == today;
                _homeWeather.TryGetValue(dayDate.Date, out var w);

                var cell = new LinearLayout(this) { Orientation = Orientation.Vertical };
                var lpCell = new LinearLayout.LayoutParams((int)(64 * density), (int)(136 * density));
                lpCell.SetMargins((int)(4 * density), (int)(4 * density), (int)(4 * density), (int)(4 * density));
                cell.LayoutParameters = lpCell;
                cell.SetGravity(GravityFlags.Center);
                cell.SetPadding(0, (int)(10 * density), 0, (int)(10 * density));
                cell.SetBackgroundResource(isSelected
                    ? Resource.Drawable.bg_week_pill_active
                    : Resource.Drawable.bg_week_pill_idle);
                cell.Elevation = isSelected ? 6 * density : 2 * density;

                var txtAbbr = new TextView(this) { Text = WeekAbbrPt[i], Gravity = GravityFlags.Center };
                txtAbbr.SetTextSize(ComplexUnitType.Sp, 12);
                txtAbbr.SetTypeface(Typeface.DefaultBold, TypefaceStyle.Bold);
                txtAbbr.SetTextColor(isSelected ? Color.White : Color.ParseColor("#1B5E20"));

                var txtDay = new TextView(this) { Text = dayDate.Day.ToString(), Gravity = GravityFlags.Center };
                txtDay.SetTextSize(ComplexUnitType.Sp, 11);
                txtDay.SetTextColor(isSelected ? Color.ParseColor("#E8F5E9") : (isToday ? Color.ParseColor("#2E7D32") : Color.ParseColor("#5A7A62")));
                if (isToday) txtDay.SetTypeface(Typeface.DefaultBold, TypefaceStyle.Bold);

                var txtIcon = new TextView(this) { Text = w?.Emoji ?? "·", Gravity = GravityFlags.Center };
                txtIcon.SetTextSize(ComplexUnitType.Sp, 28);
                var lpIcon = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                lpIcon.SetMargins(0, (int)(8 * density), 0, (int)(2 * density));
                txtIcon.LayoutParameters = lpIcon;

                var txtRain = new TextView(this)
                {
                    Text = w?.RainProbability != null ? $"{w.RainProbability}%" : " ",
                    Gravity = GravityFlags.Center
                };
                txtRain.SetTextSize(ComplexUnitType.Sp, 12);
                txtRain.SetTypeface(Typeface.DefaultBold, TypefaceStyle.Bold);
                txtRain.SetTextColor(isSelected ? Color.ParseColor("#D6F5FF") : Color.ParseColor("#1FA6C4"));

                var txtTemp = new TextView(this)
                {
                    Text = w?.TempMax != null ? $"{Math.Round(w.TempMax.Value)}°" : "--",
                    Gravity = GravityFlags.Center
                };
                txtTemp.SetTextSize(ComplexUnitType.Sp, 20);
                txtTemp.SetTextColor(isSelected ? Color.White : Color.ParseColor("#2E7D32"));
                var lpTemp = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                lpTemp.SetMargins(0, (int)(4 * density), 0, 0);
                txtTemp.LayoutParameters = lpTemp;

                cell.AddView(txtAbbr);
                cell.AddView(txtDay);
                cell.AddView(txtIcon);
                cell.AddView(txtRain);
                cell.AddView(txtTemp);

                var clicked = dayDate;
                cell.Click += (s, e) =>
                {
                    try
                    {
                        _homeSelectedDay = clicked;
                        BuildHomeWeekDays();
                        ScrollHomeToDay(clicked);
                    }
                    catch (Exception ex)
                    {
                        Android.Util.Log.Error("Agrosens.Home", ex.ToString());
                    }
                };
                _homeWeekBand.AddView(cell);
                if (isSelected) selectedPill = cell;
            }

            // Centraliza a pílula selecionada (ex.: hoje = domingo)
            if (selectedPill != null && _homeWeekScroll != null)
            {
                var pill = selectedPill;
                _homeWeekScroll.Post(() =>
                {
                    int target = pill.Left - (_homeWeekScroll.Width - pill.Width) / 2;
                    _homeWeekScroll.SmoothScrollTo(Math.Max(0, target), 0);
                });
            }
        }

        /// <summary>Rola a lista de eventos até a seção do dia clicado (se houver eventos nele).</summary>
        private void ScrollHomeToDay(DateTime day)
        {
            if (_homeScroll == null || !_homeDaySections.TryGetValue(day.Date, out var header)) return;
            var content = _homeScroll.GetChildAt(0);
            int y = 0;
            Android.Views.View? v = header;
            while (v != null && v != content)
            {
                y += v.Top;
                v = v.Parent as Android.Views.View;
            }
            int offset = (int)(12 * Resources.DisplayMetrics.Density);
            _homeScroll.SmoothScrollTo(0, Math.Max(0, y - offset));
        }

        private static string NomeDiaSemanaPt(DayOfWeek diaSemana) => diaSemana switch
        {
            DayOfWeek.Sunday => "Domingo",
            DayOfWeek.Monday => "Segunda-Feira",
            DayOfWeek.Tuesday => "Terça-Feira",
            DayOfWeek.Wednesday => "Quarta-Feira",
            DayOfWeek.Thursday => "Quinta-Feira",
            DayOfWeek.Friday => "Sexta-Feira",
            DayOfWeek.Saturday => "Sábado",
            _ => string.Empty
        };

        // ---------- Eventos da semana inteira ----------

        private async Task LoadHomeEventsAsync()
        {
            if (_homeEventsContainer == null) return;

            await _dbInitTask;

            try
            {
                var inicio = _homeWeekStart;
                var fim = _homeWeekStart.AddDays(6);
                var eventos = await DatabaseService.Instance.GetEventsForWeekAsync(inicio, fim).ConfigureAwait(false);

                var pendentes = eventos
                    .Where(ev => ev.Status == CalendarEvent.StatusPendente)
                    .OrderBy(ev => ev.EventDate.Date)
                    .ThenBy(ev => ev.StartTime)
                    .ToList();

                RunOnUiThread(() =>
                {
                    if (_homeEventsContainer == null) return;
                    _homeEventsContainer.RemoveAllViews();
                    _homeDaySections.Clear();
                    float density = Resources.DisplayMetrics.Density;

                    if (_homeWeekCounter != null)
                    {
                        _homeWeekCounter.Text = pendentes.Count == 1
                            ? "VOCÊ TEM 1 EVENTO ESSA SEMANA!"
                            : $"VOCÊ TEM {pendentes.Count} EVENTOS ESSA SEMANA!";
                    }

                    if (pendentes.Count == 0)
                    {
                        if (_txtEmptyEvents != null) _txtEmptyEvents.Visibility = ViewStates.Visible;
                        return;
                    }
                    if (_txtEmptyEvents != null) _txtEmptyEvents.Visibility = ViewStates.Gone;

                    foreach (var grupo in pendentes.GroupBy(ev => ev.EventDate.Date))
                    {
                        var dia = grupo.Key;
                        string titulo = $"{dia.Day}/{dia.Month} - {NomeDiaSemanaPt(dia.DayOfWeek)}";
                        if (dia == DateTime.Today) titulo += " - Hoje";

                        var header = new TextView(this) { Text = titulo };
                        header.SetTextSize(ComplexUnitType.Sp, 13);
                        header.SetTypeface(Typeface.DefaultBold, TypefaceStyle.Bold);
                        header.SetTextColor(Color.ParseColor(dia == DateTime.Today ? "#1B8F3A" : "#2E7D32"));
                        var lpHeader = new LinearLayout.LayoutParams(
                            ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                        lpHeader.SetMargins(0, (int)(4 * density), 0, (int)(4 * density));
                        header.LayoutParameters = lpHeader;
                        _homeEventsContainer.AddView(header);
                        _homeDaySections[dia] = header;

                        foreach (var ev in grupo)
                            _homeEventsContainer.AddView(BuildHomeEventCard(ev, density));

                        var divider = new View(this)
                        {
                            LayoutParameters = new LinearLayout.LayoutParams(
                                ViewGroup.LayoutParams.MatchParent, (int)(1 * density))
                        };
                        divider.SetBackgroundResource(Resource.Drawable.bg_divider_dashed);
                        divider.SetLayerType(LayerType.Software, null); // necessário p/ linha tracejada
                        var lpDiv = (LinearLayout.LayoutParams)divider.LayoutParameters;
                        lpDiv.SetMargins(0, (int)(10 * density), 0, (int)(12 * density));
                        divider.LayoutParameters = lpDiv;
                        _homeEventsContainer.AddView(divider);
                    }
                });
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error("Agrosens.Home", ex.ToString());
            }
        }

        private static string FormatHora(TimeSpan t) =>
            t.Minutes == 0 ? $"{t.Hours} H" : $"{t.Hours}:{t.Minutes:00} H";

        private LinearLayout BuildHomeEventCard(CalendarEvent ev, float density, Func<Task>? recarregar = null)
        {
            var card = new LinearLayout(this) { Orientation = Orientation.Horizontal };
            var lpCard = new LinearLayout.LayoutParams(
                ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
            lpCard.SetMargins(0, (int)(4 * density), 0, (int)(4 * density));
            card.LayoutParameters = lpCard;
            card.SetGravity(GravityFlags.CenterVertical);

            var time = new TextView(this) { Text = $"{FormatHora(ev.StartTime)} - {FormatHora(ev.EndTime)}" };
            time.SetTextSize(ComplexUnitType.Sp, 12);
            time.SetTextColor(Color.ParseColor("#2E7D32"));
            time.SetTypeface(Typeface.DefaultBold, TypefaceStyle.Bold);
            var lpTime = new LinearLayout.LayoutParams((int)(84 * density), ViewGroup.LayoutParams.WrapContent);
            time.LayoutParameters = lpTime;

            var info = new LinearLayout(this) { Orientation = Orientation.Vertical };
            info.LayoutParameters = new LinearLayout.LayoutParams(0, ViewGroup.LayoutParams.WrapContent, 1f);

            var title = new TextView(this)
            {
                Text = string.IsNullOrWhiteSpace(ev.Category) ? ev.Title : $"{ev.Title} [{ev.Category}]"
            };
            title.SetTextSize(ComplexUnitType.Sp, 15);
            title.SetTypeface(Typeface.DefaultBold, TypefaceStyle.Bold);
            title.SetTextColor(Color.ParseColor("#2BA84F"));
            title.SetMaxLines(2);
            title.Ellipsize = TextUtils.TruncateAt.End;

            var desc = new TextView(this)
            {
                Text = string.IsNullOrWhiteSpace(ev.Description) ? ev.Category : ev.Description
            };
            desc.SetTextSize(ComplexUnitType.Sp, 12);
            desc.SetTextColor(Color.ParseColor("#3E5C46"));
            desc.SetMaxLines(2);
            desc.Ellipsize = TextUtils.TruncateAt.End;
            if (string.IsNullOrWhiteSpace(desc.Text)) desc.Visibility = ViewStates.Gone;

            info.AddView(title);
            info.AddView(desc);

            var btnOk = new ImageView(this)
            {
                LayoutParameters = new LinearLayout.LayoutParams((int)(34 * density), (int)(34 * density)),
                Clickable = true,
                Focusable = true,
                ContentDescription = "Feito"
            };
            btnOk.SetPadding((int)(6 * density), (int)(6 * density), (int)(6 * density), (int)(6 * density));
            btnOk.SetImageResource(Resource.Drawable.ic_check);

            var btnX = new ImageView(this)
            {
                LayoutParameters = new LinearLayout.LayoutParams((int)(34 * density), (int)(34 * density)),
                Clickable = true,
                Focusable = true,
                ContentDescription = "Cancelado"
            };
            btnX.SetPadding((int)(6 * density), (int)(6 * density), (int)(6 * density), (int)(6 * density));
            btnX.SetImageResource(Resource.Drawable.ic_close);

            async void Concluir(int novoStatus)
            {
                btnOk.Enabled = false;
                btnX.Enabled = false;
                try
                {
                    ev.Status = novoStatus;
                    await DatabaseService.Instance.SaveEventAsync(ev);
                    RunOnUiThread(() =>
                    {
                        try
                        {
                            // Some com o card e recarrega a semana (remove o cabeçalho do dia se esvaziar)
                            AnimateAndRemoveCard(card, card.Parent as ViewGroup, async () =>
                            {
                                await (recarregar != null ? recarregar() : LoadHomeEventsAsync());
                            });
                        }
                        catch (Exception ex) { Android.Util.Log.Error("Agrosens.Home", ex.ToString()); }
                    });
                }
                catch (Exception ex)
                {
                    Android.Util.Log.Error("Agrosens.Home", ex.ToString());
                    btnOk.Enabled = true;
                    btnX.Enabled = true;
                }
            }

            btnOk.Click += (s2, e2) => Concluir(CalendarEvent.StatusFeito);
            btnX.Click += (s2, e2) => Concluir(CalendarEvent.StatusCancelado);

            card.AddView(time);
            card.AddView(info);
            card.AddView(btnOk);
            card.AddView(btnX);
            return card;
        }

        private void SetupPerfilEvents(View view)
        {
            try
            {
                var imgPerfilUser = view.FindViewById<ImageView>(Resource.Id.imgPerfilUser);
            var txtPerfilName = view.FindViewById<TextView>(Resource.Id.txtPerfilName);
            var txtMenuLogout = view.FindViewById<TextView>(Resource.Id.txtMenuLogout);

            void UpdateProfileUI()
            {
                if (_isLoggedOut)
                {
                    imgPerfilUser?.SetImageResource(Resource.Drawable.sem_usuario);
                    if (txtPerfilName != null) txtPerfilName.Text = "Usuario";
                    if (txtMenuLogout != null) txtMenuLogout.Text = "Login";
                }
    else
    {
        imgPerfilUser?.SetImageResource(Resource.Drawable.user_photo);
        var storePerfil = new UserProfileStore(this);
        if (txtPerfilName != null) txtPerfilName.Text = storePerfil.Name;
        if (txtMenuLogout != null) txtMenuLogout.Text = "Logout";
    }
            }

            UpdateProfileUI();

            var menuProfile = view.FindViewById(Resource.Id.menuProfile);
            if (menuProfile != null)
            {
                menuProfile.Click += (s, e) => SwitchTab(AppTab.EditProfile);
            }

            var menuPrivacy = view.FindViewById(Resource.Id.menuPrivacy);
            if (menuPrivacy != null)
            {
                menuPrivacy.Click += (s, e) => SwitchTab(AppTab.PrivacyPolicy);
            }
            var menuSettings = view.FindViewById(Resource.Id.menuSettings);
            if (menuSettings != null)
            {
                menuSettings.Click += (s, e) => SwitchTab(AppTab.Settings);
            }

            var menuHelp = view.FindViewById(Resource.Id.menuHelp);
            if (menuHelp != null)
            {
                menuHelp.Click += (s, e) => SwitchTab(AppTab.AboutUs);
            }

            var menuLogout = view.FindViewById(Resource.Id.menuLogout);
            if (menuLogout != null)
            {
                menuLogout.Click += (s, e) =>
                {
                    var prefsLogout = new PreferenceService(this);
                    prefsLogout.IsLoggedIn = false;
                    SwitchTab(AppTab.AuthWelcome);
                };
            }
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error("Agrosens.Perfil", ex.ToString());
                MostrarErroNaTela(ex, "SetupPerfilEvents");
            }
        }

        private void SetupAboutUsEvents(View view)
        {
            var btnBackAbout = view.FindViewById(Resource.Id.btnBackAbout);
            if (btnBackAbout != null)
            {
                btnBackAbout.Click += (s, e) => SwitchTab(AppTab.Perfil);
            }
        }

        private void SetupPrivacyEvents(View view)
        {
            var btnBackPrivacy = view.FindViewById(Resource.Id.btnBackPrivacy);
            if (btnBackPrivacy != null)
            {
                btnBackPrivacy.Click += (s, e) => SwitchTab(AppTab.Perfil);
            }
        }

        private void SetupSettingsEvents(View view)
        {
            var btnBackSettings = view.FindViewById(Resource.Id.btnBackSettings);
            if (btnBackSettings != null)
            {
                btnBackSettings.Click += (s, e) => SwitchTab(AppTab.Perfil);
            }

            var menuNotifications = view.FindViewById(Resource.Id.menuNotifications);
            if (menuNotifications != null)
            {
                menuNotifications.Click += (s, e) => { };
            }

            var btnChangePassword = view.FindViewById(Resource.Id.btnChangePassword);
            if (btnChangePassword != null)
            {
                btnChangePassword.Click += (s, e) => SwitchTab(AppTab.ChangePassword);
            }

            var btnClearData = view.FindViewById(Resource.Id.btnClearData);
            if (btnClearData != null)
            {
                btnClearData.Click += (s, e) =>
                {
                    var dlg = new AlertDialog.Builder(this);
                    dlg.SetTitle("Limpar dados do calendario");
                    dlg.SetMessage("Todos os eventos do calendário serão apagados. Deseja continuar?");
                    dlg.SetPositiveButton("Limpar", async (sender, args) =>
                    {
                        try
                        {
                            await _dbInitTask;
                            await DatabaseService.Instance.ClearCalendarAsync();
                        }
                        catch (Exception ex)
                        {
                            Android.Util.Log.Error("Agrosens.DB", $"Erro ao limpar dados: {ex}");
                            Toast.MakeText(this, "Erro ao limpar dados.", ToastLength.Short)?.Show();
                        }
                    });
                    dlg.SetNegativeButton("Cancelar", (sender, args) => { });
                    dlg.Show();
                };
            }
        }

        private void SetupChangePasswordEvents(View view)
        {
            var btnBackChangePassword = view.FindViewById(Resource.Id.btnBackChangePassword);
            if (btnBackChangePassword != null)
            {
                btnBackChangePassword.Click += (s, e) => SwitchTab(AppTab.Settings);
            }

            var btnSavePassword = view.FindViewById(Resource.Id.btnSavePassword);
            if (btnSavePassword != null)
            {
                btnSavePassword.Click += (s, e) =>
                {
                    Toast.MakeText(this, "Senha alterada com sucesso!", ToastLength.Short)?.Show();
                    SwitchTab(AppTab.Settings);
                };
            }
        }

        private void SetupEditProfileEvents(View view)
        {
            try
            {
                var btnBackEditProfile = view.FindViewById(Resource.Id.btnBackEditProfile);
                if (btnBackEditProfile != null)
                {
                    btnBackEditProfile.Click += (s, e) => SwitchTab(AppTab.Perfil);
                }

                var btnUpdateProfile = view.FindViewById(Resource.Id.btnUpdateProfile);

                var etFullName = view.FindViewById<EditText>(Resource.Id.etEditFullName);
                var etPhone = view.FindViewById<EditText>(Resource.Id.etEditPhone);
                var etEmail = view.FindViewById<EditText>(Resource.Id.etEditEmail);

                var storeEdit = new UserProfileStore(this);
                if (etFullName != null) etFullName.Text = storeEdit.Name;
                if (etPhone != null) etPhone.Text = storeEdit.Phone;
                if (etEmail != null) etEmail.Text = storeEdit.Email;

                if (btnUpdateProfile != null)
                {
                    btnUpdateProfile.Click += (s, e) =>
                    {
                        try
                        {
                            var nome = etFullName?.Text?.Trim() ?? string.Empty;
                            var telefone = etPhone?.Text?.Trim() ?? string.Empty;
                            var email = etEmail?.Text?.Trim() ?? string.Empty;

                            if (string.IsNullOrWhiteSpace(nome))
                            {
                                Toast.MakeText(this, "Informe o nome completo.", ToastLength.Short)?.Show();
                                return;
                            }

                            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@") || !email.Contains("."))
                            {
                                Toast.MakeText(this, "Informe um e-mail válido.", ToastLength.Short)?.Show();
                                return;
                            }

                            storeEdit.Save(nome, telefone, email);

                            Toast.MakeText(this, "Perfil atualizado", ToastLength.Short)?.Show();

                            SwitchTab(AppTab.Perfil);
                        }
                        catch (Exception ex)
                        {
                            Android.Util.Log.Error("AgroSens.EditProfile", ex.ToString());
                            Toast.MakeText(this, "Erro ao salvar o perfil.", ToastLength.Short)?.Show();
                        }
                    };
                }

                BindClick(view, Resource.Id.btnChangeAvatar, "Alterar foto de perfil");
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error("AgroSens.EditProfile", ex.ToString());
                MostrarErroNaTela(ex, "SetupEditProfileEvents");
            }
        }

        private async void SetupGeralEvents(View view)
        {
            try
            {
                var btnAddSensor = view.FindViewById<FrameLayout>(Resource.Id.btnAddSensor);
                if (btnAddSensor != null)
                {
                    btnAddSensor.Click += (s, e) =>
                    {
                        var sheet = new AddSensorBottomSheet
                        {
                            OnSaved = async () => await RefreshGeralSensorsAsync()
                        };
                        sheet.Show(SupportFragmentManager, "add_sensor_bottom_sheet");
                    };
                }

                var btnEmptyAdd = view.FindViewById<TextView>(Resource.Id.btnSensoresEmptyAdd);
                if (btnEmptyAdd != null)
                {
                    btnEmptyAdd.Click += (s, e) =>
                    {
                        var sheet = new AddSensorBottomSheet
                        {
                            OnSaved = async () => await RefreshGeralSensorsAsync()
                        };
                        sheet.Show(SupportFragmentManager, "add_sensor_bottom_sheet");
                    };
                }

                _geralView = view;
                _ = RefreshGeralSensorsAsync();
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error("Agrosens.Geral", ex.ToString());
                MostrarErroNaTela(ex, "SetupGeralEvents");
            }
        }

        private async Task RefreshGeralSensorsAsync()
        {
            if (_geralView == null) return;

            await _dbInitTask;

            try
            {
                var sensores = await DatabaseService.Instance.GetSensoresAsync();

                RunOnUiThread(() =>
                {
                    var container = _geralView!.FindViewById<LinearLayout>(Resource.Id.sensoresContainer);
                    var txtSection = _geralView.FindViewById<TextView>(Resource.Id.txtSensoresSectionTitle);
                    var imgEmpty = _geralView.FindViewById<ImageView>(Resource.Id.imgSensoresEmpty);
                    var txtEmpty = _geralView.FindViewById<TextView>(Resource.Id.txtSensoresEmpty);
                    var txtEmptySub = _geralView.FindViewById<TextView>(Resource.Id.txtSensoresEmptySub);
                    var btnEmptyAdd = _geralView.FindViewById<TextView>(Resource.Id.btnSensoresEmptyAdd);

                    container.RemoveAllViews();

                    bool vazio = sensores.Count == 0;

                    if (txtSection != null)
                    {
                        txtSection.Visibility = vazio ? ViewStates.Gone : ViewStates.Visible;
                        txtSection.Text = $"Sensores cadastrados ({sensores.Count})";
                    }

                    if (vazio)
                    {
                        imgEmpty.Visibility = ViewStates.Visible;
                        txtEmpty.Visibility = ViewStates.Visible;
                        txtEmptySub.Visibility = ViewStates.Visible;
                        btnEmptyAdd.Visibility = ViewStates.Visible;
                        return;
                    }

                    imgEmpty.Visibility = ViewStates.Gone;
                    txtEmpty.Visibility = ViewStates.Gone;
                    txtEmptySub.Visibility = ViewStates.Gone;
                    btnEmptyAdd.Visibility = ViewStates.Gone;

                    foreach (var s in sensores)
                    {
                        var card = BuildSensorCard(s);
                        card.Click += (_, __) => AbrirDetalhesSensor(s);
                        container.AddView(card);
                    }
                });
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error("Agrosens.Geral", ex.ToString());
            }
        }

        private int Dp(float dp) => (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dp, Resources.DisplayMetrics);

        private static string NomeExibicao(Sensor s) =>
            string.IsNullOrWhiteSpace(s.Nome) ? "Sensor sem nome" : s.Nome.Trim();

        private Android.Views.View BuildSensorCard(Sensor s)
        {
            bool alerta = EstaEmAlerta(s);

            var card = new FrameLayout(this)
            {
                LayoutParameters = new LinearLayout.LayoutParams(
                    ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent),
                Clickable = true,
                Focusable = true
            };
            card.SetBackgroundResource(alerta
                ? Resource.Drawable.bg_card_sensor_alert
                : Resource.Drawable.bg_card_sensor);
            int pad = Dp(14);
            card.SetPadding(pad, pad, pad, pad);
            card.SetMinimumHeight(Dp(88));
            var lpCard = new LinearLayout.LayoutParams(
                ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
            lpCard.SetMargins(0, Dp(6), 0, Dp(6));
            card.LayoutParameters = lpCard;

            var content = new LinearLayout(this) { Orientation = Orientation.Horizontal };
            content.LayoutParameters = new FrameLayout.LayoutParams(
                ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);

            // --- ESQUERDA: círculo branco 52dp, ícone 28dp verde escuro, rótulo do tipo ---
            var iconZone = new LinearLayout(this) { Orientation = Orientation.Vertical };
            iconZone.SetGravity(GravityFlags.Center);
            iconZone.LayoutParameters = new LinearLayout.LayoutParams(Dp(52), ViewGroup.LayoutParams.MatchParent);

            var iconWrap = new FrameLayout(this)
            {
                LayoutParameters = new LinearLayout.LayoutParams(Dp(52), Dp(52))
            };
            // Drawable próprio por card (instância compartilhada deforma entre views)
            iconWrap.SetBackgroundResource(Resource.Drawable.bg_circle_white_solid);
            // O ícone vive num FrameLayout: precisa de FrameLayout.LayoutParams com gravity Center,
            // senão (com LinearLayout.LayoutParams) ele gruda no canto superior esquerdo do círculo.
            var icon = new ImageView(this)
            {
                LayoutParameters = new FrameLayout.LayoutParams(Dp(28), Dp(28), GravityFlags.Center),
                ContentDescription = $"Ícone do sensor {TipoLabel(s.Tipo)}"
            };
            icon.SetScaleType(ImageView.ScaleType.FitCenter);
            icon.SetImageResource(IconRes(s.Tipo));
            icon.SetColorFilter(Color.ParseColor("#1B5E20"), PorterDuff.Mode.SrcIn);
            iconWrap.AddView(icon);

            var lblTipo = new TextView(this)
            {
                Text = TipoLabel(s.Tipo),
                Gravity = GravityFlags.CenterHorizontal
            };
            lblTipo.SetTextSize(ComplexUnitType.Sp, 10);
            lblTipo.SetTextColor(Color.White);
            lblTipo.SetTypeface(Typeface.DefaultBold, TypefaceStyle.Bold);
            lblTipo.SetMaxLines(1);
            lblTipo.Ellipsize = TextUtils.TruncateAt.End;
            var lpLbl = new LinearLayout.LayoutParams(
                ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            lpLbl.SetMargins(0, Dp(6), 0, 0);
            lblTipo.LayoutParameters = lpLbl;

            iconZone.AddView(iconWrap);
            iconZone.AddView(lblTipo);

            // --- divisor vertical fino, branco 30% de opacidade ---
            var divider = new View(this)
            {
                LayoutParameters = new LinearLayout.LayoutParams(1, ViewGroup.LayoutParams.MatchParent)
            };
            divider.SetBackgroundColor(Color.ParseColor("#4DFFFFFF"));
            var lpDiv = new LinearLayout.LayoutParams(1, ViewGroup.LayoutParams.MatchParent);
            lpDiv.SetMargins(Dp(10), 0, Dp(10), 0);
            divider.LayoutParameters = lpDiv;

            // --- CENTRO (weight 1, encolhe com ellipsis) ---
            var info = new LinearLayout(this) { Orientation = Orientation.Vertical };
            info.LayoutParameters = new LinearLayout.LayoutParams(0, ViewGroup.LayoutParams.WrapContent, 1f);

            // Linha 1 - TÍTULO: nome do sensor
            var lblTitulo = new TextView(this) { Text = NomeExibicao(s) };
            lblTitulo.SetTextSize(ComplexUnitType.Sp, 17);
            lblTitulo.SetTypeface(Typeface.DefaultBold, TypefaceStyle.Bold);
            lblTitulo.SetTextColor(Color.White);
            lblTitulo.SetMaxLines(1);
            lblTitulo.Ellipsize = TextUtils.TruncateAt.End;
            info.AddView(lblTitulo);

            // Linha 2 - SUBTÍTULO: tipo do sensor (+ limite de alerta)
            string subtitulo = TipoLabel(s.Tipo);
            if (s.Tipo == "Porteira" && s.MinutosAberta.HasValue)
                subtitulo += $" • alerta após {s.MinutosAberta.Value} min";
            else if (s.LimiteAlerta.HasValue)
                subtitulo += $" • alerta abaixo de {s.LimiteAlerta.Value}%";
            var lblSubtitulo = new TextView(this) { Text = subtitulo };
            lblSubtitulo.SetTextSize(ComplexUnitType.Sp, 12);
            lblSubtitulo.SetTextColor(Color.ParseColor("#CCFFFFFF"));
            lblSubtitulo.SetMaxLines(1);
            lblSubtitulo.Ellipsize = TextUtils.TruncateAt.End;
            var lpSub = new LinearLayout.LayoutParams(
                ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            lpSub.SetMargins(0, Dp(4), 0, 0);
            lblSubtitulo.LayoutParameters = lpSub;
            info.AddView(lblSubtitulo);

            // Linha 3 - LEITURA
            var linhaLeitura = new LinearLayout(this) { Orientation = Orientation.Horizontal };
            linhaLeitura.LayoutParameters = new LinearLayout.LayoutParams(
                ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);

            bool semDados = (s.Tipo == "Porteira")
                ? !s.PorteiraAberta.HasValue
                : !s.ValorAtual.HasValue;

            if (semDados)
            {
                var ponto = new View(this)
                {
                    LayoutParameters = new LinearLayout.LayoutParams(Dp(6), Dp(6))
                };
                ponto.SetBackgroundColor(Color.ParseColor("#FFCA28"));
                var lpPonto = (LinearLayout.LayoutParams)ponto.LayoutParameters;
                lpPonto.Gravity = GravityFlags.CenterVertical;
                lpPonto.SetMargins(0, 0, Dp(6), 0);

                var txtEsperando = new TextView(this) { Text = "Aguardando dados" };
                txtEsperando.SetTextSize(ComplexUnitType.Sp, 13);
                txtEsperando.SetTypeface(Typeface.Default, TypefaceStyle.Italic);
                txtEsperando.SetTextColor(Color.ParseColor("#D9FFFFFF"));
                txtEsperando.SetMaxLines(1);
                txtEsperando.Ellipsize = TextUtils.TruncateAt.End;

                linhaLeitura.AddView(ponto);
                linhaLeitura.AddView(txtEsperando);
            }
            else
            {
                string valor = s.Tipo == "Porteira"
                    ? (s.PorteiraAberta == true ? "Aberta" : "Fechada")
                    : $"{s.ValorAtual.Value:0.#}%";

                var txtValor = new TextView(this) { Text = valor };
                txtValor.SetTextSize(ComplexUnitType.Sp, 22);
                txtValor.SetTypeface(Typeface.DefaultBold, TypefaceStyle.Bold);
                txtValor.SetTextColor(Color.White);
                txtValor.SetMaxLines(1);
                linhaLeitura.AddView(txtValor);

                var txtAtualizado = new TextView(this) { Text = TextoAtualizado(s) };
                txtAtualizado.SetTextSize(ComplexUnitType.Sp, 11);
                txtAtualizado.SetTextColor(Color.ParseColor("#CCFFFFFF"));
                var lpUpd = new LinearLayout.LayoutParams(
                    ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                lpUpd.SetMargins(Dp(8), 0, 0, 0);
                lpUpd.Gravity = GravityFlags.CenterVertical;
                txtAtualizado.LayoutParameters = lpUpd;
                linhaLeitura.AddView(txtAtualizado);

                if (alerta)
                {
                    var icAlerta = new ImageView(this)
                    {
                        LayoutParameters = new LinearLayout.LayoutParams(Dp(16), Dp(16)),
                        ContentDescription = "Alerta"
                    };
                    icAlerta.SetImageResource(Resource.Drawable.ic_alert);
                    var lpAl = (LinearLayout.LayoutParams)icAlerta.LayoutParameters;
                    lpAl.SetMargins(Dp(8), 0, 0, 0);
                    lpAl.Gravity = GravityFlags.CenterVertical;
                    linhaLeitura.AddView(icAlerta);
                }
            }
            info.AddView(linhaLeitura);

            // --- DIREITA: botão de 3 pontinhos 40x40dp, alinhado ao topo ---
            var btnMenu = new ImageView(this)
            {
                LayoutParameters = new LinearLayout.LayoutParams(Dp(40), Dp(40)) { Gravity = GravityFlags.Top },
                Clickable = true,
                Focusable = true,
                ContentDescription = "Opções do sensor"
            };
            btnMenu.SetImageResource(Resource.Drawable.ic_menu_more);

            content.AddView(iconZone);
            content.AddView(divider);
            content.AddView(info);
            content.AddView(btnMenu);
            card.AddView(content);

            card.Click += (_, __) => AbrirDetalhesSensor(s);

            btnMenu.Click += (_, __) =>
            {
                var popup = new PopupMenu(this, btnMenu);
                popup.Menu.Add(0, 1, 0, "Detalhes");
                popup.Menu.Add(0, 2, 0, "Editar");
                popup.Menu.Add(0, 3, 0, "Excluir");
                popup.MenuItemClick += (popupSender, args) =>
                {
                    switch (args.Item.ItemId)
                    {
                        case 1: AbrirDetalhesSensor(s); break;
                        case 2: OpenEditSheet(s); break;
                        case 3: ConfirmarExclusao(s); break;
                    }
                };
                popup.Show();
            };

            return card;
        }

        private string TextoAtualizado(Sensor s)
        {
            int min = (int)Math.Round((DateTime.Now - s.CreatedAt).TotalMinutes);
            return min < 1 ? "Atualizado há 0 min" : $"Atualizado há {min} min";
        }

        private static int IconRes(string tipo) => tipo switch
        {
            "Umidade" => Resource.Drawable.ic_humidity_green,
            "Porteira" => Resource.Drawable.ic_gate_green,
            "Agua" => Resource.Drawable.ic_water_green,
            _ => Resource.Drawable.ic_silage_green
        };

        private static string TipoLabel(string tipo) => tipo switch
        {
            "Umidade" => "Umidade",
            "Porteira" => "Porteira",
            "Agua" => "Água",
            _ => "Silagem"
        };

        private static string MainText(Sensor s)
        {
            switch (s.Tipo)
            {
                case "Umidade":
                case "Agua":
                case "Silagem":
                    return s.ValorAtual.HasValue
                        ? $"{s.Nome} - {s.ValorAtual.Value:0.#}%"
                        : "Aguardando dados";
                case "Porteira":
                    return s.PorteiraAberta.HasValue
                        ? $"{s.Nome} - {(s.PorteiraAberta.Value ? "Aberta" : "Fechada")}"
                        : "Aguardando dados";
                default:
                    return "Aguardando dados";
            }
        }

        private static bool EstaEmAlerta(Sensor s)
        {
            if (s.Tipo == "Porteira")
                return s.AlertaPorteiraAberta && s.PorteiraAberta == true;
            return s.LimiteAlerta.HasValue && s.ValorAtual.HasValue && s.ValorAtual.Value < s.LimiteAlerta.Value;
        }

        private void AbrirDetalhesSensor(Sensor s)
        {
            try
            {
                var dlg = new AlertDialog.Builder(this);
                dlg.SetTitle(s.Nome);
                dlg.SetMessage(
                    $"Tipo: {TipoLabel(s.Tipo)}\n" +
                    $"Leitura: {MainText(s)}" +
                    (s.Tipo == "Porteira" && s.MinutosAberta.HasValue ? $"\nAlerta se aberta por mais de: {s.MinutosAberta.Value} min" : "") +
                    (s.LimiteAlerta.HasValue ? $"\nAlerta abaixo de: {s.LimiteAlerta.Value}%" : ""));
                dlg.SetPositiveButton("Editar", (sender, args) => OpenEditSheet(s));
                dlg.SetNeutralButton("Excluir", (sender, args) => ConfirmarExclusao(s));
                dlg.SetNegativeButton("Fechar", (sender, args) => { });
                dlg.Show();
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error("Agrosens.Geral", ex.ToString());
            }
        }

        private async void ConfirmarExclusao(Sensor s)
        {
            try
            {
                var dlg = new AlertDialog.Builder(this);
                dlg.SetTitle("Excluir sensor");
                dlg.SetMessage($"Tem certeza que deseja excluir o sensor '{s.Nome}'?");
                dlg.SetPositiveButton("Excluir", async (sender, args) =>
                {
                    try
                    {
                        await DatabaseService.Instance.DeleteSensorAsync(s);
                        Toast.MakeText(this, "Sensor excluído", ToastLength.Short)?.Show();
                        _ = RefreshGeralSensorsAsync();
                    }
                    catch (Exception ex)
                    {
                        Android.Util.Log.Error("Agrosens.Geral", ex.ToString());
                        Toast.MakeText(this, "Erro ao excluir sensor.", ToastLength.Short)?.Show();
                    }
                });
                dlg.SetNegativeButton("Cancelar", (sender, args) => { });
                dlg.Show();
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error("Agrosens.Geral", ex.ToString());
            }
        }

        private void OpenEditSheet(Sensor s)
        {
            try
            {
                var sheet = new AddSensorBottomSheet
                {
                    SensorEditando = s,
                    OnSaved = async () => await RefreshGeralSensorsAsync()
                };
                sheet.Show(SupportFragmentManager, "edit_sensor_bottom_sheet");
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error("Agrosens.Geral", ex.ToString());
            }
        }

        private void SetupAuthWelcomeEvents(View view)
        {
            var btnAuthLogIn = view.FindViewById(Resource.Id.btnAuthLogIn);
            if (btnAuthLogIn != null)
            {
                btnAuthLogIn.Click += (s, e) =>
                {
                    SwitchTab(AppTab.LoginForm);
                };
            }

            var btnAuthSignUp = view.FindViewById(Resource.Id.btnAuthSignUp);
            if (btnAuthSignUp != null)
            {
                btnAuthSignUp.Click += (s, e) =>
                {
                    SwitchTab(AppTab.SignUpForm);
                };
            }
        }

        private void SetupLoginFormEvents(View view)
        {
            var btnBackLogin = view.FindViewById(Resource.Id.btnBackLogin);
            if (btnBackLogin != null)
            {
                btnBackLogin.Click += (s, e) =>
                {
                    SwitchTab(AppTab.AuthWelcome);
                };
            }

            var btnSubmitLogIn = view.FindViewById(Resource.Id.btnSubmitLogIn);
            if (btnSubmitLogIn != null)
            {
                btnSubmitLogIn.Click += (s, e) =>
                {
                    var etEmailOrPhone = view.FindViewById<EditText>(Resource.Id.etEmailOrPhone);
                    string login = etEmailOrPhone?.Text?.Trim() ?? "";
                    if (string.IsNullOrWhiteSpace(login))
                    {
                        Toast.MakeText(this, "Informe o e-mail ou celular.", ToastLength.Short)?.Show();
                        return;
                    }
                    var prefsLogin = new PreferenceService(this);
                    prefsLogin.SaveLoginSession(login);

                    // Se o e-mail informado corresponde a um usuário cadastrado, mantém o perfil salvo.
                    // Caso contrário, restaura o perfil padrão para não vazar nome de cadastro anterior.
                    var storeLogin = new UserProfileStore(this);
                    if (storeLogin.IsEmailRegistered(login))
                    {
                        // Perfil já está no UserProfileStore (gravado no cadastro). Nada a sobrescrever.
                    }
                    else
                    {
                        storeLogin.RestoreDefaults();
                    }

                    Toast.MakeText(this, "Login efetuado com sucesso!", ToastLength.Short)?.Show();
                    SwitchTab(AppTab.Home);
                };
            }

            var btnFormSignUp = view.FindViewById(Resource.Id.btnFormSignUp);
            if (btnFormSignUp != null)
            {
                btnFormSignUp.Click += (s, e) =>
                {
                    SwitchTab(AppTab.SignUpForm);
                };
            }

            var btnForgotPassword = view.FindViewById(Resource.Id.btnForgotPassword);
            if (btnForgotPassword != null)
            {
                btnForgotPassword.Click += (s, e) =>
                {
                    Toast.MakeText(this, "Instruções enviadas para o seu e-mail.", ToastLength.Short)?.Show();
                };
            }

            var etPassword = view.FindViewById<EditText>(Resource.Id.etPassword);
            var btnTogglePassword = view.FindViewById(Resource.Id.btnTogglePassword);
            bool isPasswordVisible = false;

            if (btnTogglePassword != null && etPassword != null)
            {
                btnTogglePassword.Click += (s, e) =>
                {
                    isPasswordVisible = !isPasswordVisible;
                    if (isPasswordVisible)
                    {
                        etPassword.InputType = Android.Text.InputTypes.ClassText | Android.Text.InputTypes.TextVariationVisiblePassword;
                    }
                    else
                    {
                        etPassword.InputType = Android.Text.InputTypes.ClassText | Android.Text.InputTypes.TextVariationPassword;
                    }
                    etPassword.SetSelection(etPassword.Text?.Length ?? 0);
                };
            }
        }

        private void SetupSignUpFormEvents(View view)
        {
            var btnBackSignUp = view.FindViewById(Resource.Id.btnBackSignUp);
            if (btnBackSignUp != null)
            {
                btnBackSignUp.Click += (s, e) =>
                {
                    SwitchTab(AppTab.AuthWelcome);
                };
            }

            var btnSubmitSignUp = view.FindViewById(Resource.Id.btnSubmitSignUp);
            if (btnSubmitSignUp != null)
            {
                btnSubmitSignUp.Click += (s, e) =>
                {
                    var etFullName = view.FindViewById<EditText>(Resource.Id.etFullName);
                    var etSignUpEmail = view.FindViewById<EditText>(Resource.Id.etSignUpEmail);
                    var etSignUpPhone = view.FindViewById<EditText>(Resource.Id.etSignUpPhone);
                    string nome = etFullName?.Text?.Trim() ?? "";
                    string email = etSignUpEmail?.Text?.Trim() ?? "";
                    string phone = etSignUpPhone?.Text?.Trim() ?? "";
                    if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(email))
                    {
                        Toast.MakeText(this, "Preencha nome e e-mail antes de cadastrar.", ToastLength.Short)?.Show();
                        return;
                    }
                    var prefsSignUp = new PreferenceService(this);
                    prefsSignUp.SaveSession(nome, email);

                    // Grava o perfil cadastrado no UserProfileStore (única fonte de verdade do perfil)
                    var storeSignUp = new UserProfileStore(this);
                    storeSignUp.Name = nome;
                    storeSignUp.Email = email;
                    storeSignUp.Phone = string.IsNullOrWhiteSpace(phone) ? storeSignUp.Phone : phone;
                    storeSignUp.RegisterEmail(email);

                    Toast.MakeText(this, "Conta criada com sucesso!", ToastLength.Short)?.Show();
                    SwitchTab(AppTab.Home);
                };
            }

            var btnFooterLogIn = view.FindViewById(Resource.Id.btnFooterLogIn);
            if (btnFooterLogIn != null)
            {
                btnFooterLogIn.Click += (s, e) =>
                {
                    SwitchTab(AppTab.LoginForm);
                };
            }

            var etSignUpPassword = view.FindViewById<EditText>(Resource.Id.etSignUpPassword);
            var btnToggleSignUpPassword = view.FindViewById(Resource.Id.btnToggleSignUpPassword);
            bool isPasswordVisible = false;

            if (btnToggleSignUpPassword != null && etSignUpPassword != null)
            {
                btnToggleSignUpPassword.Click += (s, e) =>
                {
                    isPasswordVisible = !isPasswordVisible;
                    if (isPasswordVisible)
                    {
                        etSignUpPassword.InputType = Android.Text.InputTypes.ClassText | Android.Text.InputTypes.TextVariationVisiblePassword;
                    }
                    else
                    {
                        etSignUpPassword.InputType = Android.Text.InputTypes.ClassText | Android.Text.InputTypes.TextVariationPassword;
                    }
                    etSignUpPassword.SetSelection(etSignUpPassword.Text?.Length ?? 0);
                };
            }

            BindClick(view, Resource.Id.btnSocialGoogle, "Cadastro com Google");
            BindClick(view, Resource.Id.btnSocialFacebook, "Cadastro com Facebook");
            BindClick(view, Resource.Id.btnSocialFingerprint, "Cadastro com Biometria");
        }

        private void SetupCalendarEvents(View view)
        {
            _txtMonth = view.FindViewById<TextView>(Resource.Id.txtMonth);
            _txtYear = view.FindViewById<TextView>(Resource.Id.txtYear);
            _calendarDayEventsContainer = view.FindViewById<LinearLayout>(Resource.Id.calendarDayEventsContainer);

            void UpdateMonthDisplay()
            {
                if (_txtMonth != null) _txtMonth.Text = MonthNamesPt[_currentMonth.Month - 1];
                if (_txtYear != null) _txtYear.Text = _currentMonth.Year.ToString();
            }

            UpdateMonthDisplay();

            var today = DateTime.Today;
            if (_currentMonth.Year != today.Year || _currentMonth.Month != today.Month)
            {
                _currentMonth = new DateTime(today.Year, today.Month, 1);
            }
            _selectedDay = (_currentMonth.Year == today.Year && _currentMonth.Month == today.Month)
                ? today.Day : 1;
            _selectedDate = new DateTime(_currentMonth.Year, _currentMonth.Month, _selectedDay);

            RenderCalendar();

            var btnPrevMonth = view.FindViewById(Resource.Id.btnPrevMonth);
            if (btnPrevMonth != null)
            {
                btnPrevMonth.Click += (s, e) =>
                {
                    try
                    {
                        _currentMonth = _currentMonth.AddMonths(-1);
                        var today = DateTime.Today;
                        _selectedDay = (_currentMonth.Year == today.Year && _currentMonth.Month == today.Month)
                            ? today.Day : 1;
                        _selectedDate = new DateTime(_currentMonth.Year, _currentMonth.Month, _selectedDay);
                        UpdateMonthDisplay();
                        RenderCalendar();
                    }
                    catch (Exception ex)
                    {
                        Android.Util.Log.Error("Agrosens.Calendar", ex.ToString());
                    }
                };
            }

            var btnNextMonth = view.FindViewById(Resource.Id.btnNextMonth);
            if (btnNextMonth != null)
            {
                btnNextMonth.Click += (s, e) =>
                {
                    try
                    {
                        _currentMonth = _currentMonth.AddMonths(1);
                        var today = DateTime.Today;
                        _selectedDay = (_currentMonth.Year == today.Year && _currentMonth.Month == today.Month)
                            ? today.Day : 1;
                        _selectedDate = new DateTime(_currentMonth.Year, _currentMonth.Month, _selectedDay);
                        UpdateMonthDisplay();
                        RenderCalendar();
                    }
                    catch (Exception ex)
                    {
                        Android.Util.Log.Error("Agrosens.Calendar", ex.ToString());
                    }
                };
            }

            var btnAddEvent = view.FindViewById(Resource.Id.btnAddEvent);
            if (btnAddEvent != null)
            {
                btnAddEvent.Click += (s, e) => OpenAddEventBottomSheet();
            }
        }

        private void OpenAddEventBottomSheet()
        {
            var dialog = new BottomSheetDialog(this);
            var sheetView = LayoutInflater?.Inflate(Resource.Layout.bottom_sheet_add_event, null);
            if (sheetView == null) return;

            dialog.SetContentView(sheetView);

            var etEventName = sheetView.FindViewById<EditText>(Resource.Id.etEventName);
            var etNotes = sheetView.FindViewById<EditText>(Resource.Id.etNotes);
            var etDate = sheetView.FindViewById<EditText>(Resource.Id.etDate);
            var btnStartTime = sheetView.FindViewById<ImageView>(Resource.Id.btnStartTime);
            var btnEndTime = sheetView.FindViewById<ImageView>(Resource.Id.btnEndTime);
            var txtStartTime = sheetView.FindViewById<TextView>(Resource.Id.txtStartTime);
            var txtEndTime = sheetView.FindViewById<TextView>(Resource.Id.txtEndTime);
            var chipReuniao = sheetView.FindViewById<LinearLayout>(Resource.Id.chipReuniao);
            var chipManutencao = sheetView.FindViewById<LinearLayout>(Resource.Id.chipManutencao);
            var btnCreateEvent = sheetView.FindViewById<TextView>(Resource.Id.btnCreateEvent);

            _txtStartTime = txtStartTime;
            _txtEndTime = txtEndTime;

            if (_selectedDate.Year != _currentMonth.Year || _selectedDate.Month != _currentMonth.Month)
            {
                _selectedDate = new DateTime(_currentMonth.Year, _currentMonth.Month, 1);
                _selectedDay = 1;
            }
            if (etDate != null)
                etDate.Text = _selectedDate.ToString("dd/MM/yyyy");

            TimeSpan startDefault;
            if (_selectedDate.Date == DateTime.Today.Date)
            {
                var agora = DateTime.Now.TimeOfDay;
                int nextHour = agora.Hours + 1;
                if (nextHour >= 24)
                {
                    startDefault = TimeSpan.FromHours(23);
                }
                else
                {
                    startDefault = new TimeSpan(nextHour, 0, 0);
                }
            }
            else
            {
                startDefault = TimeSpan.FromHours(9);
            }
            _startTime = startDefault;
            _endTime = _startTime.Add(TimeSpan.FromHours(1));
            if (_endTime > TimeSpan.FromHours(23).Add(TimeSpan.FromMinutes(59)))
            {
                _endTime = TimeSpan.FromHours(23).Add(TimeSpan.FromMinutes(59));
            }

            if (btnStartTime != null)
            {
                btnStartTime.Tag = _startTime.ToString(@"hh\:mm");
            }
            if (btnEndTime != null)
            {
                btnEndTime.Tag = _endTime.ToString(@"hh\:mm");
            }
            AtualizarCamposDeHorario();

            // Abre sem categoria selecionada (não herda a seleção da abertura anterior)
            _selectedCategory = string.Empty;
            AtualizarSelecaoCategoria(chipReuniao, chipManutencao);

            if (etDate != null)
            {
                etDate.Focusable = false;
                etDate.Clickable = true;
                etDate.Click += (s, e) =>
                {
                    var datePicker = new DatePickerDialog(this, (sender, args) =>
                    {
                        _selectedDate = args.Date;
                        etDate.Text = _selectedDate.ToString("dd/MM/yyyy");
                    }, _selectedDate.Year, _selectedDate.Month - 1, _selectedDate.Day);
                    datePicker.Show();
                };
            }

            if (btnStartTime != null)
            {
                btnStartTime.Clickable = true;
                btnStartTime.Click += (s, e) =>
                {
                    var timePicker = new TimePickerDialog(this, (sender, args) =>
                    {
                        _startTime = new TimeSpan(args.HourOfDay, args.Minute, 0);
                        btnStartTime.Tag = _startTime.ToString(@"hh\:mm");
                        if (_endTime <= _startTime)
                        {
                            _endTime = _startTime.Add(TimeSpan.FromHours(1));
                            if (btnEndTime != null)
                                btnEndTime.Tag = _endTime.ToString(@"hh\:mm");
                        }
                        AtualizarCamposDeHorario();
                    }, _startTime.Hours, _startTime.Minutes, true);
                    timePicker.Show();
                };
            }

            if (btnEndTime != null)
            {
                btnEndTime.Clickable = true;
                btnEndTime.Click += (s, e) =>
                {
                    var timePicker = new TimePickerDialog(this, (sender, args) =>
                    {
                        _endTime = new TimeSpan(args.HourOfDay, args.Minute, 0);
                        btnEndTime.Tag = _endTime.ToString(@"hh\:mm");
                        if (_endTime <= _startTime)
                        {
                            _endTime = _startTime.Add(TimeSpan.FromHours(1));
                            btnEndTime.Tag = _endTime.ToString(@"hh\:mm");
                            Toast.MakeText(this, "O horário de término deve ser depois do início.", ToastLength.Short)?.Show();
                        }
                        AtualizarCamposDeHorario();
                    }, _endTime.Hours, _endTime.Minutes, true);
                    timePicker.Show();
                };
            }

            if (btnCreateEvent != null)
            {
                btnCreateEvent.Click += async (s, e) =>
                {
                    string name = etEventName?.Text?.Trim() ?? "";
                    if (string.IsNullOrEmpty(name))
                    {
                        Toast.MakeText(this, "Informe um título.", ToastLength.Short)?.Show();
                        return;
                    }

                    if (_endTime <= _startTime)
                    {
                        Toast.MakeText(this, "O horário de término deve ser depois do início.", ToastLength.Short)?.Show();
                        return;
                    }

                    try
                    {
                        await _dbInitTask;
                        var prefs = new PreferenceService(this);
                        var createdBy = prefs.DisplayName;

                        var eventDateUtc = DateTime.SpecifyKind(_selectedDate.Date, DateTimeKind.Unspecified);

                        var ev = new Models.CalendarEvent
                        {
                            Title = name,
                            Description = etNotes?.Text?.Trim() ?? string.Empty,
                            EventDate = eventDateUtc,
                            StartTime = _startTime,
                            EndTime = _endTime,
                            Category = _selectedCategory,
                            // Cor derivada da categoria (mesma cor da bolinha do chip)
                            ColorHex = CorDaCategoria(_selectedCategory),
                            CreatedAt = DateTime.Now,
                            CreatedBy = createdBy
                        };

                        int linhas = await DatabaseService.Instance.SaveEventAsync(ev).ConfigureAwait(false);
                        Android.Util.Log.Info("Agrosens.DB",
                            $"Salvando evento: Now={DateTime.Now:yyyy-MM-dd HH:mm:ss} Today={DateTime.Today:yyyy-MM-dd} TZ={TimeZoneInfo.Local.Id} _selectedDate={_selectedDate:yyyy-MM-dd} Kind={_selectedDate.Kind} EventDate={ev.EventDate:yyyy-MM-dd} Kind={ev.EventDate.Kind} ticks={ev.EventDate.Ticks} linhas={linhas} evento='{name}'");

                        var salvos = await DatabaseService.Instance
                            .GetEventsForMonthAsync(ev.EventDate.Year, ev.EventDate.Month)
                            .ConfigureAwait(false);
                        Android.Util.Log.Info("Agrosens.DB",
                            $"Eventos no mês {ev.EventDate:yyyy-MM} apõs salvar: {salvos.Count}");

                        RunOnUiThread(() =>
                        {
                            _ = LoadEventsAsync();
                            _ = LoadCalendarDayEventsAsync();
                            dialog.Dismiss();
                        });
                    }
                    catch (Exception ex)
                    {
                        Android.Util.Log.Error("Agrosens.DB", $"Erro ao salvar evento: {ex.ToString()}");
                        Toast.MakeText(this, "Erro ao salvar evento.", ToastLength.Short)?.Show();
                    }
                };
            }

            if (chipReuniao != null)
            {
                chipReuniao.Click += (s, e) =>
                {
                    _selectedCategory = "Reunião";
                    AtualizarSelecaoCategoria(chipReuniao, chipManutencao);
                };
            }

            if (chipManutencao != null)
            {
                chipManutencao.Click += (s, e) =>
                {
                    _selectedCategory = "Manutenção";
                    AtualizarSelecaoCategoria(chipReuniao, chipManutencao);
                };
            }

            dialog.Show();
        }

        private async Task LoadEventsAsync()
        {
            await _dbInitTask;
            try
            {
                var events = await DatabaseService.Instance
                    .GetEventsForMonthAsync(_currentMonth.Year, _currentMonth.Month)
                    .ConfigureAwait(false);
                Android.Util.Log.Info("Agrosens.Calendar",
                    $"Carregados {events.Count} evento(s) para {_currentMonth:yyyy-MM}");

                RunOnUiThread(() =>
                {
                    MarkEventDays(events);
                });
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error("Agrosens.Calendar", ex.ToString());
            }
        }

        private async Task LoadCalendarDayEventsAsync()
        {
            await _dbInitTask;
            try
            {
                var events = await DatabaseService.Instance.GetAllEventsForDayAsync(_selectedDate).ConfigureAwait(false);
                RunOnUiThread(() => BuildDayEventCards(events));
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error("Agrosens.Calendar", ex.ToString());
            }
        }

        private void BuildDayEventCards(List<CalendarEvent> events)
        {
            if (_calendarDayEventsContainer == null)
                return;

            _calendarDayEventsContainer.RemoveAllViews();

            var ordered = events.OrderBy(ev => ev.StartTime).ToList();

            if (ordered.Count == 0)
            {
                var empty = new TextView(this)
                {
                    Text = "Nenhum compromisso para este dia",
                    TextSize = 12,
                    Gravity = GravityFlags.Center
                };
                empty.SetTextColor(Android.Graphics.Color.ParseColor("#8A968D"));
                var lpEmpty = new LinearLayout.LayoutParams(
                    ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                lpEmpty.SetMargins(0, (int)(15 * Resources.DisplayMetrics.Density), 0, 0);
                empty.LayoutParameters = lpEmpty;
                _calendarDayEventsContainer.AddView(empty);
                return;
            }

            float density = Resources.DisplayMetrics.Density;
            int cardPadding = (int)(12 * density);
            int cardMargin = (int)(10 * density);

            foreach (var ev in ordered)
            {
                var card = new LinearLayout(this)
                {
                    Orientation = Orientation.Vertical,
                    LayoutParameters = new LinearLayout.LayoutParams(
                        ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent)
                };
                card.SetPadding(cardPadding, cardPadding, cardPadding, cardPadding);
                var lpCard = new LinearLayout.LayoutParams(
                    ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                lpCard.SetMargins(0, cardMargin, 0, cardMargin);
                card.LayoutParameters = lpCard;

                var row = new LinearLayout(this)
                {
                    Orientation = Orientation.Horizontal,
                    LayoutParameters = new LinearLayout.LayoutParams(
                        ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent)
                };
                row.SetGravity(GravityFlags.CenterVertical);

                var dot = new TextView(this)
                {
                    Text = "●",
                    TextSize = 10
                };
                dot.SetTextColor(CorDoEvento(ev));
                var lpDot = new LinearLayout.LayoutParams(
                    ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                lpDot.SetMargins(0, 0, (int)(6 * density), 0);
                dot.LayoutParameters = lpDot;

                var time = new TextView(this)
                {
                    Text = $"{ev.StartTime:hh\\:mm} - {ev.EndTime:hh\\:mm}",
                    TextSize = 13
                };
                time.SetTextColor(Android.Graphics.Color.ParseColor("#8A968D"));
                var lpTime = new LinearLayout.LayoutParams(
                    0, ViewGroup.LayoutParams.WrapContent, 1f);
                time.LayoutParameters = lpTime;

                var title = new TextView(this)
                {
                    Text = string.IsNullOrWhiteSpace(ev.Category) ? ev.Title : $"{ev.Title} [{ev.Category}]",
                    TextSize = 16
                };
                title.SetTypeface(Android.Graphics.Typeface.DefaultBold, Android.Graphics.TypefaceStyle.Bold);
                title.SetTextColor(Android.Graphics.Color.ParseColor("#1B3A24"));

                var desc = new TextView(this)
                {
                    Text = string.IsNullOrWhiteSpace(ev.Description) ? "Sem descrição." : ev.Description,
                    TextSize = 13
                };
                desc.SetTextColor(Android.Graphics.Color.ParseColor("#8A968D"));

                row.AddView(dot);
                row.AddView(time);
                card.AddView(row);
                card.AddView(title);
                card.AddView(desc);

                if (ev.Status == CalendarEvent.StatusFeito)
                {
                    var status = new TextView(this)
                    {
                        Text = "Feito",
                        TextSize = 11
                    };
                    status.SetTextColor(Android.Graphics.Color.ParseColor("#2E7D32"));
                    var lpStatus = new LinearLayout.LayoutParams(
                        ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                    lpStatus.SetMargins(0, (int)(4 * density), 0, 0);
                    status.LayoutParameters = lpStatus;
                    card.AddView(status);
                }
                else if (ev.Status == CalendarEvent.StatusCancelado)
                {
                    var status = new TextView(this)
                    {
                        Text = "Cancelado",
                        TextSize = 11
                    };
                    status.SetTextColor(Android.Graphics.Color.ParseColor("#757575"));
                    var lpStatus = new LinearLayout.LayoutParams(
                        ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                    lpStatus.SetMargins(0, (int)(4 * density), 0, 0);
                    status.LayoutParameters = lpStatus;
                    card.AddView(status);
                }

                _calendarDayEventsContainer.AddView(card);
            }
        }

        private void RenderCalendar()
        {
            if (_contentContainer == null || LayoutInflater == null)
                return;

            _dayCells.Clear();

            var monthName = MonthNamesPt[_currentMonth.Month - 1];
            if (_txtMonth != null) _txtMonth.Text = monthName;
            if (_txtYear != null) _txtYear.Text = _currentMonth.Year.ToString();

            var calendarGrid = _contentContainer.FindViewById<LinearLayout>(Resource.Id.calendarGrid);
            if (calendarGrid == null) return;
            calendarGrid.RemoveAllViews();

            int year = _currentMonth.Year;
            int month = _currentMonth.Month;
            int daysInMonth = DateTime.DaysInMonth(year, month);
            DateTime firstDay = new DateTime(year, month, 1);

            int startCol = ((int)firstDay.DayOfWeek + 6) % 7;

            DateTime today = DateTime.Today;
            bool isCurrentMonthView = (year == today.Year && month == today.Month);

            float density = Resources.DisplayMetrics.Density;
            int cellPadding = (int)(6 * density);
            int cellSize = (int)(44 * density);

            int day = 1;
            int nextMonthDay = 1;
            for (int row = 0; day <= daysInMonth; row++)
            {
                var rowLayout = new LinearLayout(this)
                {
                    Orientation = Orientation.Horizontal,
                    LayoutParameters = new LinearLayout.LayoutParams(
                        ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent)
                };

                for (int c = 0; c < 7; c++)
                {
                    var cell = new LinearLayout(this)
                    {
                        Orientation = Orientation.Vertical,
                        LayoutParameters = new LinearLayout.LayoutParams(
                            0, cellSize, 1f),
                        Clickable = true,
                        Focusable = true
                    };
                    cell.SetGravity(GravityFlags.Center);
                    cell.SetPadding((int)(2 * density), cellPadding, (int)(2 * density), cellPadding);

                    DateTime cellDate;
                    bool isRealDay;

                    if (row == 0 && c < startCol)
                    {
                        cellDate = firstDay.AddDays(c - startCol);
                        isRealDay = false;
                    }
                    else if (day > daysInMonth)
                    {
                        cellDate = firstDay.AddMonths(1).AddDays(nextMonthDay - 1);
                        nextMonthDay++;
                        isRealDay = false;
                    }
                    else
                    {
                        cellDate = new DateTime(year, month, day);
                        isRealDay = true;
                        day++;
                    }

                    _cellDates[cell] = cellDate;

                    if (!isRealDay)
                    {
                        var empty = new TextView(this)
                        {
                            Text = "",
                            LayoutParameters = new LinearLayout.LayoutParams(cellSize, cellSize)
                        };
                        cell.AddView(empty);
                        cell.Clickable = false;
                        cell.Focusable = false;
                    }
                    else
                    {
                        int dayNumber = cellDate.Day;
                        var dayText = new TextView(this)
                        {
                            Text = dayNumber.ToString(),
                            LayoutParameters = new LinearLayout.LayoutParams(
                                ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent),
                            Gravity = GravityFlags.Center
                        };
                        dayText.SetTextSize(Android.Util.ComplexUnitType.Sp, 17);

                        AplicarEstiloCelula(cell, dayText, cellDate);

                        cell.AddView(dayText);
                        _dayCells[dayNumber] = cell;

                        cell.Click += async (s, e) =>
                        {
                            try
                            {
                                var clickedDate = _cellDates[cell];
                                _selectedDate = clickedDate;
                                _selectedDay = clickedDate.Day;

                                if (clickedDate.Year != _currentMonth.Year || clickedDate.Month != _currentMonth.Month)
                                {
                                    _currentMonth = new DateTime(clickedDate.Year, clickedDate.Month, 1);
                                    if (_txtMonth != null) _txtMonth.Text = MonthNamesPt[_currentMonth.Month - 1];
                                    if (_txtYear != null) _txtYear.Text = _currentMonth.Year.ToString();
                                }

                                RenderCalendar();
                                await LoadEventsAsync();
                                await LoadCalendarDayEventsAsync();
                            }
                            catch (Exception ex)
                            {
                                Android.Util.Log.Error("Agrosens.Calendar", ex.ToString());
                            }
                        };
                    }

                    rowLayout.AddView(cell);
                }

                calendarGrid.AddView(rowLayout);
            }

            _ = LoadEventsAsync();
            _ = LoadCalendarDayEventsAsync();
        }

        private void MarkEventDays(List<CalendarEvent> events)
        {
            float density = Resources.DisplayMetrics.Density;
            foreach (var cell in _dayCells.Values)
            {
                for (int i = cell.ChildCount - 1; i >= 0; i--)
                {
                    var child = cell.GetChildAt(i);
                    if (child is LinearLayout dotContainer)
                    {
                        cell.RemoveViewAt(i);
                    }
                }
            }

            foreach (var ev in events)
            {
                int day = ev.EventDate.Day;
                if (_dayCells.TryGetValue(day, out var cell))
                {
                    var dotContainer = new LinearLayout(this)
                    {
                        Orientation = Orientation.Horizontal,
                        LayoutParameters = new LinearLayout.LayoutParams(
                            ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent)
                    };
                    dotContainer.SetGravity(GravityFlags.Center);
                    var dot = new ImageView(this)
                    {
                        LayoutParameters = new LinearLayout.LayoutParams(
                            (int)(8 * density), (int)(8 * density))
                    };
                    dot.SetImageResource(Resource.Drawable.bg_dot_event);
                    dot.SetColorFilter(CorDoEvento(ev));
                    dotContainer.AddView(dot);
                    cell.AddView(dotContainer);
                }
            }
        }

        private void AplicarEstiloCelula(LinearLayout cell, TextView dayText, DateTime cellDate)
        {
            DateTime hoje = DateTime.Today;
            bool selecionado = cellDate.Date == _selectedDate.Date;
            bool eHoje = cellDate.Date == hoje.Date;

            if (selecionado)
            {
                dayText.SetTextColor(Android.Graphics.Color.White);
                dayText.SetTypeface(dayText.Typeface, Android.Graphics.TypefaceStyle.Bold);
                cell.SetBackgroundResource(Resource.Drawable.bg_day_selected_circle);
            }
            else if (eHoje)
            {
                dayText.SetTextColor(Android.Graphics.Color.ParseColor("#2E7D32"));
                dayText.SetTypeface(dayText.Typeface, Android.Graphics.TypefaceStyle.Bold);
                cell.SetBackgroundResource(Resource.Drawable.bg_badge_green_border);
            }
            else
            {
                dayText.SetTextColor(Android.Graphics.Color.ParseColor("#1B3A24"));
                cell.SetBackgroundResource(0);
            }
        }

        private void AtualizarCamposDeHorario()
        {
            if (_txtStartTime != null) _txtStartTime.Text = _startTime.ToString(@"hh\:mm");
            if (_txtEndTime != null) _txtEndTime.Text = _endTime.ToString(@"hh\:mm");
        }

        private void AtualizarSelecaoCategoria(View? chipReuniao, View? chipManutencao)
        {
            AplicarFundoCategoria(chipReuniao, "Reunião", "#F3EEFF", "#8E64C7");
            AplicarFundoCategoria(chipManutencao, "Manutenção", "#E8F5E9", "#2E7D32");
        }

        private void AplicarFundoCategoria(View? chip, string categoria, string corFundo, string corBorda)
        {
            if (chip == null) return;
            var gd = new GradientDrawable();
            gd.SetColor(Android.Graphics.Color.ParseColor(corFundo));
            gd.SetCornerRadius(18);
            if (_selectedCategory == categoria)
            {
                gd.SetStroke(2, Android.Graphics.Color.ParseColor(corBorda));
            }
            else
            {
                gd.SetStroke(0, Android.Graphics.Color.Transparent);
            }
            chip.Background = gd;
        }

        // Cor do evento derivada da categoria: a mesma cor da bolinha do chip
        private static string CorDaCategoria(string categoria) => categoria switch
        {
            "Reunião" => "#8E64C7",
            "Manutenção" => "#2E7D32",
            _ => "#1B3A24"
        };

        // Cor segura para exibição: eventos sem categoria ou antigos (ColorHex nulo/inválido) usam a cor neutra
        private static Android.Graphics.Color CorDoEvento(CalendarEvent ev)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(ev.ColorHex))
                    return Android.Graphics.Color.ParseColor(ev.ColorHex);
            }
            catch (Exception)
            {
            }
            return Android.Graphics.Color.ParseColor("#1B3A24");
        }

        private void SetupPlaceholder(View view, string icon, string title, string subtext)
        {
            var txtIcon = view.FindViewById<TextView>(Resource.Id.txtPlaceholderIcon);
            var txtTitle = view.FindViewById<TextView>(Resource.Id.txtPlaceholderTitle);
            var txtSub = view.FindViewById<TextView>(Resource.Id.txtPlaceholderSub);

            if (txtIcon != null) txtIcon.Text = icon;
            if (txtTitle != null) txtTitle.Text = title;
            if (txtSub != null) txtSub.Text = subtext;
        }

        private void BindClick(View parent, int resId, string message)
        {
            var target = parent.FindViewById(resId);
            if (target != null)
            {
                target.Click += (s, e) => Toast.MakeText(this, message, ToastLength.Short)?.Show();
            }
        }

        private void AnimateAndRemoveCard(LinearLayout card, ViewGroup? container, Action? onRemoved = null)
        {
            if (container == null) return;
            if (container.IndexOfChild(card) < 0) return;

            card.Animate().Alpha(0f).SetDuration(200).Start();

            new Handler(Looper.MainLooper).PostDelayed(() =>
            {
                try
                {
                    if (container != null && container.IndexOfChild(card) >= 0)
                    {
                        container.RemoveView(card);
                    }
                    onRemoved?.Invoke();
                }
                catch (Exception ex)
                {
                    Android.Util.Log.Error("Agrosens.Home", ex.ToString());
                }
            }, 200);
        }

        private class MapWebViewClient : WebViewClient
        {
            private readonly WebView _webView;
            private readonly ProgressBar _loading;
            private readonly TextView _errorView;
            private readonly Action? _onFinished;

            public MapWebViewClient(WebView webView, ProgressBar loading, TextView errorView, Action? onFinished = null)
            {
                _webView = webView;
                _loading = loading;
                _errorView = errorView;
                _onFinished = onFinished;
            }

            public override void OnPageFinished(WebView? view, string url)
            {
                base.OnPageFinished(view, url);
                if (_loading != null) _loading.Visibility = ViewStates.Gone;
                if (_webView != null) _webView.Visibility = ViewStates.Visible;
                _onFinished?.Invoke();
            }

            // Só falha de página principal derruba o mapa; erro de um tile/telemetria do mapa é ignorado
            public override void OnReceivedError(WebView? view, IWebResourceRequest? request, WebResourceError? error)
            {
                base.OnReceivedError(view, request, error);
                Android.Util.Log.Error("Agrosens.Map", $"Erro de rede no mapa: {error?.Description} [{request?.Url}]");
                if (request == null || !request.IsForMainFrame) return;
                if (_loading != null) _loading.Visibility = ViewStates.Gone;
                if (_webView != null) _webView.Visibility = ViewStates.Gone;
                if (_errorView != null) _errorView.Visibility = ViewStates.Visible;
            }
        }

        private sealed class StatusBarInsetsListener : Java.Lang.Object, View.IOnApplyWindowInsetsListener
        {
            private readonly View _spacer;
            public StatusBarInsetsListener(View spacer) => _spacer = spacer;

            public WindowInsets OnApplyWindowInsets(View v, WindowInsets insets)
            {
                try
                {
                    int top = insets.SystemWindowInsetTop; // altura da barra de status (0 se o app não desenha atrás dela)
                    var lp = _spacer.LayoutParameters;
                    if (lp != null && lp.Height != top)
                    {
                        lp.Height = top;
                        _spacer.LayoutParameters = lp;
                    }
                }
                catch (Exception ex)
                {
                    Android.Util.Log.Warn("Agrosens.Insets", ex.Message);
                }
                return insets;
            }
        }

        private class LayoutListener : Java.Lang.Object, ViewTreeObserver.IOnGlobalLayoutListener
        {
            private readonly Action _onLayout;
            public LayoutListener(Action onLayout) => _onLayout = onLayout;
            public void OnGlobalLayout() => _onLayout?.Invoke();
        }

        private class MapWebChromeClient : WebChromeClient
        {
            public override bool OnConsoleMessage(ConsoleMessage consoleMessage)
            {
                Android.Util.Log.Info("AgroSensMap.JS", $"[{consoleMessage.SourceId}:{consoleMessage.LineNumber}] {consoleMessage.Message}");
                return base.OnConsoleMessage(consoleMessage);
            }
        }

        // ---------- Mapa: menu lateral "Mais opções" e seleção da garagem ----------

        private View? _mapMenuOverlay;
        private View? _mapMenuPanel;
        private TextView? _mapGaragemText;
        private bool _mapGaragem = false;
        private string _garageJson = "[]";

        private void SetupMapMenu(View mapaView)
        {
            // Sempre que a aba Mapa abre, volta ao estado normal
            _mapSelecao = false;
            _mapGaragem = false;

            _mapMenuOverlay = mapaView.FindViewById(Resource.Id.mapMenuOverlay);
            _mapMenuPanel = mapaView.FindViewById(Resource.Id.mapMenuPanel);
            _mapGaragemText = mapaView.FindViewById<TextView>(Resource.Id.mapGaragemText);

            mapaView.FindViewById(Resource.Id.mapMenuScrim)!.Click += (s, e) => FecharMenuMapa();
            mapaView.FindViewById(Resource.Id.mapMenuClose)!.Click += (s, e) => FecharMenuMapa();

            mapaView.FindViewById(Resource.Id.mapMenuAlterar)!.Click += (s, e) =>
            {
                FecharMenuMapa();
                EntrarModoSelecao();
            };
            mapaView.FindViewById(Resource.Id.mapMenuIrFazenda)!.Click += (s, e) =>
            {
                FecharMenuMapa();
                ApplySavedView();
            };
            mapaView.FindViewById(Resource.Id.mapMenuGaragem)!.Click += (s, e) =>
            {
                FecharMenuMapa();
                EntrarModoGaragem();
            };
        }

        private void AbrirMenuMapa()
        {
            if (_mapMenuOverlay == null || _mapMenuPanel == null) return;
            var scrim = _mapMenuOverlay.FindViewById(Resource.Id.mapMenuScrim);
            _mapMenuOverlay.Visibility = ViewStates.Visible;
            _mapMenuPanel.TranslationX = -Dp(290);
            _mapMenuPanel.Animate()!.TranslationX(0).SetDuration(220).Start();
            if (scrim != null)
            {
                scrim.Alpha = 0f;
                scrim.Animate()!.Alpha(1f).SetDuration(220).Start();
            }
        }

        private void FecharMenuMapa()
        {
            if (_mapMenuOverlay == null || _mapMenuPanel == null) return;
            var overlay = _mapMenuOverlay;
            var scrim = overlay.FindViewById(Resource.Id.mapMenuScrim);
            scrim?.Animate()!.Alpha(0f).SetDuration(180).Start();
            _mapMenuPanel.Animate()!
                .TranslationX(-Dp(290))
                .SetDuration(180)
                .WithEndAction(new Java.Lang.Runnable(() => overlay.Visibility = ViewStates.Gone))
                .Start();
        }

        private AppCompatButton NovoBotaoMapa(string texto, bool preenchido, int iconRes = 0, int iconCor = unchecked((int)0xFFFFFFFF))
        {
            var b = new AppCompatButton(this) { Text = texto };
            b.SetBackgroundResource(preenchido ? Resource.Drawable.bg_btn_green_rounded : Resource.Drawable.bg_btn_outline);
            b.SetTextColor(preenchido ? Android.Graphics.Color.White : Android.Graphics.Color.ParseColor("#616161"));
            b.SetTextSize(ComplexUnitType.Sp, 13);
            b.SetMaxLines(1);
            b.Ellipsize = TextUtils.TruncateAt.End;
            b.SetAllCaps(false);
            b.Gravity = GravityFlags.Center;
            if (iconRes != 0)
            {
                var d = Resources.GetDrawable(iconRes)?.Mutate();
                d?.SetColorFilter(new Android.Graphics.Color(iconCor), PorterDuff.Mode.SrcIn);
                d?.SetBounds(0, 0, Dp(18), Dp(18));
                b.SetCompoundDrawables(d, null, null, null);
                b.CompoundDrawablePadding = Dp(8);
            }
            return b;
        }

        private void CriarBotoesCabecalhoMapa()
        {
            if (_mapHeaderActions == null) return;
            _mapHeaderActions.RemoveAllViews();

            int btnHeight = Dp(40);
            LinearLayout.LayoutParams Lp(float peso, int ini = 0, int fim = 0)
            {
                var lp = new LinearLayout.LayoutParams(0, btnHeight, peso);
                lp.SetMargins(ini, 0, fim, 0);
                return lp;
            }

            if (_mapGaragem)
            {
                // Modo "Selecionar garagem": Salvar área | Desfazer | Cancelar
                var salvar = NovoBotaoMapa("Salvar área", true, Resource.Drawable.ic_car_green);
                salvar.LayoutParameters = Lp(1.3f, 0, Dp(6));
                salvar.Click += (s, e) => SalvarGaragem();

                var desfazer = NovoBotaoMapa("Desfazer", false);
                desfazer.LayoutParameters = Lp(1f, Dp(6), Dp(6));
                desfazer.Click += (s, e) => _mapWebView?.EvaluateJavascript("if(window.agroGarageUndo) window.agroGarageUndo();", null);

                var cancelar = NovoBotaoMapa("Cancelar", false);
                cancelar.LayoutParameters = Lp(1f, Dp(6), 0);
                cancelar.Click += (s, e) => SairModoGaragem(restaurar: true);

                _mapHeaderActions.AddView(salvar);
                _mapHeaderActions.AddView(desfazer);
                _mapHeaderActions.AddView(cancelar);
                return;
            }

            if (_mapSelecao)
            {
                // Modo "Alterar localização": Confirmar | Cancelar
                _mapBtnSalvar = NovoBotaoMapa("Confirmar", true, Resource.Drawable.ic_farm_pin);
                _mapBtnSalvar.LayoutParameters = Lp(1f, 0, Dp(8));
                _mapBtnSalvar.Click += MapBtnSalvar_Click;

                _mapBtnCancelar = NovoBotaoMapa("Cancelar", false);
                _mapBtnCancelar.LayoutParameters = Lp(1f, Dp(8), 0);
                _mapBtnCancelar.Click += (s, e) => SairModoSelecao();

                _mapHeaderActions.AddView(_mapBtnSalvar);
                _mapHeaderActions.AddView(_mapBtnCancelar);
                return;
            }

            if (!_hasFarmLocation)
            {
                // Primeira vez: só "Salvar localização"
                _mapBtnSalvar = NovoBotaoMapa("Salvar localização", true, Resource.Drawable.ic_farm_pin);
                _mapBtnSalvar.LayoutParameters = Lp(1f);
                _mapBtnSalvar.Click += MapBtnSalvar_Click;
                _mapHeaderActions.AddView(_mapBtnSalvar);
                return;
            }

            // Com fazenda salva: um único botão "Mais opções" (menu lateral)
            var maisOpcoes = NovoBotaoMapa("Mais opções", true, Resource.Drawable.ic_menu_hamburger);
            maisOpcoes.LayoutParameters = Lp(1f);
            maisOpcoes.Click += (s, e) => AbrirMenuMapa();
            _mapHeaderActions.AddView(maisOpcoes);
        }

        // ---------- Selecionar garagem (área marcada no mapa) ----------

        private static int ContarPontos(string? json)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(json)) return 0;
                using var doc = System.Text.Json.JsonDocument.Parse(json);
                if (doc.RootElement.ValueKind != System.Text.Json.JsonValueKind.Array) return 0;
                int n = 0;
                foreach (var p in doc.RootElement.EnumerateArray())
                {
                    if (p.ValueKind == System.Text.Json.JsonValueKind.Array && p.GetArrayLength() == 2 &&
                        p[0].ValueKind == System.Text.Json.JsonValueKind.Number &&
                        p[1].ValueKind == System.Text.Json.JsonValueKind.Number)
                        n++;
                    else
                        return -1; // formato inválido
                }
                return n;
            }
            catch
            {
                return -1;
            }
        }

        private void AtualizarTextoGaragem()
        {
            if (_mapGaragemText == null) return;
            int n = ContarPontos(_garageJson);
            _mapGaragemText.Text = n >= 3 ? $"Garagem: definida ({n} pontos)" : "Garagem: não definida";
        }

        private Task<string?> EvalJsAsync(string js)
        {
            var tcs = new TaskCompletionSource<string?>();
            if (_mapWebView == null)
            {
                tcs.SetResult(null);
                return tcs.Task;
            }
            _mapWebView.EvaluateJavascript(js, new MapValueCallback(v => tcs.TrySetResult(v?.ToString())));
            return tcs.Task;
        }

        private void EntrarModoGaragem()
        {
            _mapGaragem = true;
            _mapSelecao = false;
            if (_mapCenterPin != null) _mapCenterPin.Visibility = ViewStates.Gone;
            if (_mapTipBanner != null)
            {
                _mapTipBanner.Text = "Toque no mapa para marcar os cantos da área da garagem. Arraste os pontos para ajustar.";
                _mapTipBanner.Visibility = ViewStates.Visible;
            }
            CriarBotoesCabecalhoMapa();
            ApplySavedView(); // vai para a fazenda
            _mapWebView?.EvaluateJavascript(
                "if(window.agroGarageEdit){window.agroGarageEdit(true);window.agroGarageFit();}", null);
        }

        private void SairModoGaragem(bool restaurar)
        {
            _mapGaragem = false;
            if (_mapTipBanner != null) _mapTipBanner.Visibility = ViewStates.Gone;
            if (restaurar)
                _mapWebView?.EvaluateJavascript($"if(window.agroGarageLoad) window.agroGarageLoad({_garageJson});", null);
            _mapWebView?.EvaluateJavascript("if(window.agroGarageEdit) window.agroGarageEdit(false);", null);
            CriarBotoesCabecalhoMapa();
        }

        private async void SalvarGaragem()
        {
            try
            {
                var bruto = await EvalJsAsync("window.agroGarageGet ? window.agroGarageGet() : null");
                var json = (bruto ?? "null").Trim();
                if (json == "null") json = "[]";

                int n = ContarPontos(json);
                if (n < 0)
                {
                    Toast.MakeText(this, "Não foi possível ler a área marcada.", ToastLength.Short)?.Show();
                    return;
                }

                var prefs = GetSharedPreferences("agrosens_map", FileCreationMode.Private);

                if (n == 0)
                {
                    if (ContarPontos(_garageJson) >= 3)
                    {
                        // Sem pontos + já havia garagem salva = remover a área
                        prefs?.Edit()?.Remove(PrefGarage)?.Apply();
                        _garageJson = "[]";
                        AtualizarTextoGaragem();
                        SairModoGaragem(restaurar: false);
                        Toast.MakeText(this, "Área da garagem removida", ToastLength.Short)?.Show();
                    }
                    else
                    {
                        Toast.MakeText(this, "Marque pelo menos 3 pontos no mapa.", ToastLength.Short)?.Show();
                    }
                    return;
                }

                if (n < 3)
                {
                    Toast.MakeText(this, "Marque pelo menos 3 pontos para formar a área.", ToastLength.Short)?.Show();
                    return;
                }

                prefs?.Edit()?.PutString(PrefGarage, json)?.Apply();
                _garageJson = json;
                AtualizarTextoGaragem();
                SairModoGaragem(restaurar: false);
                Toast.MakeText(this, "Área da garagem salva", ToastLength.Short)?.Show();
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error("AgroSensMap", "Falha ao salvar garagem: " + ex);
                Toast.MakeText(this, "Não foi possível salvar a garagem.", ToastLength.Short)?.Show();
            }
        }

        private void EntrarModoSelecao()
        {
            _mapSelecao = true;
            if (_mapCenterPin != null)
            {
                _mapCenterPin.Visibility = ViewStates.Visible;
                _mapCenterPin.Clickable = false;
                _mapCenterPin.Focusable = false;
            }
            if (_mapTipBanner != null)
            {
                _mapTipBanner.Text = "Arraste o mapa até a sua fazenda e toque em Confirmar";
                _mapTipBanner.Visibility = ViewStates.Visible;
            }
            CriarBotoesCabecalhoMapa();
            InvalidateMapSizeNow();
        }

        private void SairModoSelecao()
        {
            _mapSelecao = false;
            if (_mapCenterPin != null) _mapCenterPin.Visibility = ViewStates.Gone;
            if (_mapTipBanner != null) _mapTipBanner.Visibility = ViewStates.Gone;
            CriarBotoesCabecalhoMapa();
            InvalidateMapSizeNow();
        }

        private void InvalidateMapSizeNow()
        {
            if (_mapWebView == null) return;
            try
            {
                _mapWebView.EvaluateJavascript("if(window.agroResize) window.agroResize();", null);
            }
            catch { }
        }

        private void MapBtnSalvar_Click(object? sender, EventArgs e)
        {
            if (!_mapSelecao)
                EntrarModoSelecao();
            else
                ConfirmarLocalizacao();
        }

        private async void ConfirmarLocalizacao()
        {
            try
            {
                if (_mapWebView == null)
                {
                    FalhaSalvar("WebView do mapa indisponível");
                    return;
                }

                var tcs = new TaskCompletionSource<string?>();
                _mapWebView.EvaluateJavascript(
                    "window.agroGetView();",
                    new MapValueCallback(v => tcs.SetResult(v?.ToString())));

                var json = await tcs.Task;
                if (string.IsNullOrWhiteSpace(json))
                {
                    FalhaSalvar("Centro do mapa não obtido");
                    return;
                }

                var s = json.Trim();
                if (s.Length >= 2 && s.StartsWith("\"") && s.EndsWith("\""))
                    s = s.Substring(1, s.Length - 2);
                s = s.Replace("\\\"", "\"").Replace("\\\\", "\\");

                if (!TryParseMapJson(s, out var lat, out var lng, out var zoom))
                {
                    FalhaSalvar("Não foi possível ler o centro do mapa");
                    return;
                }

                if (lat < -90 || lat > 90 || lng < -180 || lng > 180)
                {
                    FalhaSalvar("Coordenadas fora do intervalo válido");
                    return;
                }

                var prefs = GetSharedPreferences("agrosens_map", FileCreationMode.Private);
                bool jaTinha = prefs.Contains(PrefFarmLat);
                if (jaTinha)
                {
                    var dlg = new AlertDialog.Builder(this);
                    dlg.SetTitle("Substituir a localização salva?");
                    dlg.SetMessage("Já existe uma localização da fazenda salva. Deseja sobrescrever?");
                    dlg.SetPositiveButton("Salvar", (_s, _a) => SalvarLocalizacao(lat, lng, zoom));
                    dlg.SetNegativeButton("Cancelar", (_s, _a) => { });
                    dlg.Show();
                }
                else
                {
                    SalvarLocalizacao(lat, lng, zoom);
                }
            }
            catch (Exception ex)
            {
                FalhaSalvar(ex.Message);
            }
        }

        private void FalhaSalvar(string motivo)
        {
            try
            {
                Toast.MakeText(this, "Não foi possível salvar a localização", ToastLength.Short)?.Show();
            }
            catch { }
            Android.Util.Log.Error("AgroSensMap", "Falha ao salvar localização: " + motivo);
        }

        private static bool TryParseMapJson(string s, out double lat, out double lng, out int zoom)
        {
            lat = 0;
            lng = 0;
            zoom = 0;
            try
            {
                var mLat = Regex.Match(s, @"""lat""\s*:\s*([-+]?\d+(?:\.\d+)?(?:[eE][-+]?\d+)?)");
                var mLng = Regex.Match(s, @"""lng""\s*:\s*([-+]?\d+(?:\.\d+)?(?:[eE][-+]?\d+)?)");
                var mZoom = Regex.Match(s, @"""zoom""\s*:\s*(\d+)");
                if (!mLat.Success || !mLng.Success || !mZoom.Success)
                    return false;
                lat = double.Parse(mLat.Groups[1].Value, CultureInfo.InvariantCulture);
                lng = double.Parse(mLng.Groups[1].Value, CultureInfo.InvariantCulture);
                zoom = int.Parse(mZoom.Groups[1].Value, CultureInfo.InvariantCulture);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void SalvarLocalizacao(double lat, double lng, int zoom)
        {
            try
            {
                var prefs = GetSharedPreferences("agrosens_map", FileCreationMode.Private);
                var ed = prefs.Edit();
                ed.PutString(PrefFarmLat, lat.ToString(CultureInfo.InvariantCulture));
                ed.PutString(PrefFarmLng, lng.ToString(CultureInfo.InvariantCulture));
                ed.PutString(PrefFarmZoom, zoom.ToString(CultureInfo.InvariantCulture));
                ed.Apply();

                _hasFarmLocation = true;
                _farmLat = lat;
                _farmLng = lng;
                _farmZoom = zoom;

                RunOnUiThread(() =>
                {
                    if (_mapTipBanner != null) _mapTipBanner.Visibility = ViewStates.Gone;
                    SairModoSelecao();
                    if (_mapLocalizacaoSalvaText != null)
                        _mapLocalizacaoSalvaText.Text =
                            $"Localização salva: {lat.ToString("F4", CultureInfo.InvariantCulture)}, {lng.ToString("F4", CultureInfo.InvariantCulture)}";
                    Toast.MakeText(this, "Localização da fazenda salva", ToastLength.Short)?.Show();
                });
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error("AgroSensMap", ex.ToString());
            }
        }

        private void ApplySavedView()
        {
            if (!_hasFarmLocation || _mapWebView == null) return;
            try
            {
                _mapWebView.EvaluateJavascript(
                    $"if(window.agroSetView) window.agroSetView({_farmLat.ToString(CultureInfo.InvariantCulture)},{_farmLng.ToString(CultureInfo.InvariantCulture)},{_farmZoom})", null);
            }
            catch { }
        }

        private sealed class MapValueCallback : Java.Lang.Object, Android.Webkit.IValueCallback
        {
            private readonly Action<Java.Lang.Object?> _callback;
            public MapValueCallback(Action<Java.Lang.Object?> callback) => _callback = callback;
            public void OnReceiveValue(Java.Lang.Object? value) => _callback?.Invoke(value);
        }

        protected override void OnResume()
        {
            try
            {
                base.OnResume();
                if (_currentTab == AppTab.Home)
                {
                    _homeSelectedDay = DateTime.Today;
                    _homeWeekStart = StartOfWeek(_homeSelectedDay);
                    BuildHomeWeekDays();
                    _ = LoadHomeWeatherAsync();
                    _ = LoadHomeEventsAsync();

                    if (_homeTxtUserName != null) _homeTxtUserName.Text = NomeUsuarioLogado();
                }
                else if (_currentTab == AppTab.Calendario)
                {
                    _ = LoadCalendarDayEventsAsync();
                }
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error("AgroSens.OnResume", ex.ToString());
            }
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
        }

        public override void OnLowMemory()
        {
            base.OnLowMemory();
        }
    }
}
