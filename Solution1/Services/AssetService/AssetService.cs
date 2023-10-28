using BusinessObjects.Models;
using Repository.AssetHistoryRepository;
using Repository.AssetRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessObjects.Enums.Status;

namespace Services.AssetService 
{ 
    public class AssetService : IAssetService
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IAssetHistoryRepository _assetHistoryRepository;

        public AssetService()
        {
            _assetRepository = new AssetRepository();
            _assetHistoryRepository = new AssetHistoryRepository();
        }

        public async Task CreateAssetService(Asset asset)
        {   
            asset.AssetId = Guid.NewGuid().ToString();
            asset.Status = AssetStatus.Functional;
            asset.ImportedDate = DateTime.Now;
            await _assetRepository.CreateAsync(asset);
        }

        public async Task DeleteAssetService(string assetId)
        {
            await _assetRepository.DeleteAsync(assetId);
        }

        public async Task<Asset?> GetAssetDetailsService(string assetId)
        {
            return await _assetRepository.GetByIdAsync(assetId) ?? throw new Exception("Asset not found");
        }

        public async Task<List<Asset>> GetAssetPaginationService(int pageNumber, int pageSize)
        {
            return await _assetRepository.GetPaginationAsync(pageNumber, pageSize);
        }

        public async Task<Asset?> UpdateAssetStatusService(string assetId, AssetStatus status)
        {
            var asset = await _assetRepository.GetByIdAsync(assetId) ?? throw new Exception("Asset not found");
            asset.Status = status;

            var historyId = Guid.NewGuid().ToString();

            var desc = asset.Status switch
            {
                AssetStatus.NonFunctional => "The asset is currently non-functional and under maintenance.",
                AssetStatus.Functional => "The asset is functional and operational.",
                _ => "The asset is under maintenance."
            };

            await _assetHistoryRepository.CreateAsync(new AssetHistory()
            {
                AssetId = assetId,
                Title = $"Update asset {asset.AssetName} status",
                Description = desc,
                HistoryId = historyId
            });
            

            await _assetRepository.UpdateAsync(asset);
            return asset;
        }

        public async Task<Asset?> UpdateAssetService(Asset asset)
        {
            await _assetRepository.UpdateAsync(asset);
            return await _assetRepository.GetByIdAsync(asset.AssetId);
        }

        public async Task<List<Asset>> GetAssetPaginationWithSearchKeyService(string searchKey, int pageNumber, int pageSize)
        {
            return await _assetRepository.GetPaginationWithSearchKeyAsync(searchKey, pageNumber, pageSize);
        }

        public Task<Asset?> UpdateAssetStatusService(string assetId, string status)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Asset>> GetAllAssetsService()
        {
            return await _assetRepository.GetAllAsync();
        }

        public async Task<int> GetNumAssetsService()
        {
            return await _assetRepository.Count();
        }
    }
}
