using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using Nursan.Caching.DTOs;
using Nursan.Domain.Entity;
using Nursan.Logging.Messages;
using SQLitePCL;
using System.Reflection;

namespace Nursan.Caching
{

    public class LocalCacheService : ILocalCache
    {
        private readonly string _dbPath;
        private readonly Messaglama _messaglama;
        private SqliteConnection _currentConnection;
        private SqliteTransaction _currentTransaction;

        public LocalCacheService()
        {
            // Инициализация на SQLite провайдер - необходимо е за правилната работа на връзката
            Batteries.Init();
            
            _dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LocalCache.db");
            _messaglama = new Messaglama();

            // Показваме пътя до базата
            _messaglama.messanger($"SQLite база данни създадена в: {_dbPath}");

            // Проверяваме директорията дали съществува
            var dbDirectory = Path.GetDirectoryName(_dbPath);
            if (!Directory.Exists(dbDirectory))
            {
                Directory.CreateDirectory(dbDirectory);
                _messaglama.messanger($"Създадена е директория: {dbDirectory}");
            }

            try
            {
                InitializeDatabase();
                _messaglama.messanger("SQLite базата данни е инициализирана успешно");
            }
            catch (Exception ex)
            {
                _messaglama.messanger($"Грешка при инициализацията на SQLite: {ex.Message}");
            }
        }

