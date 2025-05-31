using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nursan.Caching
{
    /// <summary>
    /// Помощен клас за JSON сериализация/десериализация с предотвратяване на циклични референции
    /// </summary>
    public static class JsonHelper
    {
        private static readonly JsonSerializerSettings _serializerSettings;
        private static readonly JsonSerializerSettings _deserializerSettings;

        static JsonHelper()
        {
            // Инициализираме настройките за сериализатора
            _serializerSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Auto,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };

            // Настройки за десериализация
            _deserializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Auto,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };
        }

        /// <summary>
        /// Сериализира обект до JSON низ, като игнорира циклични референции
        /// </summary>
        /// <typeparam name="T">Тип на обекта</typeparam>
        /// <param name="obj">Обект за сериализиране</param>
        /// <returns>JSON низ</returns>
        public static string SerializeObject<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, _serializerSettings);
        }

        /// <summary>
        /// Десериализира JSON низ до обект
        /// </summary>
        /// <typeparam name="T">Тип на обекта за десериализиране</typeparam>
        /// <param name="json">JSON низ</param>
        /// <returns>Десериализираният обект</returns>
        public static T DeserializeObject<T>(string json) where T : class
        {
            return JsonConvert.DeserializeObject<T>(json, _deserializerSettings);
        }
    }
} 