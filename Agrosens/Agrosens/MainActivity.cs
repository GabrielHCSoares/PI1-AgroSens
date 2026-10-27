using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.BottomSheet;
using System;
using System.Linq;
using System.Threading.Tasks;

using Agrosens.Models;
using Agrosens.Services;

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
        EditProfile
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

        private AppTab _currentTab = AppTab.Home;
        private bool _isLoggedOut = false;

        private LinearLayout? _taskListContainer;
        private TextView? _txtEmptyTasks;
        private TextView? _txtMonth;
        private TextView? _txtYear;

        private LinearLayout? _calendarDayEventsContainer;

        private LinearLayout? _homeWeekDaysContainer;
        private TextView? _homeDateTitle;
        private LinearLayout? _homeEventsContainer;
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

        private readonly Dictionary<int, LinearLayout> _dayCells = new();

        private readonly Dictionary<LinearLayout, DateTime> _cellDates = new();

        private TextView? _txtStartTime;
        private TextView? _txtEndTime;

        private RelativeLayout? _rootLayout;
        private RelativeLayout? _bottomBarContainer;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            _rootLayout = FindViewById<RelativeLayout>(Resource.Id.rootLayout);
            _contentContainer = FindViewById<FrameLayout>(Resource.Id.contentContainer);
            _bottomBarContainer = FindViewById<RelativeLayout>(Resource.Id.bottomBarContainer);
            _navCalendario = FindViewById<LinearLayout>(Resource.Id.navCalendario);
            _navGeral = FindViewById<LinearLayout>(Resource.Id.navGeral);
            _navMapa = FindViewById<LinearLayout>(Resource.Id.navMapa);
            _navPerfil = FindViewById<LinearLayout>(Resource.Id.navPerfil);
            _btnCentral = FindViewById<FrameLayout>(Resource.Id.btnCentral);
            _centralCircleBg = FindViewById<FrameLayout>(Resource.Id.centralCircleBg);

            _taskListContainer = FindViewById<LinearLayout>(Resource.Id.taskListContainer);
            _txtEmptyTasks = FindViewById<TextView>(Resource.Id.txtEmptyTasks);

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

            var flags = GetSharedPreferences("agrosens_flags", FileCreationMode.Private);
            bool demoSeedado = flags.GetBoolean("demo_user_seeded", false);
            if (!demoSeedado)
            {
                var prefsDemo = new PreferenceService(this);
                prefsDemo.SaveSession("Pedro", "pedro@demo.com");
                var edFlags = flags.Edit();
                edFlags.PutBoolean("demo_user_seeded", true);
                edFlags.Apply();
            }

            var prefsBoot = new PreferenceService(this);
            SwitchTab(prefsBoot.IsLoggedIn ? AppTab.Home : AppTab.AuthWelcome);
        }

        private void SwitchTab(AppTab tab)
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
                    _navMapa?.SetBackgroundResource(Resource.Drawable.bg_tab_active_pill);
                    _centralCircleBg?.SetBackgroundResource(Resource.Drawable.bg_circle_black);
                    View mapaView = LayoutInflater.Inflate(Resource.Layout.fragment_placeholder, _contentContainer, false)!;
                    _contentContainer.AddView(mapaView);
                    SetupPlaceholder(mapaView, "📍", "Mapeamento da Fazenda", "Visualização dos talhões, piquetes e localização dos sensores.");
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

                case AppTab.EditProfile:
                    _navPerfil?.SetBackgroundResource(Resource.Drawable.bg_tab_active_pill);
                    _centralCircleBg?.SetBackgroundResource(Resource.Drawable.bg_circle_black);
                    View editProfileView = LayoutInflater.Inflate(Resource.Layout.fragment_edit_profile, _contentContainer, false)!;
                    _contentContainer.AddView(editProfileView);
                    SetupEditProfileEvents(editProfileView);
                    break;
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

                _homeWeekDaysContainer = view.FindViewById<LinearLayout>(Resource.Id.homeWeekDaysContainer);
                _homeDateTitle = view.FindViewById<TextView>(Resource.Id.homeDateTitle);
                _homeEventsContainer = view.FindViewById<LinearLayout>(Resource.Id.homeEventsContainer);

                if (_homeSelectedDay == default(DateTime))
                    _homeSelectedDay = DateTime.Today;

                if (_isLoggedOut)
                {
                    imgUser?.SetImageResource(Resource.Drawable.sem_usuario);
                    if (txtUserName != null) txtUserName.Text = "Usuario";
                }
                else
                {
                    imgUser?.SetImageResource(Resource.Drawable.user_photo);
                    var prefsHome = new PreferenceService(this);
                    var nomeHome = prefsHome.DisplayName;
                    if (txtUserName != null) txtUserName.Text = nomeHome;
                }

                BuildHomeWeekDays();
                await LoadHomeEventsAsync();
                await LoadHomeTasksAsync();
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error("Agrosens.Home", ex.ToString());
            }
        }

        private async Task LoadHomeTasksAsync()
        {
            if (_taskListContainer == null || _txtEmptyTasks == null)
                return;

            await _dbInitTask;

            var tasks = await DatabaseService.Instance.GetPendingTasksAsync();

            _taskListContainer.RemoveAllViews();

            if (tasks.Count == 0)
            {
                _txtEmptyTasks.Visibility = ViewStates.Visible;
                return;
            }

            _txtEmptyTasks.Visibility = ViewStates.Gone;

            float density = Resources.DisplayMetrics.Density;
            int cardMargin = (int)(15 * density);
            int cardPadding = (int)(15 * density);

            foreach (var task in tasks)
            {
                var card = new LinearLayout(this)
                {
                    Orientation = Orientation.Horizontal,
                    LayoutParameters = new LinearLayout.LayoutParams(
                        ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent)
                };
                card.SetBackgroundResource(Resource.Drawable.bg_card_medium);
                card.SetPadding(cardPadding, cardPadding, cardPadding, cardPadding);
                var lp = new LinearLayout.LayoutParams(
                    ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                lp.SetMargins(cardMargin, 0, cardMargin, cardMargin);
                card.LayoutParameters = lp;

                var info = new LinearLayout(this)
                {
                    Orientation = Orientation.Vertical,
                    LayoutParameters = new LinearLayout.LayoutParams(
                        0, ViewGroup.LayoutParams.WrapContent, 1f)
                };

                var title = new TextView(this)
                {
                    Text = task.Title,
                    TextSize = 14 * density
                };
                title.SetTypeface(Android.Graphics.Typeface.DefaultBold, Android.Graphics.TypefaceStyle.Bold);
                title.SetTextColor(Android.Graphics.Color.ParseColor("#2E7D32"));
                var desc = new TextView(this)
                {
                    Text = task.Description,
                    TextSize = 12 * density
                };
                desc.SetTextColor(Android.Graphics.Color.ParseColor("#3E5C46"));
                info.AddView(title);
                info.AddView(desc);

                var btnOk = new ImageView(this)
                {
                    LayoutParameters = new LinearLayout.LayoutParams(
                        (int)(40 * density), (int)(40 * density)),
                    Clickable = true,
                    Focusable = true,
                    ContentDescription = "Aceitar"
                };
                btnOk.SetImageResource(Resource.Drawable.ic_check);
                var btnX = new ImageView(this)
                {
                    LayoutParameters = new LinearLayout.LayoutParams(
                        (int)(40 * density), (int)(40 * density)),
                    Clickable = true,
                    Focusable = true,
                    ContentDescription = "Recusar"
                };
                btnX.SetImageResource(Resource.Drawable.ic_close);

                btnOk.Click += async (s, e) =>
                {
                    btnOk.Enabled = false;
                    btnX.Enabled = false;
                    try
                    {
                        task.IsCompleted = true;
                        await DatabaseService.Instance.ToggleCompletedAsync(task);
                        RunOnUiThread(() =>
                        {
                            try
                            {
                                AnimateAndRemoveCard(card, _taskListContainer, () =>
                                {
                                    if (_taskListContainer.ChildCount == 0)
                                        _txtEmptyTasks.Visibility = ViewStates.Visible;
                                });
                            }
                            catch (Exception ex)
                            {
                                Android.Util.Log.Error("Agrosens.Home", ex.ToString());
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        Android.Util.Log.Error("Agrosens.Home", ex.ToString());
                        btnOk.Enabled = true;
                        btnX.Enabled = true;
                    }
                };

                btnX.Click += async (s, e) =>
                {
                    btnOk.Enabled = false;
                    btnX.Enabled = false;
                    try
                    {
                        await DatabaseService.Instance.DeleteTaskAsync(task);
                        RunOnUiThread(() =>
                        {
                            try
                            {
                                AnimateAndRemoveCard(card, _taskListContainer, () =>
                                {
                                    if (_taskListContainer.ChildCount == 0)
                                        _txtEmptyTasks.Visibility = ViewStates.Visible;
                                });
                            }
                            catch (Exception ex)
                            {
                                Android.Util.Log.Error("Agrosens.Home", ex.ToString());
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        Android.Util.Log.Error("Agrosens.Home", ex.ToString());
                        btnOk.Enabled = true;
                        btnX.Enabled = true;
                    }
                };

                card.AddView(info);
                card.AddView(btnOk);
                card.AddView(btnX);
                _taskListContainer.AddView(card);
            }
        }

        private void SetupPerfilEvents(View view)
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
                    var prefsPerfil = new PreferenceService(this);
                    var nomePerfil = prefsPerfil.DisplayName;
                    if (txtPerfilName != null) txtPerfilName.Text = nomePerfil;
                    if (txtMenuLogout != null) txtMenuLogout.Text = "Logout";
                }
            }

            UpdateProfileUI();

            var menuProfile = view.FindViewById(Resource.Id.menuProfile);
            if (menuProfile != null)
            {
                menuProfile.Click += (s, e) => SwitchTab(AppTab.EditProfile);
            }

            BindClick(view, Resource.Id.menuPrivacy, "Política de privacidade");
            BindClick(view, Resource.Id.menuSettings, "Configuração");

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
                    prefsLogout.Clear();
                    SwitchTab(AppTab.AuthWelcome);
                };
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

        private void SetupEditProfileEvents(View view)
        {
            var btnBackEditProfile = view.FindViewById(Resource.Id.btnBackEditProfile);
            if (btnBackEditProfile != null)
            {
                btnBackEditProfile.Click += (s, e) => SwitchTab(AppTab.Perfil);
            }

            var btnUpdateProfile = view.FindViewById(Resource.Id.btnUpdateProfile);
            if (btnUpdateProfile != null)
            {
                btnUpdateProfile.Click += (s, e) =>
                {
                    Toast.MakeText(this, "Perfil atualizado com sucesso!", ToastLength.Short)?.Show();
                    SwitchTab(AppTab.Perfil);
                };
            }

            BindClick(view, Resource.Id.btnChangeAvatar, "Alterar foto de perfil");
        }

        private async void SetupGeralEvents(View view)
        {
            try
            {
                await _dbInitTask;

                var totalMes = await DatabaseService.Instance.GetEventsForMonthAsync(DateTime.Today.Year, DateTime.Today.Month);
                var txtGalpao = view.FindViewById<TextView>(Resource.Id.txtCounterGalpao);
                if (txtGalpao != null) txtGalpao.Text = totalMes.Count + " eventos";

                var pendentesHoje = await DatabaseService.Instance.GetEventsForDayAsync(DateTime.Today);
                var txtAgua = view.FindViewById<TextView>(Resource.Id.txtCounterAgua);
                if (txtAgua != null) txtAgua.Text = pendentesHoje.Count + " hoje";

                var tarefasPendentes = await DatabaseService.Instance.GetPendingTasksAsync();
                var txtSilagem = view.FindViewById<TextView>(Resource.Id.txtCounterSilagem);
                if (txtSilagem != null) txtSilagem.Text = tarefasPendentes.Count + " pendentes";

                var txtPorteiras = view.FindViewById<TextView>(Resource.Id.txtCounterPorteiras);
                if (txtPorteiras != null) txtPorteiras.Text = "--";
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error("Agrosens.Geral", ex.ToString());
            }

            BindClick(view, Resource.Id.btnAddSensor, "Adicionar novo sensor");
            BindClick(view, Resource.Id.cardGalpao, "Galpão selecionado");
            BindClick(view, Resource.Id.cardAgua, "Monitoramento de Água selecionado");
            BindClick(view, Resource.Id.cardSilagem, "Silagem selecionada");
            BindClick(view, Resource.Id.cardPorteiras, "Porteiras selecionadas");
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
                    string nome = etFullName?.Text?.Trim() ?? "";
                    string email = etSignUpEmail?.Text?.Trim() ?? "";
                    if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(email))
                    {
                        Toast.MakeText(this, "Preencha nome e e-mail antes de cadastrar.", ToastLength.Short)?.Show();
                        return;
                    }
                    var prefsSignUp = new PreferenceService(this);
                    prefsSignUp.SaveSession(nome, email);
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
            var btnAddCategory = sheetView.FindViewById<TextView>(Resource.Id.btnAddCategory);
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
                            Toast.MakeText(this, $"Evento '{name}' salvo com sucesso!", ToastLength.Short)?.Show();
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
                    Toast.MakeText(this, "Categoria selecionada: Reunião", ToastLength.Short)?.Show();
            }

            if (chipManutencao != null)
            {
                chipManutencao.Click += (s, e) =>
                    Toast.MakeText(this, "Categoria selecionada: Manutenção", ToastLength.Short)?.Show();
            }

            if (btnAddCategory != null)
            {
                btnAddCategory.Click += (s, e) =>
                    Toast.MakeText(this, "Adicionar nova categoria", ToastLength.Short)?.Show();
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
                dot.SetTextColor(Android.Graphics.Color.ParseColor("#8E64C7"));
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
                    Text = ev.Title,
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

                if (ev.Status == 1)
                {
                    var status = new TextView(this)
                    {
                        Text = "Aceito",
                        TextSize = 11
                    };
                    status.SetTextColor(Android.Graphics.Color.ParseColor("#2E7D32"));
                    var lpStatus = new LinearLayout.LayoutParams(
                        ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                    lpStatus.SetMargins(0, (int)(4 * density), 0, 0);
                    status.LayoutParameters = lpStatus;
                    card.AddView(status);
                }
                else if (ev.Status == 2)
                {
                    var status = new TextView(this)
                    {
                        Text = "Recusado",
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

        protected override void OnResume()
        {
            base.OnResume();
            if (_currentTab == AppTab.Home)
            {
                _homeSelectedDay = DateTime.Today;
                BuildHomeWeekDays();
                _ = LoadHomeEventsAsync();
                _ = LoadHomeTasksAsync();
            }
            else if (_currentTab == AppTab.Calendario)
            {
                _ = LoadCalendarDayEventsAsync();
            }
        }

        private void BuildHomeWeekDays()
        {
            if (_homeWeekDaysContainer == null) return;
            var today = DateTime.Today;
            _homeSelectedDay = new DateTime(today.Year, today.Month, today.Day);
            var startDate = today.AddDays(-3);
            _homeWeekDaysContainer.RemoveAllViews();
            float density = Resources.DisplayMetrics.Density;
            int cellHeight = (int)(70 * density);
            int cellMargin = (int)(4 * density);
            string[] dayAbbr = new string[] { "DOM", "SEG", "TER", "QUA", "QUI", "SEX", "SÁB" };
            for (int i = 0; i < 7; i++)
            {
                var dayDate = startDate.AddDays(i);
                var cell = new LinearLayout(this) { Orientation = Orientation.Vertical, LayoutParameters = new LinearLayout.LayoutParams(0, cellHeight, 1f) };
                var lpCell = (LinearLayout.LayoutParams)cell.LayoutParameters; lpCell.SetMargins(cellMargin, cellMargin, cellMargin, cellMargin); cell.LayoutParameters = lpCell; cell.SetGravity(GravityFlags.Center);
                bool isSelected = dayDate.Date == _homeSelectedDay.Date;
                cell.SetBackgroundResource(isSelected ? Resource.Drawable.bg_cell_green : Resource.Drawable.bg_cell_white);

                var txtDayNum = new TextView(this)
                {
                    Text = dayDate.Day.ToString(),
                    TextSize = 14,
                    Gravity = GravityFlags.Center
                };
                txtDayNum.SetTextColor(isSelected ? Android.Graphics.Color.White : Android.Graphics.Color.ParseColor("#1B3A24"));

                var txtDayAbbr = new TextView(this)
                {
                    Text = dayAbbr[i],
                    TextSize = 10,
                    Gravity = GravityFlags.Center
                };
                txtDayAbbr.SetTextColor(Android.Graphics.Color.ParseColor("#1B3A24"));

                cell.AddView(txtDayNum);
                cell.AddView(txtDayAbbr);
                cell.Click += async (s, e) =>
                {
                    try
                    {
                        _homeSelectedDay = dayDate.Date;
                        BuildHomeWeekDays();
                        await LoadHomeEventsAsync();
                        await LoadHomeTasksAsync();
                    }
                    catch (Exception ex)
                    {
                        Android.Util.Log.Error("Agrosens.Home", ex.ToString());
                    }
                };
                _homeWeekDaysContainer.AddView(cell);
            }
            UpdateHomeDateTitle();
        }

        private void UpdateHomeDateTitle()
        {
            if (_homeDateTitle == null) return;
            var today = DateTime.Today;
            if (_homeSelectedDay.Date == today.Date)
            {
                _homeDateTitle.Text = _homeSelectedDay.Day + " " + MonthNamesPt[_homeSelectedDay.Month - 1] + " - Hoje";
            }
            else
            {
                _homeDateTitle.Text = _homeSelectedDay.Day + " " + MonthNamesPt[_homeSelectedDay.Month - 1];
            }
        }

        private async Task LoadHomeEventsAsync()
        {
            if (_homeEventsContainer == null) return;

            await _dbInitTask;

            try
            {
                var events = await DatabaseService.Instance.GetEventsForDayAsync(_homeSelectedDay).ConfigureAwait(false);

                RunOnUiThread(() =>
                {
                    _homeEventsContainer.RemoveAllViews();
                    var divider = new View(this)
                    {
                        LayoutParameters = new LinearLayout.LayoutParams(
                            ViewGroup.LayoutParams.MatchParent, (int)(1 * Resources.DisplayMetrics.Density))
                    };
                    divider.SetBackgroundColor(Android.Graphics.Color.ParseColor("#B2DFB9"));
                    _homeEventsContainer.AddView(divider);

                    if (events.Count == 0)
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
                        _homeEventsContainer.AddView(empty);
                        return;
                    }

                    float density = Resources.DisplayMetrics.Density;
                    foreach (var ev in events)
                    {
                        var card = new LinearLayout(this)
                        {
                            Orientation = Orientation.Horizontal,
                            LayoutParameters = new LinearLayout.LayoutParams(
                                ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent)
                        };
                        card.SetBackgroundResource(Resource.Drawable.bg_event_green);
                        card.SetPadding((int)(12 * density), (int)(12 * density), (int)(12 * density), (int)(12 * density));
                        var lpCard = new LinearLayout.LayoutParams(
                            ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                        lpCard.SetMargins(0, (int)(10 * density), 0, 0);
                        card.LayoutParameters = lpCard;

                        var info = new LinearLayout(this)
                        {
                            Orientation = Orientation.Vertical,
                            LayoutParameters = new LinearLayout.LayoutParams(0, ViewGroup.LayoutParams.WrapContent, 1f)
                        };

                        var title = new TextView(this)
                        {
                            Text = ev.Title,
                            TextSize = 13
                        };
                        title.SetTypeface(Android.Graphics.Typeface.DefaultBold, Android.Graphics.TypefaceStyle.Bold);
                        title.SetTextColor(Android.Graphics.Color.ParseColor("#1B3A24"));

                        var desc = new TextView(this)
                        {
                            Text = string.IsNullOrWhiteSpace(ev.Description) ? "" : ev.Description,
                            TextSize = 11
                        };
                        desc.SetTextColor(Android.Graphics.Color.ParseColor("#3E5C46"));
                        info.AddView(title);
                        if (!string.IsNullOrWhiteSpace(ev.Description))
                            info.AddView(desc);

                        var time = new TextView(this)
                        {
                            Text = ev.StartTime.ToString(@"hh\:mm") + " - " + ev.EndTime.ToString(@"hh\:mm"),
                            TextSize = 11,
                            Gravity = GravityFlags.CenterVertical
                        };
                        time.SetTextColor(Android.Graphics.Color.ParseColor("#2E7D32"));
                        var lpTime = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.MatchParent);
                        lpTime.SetMargins((int)(5 * density), 0, (int)(10 * density), 0);
                        time.LayoutParameters = lpTime;

                        var btnOk = new ImageView(this)
                        {
                            LayoutParameters = new LinearLayout.LayoutParams(
                                (int)(40 * density), (int)(40 * density)),
                            Clickable = true,
                            Focusable = true,
                            ContentDescription = "Aceitar"
                        };
                        btnOk.SetImageResource(Resource.Drawable.ic_check);

                        var btnX = new ImageView(this)
                        {
                            LayoutParameters = new LinearLayout.LayoutParams(
                                (int)(40 * density), (int)(40 * density)),
                            Clickable = true,
                            Focusable = true,
                            ContentDescription = "Recusar"
                        };
                        btnX.SetImageResource(Resource.Drawable.ic_close);

                        btnOk.Click += async (s2, e2) =>
                        {
                            btnOk.Enabled = false;
                            btnX.Enabled = false;
                            try
                            {
                                ev.Status = 1;
                                await DatabaseService.Instance.SaveEventAsync(ev);
                                RunOnUiThread(() =>
                                {
                                    try
                                    {
                                        AnimateAndRemoveCard(card, _homeEventsContainer, () =>
                                        {
                                            if (_homeEventsContainer != null && _homeEventsContainer.ChildCount <= 1)
                                            {
                                                var empty2 = new TextView(this)
                                                {
                                                    Text = "Nenhum compromisso para este dia",
                                                    TextSize = 12,
                                                    Gravity = GravityFlags.Center
                                                };
                                                empty2.SetTextColor(Android.Graphics.Color.ParseColor("#8A968D"));
                                                var lpEmpty2 = new LinearLayout.LayoutParams(
                                                    ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                                                lpEmpty2.SetMargins(0, (int)(15 * density), 0, 0);
                                                empty2.LayoutParameters = lpEmpty2;
                                                _homeEventsContainer.AddView(empty2);
                                            }
                                        });
                                    }
                                    catch (Exception ex)
                                    {
                                        Android.Util.Log.Error("Agrosens.Home", ex.ToString());
                                    }
                                });
                            }
                            catch (Exception ex)
                            {
                                Android.Util.Log.Error("Agrosens.Home", ex.ToString());
                                btnOk.Enabled = true;
                                btnX.Enabled = true;
                            }
                        };

                        btnX.Click += async (s2, e2) =>
                        {
                            btnOk.Enabled = false;
                            btnX.Enabled = false;
                            try
                            {
                                ev.Status = 2;
                                await DatabaseService.Instance.SaveEventAsync(ev);
                                RunOnUiThread(() =>
                                {
                                    try
                                    {
                                        AnimateAndRemoveCard(card, _homeEventsContainer, () =>
                                        {
                                            if (_homeEventsContainer != null && _homeEventsContainer.ChildCount <= 1)
                                            {
                                                var empty2 = new TextView(this)
                                                {
                                                    Text = "Nenhum compromisso para este dia",
                                                    TextSize = 12,
                                                    Gravity = GravityFlags.Center
                                                };
                                                empty2.SetTextColor(Android.Graphics.Color.ParseColor("#8A968D"));
                                                var lpEmpty2 = new LinearLayout.LayoutParams(
                                                    ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                                                lpEmpty2.SetMargins(0, (int)(15 * density), 0, 0);
                                                empty2.LayoutParameters = lpEmpty2;
                                                _homeEventsContainer.AddView(empty2);
                                            }
                                        });
                                    }
                                    catch (Exception ex)
                                    {
                                        Android.Util.Log.Error("Agrosens.Home", ex.ToString());
                                    }
                                });
                            }
                            catch (Exception ex)
                            {
                                Android.Util.Log.Error("Agrosens.Home", ex.ToString());
                                btnOk.Enabled = true;
                                btnX.Enabled = true;
                            }
                        };

                        var horaInicio = new TextView(this)
                        {
                            Text = ev.StartTime.ToString(@"hh\:mm"),
                            TextSize = 11,
                            Gravity = GravityFlags.CenterVertical
                        };
                        horaInicio.SetTextColor(Android.Graphics.Color.ParseColor("#2E7D32"));
                        var lpHora = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.MatchParent);
                        lpHora.SetMargins(0, 0, (int)(8 * density), 0);
                        horaInicio.LayoutParameters = lpHora;

                        card.AddView(horaInicio);
                        card.AddView(info);
                        card.AddView(time);
                        card.AddView(btnOk);
                        card.AddView(btnX);
                        _homeEventsContainer.AddView(card);
                    }
                });
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error("Agrosens.Home", ex.ToString());
            }
        }
    }
}
