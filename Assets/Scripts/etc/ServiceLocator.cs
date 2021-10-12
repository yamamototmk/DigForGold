using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// サービスロケータークラス
/// サービスの参照をキャッシュしておき、要求された機能に応じて実体を返す
/// </summary>
public class ServiceLocator : SingletonMonoBehaviour<ServiceLocator>
{
    Dictionary<Type, Service> serveceReferencePairs;

    //public T[] Hoge<T>(T a)
    public Service GetService(Type t)
    {
        return serveceReferencePairs[t];
    }

    public void Register(Service service)
    {
        Type type = service.GetType();
        if (serveceReferencePairs[type] == null) serveceReferencePairs.Add(type, service);
        else
        {
            serveceReferencePairs[type] = service;
            DebugLogManager dm = GetService(typeof(DebugLogManager)) as DebugLogManager;
            dm.AddLog(service.GetType().Name + " は既にキャッシュされていたため参照を更新しました。");
        }
    }
}

public interface Service
{

}