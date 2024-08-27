using Newtonsoft.Json;
using System;

namespace HomeStuff.Models
{
    public interface IItemService
    {
        Task<List<Item>> GetPaginatedResult(int currentPage, string? query, string? locationId, double? minPrice, int? itemStatus, int? itemSetId, int pageSize = 10);
        Task<int> GetCount(string? query, string? locationId, double? minPrice, int? itemStatus, int? itemSetId);
    }

    public class ItemService : IItemService
    {
        private readonly HomeStuff.Data.SqliteContext _context;
        public ItemService(HomeStuff.Data.SqliteContext context)
        {
            _context = context;
        }

        public async Task<List<Item>> GetPaginatedResult(int currentPage, string? query, string? locationId, double? minPrice, int? itemStatus, int? itemSetId, int pageSize = 10)
        {
            var data = await GetData(query, locationId, minPrice, itemStatus, itemSetId);
            return data.OrderByDescending(i => i.LastModifiedUtc).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        }

        public async Task<int> GetCount(string? query, string? locationId, double? minPrice, int? itemStatus, int? itemSetId)
        {
            var data = await GetData(query, locationId, minPrice, itemStatus, itemSetId);
            return data.Count;
        }

        private async Task<List<Item>> GetData(string? query, string? locationId, double? minPrice, int? itemStatus, int? itemSetId)
        {
            var items = from i in _context.Item select i;
            if (!string.IsNullOrEmpty(query))
            {
                query = query.Trim();
                items = items.Where(s => s.Name.ToLower().Contains(query.ToLower()) ||
                (s.Description != null && s.Description.ToLower().Contains(query.ToLower())) ||
                (s.Manufacturer != null && s.Manufacturer.ToLower().Contains(query.ToLower())) ||
                (s.ModelNumber != null && s.ModelNumber.ToLower().Contains(query.ToLower())) ||
                (s.SerialNumber != null && s.SerialNumber.ToLower().Contains(query.ToLower())) ||
                (s.Vendor != null && s.Vendor.ToLower().Contains(query.ToLower())) ||
                (s.SKU != null && s.SKU.ToLower().Contains(query.ToLower())) ||
                (s.ItemSetId != null && _context.ItemSet.FirstOrDefault(l => l.Id == s.ItemSetId)!.Name.ToLower().Contains(query.ToLower())));
            }
            if (!string.IsNullOrEmpty(locationId))
            {
                items = items.Where(i => i.LocationId.ToString() == locationId);
            }
            if (minPrice != null)
            {
                items = items.Where(i => i.PurchasePrice >= minPrice);
            }
            if (itemStatus != null)
            {
                items = items.Where(i => i.Status == (Item.ItemStatus)itemStatus);
            }
            if (itemSetId != null)
            {
                items = items.Where(i => i.ItemSetId == itemSetId);
            }
            //if (notUpdatedSince != null)
            //{
            //    items = items.Where(i => i.LastModifiedUtc <= notUpdatedSince.Value.ToDateTime(TimeOnly.MaxValue).ToUniversalTime()); 
            //}
            foreach (var item in items)
            {
                item.Location = _context.Location.FirstOrDefault(l => l.Id == item.LocationId);
            }
            foreach (var item in items)
            {
                item.ItemSet = _context.ItemSet.FirstOrDefault(l => l.Id == item.ItemSetId);
            }
            return items.ToList();

            //Items = items.OrderByDescending(i => i.LastModifiedUtc).ToList();
        }
    }
}
