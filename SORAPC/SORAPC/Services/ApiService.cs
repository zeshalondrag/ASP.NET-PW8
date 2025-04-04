using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SORAPC.Models;
using SORAPC_API.Models;
using System.Net.Http.Headers;
using System.Text;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public ApiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _httpClient.BaseAddress = new Uri(_configuration["ApiSettings:BaseUrl"]);
    }

    public async Task<string> Register(RegisterModel model)
    {
        var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("/api/Auth/register", content);
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<LoginResponse> Login(LoginModel model)
    {
        var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("/api/Auth/login", content);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<LoginResponse>(json);
        }
        return null;
    }

    public async Task<User> GetUserProfile(int userId, string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.GetAsync($"/api/Users/{userId}");
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<User>(json);
        }
        return null;
    }

    public async Task<User> GetUserProfileByLogin(string login, string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.GetAsync($"/api/Users?login={login}");
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<User>>(json);
            return users.FirstOrDefault(u => u.Logins == login);
        }
        return null;
    }

    // Получение корзины пользователя
    public async Task<List<Cart>> GetUserCart(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.GetAsync("/api/Carts/user");
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Cart>>(json);
        }
        return new List<Cart>();
    }

    // Добавление товара в корзину
    public async Task<string> AddToCart(Cart cart, string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var content = new StringContent(JsonConvert.SerializeObject(cart), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("/api/Carts", content);
        return await response.Content.ReadAsStringAsync();
    }

    // Удаление товара из корзины
    public async Task<string> RemoveFromCart(int cartId, string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.DeleteAsync($"/api/Carts/{cartId}");
        return await response.Content.ReadAsStringAsync();
    }

    // Продукты
        public async Task<List<Product>> GetProducts(string token = null)
        {
            if (!string.IsNullOrEmpty(token))
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("/api/Products");
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Product>>(json);
        }

        // Получение продукта по ID
        public async Task<Product> GetProduct(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"/api/Products/{id}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Product>(json);
            }
            return null; 
        }

        // Создание нового продукта
        public async Task<string> CreateProduct(Product product, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/Products", content);
            return await response.Content.ReadAsStringAsync();
        }

        // Обновление продукта
        public async Task<string> UpdateProduct(int id, Product product, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"/api/Products/{id}", content);
            return await response.Content.ReadAsStringAsync();
        }

        // Удаление продукта
        public async Task<string> DeleteProduct(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.DeleteAsync($"/api/Products/{id}");
            return await response.Content.ReadAsStringAsync();
        }

    // Статусы заказов
    public async Task<List<OrderStatus>> GetOrderStatuses(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync("/api/OrderStatus");
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<OrderStatus>>(json);
        }

        public async Task<OrderStatus> GetOrderStatus(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"/api/OrderStatus/{id}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<OrderStatus>(json);
            }
            return null; 
        }

        public async Task<string> CreateOrderStatus(OrderStatus orderStatus, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonConvert.SerializeObject(orderStatus), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/OrderStatus", content);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateOrderStatus(int id, OrderStatus orderStatus, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonConvert.SerializeObject(orderStatus), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"/api/OrderStatus/{id}", content);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> DeleteOrderStatus(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.DeleteAsync($"/api/OrderStatus/{id}");
            return await response.Content.ReadAsStringAsync();
        }

    // Категория продуктов
    public async Task<List<ProductCategory>> GetProductCategories(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync("/api/ProductCategories");
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ProductCategory>>(json);
        }

        public async Task<ProductCategory> GetProductCategory(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"/api/ProductCategories/{id}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ProductCategory>(json);
            }
            return null;
        }

        public async Task<string> CreateProductCategory(ProductCategory productCategory, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonConvert.SerializeObject(productCategory), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/ProductCategories", content);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateProductCategory(int id, ProductCategory productCategory, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonConvert.SerializeObject(productCategory), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"/api/ProductCategories/{id}", content);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> DeleteProductCategory(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.DeleteAsync($"/api/ProductCategories/{id}");
            return await response.Content.ReadAsStringAsync();
        }

    // Статус продукта
    public async Task<List<ProductStatus>> GetProductStatuses(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync("/api/ProductStatus");
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ProductStatus>>(json);
        }

        public async Task<ProductStatus> GetProductStatus(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"/api/ProductStatus/{id}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ProductStatus>(json);
            }
            return null; 
        }

        public async Task<string> CreateProductStatus(ProductStatus productStatus, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonConvert.SerializeObject(productStatus), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/ProductStatus", content);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateProductStatus(int id, ProductStatus productStatus, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonConvert.SerializeObject(productStatus), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"/api/ProductStatus/{id}", content);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> DeleteProductStatus(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.DeleteAsync($"/api/ProductStatus/{id}");
            return await response.Content.ReadAsStringAsync();
        }


    // Роли
    public async Task<List<Role>> GetRoles(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync("/api/Roles");
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Role>>(json);
        }

        public async Task<Role> GetRole(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"/api/Roles/{id}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Role>(json);
            }
            return null;
        }

        public async Task<string> CreateRole(Role role, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonConvert.SerializeObject(role), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/Roles", content);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateRole(int id, Role role, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonConvert.SerializeObject(role), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"/api/Roles/{id}", content);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> DeleteRole(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.DeleteAsync($"/api/Roles/{id}");
            return await response.Content.ReadAsStringAsync();
        }

    // Пользователи
        public async Task<List<User>> GetUsers(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync("/api/Users");
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<User>>(json);
        }

        public async Task<User> GetUser(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"/api/Users/{id}");
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<User>(json);
        }

        public async Task<string> CreateUser(User user, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/Users", content);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateUser(int id, User user, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"/api/Users/{id}", content);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> DeleteUser(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.DeleteAsync($"/api/Users/{id}");
            return await response.Content.ReadAsStringAsync();
        }
}