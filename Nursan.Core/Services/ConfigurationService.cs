using Nursan.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Nursan.Core.Services
{
    /// <summary>
    /// Услуга за достъп до конфигурационни настройки
    /// </summary>
    public class ConfigurationService : BaseService, IConfigurationService
    {
        private Dictionary<string, string> _settings;
        private readonly string _defaultConfigPath;

        /// <summary>
        /// Инициализира нова инстанция на ConfigurationService
        /// </summary>
        /// <param name="logger">Логър за записване на събития</param>
        /// <param name="defaultConfigPath">Път до конфигурационния файл по подразбиране</param>
        public ConfigurationService(ILogger logger, string defaultConfigPath = "config.json") : base(logger)
        {
            _defaultConfigPath = defaultConfigPath;
            _settings = new Dictionary<string, string>();
            LoadSettings(_defaultConfigPath);
        }

        /// <inheritdoc/>
        public string GetSetting(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                _logger.LogError("Ключът не може да бъде null или празен");
                return null;
            }

            if (_settings.TryGetValue(key, out string value))
            {
                return value;
            }

            _logger.LogDebug($"Настройката с ключ '{key}' не е намерена");
            return null;
        }

        /// <inheritdoc/>
        public string GetSetting(string key, string defaultValue)
        {
            string value = GetSetting(key);
            return value ?? defaultValue;
        }

        /// <inheritdoc/>
        public bool SetSetting(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                _logger.LogError("Ключът не може да бъде null или празен");
                return false;
            }

            _settings[key] = value;
            _logger.LogDebug($"Зададена е настройка с ключ '{key}'");
            return true;
        }

        /// <inheritdoc/>
        public Dictionary<string, string> GetAllSettings()
        {
            return new Dictionary<string, string>(_settings);
        }

        /// <inheritdoc/>
        public bool LoadSettings(string source)
        {
            try
            {
                if (string.IsNullOrEmpty(source))
                {
                    _logger.LogError("Източникът не може да бъде null или празен");
                    return false;
                }

                if (File.Exists(source))
                {
                    string json = File.ReadAllText(source);
                    _settings = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                    _logger.LogInformation($"Настройките са заредени успешно от '{source}'");
                    return true;
                }
                else
                {
                    _logger.LogWarning($"Конфигурационният файл '{source}' не съществува. Създават се нови настройки.");
                    _settings = new Dictionary<string, string>();
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Грешка при зареждане на настройки от '{source}': {ex.Message}");
                _settings = new Dictionary<string, string>();
                return false;
            }
        }

        /// <inheritdoc/>
        public bool SaveSettings(string destination)
        {
            try
            {
                if (string.IsNullOrEmpty(destination))
                {
                    destination = _defaultConfigPath;
                }

                string json = JsonSerializer.Serialize(_settings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(destination, json);
                _logger.LogInformation($"Настройките са записани успешно в '{destination}'");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Грешка при записване на настройки в '{destination}': {ex.Message}");
                return false;
            }
        }
    }
} 