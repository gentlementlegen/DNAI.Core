using System;

namespace CorePluginAndroid.Model
{
    public interface IDataService
    {
        void GetData(Action<DataItem, Exception> callback);
    }
}