        private void InitializeDatabase()
        {
            try
            {
                var dbDirectory = Path.GetDirectoryName(_dbPath);
                if (!Directory.Exists(dbDirectory))
                {
                    Directory.CreateDirectory(dbDirectory);
                }

                using (var connection = new SqliteConnection($"Data Source={_dbPath}"))
                {
                    connection.Open();

                    // Създаване на таблица за кеширани модели и операционни данни
                    using (var command = new SqliteCommand(
                        @"CREATE TABLE IF NOT EXISTS CachedModels (
                            Id TEXT PRIMARY KEY,
                            ModelType TEXT,
                            JsonData TEXT,
                            LastUpdated DATETIME
                        )", connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Създаване на таблица за производствен план
                    using (var command = new SqliteCommand(
                        @"CREATE TABLE IF NOT EXISTS ProductionPlan (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            HarnessModelName TEXT,
                            TargetQuantity INTEGER,
                            CurrentQuantity INTEGER,
                            StationName TEXT,
                            FamilyName TEXT,
                            FabrikaId INTEGER,
                            PlanDate DATE,
                            LastUpdate DATETIME
                        )", connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Създаване на таблица за операции в очакване
                    using (var command = new SqliteCommand(
                        @"CREATE TABLE IF NOT EXISTS PendingOperations (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            OperationType TEXT,
                            Data TEXT,
                            Status TEXT,
                            Timestamp DATETIME
                        )", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                _messaglama.messanger($"Грешка в SQLite: {ex.Message}");
                throw;
            }
        }

        public void CacheModel<T>(string id, T model)
        {
            SqliteConnection connection = null;
            bool needToCloseConnection = false;
            
            try
            {
                if (_currentConnection != null)
                {
                    connection = _currentConnection;
                }
                else
                {
                    connection = new SqliteConnection($"Data Source={_dbPath}");
                    connection.Open();
                    needToCloseConnection = true;
                }
                
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        INSERT OR REPLACE INTO CachedModels (Id, ModelType, JsonData, LastUpdated)
                        VALUES (@Id, @ModelType, @JsonData, @LastUpdated)";

                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@ModelType", typeof(T).FullName);
                    command.Parameters.AddWithValue("@JsonData", JsonHelper.SerializeObject(model));
                    command.Parameters.AddWithValue("@LastUpdated", DateTime.Now);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                _messaglama.messanger($"Грешка при кеширане на модел: {ex.Message}");
                throw;
            }
            finally
            {
                if (needToCloseConnection && connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public T GetCachedModel<T>(string id) where T : class
        {
            if (string.IsNullOrEmpty(id))
            {
                _messaglama.messanger("Грешка: ID не може да бъде празно");
                return null;
            }

            try
            {
                using (var connection = new SqliteConnection($"Data Source={_dbPath}"))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT JsonData FROM CachedModels WHERE Id = @Id AND ModelType = @ModelType";
                        command.Parameters.AddWithValue("@Id", id);
                        command.Parameters.AddWithValue("@ModelType", typeof(T).FullName);

                        var result = command.ExecuteScalar() as string;
                        if (result != null)
                        {
                            try
                            {
                                var model = JsonHelper.DeserializeObject<T>(result);
                                if (model != null)
                                {
                                    _messaglama.messanger($"Успешно зареден кеширан модел от тип {typeof(T).Name} с ID {id}");
                                    return model;
                                }
                            }
                            catch (JsonException ex)
                            {
                                _messaglama.messanger($"Грешка при десериализиране на модел: {ex.Message}");
                            }
                        }
                        else
                        {
                            _messaglama.messanger($"Не е намерен кеширан модел от тип {typeof(T).Name} с ID {id}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _messaglama.messanger($"Грешка при четене на кеширан модел: {ex.Message}");
            }
            return null;
        }

        public void AddPendingOperation<T>(string operationType, T data)
        {
            try
            {
                // Опитваме се да конвертираме данните до DTO, ако е възможно
                object dataToSerialize = data;
                
                // Проверка за известни типове и конвертиране
                if (typeof(T) == typeof(IzGenerateId))
                {
                    var model = data as IzGenerateId;
                    dataToSerialize = model.ToDto();
                }
                else if (typeof(T) == typeof(OrHarnessModel))
                {
                    // Тук може да добавите конвертиране на OrHarnessModel към DTO
                    // ако имате такава реализация
                }

                using (var connection = new SqliteConnection($"Data Source={_dbPath}"))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                            INSERT INTO PendingOperations (OperationType, ModelType, JsonData, CreatedAt, Status)
                            VALUES (@OperationType, @ModelType, @JsonData, @CreatedAt, 'Pending')";

                        command.Parameters.AddWithValue("@OperationType", operationType);
                        command.Parameters.AddWithValue("@ModelType", typeof(T).Name);
                        command.Parameters.AddWithValue("@JsonData", JsonHelper.SerializeObject(dataToSerialize));
                        command.Parameters.AddWithValue("@CreatedAt", DateTime.Now.ToString("O"));

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                _messaglama.messanger($"Грешка при добавяне на pending операция: {ex.Message}");
            }
        }

        public IEnumerable<PendingOperation> GetPendingOperations()
        {
            var pendingOps = new List<PendingOperation>();
            try
            {
                using (var connection = new SqliteConnection($"Data Source={_dbPath}"))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM PendingOperations WHERE Status = 'Pending'";
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                pendingOps.Add(new PendingOperation
                                {
                                    Id = reader.GetInt32(0),
                                    OperationType = reader.GetString(1),
                                    ModelType = reader.GetString(2),
                                    JsonData = reader.GetString(3),
                                    CreatedAt = DateTime.Parse(reader.GetString(4)),
                                    Status = reader.GetString(5)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _messaglama.messanger($"Грешка при четене на pending операции: {ex.Message}");
            }
            return pendingOps;
        }

        public void UpdatePendingOperationStatus(int id, string status)
        {
            try
            {
                using (var connection = new SqliteConnection($"Data Source={_dbPath}"))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "UPDATE PendingOperations SET Status = @Status WHERE Id = @Id";
                        command.Parameters.AddWithValue("@Status", status);
                        command.Parameters.AddWithValue("@Id", id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                _messaglama.messanger($"Грешка при обновяване на статус на pending операция: {ex.Message}");
            }
        }

        public void CacheProductionPlan(IEnumerable<DailyProductionInfo> productionPlan, int fabrikaId)
        {
            try
            {
                using (var connection = new SqliteConnection($"Data Source={_dbPath}"))
                {
                    connection.Open();

                    // Изтриваме стария план
                    using (var command = new SqliteCommand(
                        "DELETE FROM ProductionPlan WHERE FabrikaId = @FabrikaId AND Date(PlanDate) = Date(@PlanDate)", connection))
                    {
                        command.Parameters.AddWithValue("@FabrikaId", fabrikaId);
                        command.Parameters.AddWithValue("@PlanDate", DateTime.Now.Date);
                        command.ExecuteNonQuery();
                    }

                    // Създаваме нов план
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (var command = new SqliteCommand(
                                @"INSERT INTO ProductionPlan 
                                (HarnessModelName, TargetQuantity, CurrentQuantity, StationName, FamilyName, FabrikaId, PlanDate, LastUpdate)
                                VALUES 
                                (@HarnessModelName, @TargetQuantity, @CurrentQuantity, @StationName, @FamilyName, @FabrikaId, @PlanDate, @LastUpdate)",
                                connection))
                            {
                                foreach (var item in productionPlan)
                                {
                                    command.Parameters.Clear();
                                    command.Parameters.AddWithValue("@HarnessModelName", item.HarnessModelName);
                                    command.Parameters.AddWithValue("@TargetQuantity", item.TargetQuantity ?? 0);
                                    command.Parameters.AddWithValue("@CurrentQuantity", item.CurrentQuantity ?? 0);
                                    command.Parameters.AddWithValue("@StationName", item.StationName);
                                    command.Parameters.AddWithValue("@FamilyName", item.FamilyName);
                                    command.Parameters.AddWithValue("@FabrikaId", fabrikaId);
                                    command.Parameters.AddWithValue("@PlanDate", DateTime.Now.Date);
                                    command.Parameters.AddWithValue("@LastUpdate", DateTime.Now);
                                    command.ExecuteNonQuery();
                                }
                            }
                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Грешка при кеширане на производствения план: {ex.Message}");
            }
        }

        public IEnumerable<DailyProductionInfo> GetCachedProductionPlan(int fabrikaId)
        {
            try
            {
                var result = new List<DailyProductionInfo>();
                using (var connection = new SqliteConnection($"Data Source={_dbPath}"))
                {
                    connection.Open();
                    using (var command = new SqliteCommand(
                        @"SELECT * FROM ProductionPlan 
                        WHERE FabrikaId = @FabrikaId 
                        AND Date(PlanDate) = Date(@PlanDate)
                        ORDER BY FamilyName, StationName",
                        connection))
                    {
                        command.Parameters.AddWithValue("@FabrikaId", fabrikaId);
                        command.Parameters.AddWithValue("@PlanDate", DateTime.Now.Date);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.Add(new DailyProductionInfo
                                {
                                    HarnessModelName = reader["HarnessModelName"].ToString(),
                                    TargetQuantity = Convert.ToInt32(reader["TargetQuantity"]),
                                    CurrentQuantity = Convert.ToInt32(reader["CurrentQuantity"]),
                                    StationName = reader["StationName"].ToString(),
                                    FamilyName = reader["FamilyName"].ToString(),
                                    Date = Convert.ToDateTime(reader["PlanDate"])
                                });
                            }
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Грешка при четене на кеширания производствен план: {ex.Message}");
            }
        }

        public void UpdateProductionPlanQuantity(string harnessModelName, string stationName, int currentQuantity)
        {
            try
            {
                using (var connection = new SqliteConnection($"Data Source={_dbPath}"))
                {
                    connection.Open();
                    using (var command = new SqliteCommand(
                        @"UPDATE ProductionPlan 
                        SET CurrentQuantity = @CurrentQuantity, LastUpdate = @LastUpdate
                        WHERE HarnessModelName = @HarnessModelName 
                        AND StationName = @StationName 
                        AND Date(PlanDate) = Date(@PlanDate)",
                        connection))
                    {
                        command.Parameters.AddWithValue("@CurrentQuantity", currentQuantity);
                        command.Parameters.AddWithValue("@LastUpdate", DateTime.Now);
                        command.Parameters.AddWithValue("@HarnessModelName", harnessModelName);
                        command.Parameters.AddWithValue("@StationName", stationName);
                        command.Parameters.AddWithValue("@PlanDate", DateTime.Now.Date);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Грешка при обновяване на количества в производствения план: {ex.Message}");
            }
        }

        public void BeginCacheOperation()
        {
            try
            {
                if (_currentTransaction != null)
                {
                    throw new InvalidOperationException("Транзакция вече е започната");
                }

                _currentConnection = new SqliteConnection($"Data Source={_dbPath}");
                _currentConnection.Open();
                _currentTransaction = _currentConnection.BeginTransaction();
                _messaglama.messanger("Започната нова транзакция");
            }
            catch (Exception ex)
            {
                _messaglama.messanger($"Грешка при започване на нова операция: {ex.Message}");
                throw;
            }
        }

        public void CommitCacheOperation()
        {
            try
            {
                if (_currentTransaction == null)
                {
                    throw new InvalidOperationException("Няма активна транзакция за потвърждение");
                }

                _currentTransaction.Commit();
                _messaglama.messanger("Кеш операцията е успешно потвърдена");
            }
            catch (Exception ex)
            {
                _messaglama.messanger($"Грешка при потвърждение на кеш операция: {ex.Message}");
                throw;
            }
            finally
            {
                CleanupTransaction();
            }
        }

        public void RollbackCacheOperation()
        {
            try
            {
                if (_currentTransaction == null)
                {
                    throw new InvalidOperationException("Няма активна транзакция за отмяна");
                }

                _currentTransaction.Rollback();
                _messaglama.messanger("Кеш операцията е отменена");
            }
            catch (Exception ex)
            {
                _messaglama.messanger($"Грешка при отмяна на кеш операция: {ex.Message}");
                throw;
            }
            finally
            {
                CleanupTransaction();
            }
        }

        private void CleanupTransaction()
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
            if (_currentConnection != null)
            {
                _currentConnection.Close();
                _currentConnection.Dispose();
                _currentConnection = null;
            }
        }
        
        // Имплементация на нови методи за кеширане на баркод информация
        public void CacheBarcodeInfo(string barcode, object barcodeInfo)
        {
            // Използваме метода CacheModel с префикс за баркода
            CacheModel($"barcode_{barcode}", barcodeInfo);
        }
        
        public T GetCachedBarcodeInfo<T>(string barcode) where T : class
        {
            // Използваме метода GetCachedModel с префикс за баркода
            return GetCachedModel<T>($"barcode_{barcode}");
        }
        
        // Имплементация на нови методи за кеширане на харнес модели с аларми
        public void CacheHarnessModelWithAlerts(string harnessModelName, object modelWithAlerts)
        {
            // Използваме метода CacheModel с префикс за харнес модела с аларти
            CacheModel($"harness_with_alerts_{harnessModelName}", modelWithAlerts);
        }
        
        public T GetCachedHarnessModelWithAlerts<T>(string harnessModelName) where T : class
        {
            // Използваме метода GetCachedModel с префикс за харнес модела с аларти
            return GetCachedModel<T>($"harness_with_alerts_{harnessModelName}");
        }
        
        // Нови методи за управление на аларми
        
        /// <summary>
        /// Проверява дали има кеширани аларми за даден харнес модел
        /// </summary>
        /// <param name="harnessModelName">Име на харнес модела</param>
        /// <returns>true ако има кеширани аларми, false в противен случай</returns>
        public bool HasCachedAlerts(string harnessModelName)
        {
            try
            {
                using (var connection = new SqliteConnection($"Data Source={_dbPath}"))
                {
                    connection.Open();
                    using (var command = new SqliteCommand(
                        "SELECT COUNT(*) FROM CachedModels WHERE Id = @Id",
                        connection))
                    {
                        command.Parameters.AddWithValue("@Id", $"harness_with_alerts_{harnessModelName}");
                        var count = Convert.ToInt32(command.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                _messaglama.messanger($"Грешка при проверка на кеширани аларми: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Премахва алармите за даден харнес модел от кеша
        /// </summary>
        /// <param name="harnessModelName">Име на харнес модела</param>
        /// <returns>true ако операцията е успешна, false в противен случай</returns>
        public bool RemoveCachedAlerts(string harnessModelName)
        {
            try
            {
                using (var connection = new SqliteConnection($"Data Source={_dbPath}"))
                {
                    connection.Open();
                    using (var command = new SqliteCommand(
                        "DELETE FROM CachedModels WHERE Id = @Id",
                        connection))
                    {
                        command.Parameters.AddWithValue("@Id", $"harness_with_alerts_{harnessModelName}");
                        int affectedRows = command.ExecuteNonQuery();
                        _messaglama.messanger($"Премахнати аларми за харнес модел {harnessModelName}");
                        return affectedRows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                _messaglama.messanger($"Грешка при премахване на кеширани аларми: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Обновява статуса на алармите за даден харнес модел
        /// </summary>
        /// <param name="harnessModelName">Име на харнес модела</param>
        /// <param name="alerts">Нови аларми, които да бъдат кеширани</param>
        /// <returns>true ако операцията е успешна, false в противен случай</returns>
        public bool UpdateHarnessModelAlerts<T>(string harnessModelName, T alerts)
        {
            try
            {
                // Първо премахваме съществуващите кеширани аларми (ако има такива)
                RemoveCachedAlerts(harnessModelName);
                
                // След това кешираме новите аларми
                CacheHarnessModelWithAlerts(harnessModelName, alerts);
                
                _messaglama.messanger($"Обновени аларми за харнес модел {harnessModelName}");
                return true;
            }
            catch (Exception ex)
            {
                _messaglama.messanger($"Грешка при обновяване на кеширани аларми: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Намалява броя на алармите за даден харнес модел (сваля аларма)
        /// </summary>
        /// <typeparam name="T">Тип на обекта с аларми</typeparam>
        /// <param name="harnessModelName">Име на харнес модела</param>
        /// <param name="alertId">ID на алармата, която трябва да бъде свалена</param>
        /// <param name="processAlerts">Функция за обработка на алармите, която намалява броя им</param>
        /// <returns>true ако операцията е успешна, false в противен случай</returns>
        public bool DecreaseAlert<T>(string harnessModelName, int alertId, Func<T, int, T> processAlerts) where T : class
        {
            try
            {
                // Извличаме текущите аларми от кеша
                var currentAlerts = GetCachedHarnessModelWithAlerts<T>(harnessModelName);
                
                if (currentAlerts == null)
                {
                    _messaglama.messanger($"Няма намерени кеширани аларми за харнес модел {harnessModelName}");
                    return false;
                }
                
                // Прилагаме функцията за обработка, която трябва да промени алармите
                var updatedAlerts = processAlerts(currentAlerts, alertId);
                
                // Обновяваме кеша с новите аларми
                CacheHarnessModelWithAlerts(harnessModelName, updatedAlerts);
                
                _messaglama.messanger($"Намален брой аларми за харнес модел {harnessModelName}, аларма ID: {alertId}");
                return true;
            }
            catch (Exception ex)
            {
                _messaglama.messanger($"Грешка при намаляване на аларми: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Кешира списък с аларми за даден харнес модел
        /// </summary>
        /// <param name="harnessModelName">Име на харнес модела</param>
        /// <param name="alerts">Списък с аларми за кеширане</param>
        public void CacheAlerts(string harnessModelName, IEnumerable<object> alerts)
        {
            if (string.IsNullOrEmpty(harnessModelName))
            {
                _messaglama.messanger("Грешка: Името на харнес модела не може да бъде празно");
                return;
            }

            try
            {
                // Кешираме списъка с аларми под уникален ключ за харнес модела
                CacheModel($"alerts_{harnessModelName}", alerts);
                _messaglama.messanger($"Кеширани {alerts.Count()} аларми за харнес модел {harnessModelName}");
            }
            catch (Exception ex)
            {
                _messaglama.messanger($"Грешка при кеширане на аларми: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Връща кеширани аларми за даден харнес модел
        /// </summary>
        /// <typeparam name="T">Тип на алармите</typeparam>
        /// <param name="harnessModelName">Име на харнес модела</param>
        /// <returns>Списък с кеширани аларми или null, ако няма такива</returns>
        public IEnumerable<T> GetCachedAlerts<T>(string harnessModelName) where T : class
        {
            if (string.IsNullOrEmpty(harnessModelName))
            {
                _messaglama.messanger("Грешка: Името на харнес модела не може да бъде празно");
                return null;
            }

            try
            {
                // Извличаме списъка с аларми по уникалния ключ за харнес модела
                var cachedAlerts = GetCachedModel<IEnumerable<T>>($"alerts_{harnessModelName}");
                if (cachedAlerts != null)
                {
                    _messaglama.messanger($"Успешно извлечени кеширани аларми за харнес модел {harnessModelName}");
                    return cachedAlerts;
                }
                else
                {
                    _messaglama.messanger($"Няма намерени кеширани аларми за харнес модел {harnessModelName}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _messaglama.messanger($"Грешка при извличане на кеширани аларми: {ex.Message}");
                return null;
            }
        }
        
        /// <summary>
        /// Връща кеширана аларма по ID
        /// </summary>
        /// <typeparam name="T">Тип на алармата</typeparam>
        /// <param name="alertId">ID на алармата</param>
        /// <returns>Кеширана аларма или null, ако няма такава</returns>
        public T GetCachedAlert<T>(int alertId) where T : class
        {
            try
            {
                // Извличаме алармата по ID
                var cachedAlert = GetCachedModel<T>($"alert_{alertId}");
                if (cachedAlert != null)
                {
                    _messaglama.messanger($"Успешно извлечена кеширана аларма с ID {alertId}");
                    return cachedAlert;
                }
                else
                {
                    _messaglama.messanger($"Няма намерена кеширана аларма с ID {alertId}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _messaglama.messanger($"Грешка при извличане на кеширана аларма: {ex.Message}");
                return null;
            }
        }
        
        /// <summary>
        /// Актуализира статуса на кеширана аларма
        /// </summary>
        /// <param name="alertId">ID на алармата</param>
        /// <param name="isActive">Дали алармата е активна</param>
        /// <param name="count">Брой на алармата</param>
        public void UpdateAlertStatus(int alertId, bool isActive, int count)
        {
            try
            {
                // Извличаме съществуващата аларма
                var alert = GetCachedAlert<OrAlert>(alertId);
                if (alert != null)
                {
                    // Актуализираме статуса и броя
                    alert.Activ = isActive;
                    //alert.Count = count;
                    
                    // Кешираме обратно
                    CacheModel($"alert_{alertId}", alert);
                    _messaglama.messanger($"Успешно актуализиран статус на аларма с ID {alertId}");
                }
                else
                {
                    _messaglama.messanger($"Не може да се актуализира статус на аларма с ID {alertId} - не е намерена в кеша");
                }
            }
            catch (Exception ex)
            {
                _messaglama.messanger($"Грешка при актуализиране на статус на аларма: {ex.Message}");
            }
        }
    }

    public class PendingOperation
    {
        public int Id { get; set; }
        public string OperationType { get; set; }
        public string ModelType { get; set; }
        public string JsonData { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
    }
} 