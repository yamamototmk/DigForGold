using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// �T�[�r�X���P�[�^�[�N���X
/// �T�[�r�X�̎Q�Ƃ��L���b�V�����Ă����A�v�����ꂽ�@�\�ɉ����Ď��̂�Ԃ�
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
            dm.AddLog(service.GetType().Name + " �͊��ɃL���b�V������Ă������ߎQ�Ƃ��X�V���܂����B");
        }
    }
}

public interface Service
{

}