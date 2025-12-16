using System.Text.Json;
using Microsoft.AspNetCore.Http; // 🚨 Нужно для работы с ISession

namespace WebApplication1.Extensions
{
    public static class SessionExtensions
    {
        // Метод для сохранения объекта в сессии
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        // Метод для получения объекта из сессии
        public static T? Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            // Если значение найдено, десериализуем его из JSON
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}