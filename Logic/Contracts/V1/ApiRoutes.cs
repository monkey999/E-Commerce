namespace E_Commerce_Shop.Contracts.V1
{
    public static class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "v1";
        public const string Base = Root + "/" + Version;
        public static class Users
        {
            public const string GetAllUsers = Base + "/users";
            public const string GetUserByID = Base + "/users/user/{userId}";
            public const string GetLoggedInUser = Base + "/users/user";
            public const string CreateUser = Base + "/users";
            public const string DeleteUser = Base + "/users/{userId}";
            public const string UpdateUser = Base + "/users/{userId}";
        }

        public static class Categories
        {
            public const string GetAllCategories = Base + "/categories";
            public const string GetCategoryByID = Base + "/categories/{categoryId}";
            public const string AddCategory = Base + "/categories";
            public const string DeleteCategory = Base + "/categories/{categoryId}";
            public const string UpdateCategory = Base + "/categories/{categoryId}";
        }

        public static class Orders
        {
            public const string CreateOrder = Base + "/orders";
            public const string GetAllOrders = Base + "/orders";
            public const string GetOrderByID = Base + "/orders/{orderId}";
            public const string GetOrderByReference = Base + "/orders/{orderReference}";
            public const string DeleteOrder = Base + "/orders/{orderId}";
            public const string UpdateOrder = Base + "/orders/{orderId}";
        }
        public static class Products
        {
            public const string GetAllProducts = Base + "/products";
            public const string GetProductByID = Base + "/products/{productId}";
            public const string AddProduct = Base + "/products";
            public const string DeleteProduct = Base + "/products/{productId}";
            public const string UpdateProduct = Base + "/products/{productId}";
        }
        public static class PurchasedProducts
        {
            public const string GetAllPurchasedProducts = Base + "/purchasedProducts";
            public const string GetPurchasedProductByID = Base + "/purchasedProducts/{purchasedProductId}";
            public const string AddPurchasedProduct = Base + "/purchasedProducts";
            public const string DeletePurchasedProduct = Base + "/purchasedProducts/{purchasedProductId}";
            public const string UpdatePurchasedProduct = Base + "/purchasedProducts/{purchasedProductId}";
        }

        public static class Identity
        {
            public const string Login = Base + "/identity/login";
            public const string Register = Base + "/identity/register";
            public const string RefreshJWTToken = Base + "/identity/refreshJwtToken";
            public const string Logout = Base + "/identity/logout";
        }

        public static class Stock
        {
            public const string GetStock = Base + "/stock";
            public const string AddStock = Base + "/stock";
            public const string DeleteStock = Base + "/stock/{stockId}";
            public const string UpdateStock = Base + "/stock/{stockId}";
        }

        public static class ShoppingCart
        {
            public const string GetCart = Base + "/ShoppingCart";
            public const string GetCartItemById = Base + "/ShoppingCart/{stockId}";
            public const string AddToCart = Base + "/ShoppingCart";
            public const string DeleteCartItem = Base + "/ShoppingCart/{stockId}";
            public const string UpdateCartItem = Base + "/ShoppingCart/{stockId}";
        }

        public static class CustomerInfo
        {
            public const string GetCustomerInfo = Base + "/CustomerInfo";
            public const string AddCustomerInfo = Base + "/CustomerInfo";
        }

        public static class Stripe
        {
            public const string AddPayment = Base + "/Payment";
        }
    }
}
