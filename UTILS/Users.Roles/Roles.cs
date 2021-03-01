using System;

namespace Users.Roles
{
    public class Roles
    {
        public const string Admin = "Administrator";
        public const string Employee = "Employee";
        public const string Student = "Student";
        public const string WithoutRole = "WithoutRole";
        public const string EmployeesCanRead = "EmployeesCanRead"; //может читать данные (о) сотрудников
        public const string EmployeesCanWrite = "EmployeesCanWrite"; //может редактировать сотрудников
        public const string CanReadLevelDown = "CanReadLevelDown"; //может читать даннные тех, кто ниже в иерархии
        public const string CanWriteLevelDown = "CanWriteLevelDown"; //может изменять данные тех, кто ниже в иерархии
        public const string CanReadLevelOwn = "CanReadLevelOwn"; //может читать данные других на собственном уровне
        public const string CanWriteLevelOwn = "CanWriteLevelOwn"; //может изменять данные других на собственном уровне
        public const string AllRoles = Admin + "," + Employee + "," + Student;
    }
}
