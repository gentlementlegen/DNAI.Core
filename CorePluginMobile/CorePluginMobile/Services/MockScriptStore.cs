using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CorePluginMobile.Models;
using CorePluginMobile.Services.API;

namespace CorePluginMobile.Services
{
    public class MockScriptStore : IDataStore<File>
    {
        private readonly List<File> items;

        public MockScriptStore()
        {
            items = new List<File>();
            var mockItems = new List<File>
            {
                new File { _id = Guid.NewGuid().ToString(), Title = "First item", Description="This is an item description." },
                new File { _id = Guid.NewGuid().ToString(), Title = "Second item", Description="This is an item description." }
            };

            foreach (var item in mockItems)
            {
                items.Add(item);
            }
        }

        public async Task<bool> AddItemAsync(File item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(File item)
        {
            var oldItem = items.Where((File arg) => arg._id == item._id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((File arg) => arg._id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<File> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s._id == id));
        }

        public async Task<IEnumerable<File>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}