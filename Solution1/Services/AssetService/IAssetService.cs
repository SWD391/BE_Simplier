using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.AssetService
{
    public interface IAssetService : IService<Asset>
    {
        public Task CreateAssetService(Asset asset);
        public Task<Asset?> UpdateAssetStatusService(string assetId, string status);
        public Task DeleteAssetService(string assetId);
        public Task<List<Asset>> GetAssetPaginationService(int pageNumber, int pageSize);
        public Task<List<Asset>> GetAssetPaginationWithSearchKeyService(string searchKey, int pageNumber, int pageSize);
        public Task<Asset?> GetAssetDetailsService(string assetId);
        public Task<Asset?> UpdateAssetService(Asset asset);
        public Task<List<Asset>> GetAllAssetsService();
        public Task<int> GetNumAssetsService();
    }
}
