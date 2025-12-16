using Microsoft.AspNetCore.Http;
using System.Text.Json; 

namespace WebApplication1.Extensions
{
    public static class SessionExtensions
    {
        // Метод для сохранения объекта в сессии (сериализация)
        public static void Set<T>(this ISession session, string key, T value)
        {
            // Используем JsonSerializer для преобразования объекта в строку JSON
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        // Метод для получения объекта из сессии (десериализация)
        public static T? Get<T>(this ISession session, string key)
        {
            // Получаем строку
            var value = session.GetString(key);

            // Если строка пуста, возвращаем null. Иначе десериализуем.
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}