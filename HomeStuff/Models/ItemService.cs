using Newtonsoft.Json;
using System;

namespace HomeStuff.Models
{
    public interface IItemService
    {
        Task<List<Item>> GetPaginatedResult(int currentPage, string? q, string? l, double? MinPrice, int? status, int pageSize = 10);
        Task<int> GetCount(string? q, string? l, double? MinPrice, int? status);
    }

    public class ItemService : IItemService
    {
        private readonly HomeStuff.Data.SqliteContext _context;
        public ItemService(HomeStuff.Data.SqliteContext context)
        {
            _context = context;
        }

        public async Task<List<Item>> GetPaginatedResult(int currentPage, string? q, string? l, double? MinPrice, int? status, int pageSize = 10)
        {
            var data = await GetData(q,l,MinPrice, status);
            return data.OrderByDescending(i => i.LastModifiedUtc).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        }

        public async Task<int> GetCount(string? q, string? l, double? MinPrice, int? status)
        {
            var data = await GetData(q,l,MinPrice, status);
            return data.Count;
        }

        private async Task<List<Item>> GetData(string? q, string? l, double? MinPrice, int? status)
        {
            var items = from i in _context.Item select i;
            if (!string.IsNullOrEmpty(q))
            {
                q = q.Trim();
                items = items.Where(s => s.Name.ToLower().Contains(q.ToLower()) ||
                (s.Description != null && s.Description.ToLower().Contains(q.ToLower())) ||
                (s.Manufacturer != null && s.Manufacturer.ToLower().Contains(q.ToLower())) ||
                (s.ModelNumber != null && s.ModelNumber.ToLower().Contains(q.ToLower())) ||
                (s.SerialNumber != null && s.SerialNumber.ToLower().Contains(q.ToLower())) ||
                (s.Vendor != null && s.Vendor.ToLower().Contains(q.ToLower())) ||
                (s.SKU != null && s.SKU.ToLower().Contains(q.ToLower())));
            }
            if (!string.IsNullOrEmpty(l))
            {
                items = items.Where(i => i.LocationId.ToString() == l);
            }
            if (MinPrice != null)
            {
                items = items.Where(i => i.PurchasePrice >= MinPrice);
            }
            if (status != null)
            {
                items = items.Where(i => i.Status == (Item.ItemStatus)status);
            }
            foreach (var item in items)
            {
                item.Location = _context.Location.FirstOrDefault(l => l.Id == item.LocationId);
            }
            return items.ToList();

            //Items = items.OrderByDescending(i => i.LastModifiedUtc).ToList();
        }
    }
}
