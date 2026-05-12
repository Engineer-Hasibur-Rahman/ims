namespace ims.Shared.Constants
{
    public class Permissions
    {
        public static class Products
        {
            public const string View = "Permissions.Products.View";
            public const string Create = "Permissions.Products.Create";
            public const string Update = "Permissions.Products.Update";
            public const string Delete = "Permissions.Products.Delete";
        }

        public static class Categories
        {
            public const string View = "Permissions.Categories.View";
            public const string Create = "Permissions.Categories.Create";
            public const string Update = "Permissions.Categories.Update";
            public const string Delete = "Permissions.Categories.Delete";
        }

        public static class Brands
        {
            public const string View = "Permissions.Brands.View";
            public const string Create = "Permissions.Brands.Create";
            public const string Update = "Permissions.Brands.Update";
            public const string Delete = "Permissions.Brands.Delete";
        }

        public static class Suppliers
        {
            public const string View = "Permissions.Suppliers.View";
            public const string Create = "Permissions.Suppliers.Create";
            public const string Update = "Permissions.Suppliers.Update";
            public const string Delete = "Permissions.Suppliers.Delete";
        }

        public static class Customers
        {
            public const string View = "Permissions.Customers.View";
            public const string Create = "Permissions.Customers.Create";
            public const string Update = "Permissions.Customers.Update";
            public const string Delete = "Permissions.Customers.Delete";
        }

        public static class Purchases
        {
            public const string View = "Permissions.Purchases.View";
            public const string Create = "Permissions.Purchases.Create";
            public const string Update = "Permissions.Purchases.Update";
            public const string Delete = "Permissions.Purchases.Delete";
        }

        public static class Sales
        {
            public const string View = "Permissions.Sales.View";
            public const string Create = "Permissions.Sales.Create";
            public const string Update = "Permissions.Sales.Update";
            public const string Delete = "Permissions.Sales.Delete";
        }

        public static class Stock
        {
            public const string View = "Permissions.Stock.View";
            public const string Create = "Permissions.Stock.Create";
            public const string Update = "Permissions.Stock.Update";
            public const string Delete = "Permissions.Stock.Delete";
        }

        public static class Warehouses
        {
            public const string View = "Permissions.Warehouses.View";
            public const string Create = "Permissions.Warehouses.Create";
            public const string Update = "Permissions.Warehouses.Update";
            public const string Delete = "Permissions.Warehouses.Delete";
        }

        public static class Invoices
        {
            public const string View = "Permissions.Invoices.View";
            public const string Create = "Permissions.Invoices.Create";
            public const string Update = "Permissions.Invoices.Update";
            public const string Delete = "Permissions.Invoices.Delete";
        }

        public static class Reports
        {
            public const string View = "Permissions.Reports.View";
            public const string Create = "Permissions.Reports.Create";
            public const string Update = "Permissions.Reports.Update";
            public const string Delete = "Permissions.Reports.Delete";
        }

        public static class Users
        {
            public const string View = "Permissions.Users.View";
            public const string Create = "Permissions.Users.Create";
            public const string Update = "Permissions.Users.Update";
            public const string Delete = "Permissions.Users.Delete";
        }

        public static class Roles
        {
            public const string View = "Permissions.Roles.View";
            public const string Create = "Permissions.Roles.Create";
            public const string Update = "Permissions.Roles.Update";
            public const string Delete = "Permissions.Roles.Delete";
        }

        public static class AuditLogs
        {
            public const string View = "Permissions.AuditLogs.View";
            public const string Create = "Permissions.AuditLogs.Create";
            public const string Update = "Permissions.AuditLogs.Update";
            public const string Delete = "Permissions.AuditLogs.Delete";
        }

        public static class Settings
        {
            public const string View = "Permissions.Settings.View";
            public const string Create = "Permissions.Settings.Create";
            public const string Update = "Permissions.Settings.Update";
            public const string Delete = "Permissions.Settings.Delete";
        }

        public static readonly IReadOnlyCollection<string> AllPermissions =
        [
         Products.View, Products.Create, Products.Update, Products.Delete,
        Categories.View, Categories.Create, Categories.Update, Categories.Delete,
        Brands.View, Brands.Create, Brands.Update, Brands.Delete,
        Suppliers.View, Suppliers.Create, Suppliers.Update, Suppliers.Delete,
        Customers.View, Customers.Create, Customers.Update, Customers.Delete,
        Purchases.View, Purchases.Create, Purchases.Update, Purchases.Delete,
        Sales.View, Sales.Create, Sales.Update, Sales.Delete,
        Stock.View, Stock.Create, Stock.Update, Stock.Delete,
        Warehouses.View, Warehouses.Create, Warehouses.Update, Warehouses.Delete,
        Invoices.View, Invoices.Create, Invoices.Update, Invoices.Delete,
        Reports.View, Reports.Create, Reports.Update, Reports.Delete,
        Users.View, Users.Create, Users.Update, Users.Delete,
        Roles.View, Roles.Create, Roles.Update, Roles.Delete,
        AuditLogs.View, AuditLogs.Create, AuditLogs.Update, AuditLogs.Delete,
        Settings.View, Settings.Create, Settings.Update, Settings.Delete
        ];
    }
}
