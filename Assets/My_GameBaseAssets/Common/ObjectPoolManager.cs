using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ObjectPoolManager : SingletonMonoBehaviour<ObjectPoolManager>
{
    // Start is called before the first frame update
    public PoolObject[] poolObjects;
    public Dictionary<string, List<GameObject>> objectPool;
    public Transform m_Parent;
    public void Awake()
    {
        base.Awake();
        objectPool = new Dictionary<string, List<GameObject>>();
        if (m_Parent == null)
            m_Parent = transform;

        foreach (PoolObject p in poolObjects)
        {
            CreatePool(p.Prefab, p.Parent, p.CreateCount);
        }
    }

    public bool IsExistPool(GameObject obj)
    {
        return objectPool.ContainsKey(obj.name);
    }
    public List<GameObject> CreatePool(GameObject obj, Transform parent = null, int createCount = 1, bool setActive = false)
    {
        objectPool.Add(obj.name, new List<GameObject>());

        Transform curPanret = parent != null ? parent : this.m_Parent;
        for (int i = 0; i < createCount; i++)
        {
            GameObject instance = GameObject.Instantiate(obj, curPanret);
            objectPool[obj.name].Add(instance);
            if (!setActive)
                instance.SetActive(false);
        }
        return objectPool[obj.name];
    }

    /// <summary>
    /// プールされたオブジェクトを返す。プールが存在しなければ作成する。返せるオブジェクトが無ければnullを返す
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="makeIfEmpty"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public GameObject GetObject(GameObject obj, Transform parent = null, bool makeIfEmpty = true, int max = 999)
    {
        List<GameObject> list = new List<GameObject>();

        if (objectPool.ContainsKey(obj.name))
            list = objectPool[obj.name];
        else
        {
            list = CreatePool(obj, setActive: true);
            return list[0];
        }

        foreach (GameObject go in list)
        {
            if (!go.activeSelf)
            {
                if (parent != null) go.transform.parent = parent;
                else go.transform.parent = this.m_Parent;
                go.SetActive(true);
                return go;
            }
        }

        if (makeIfEmpty && list.Count < max)
        {

            GameObject instance = Instantiate(list[0], m_Parent);
            objectPool[obj.name].Add(instance);
            return instance;
        }

        print("オブジェクト生成上限に達しました :" + obj.name);
        return null;
    }
    /// <summary>
    /// プールされたオブジェクトを返す。返せるオブジェクトが無ければnullを返す
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="makeIfEmpty"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public GameObject GetObject(string key, Transform parent = null, bool makeIfEmpty = true)
    {
        List<GameObject> list = new List<GameObject>();
        IPoolObject poolObj;

        if (objectPool.ContainsKey(key))
            list = objectPool[key];
        else
        {
            Debug.Log("<color=red>PoolManagerエラー キーが存在しません key=" + key + "</color>");
            return null;
        }

        for (int i = 1; i < list.Count; i++)
        {
            if (!list[i].activeSelf)
            {
               
                if (parent == null) list[i].transform.parent = m_Parent;
                else list[i].transform.parent = parent;
                list[i].SetActive(true);

                if (list[i].TryGetComponent(out poolObj)) poolObj.Initialize();
                else
                {
                    Debug.Log("<color=red>指定されたオブジェクトはIPoolObjectではありません key=" + key + "</color>");
                    return null;
                }


                return list[i];
            }
        }

        if (makeIfEmpty)
        {
            GameObject instance = GameObject.Instantiate(list[0], objectPool[key][0].transform.parent);
            objectPool[key].Add(instance);

            if (parent == null) instance.transform.parent = m_Parent;
            else instance.transform.parent = parent;
            instance.SetActive(true);
            if (instance.TryGetComponent(out poolObj)) poolObj.Initialize();
            else
            {
                Debug.Log("<color=red>指定されたオブジェクトはIPoolObjectではありません key=" + key + "</color>");
                return null;
            }
            return instance;
        }

        print("オブジェクト生成上限に達しました :" + key);
        return null;
    }
    public void PoolOut(GameObject obj)
    {
        List<GameObject> list = new List<GameObject>();
        string name = obj.name.Replace("(Clone)", "");

        if (objectPool.ContainsKey(name))
        {
            list = objectPool[name];
        }
        else
        {
            Debug.Log(obj.name + " はプールされたオブジェクトではありません。");
            return;
        }

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Equals(obj))
            {
                list.Remove(obj);
                return;
            }
        }
        Debug.Log("プール　:" + name + "に " + obj.name + " が存在しません。");

    }
}

[System.Serializable]
/// <summary>
/// プールされるオブジェクト
/// </summary>
public class PoolObject
{
    public GameObject Prefab;
    public Transform Parent;
    public int CreateCount;
}
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(PoolObject))]
public class PoolObjectDrawer : PropertyDrawer
{
    private static float GetPropertyHeight(SerializedProperty property = null)
    {
        var height = property == null
            ? EditorGUIUtility.singleLineHeight
            : EditorGUI.GetPropertyHeight(property, true);

        return height + EditorGUIUtility.standardVerticalSpacing;
    }

    private static bool FoldoutField(ref Rect rect, SerializedProperty property, string label, string propertyName)
    {
        var prop = property.FindPropertyRelative(propertyName);
        prop.isExpanded = EditorGUI.Foldout(rect, prop.isExpanded, GUIContent.none);
        EditorGUI.PropertyField(rect, prop, new GUIContent(label));
        rect.y += GetPropertyHeight(prop);

        return prop.isExpanded;
    }
    private static void Field(ref Rect rect, SerializedProperty property, string label, string propertyName)
    {
        var prop = property.FindPropertyRelative(propertyName);
        EditorGUI.PropertyField(rect, prop, new GUIContent(label), true);
        rect.y += GetPropertyHeight(prop);
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position.height = EditorGUIUtility.singleLineHeight;


        if (FoldoutField(ref position, property, "生成するオブジェクト", "Prefab"))
        {
            EditorGUI.indentLevel++;
            Field(ref position, property, "親オブジェクト", "Parent");
            Field(ref position, property, "生成数", "CreateCount");
            EditorGUI.indentLevel--;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var enableSkinName = property.FindPropertyRelative("Prefab");
        var height = GetPropertyHeight(enableSkinName);

        if (enableSkinName.isExpanded)
        {
            height += GetPropertyHeight(property.FindPropertyRelative("Panrent"));
            height += GetPropertyHeight(property.FindPropertyRelative("CreateCount"));
        }

        return height;
    }

}
#endif