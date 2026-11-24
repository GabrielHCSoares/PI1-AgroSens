using Android.Content;
using System.Collections.Generic;
using System.Linq;
using Agrosens.Models;

namespace Agrosens.Services
{
    public class UserProfileStore
    {
        private const string PrefsName = "agrosens_profile";
        private const string KeyName = "user_name";
        private const string KeyPhone = "user_phone";
        private const string KeyEmail = "user_email";
        private const string KeyRegisteredEmails = "registered_emails";

        public const string DefaultName = "João Francisco";
        public const string DefaultPhone = "+55 31987456363";
        public const string DefaultEmail = "joao@example.com";

        private readonly ISharedPreferences _prefs;
        private readonly ISharedPreferencesEditor _editor;

        public UserProfileStore(Context context)
        {
            _prefs = context.GetSharedPreferences(PrefsName, FileCreationMode.Private)!;
            _editor = _prefs.Edit()!;
        }

        public string Name
        {
            get => _prefs.GetString(KeyName, DefaultName) ?? DefaultName;
            set
            {
                _editor.PutString(KeyName, value ?? DefaultName);
                _editor.Apply();
            }
        }

        public string FirstName
        {
            get
            {
                var name = Name;
                if (string.IsNullOrWhiteSpace(name))
                    return string.Empty;
                var parts = name.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                return parts.Length > 0 ? parts[0] : string.Empty;
            }
        }

        public string Phone
        {
            get => _prefs.GetString(KeyPhone, DefaultPhone) ?? DefaultPhone;
            set
            {
                _editor.PutString(KeyPhone, value ?? DefaultPhone);
                _editor.Apply();
            }
        }

        public string Email
        {
            get => _prefs.GetString(KeyEmail, DefaultEmail) ?? DefaultEmail;
            set
            {
                _editor.PutString(KeyEmail, value ?? DefaultEmail);
                _editor.Apply();
            }
        }

        public List<string> RegisteredEmails
        {
            get
            {
                var val = _prefs.GetString(KeyRegisteredEmails, string.Empty) ?? string.Empty;
                if (string.IsNullOrWhiteSpace(val))
                    return new List<string>();
                return val.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            set
            {
                _editor.PutString(KeyRegisteredEmails, string.Join(";", value ?? new List<string>()));
                _editor.Apply();
            }
        }

        public void RegisterEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return;
            var list = RegisteredEmails;
            if (!list.Contains(email))
            {
                list.Add(email);
                RegisteredEmails = list;
            }
        }

        public bool IsEmailRegistered(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return RegisteredEmails.Contains(email);
        }

        public void RestoreDefaults()
        {
            Name = DefaultName;
            Phone = DefaultPhone;
            Email = DefaultEmail;
        }

        public void Save(string name, string phone, string email)
        {
            _editor.PutString(KeyName, name ?? DefaultName);
            _editor.PutString(KeyPhone, phone ?? DefaultPhone);
            _editor.PutString(KeyEmail, email ?? DefaultEmail);
            _editor.Apply();
        }

        public UserProfile Load() => new UserProfile
        {
            Name = Name,
            Phone = Phone,
            Email = Email
        };
    }
}
