namespace Nursan.Caching
{
    public interface ILocalCache
    {
        void CacheModel<T>(string id, T model);
        T GetCachedModel<T>(string id) where T : class;
        void AddPendingOperation<T>(string operationType, T data);
        IEnumerable<PendingOperation> GetPendingOperations();
        void UpdatePendingOperationStatus(int id, string status);
        void CacheProductionPlan(IEnumerable<DailyProductionInfo> productionPlan, int fabrikaId);
        IEnumerable<DailyProductionInfo> GetCachedProductionPlan(int fabrikaId);
        void UpdateProductionPlanQuantity(string harnessModelName, string stationName, int currentQuantity);
        void BeginCacheOperation();
        void CommitCacheOperation();
        void RollbackCacheOperation();
        
        // Нови методи за кеширане на данни за баркод
        void CacheBarcodeInfo(string barcode, object barcodeInfo);
        T GetCachedBarcodeInfo<T>(string barcode) where T : class;
        
        // Нови методи за кеширане на харнес модели и техните аларми
        void CacheHarnessModelWithAlerts(string harnessModelName, object modelWithAlerts);
        T GetCachedHarnessModelWithAlerts<T>(string harnessModelName) where T : class;
        
        // Нови методи за работа с аларми
        void CacheAlerts(string harnessModelName, IEnumerable<object> alerts);
        IEnumerable<T> GetCachedAlerts<T>(string harnessModelName) where T : class;
        void UpdateAlertStatus(int alertId, bool isActive, int count);
        T GetCachedAlert<T>(int alertId) where T : class;
        
        // Нови методи за управление на аларми
        bool HasCachedAlerts(string harnessModelName);
        bool RemoveCachedAlerts(string harnessModelName);
        bool UpdateHarnessModelAlerts<T>(string harnessModelName, T alerts);
        bool DecreaseAlert<T>(string harnessModelName, int alertId, Func<T, int, T> processAlerts) where T : class;
    }
} 