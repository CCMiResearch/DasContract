using Blazored.LocalStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DasContract.Web.Editor.Tests.Unit.LocalStorage
{
    public class LocalStorageServiceMock : ILocalStorageService
    {
        public event EventHandler<ChangingEventArgs> Changing;
        public event EventHandler<ChangedEventArgs> Changed;

        private IDictionary<string, string> _storedItems = new Dictionary<string, string>();

        public ValueTask ClearAsync(CancellationToken? cancellationToken = null)
        {
            throw new NotImplementedException();
        }

        public ValueTask<bool> ContainKeyAsync(string key, CancellationToken? cancellationToken = null)
        {
            throw new NotImplementedException();
        }

        public ValueTask<string> GetItemAsStringAsync(string key, CancellationToken? cancellationToken = null)
        {
            return ValueTask.FromResult(_storedItems[key]);
        }

        public ValueTask<T> GetItemAsync<T>(string key, CancellationToken? cancellationToken = null)
        {
            var item = JsonSerializer.Deserialize<T>(_storedItems[key]);
            return ValueTask.FromResult(item);
        }

        public ValueTask<string> KeyAsync(int index, CancellationToken? cancellationToken = null)
        {
            throw new NotImplementedException();
        }

        public ValueTask<int> LengthAsync(CancellationToken? cancellationToken = null)
        {
            throw new NotImplementedException();
        }

        public ValueTask RemoveItemAsync(string key, CancellationToken? cancellationToken = null)
        {
            _storedItems.Remove(key);
            return ValueTask.CompletedTask;
        }

        public ValueTask SetItemAsStringAsync(string key, string data, CancellationToken? cancellationToken = null)
        {
            _storedItems[key] = data;

            return ValueTask.CompletedTask;
        }

        public ValueTask SetItemAsync<T>(string key, T data, CancellationToken? cancellationToken = null)
        {
            var serialized = JsonSerializer.Serialize(data);
            _storedItems[key] = serialized;

            return ValueTask.CompletedTask;
        }
    }
}
