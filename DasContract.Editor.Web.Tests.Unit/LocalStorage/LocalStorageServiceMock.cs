using Blazored.LocalStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Tests.Unit.LocalStorage
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
            if (_storedItems.TryGetValue(key, out var val))
                return ValueTask.FromResult(val);
            return ValueTask.FromResult<string>(null);
        }

        public ValueTask<T> GetItemAsync<T>(string key, CancellationToken? cancellationToken = null)
        {
            if (_storedItems.TryGetValue(key, out var val))
            {
                var item = JsonSerializer.Deserialize<T>(val);
                return ValueTask.FromResult(item);
            }
            return ValueTask.FromResult<T>(default);
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
