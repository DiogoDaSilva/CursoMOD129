namespace CursoMOD129
{
    public static class CursoMOD129Constants
    {
        public readonly struct EMAIL
        {
            public static readonly string EMAIL_TEMPLATES_FOLDER = "EmailTemplates";
            public static readonly string TOMORROW_APPOINTMENT_EMAIL_TEMPLATE = "tomorrow_appointment.html";
        }


        public readonly struct ROLES
        {
            public static readonly string ADMIN = "admin";
            public static readonly string ADMINISTRATIVE = "administrative";
        }

        public readonly struct USERS
        {
            public readonly struct ADMIN
            {
                public static readonly string USERNAME = "admin@admin.pt";
                public static readonly string PASSWORD = "admin2023";
            }

            public readonly struct ADMINISTRATIVE
            {
                public static readonly string USERNAME = "administrative@admin.pt";
                public static readonly string PASSWORD = "admin2023";
            }
        }

        

        public readonly struct POLICIES
        {
            public readonly struct APP_POLICY
            {
                public const string NAME = "APP_POLICY";
                public static readonly string[] APP_POLICY_ROLES =
                {
                    ROLES.ADMIN,
                    ROLES.ADMINISTRATIVE
                };
            }

            public readonly struct APP_POLICY_ADMIN
            {
                public const string NAME = "APP_POLICY_ADMIN";
                public static readonly string[] APP_POLICY_ROLES =
                {
                    ROLES.ADMIN
                };
            }
        }
    }
}
