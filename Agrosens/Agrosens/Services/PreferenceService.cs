using Android.Content;

namespace Agrosens.Services
{
    public class PreferenceService
    {
        private const string PrefsName = "agrosens_prefs";
        private const string KeyUserName = "user_name";
        private const string KeyUserEmail = "user_email";
        private const string KeyIsLoggedIn = "is_logged_in";

        private readonly ISharedPreferences _prefs;
        private readonly ISharedPreferencesEditor _editor;

        public PreferenceService(Context context)
        {
            _prefs = context.GetSharedPreferences(PrefsName, FileCreationMode.Private)!;
            _editor = _prefs.Edit()!;
        }

        public string UserName
        {
            get => _prefs.GetString(KeyUserName, string.Empty) ?? string.Empty;
            set
            {
                _editor.PutString(KeyUserName, value);
                _editor.Apply();
            }
        }

        public string UserEmail
        {
            get => _prefs.GetString(KeyUserEmail, string.Empty) ?? string.Empty;
            set
            {
                _editor.PutString(KeyUserEmail, value);
                _editor.Apply();
            }
        }

        public bool IsLoggedIn
        {
            get => _prefs.GetBoolean(KeyIsLoggedIn, false);
            set
            {
                _editor.PutBoolean(KeyIsLoggedIn, value);
                _editor.Apply();
            }
        }

        public string DisplayName
        {
            get
            {
                var name = UserName;
                if (!string.IsNullOrWhiteSpace(name))
                    return name;

                var email = UserEmail;
                if (!string.IsNullOrWhiteSpace(email))
                {
                    var at = email.IndexOf('@');
                    return at > 0 ? email.Substring(0, at) : email;
                }

                return "Visitante";
            }
        }

        public void SaveSession(string name, string email)
        {
            _editor.PutString(KeyUserName, name ?? string.Empty);
            _editor.PutString(KeyUserEmail, email ?? string.Empty);
            _editor.PutBoolean(KeyIsLoggedIn, true);
            _editor.Apply();
        }

        public void SaveLoginSession(string credential)
        {
            _editor.PutString(KeyUserEmail, credential ?? string.Empty);
            _editor.PutBoolean(KeyIsLoggedIn, true);
            _editor.Apply();
        }

        public void Clear()
        {
            _editor.Clear();
            _editor.Apply();
        }
    }
}
