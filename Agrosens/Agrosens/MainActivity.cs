using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.BottomSheet;

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

            // Configuração dos eventos de navegação da BottomBar
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

            // Carrega a tela inicial por padrão
            SwitchTab(AppTab.Home);
        }

        private void SwitchTab(AppTab tab)
        {
            _currentTab = tab;

            if (_contentContainer == null || LayoutInflater == null)
                return;

            _contentContainer.RemoveAllViews();

            // Atualiza os estilos visuais das abas na barra inferior
            ResetTabHighlights();

            // Ajusta o fundo da tela (Verde para AuthWelcome, Branco para as outras telas)
            if (_rootLayout != null)
            {
                if (tab == AppTab.AuthWelcome)
                    _rootLayout.SetBackgroundColor(Android.Graphics.Color.ParseColor("#00B050"));
                else
                    _rootLayout.SetBackgroundColor(Android.Graphics.Color.ParseColor("#FFFFFF"));
            }

            // Controla visibilidade da barra inferior (esconde nas telas de autenticação)
            if (_bottomBarContainer != null)
            {
                bool hideBar = (tab == AppTab.AuthWelcome || tab == AppTab.LoginForm || tab == AppTab.SignUpForm);
                _bottomBarContainer.Visibility = hideBar ? ViewStates.Gone : ViewStates.Visible;
            }

            switch (tab)
            {
                case AppTab.Home:
                    _centralCircleBg?.SetBackgroundResource(Resource.Drawable.bg_circle_green);
                    View homeView = LayoutInflater.Inflate(Resource.Layout.fragment_home, _contentContainer, false)!;
                    _contentContainer.AddView(homeView);
                    SetupHomeEvents(homeView);
                    break;

                case AppTab.Calendario:
                    _navCalendario?.SetBackgroundResource(Resource.Drawable.bg_tab_active_pill);
                    _centralCircleBg?.SetBackgroundResource(Resource.Drawable.bg_circle_black);
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

        private void SetupHomeEvents(View view)
        {
            var imgUser = view.FindViewById<ImageView>(Resource.Id.imgUser);
            var txtUserName = view.FindViewById<TextView>(Resource.Id.txtUserName);

            if (_isLoggedOut)
            {
                imgUser?.SetImageResource(Resource.Drawable.sem_usuario);
                if (txtUserName != null) txtUserName.Text = "Usuario";
            }
            else
            {
                imgUser?.SetImageResource(Resource.Drawable.user_photo);
                if (txtUserName != null) txtUserName.Text = "João Francisco";
            }

            BindClick(view, Resource.Id.btnOk1, "Card 1 aceito");
            BindClick(view, Resource.Id.btnX1, "Card 1 recusado");
            BindClick(view, Resource.Id.btnOk2, "Card 2 aceito");
            BindClick(view, Resource.Id.btnX2, "Card 2 recusado");
            BindClick(view, Resource.Id.btnOk3, "Card 3 aceito");
            BindClick(view, Resource.Id.btnX3, "Card 3 recusado");
            BindClick(view, Resource.Id.btnOkTimeline, "Reunião confirmada");
            BindClick(view, Resource.Id.btnXTimeline, "Reunião cancelada");
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
                    if (txtPerfilName != null) txtPerfilName.Text = "João Francisco";
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
                    if (_isLoggedOut)
                    {
                        SwitchTab(AppTab.AuthWelcome);
                    }
                    else
                    {
                        _isLoggedOut = true;
                        UpdateProfileUI();
                        Toast.MakeText(this, "Logout realizado com sucesso!", ToastLength.Short)?.Show();
                    }
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

        private void SetupGeralEvents(View view)
        {
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
                    _isLoggedOut = false;
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
                    _isLoggedOut = false;
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

        private System.DateTime _currentCalendarDate = new System.DateTime(2026, 9, 1);

        private static readonly string[] MonthNamesPt = new string[]
        {
            "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho",
            "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"
        };

        private void SetupCalendarEvents(View view)
        {
            var txtMonth = view.FindViewById<TextView>(Resource.Id.txtMonth);
            var txtYear = view.FindViewById<TextView>(Resource.Id.txtYear);

            void UpdateMonthDisplay()
            {
                if (txtMonth != null) txtMonth.Text = MonthNamesPt[_currentCalendarDate.Month - 1];
                if (txtYear != null) txtYear.Text = _currentCalendarDate.Year.ToString();
            }

            UpdateMonthDisplay();

            var btnPrevMonth = view.FindViewById(Resource.Id.btnPrevMonth);
            if (btnPrevMonth != null)
            {
                btnPrevMonth.Click += (s, e) =>
                {
                    _currentCalendarDate = _currentCalendarDate.AddMonths(-1);
                    UpdateMonthDisplay();
                };
            }

            var btnNextMonth = view.FindViewById(Resource.Id.btnNextMonth);
            if (btnNextMonth != null)
            {
                btnNextMonth.Click += (s, e) =>
                {
                    _currentCalendarDate = _currentCalendarDate.AddMonths(1);
                    UpdateMonthDisplay();
                };
            }

            var btnAddEvent = view.FindViewById(Resource.Id.btnAddEvent);
            if (btnAddEvent != null)
            {
                btnAddEvent.Click += (s, e) => OpenAddEventBottomSheet();
            }

            BindClick(view, Resource.Id.btnEventoMenu, "Opções do evento");
        }

        private void OpenAddEventBottomSheet()
        {
            var dialog = new BottomSheetDialog(this);
            var sheetView = LayoutInflater?.Inflate(Resource.Layout.bottom_sheet_add_event, null);
            if (sheetView == null) return;

            dialog.SetContentView(sheetView);

            var etEventName = sheetView.FindViewById<EditText>(Resource.Id.etEventName);
            var chipReuniao = sheetView.FindViewById<LinearLayout>(Resource.Id.chipReuniao);
            var chipManutencao = sheetView.FindViewById<LinearLayout>(Resource.Id.chipManutencao);
            var btnAddCategory = sheetView.FindViewById<TextView>(Resource.Id.btnAddCategory);
            var btnCreateEvent = sheetView.FindViewById<TextView>(Resource.Id.btnCreateEvent);

            if (btnCreateEvent != null)
            {
                btnCreateEvent.Click += (s, e) =>
                {
                    string name = etEventName?.Text?.Trim() ?? "";
                    string msg = string.IsNullOrEmpty(name)
                        ? "Evento criado com sucesso!"
                        : $"Evento '{name}' criado com sucesso!";
                    Toast.MakeText(this, msg, ToastLength.Short)?.Show();
                    dialog.Dismiss();
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
    }
}