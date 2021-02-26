using System;

namespace Users.Roles
{
    public class Roles
    {
        public const string Admin = "Administrator";
        public const string Employee = "Employee";
        public const string Student = "Student";
        public const string WithoutRole = "WithoutRole";
        public const string AllRoles = Admin + "," + Employee + "," + Student;
    }
}
