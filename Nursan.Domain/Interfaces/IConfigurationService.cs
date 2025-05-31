using System.Collections.Generic;

namespace Nursan.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс за достъп до конфигурационни настройки
    /// </summary>
    public interface IConfigurationService
    {
        /// <summary>
        /// Взема стойността на настройка по ключ
        /// </summary>
        /// <param name="key">Ключ на настройката</param>
        /// <returns>Стойност на настройката или null ако няма такава</returns>
        string GetSetting(string key);

        /// <summary>
        /// Взема стойността на настройка по ключ с възможност за дефиниране на стойност по подразбиране
        /// </summary>
        /// <param name="key">Ключ на настройката</param>
        /// <param name="defaultValue">Стойност по подразбиране, ако настройката не съществува</param>
        /// <returns>Стойност на настройката или стойността по подразбиране</returns>
        string GetSetting(string key, string defaultValue);

        /// <summary>
        /// Записва стойност на настройка
        /// </summary>
        /// <param name="key">Ключ на настройката</param>
        /// <param name="value">Стойност на настройката</param>
        /// <returns>true ако записването е успешно, false в противен случай</returns>
        bool SetSetting(string key, string value);

        /// <summary>
        /// Взема всички настройки като речник
        /// </summary>
        /// <returns>Речник с всички настройки</returns>
        Dictionary<string, string> GetAllSettings();

        /// <summary>
        /// Зарежда настройките от файл или друг източник
        /// </summary>
        /// <param name="source">Път към файл или идентификатор на източник</param>
        /// <returns>true ако зареждането е успешно, false в противен случай</returns>
        bool LoadSettings(string source);

        /// <summary>
        /// Записва настройките във файл или друг източник
        /// </summary>
        /// <param name="destination">Път към файл или идентификатор на източник</param>
        /// <returns>true ако записването е успешно, false в противен случай</returns>
        bool SaveSettings(string destination);
    }
} 