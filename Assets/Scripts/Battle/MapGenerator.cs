using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using UnityEngine.AI;
[DefaultExecutionOrder(-103)]
public class MapGenerator : MonoBehaviour
{
    public int width;
    public int height;
    public float cellSize;

    List<string[]> mapData = new List<string[]>();
    List<string[]> objectData = new List<string[]>();
    float position; //���݂̈ʒu(��ʍ��[�̍��W)
    string filePath;
    [SerializeField] TextAsset mapCsvTextAsset;
    [SerializeField] TextAsset objCsvTextAsset;
    [SerializeField] GameObject[] mapPrefab;
    [SerializeField] GameObject[] objPrefab;
    [SerializeField] FieldAssetsList fieldAssetsList;

    public Transform stageParent;
    public Transform objParent;

    [SerializeField] int currentLevel;

    private void Awake()
    {
        currentLevel = BattleLevelManager.Instance.currentLevel;
        Load(filePath);
        Generate_PreObjects();
        Generate_Tile();
        Generate_Object();
    }
    void Load(string filePath)
    {
        try
        {
            mapCsvTextAsset = BattleLevelManager.Instance.levelDatas[currentLevel].map_csv;
            objCsvTextAsset = BattleLevelManager.Instance.levelDatas[currentLevel].obj_csv;
        }
        catch
        {
            Debug.LogError("�}�b�vcsv�̓ǂݍ��݂Ɏ��s���܂����B level:" + currentLevel);
        }

        mapData = new List<string[]>();
        StringReader reader = new StringReader(mapCsvTextAsset.text);


        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            mapData.Add(line.Split(',')); // ���X�g�ɓ����
            height++; // �s�����Z
        }
        width = mapData[0].Count();

        reader = new StringReader(objCsvTextAsset.text);

        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            objectData.Add(line.Split(',')); // ���X�g�ɓ����

        }
    }
    private void Generate_PreObjects()
    {
        List<GameObject> preList = BattleLevelManager.Instance.levelDatas[currentLevel].preInstantPrefabs;

        for (int i = 0; i < preList.Count; i++)
        {
            GameObject go = Instantiate(preList[i]);
            go.transform.parent = transform;
        }
    }
    // ��ʃX�N���[���i�ʒu�̍X�V�j
    public void Generate_Tile()
    {
        //�X�e�[�W�A�Z�b�g���
        int stageType = BattleLevelManager.Instance.levelDatas[currentLevel].stageType;
        //�X�J�C�{�b�N�X�̕ύX
        //RenderSettings.skybox = fieldAssetsList.skyboxes[stageType];
        mapPrefab = fieldAssetsList.datas[stageType].prefabList;
        Debug.Log("�X�e�[�W�A�Z�b�g�F" + stageType);
        Vector3 offset = new Vector3(-((width * cellSize) / 2 - cellSize / 2), -1, ((height * cellSize) / 2 - cellSize / 2));
        for (int cellX = 0; cellX < width; cellX++)
        {

            // ��[�̃Z�����牺�[�̃Z���܂��r�߂āA�G�f�[�^���z�u����Ă�����A�G�𐶐�����
            for (int cellY = 0; cellY < height; cellY++)
            {
                float x = +cellX * cellSize;
                float y = -cellY * cellSize;


                int id = int.Parse(mapData[cellY][cellX]);

                if (id == -1) { GeneratePrefab(mapPrefab[0], new Vector3(x, 0, y) + offset); }
                else if (id == 0)
                {
                    GameObject go = GeneratePrefab(mapPrefab[1], new Vector3(x, 0, y) + offset);
                    if (go.transform.childCount != 0)
                        go.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 90 * Random.Range(1, 4), 0);
                }
                else if (id == 1) { GeneratePrefab(mapPrefab[2], new Vector3(x, 0, y) + offset); }
                else if (id == 2) { GeneratePrefab(mapPrefab[3], new Vector3(x, 0, y) + offset); }
                else if (id == 3) { GeneratePrefab(mapPrefab[4], new Vector3(x, 0, y) + offset); }
                else if (id == 4) { GeneratePrefab(mapPrefab[5], new Vector3(x, 0, y) + offset); }
                else if (id == 5) { GeneratePrefab(mapPrefab[6], new Vector3(x, 0, y) + offset); }
                else if (id == 6) { GeneratePrefab(mapPrefab[7], new Vector3(x, 0, y) + offset); }
                else if (id == 7) { GeneratePrefab(mapPrefab[8], new Vector3(x, 0, y) + offset); }
                else if (id == 8) { GeneratePrefab(mapPrefab[9], new Vector3(x, 0, y) + offset); }
                else if (id == 9) { GeneratePrefab(mapPrefab[10], new Vector3(x, 0, y) + offset); }
                else if (id == 10) { GeneratePrefab(mapPrefab[11], new Vector3(x, 0, y) + offset); }
                else if (id == 11) { GeneratePrefab(mapPrefab[12], new Vector3(x, 0, y) + offset); }
                else if (id == 12) { GeneratePrefab(mapPrefab[13], new Vector3(x, 0, y) + offset); }
                else if (id == 13) { GeneratePrefab(mapPrefab[14], new Vector3(x, 0, y) + offset); }
                else if (id == 14) { GeneratePrefab(mapPrefab[15], new Vector3(x, 0, y) + offset); }
                else if (id == 15) { GeneratePrefab(mapPrefab[16], new Vector3(x, 0, y) + offset); }
                else if (id == 16) { GeneratePrefab(mapPrefab[17], new Vector3(x, 0, y) + offset); }
                else if (id == 17) { GeneratePrefab(mapPrefab[18], new Vector3(x, 0, y) + offset); }
                else if (id == 18) { GeneratePrefab(mapPrefab[19], new Vector3(x, 0, y) + offset); }
                else if (id == 19) { GeneratePrefab(mapPrefab[20], new Vector3(x, 0, y) + offset); }


                else { System.Diagnostics.Debug.Assert(false, "ID" + id + "�Ԃ̐��������͖������ł��B"); continue; }
            }
        }
        //NavMesh����
        GetComponent<NavMeshSurface>().BuildNavMesh();
    }
    GameObject GeneratePrefab(GameObject prefab, Vector3 position)
    {
        if (BattleLevelManager.Instance.ObjectExists(currentLevel, position))
        {
            Debug.Log("Exists! level�F" + currentLevel + " objName�F" + prefab.gameObject.name + " pos�F" + position);
            return null;
        }
        GameObject currentGO = Instantiate(prefab);
        currentGO.transform.position = position;
        currentGO.transform.parent = stageParent;
        return currentGO;
    }
    public void Generate_Object()
    {
        width = objectData[0].Length;
        height = objectData.Count;
        cellSize = 8f / 3f;
        Vector3 offset = new Vector3(-((width * cellSize) / 2 - cellSize / 2), 0, ((height * cellSize) / 2 - cellSize / 2));

        for (int cellX = 0; cellX < width; cellX++)
        {

            // ��[�̃Z�����牺�[�̃Z���܂��r�߂āA�G�f�[�^���z�u����Ă�����A�G�𐶐�����
            for (int cellY = 0; cellY < height; cellY++)
            {
                float x = +cellX * cellSize;
                float y = -cellY * cellSize;


                int id = int.Parse(objectData[cellY][cellX]);
                // �ԍ��ɉ����ēG�𐶐�����
                if (id == -1) { continue; } // -1�͋󔒂Ȃ̂ŁA�������Ȃ�
                else if (id == 0)//�t���A�̓����A�ォ��~��Ă����Ȃ炱�̈ʒu�Ƀv���C���[���ړ�(�o��(index3��)�������ɐ���)
                {
                    GeneratePrefab(objPrefab[3], new Vector3(x, -0.5f, y) + offset);
                    if (BattleLevelManager.Instance.startType == BattleLevelManager.StartType.GoUp)
                    {
                        GameObject.FindWithTag("Player").transform.position = new Vector3(x, -0.5f, y + 2) + offset;
                        MyCamera.Instance.SetPos(new Vector3(x, 0, y) + offset);
                    }
                }
                else if (id == 1)//�t���A�̏o���A������オ���Ă����Ȃ炱�̈ʒu�Ƀv���C���[���ړ�
                {
                    GeneratePrefab(objPrefab[id], new Vector3(x, 0, y) + offset);
                    if (BattleLevelManager.Instance.startType == BattleLevelManager.StartType.GetDown)
                    {
                        GameObject.FindWithTag("Player").transform.position = new Vector3(x, -0.5f, y - 2) + offset;
                        MyCamera.Instance.SetPos(new Vector3(x, 0, y) + offset);
                    }

                }
                else if (id == 2) { GeneratePrefab(objPrefab[id], new Vector3(x, 0, y) + offset); }
                else if (id == 3) { GeneratePrefab(objPrefab[id], new Vector3(x, 0, y) + offset); }
                //�z��
                else if (id == 10) { GeneratePrefab(objPrefab[id], new Vector3(x, -0.5f, y) + offset); }
                else if (id == 11) { GeneratePrefab(objPrefab[id], new Vector3(x, -0.5f, y) + offset); }
                else if (id == 12) { GeneratePrefab(objPrefab[id], new Vector3(x, -0.5f, y) + offset); }
                else if (id == 13) { GeneratePrefab(objPrefab[id], new Vector3(x, -0.5f, y) + offset); }
                //��������G�l�~�[
                else if (id == 20) { GameObject go = GeneratePrefab(objPrefab[id], new Vector3(x, 0, y) + offset); if (go != null) go.GetComponent<Character>().SetInitPos(new Vector3(x, 0, y) + offset); }//�X�P���g��
                else if (id == 21) { GameObject go = GeneratePrefab(objPrefab[id], new Vector3(x, 0, y) + offset); if (go != null) go.GetComponent<Character>().SetInitPos(new Vector3(x, 0, y) + offset); }//�|�X�P���g��
                else if (id == 22) { GameObject go = GeneratePrefab(objPrefab[id], new Vector3(x, 1, y) + offset);}//�C�J
                else if (id == 23) { GameObject go = GeneratePrefab(objPrefab[id], new Vector3(x, 0, y) + offset); if (go != null) go.GetComponent<Character>().SetInitPos(new Vector3(x, 0, y) + offset); }//�X�P���g��2


                //��������M�~�b�N
                else if (id == 50) { GameObject go = GeneratePrefab(objPrefab[id], new Vector3(x, 0, y) + offset); }//�G�S�łŊJ���Q�[�g
                else if (id == 51) { GameObject go = GeneratePrefab(objPrefab[id], new Vector3(x, 0, y) + offset); go.tag = "Gate0"; }//�C�x���g�ŊJ���Q�[�g
                else if (id == 52) { GameObject go = GeneratePrefab(objPrefab[id], new Vector3(x, 0, y) + offset); go.tag = "Gate1"; }//�C�x���g�ŊJ���Q�[�g
                else if (id == 53) { GameObject go = GeneratePrefab(objPrefab[id], new Vector3(x, 0, y) + offset); go.tag = "Gate2"; }//�C�x���g�ŊJ���Q�[�g
                //if (id == 1) { currentGO = Instantiate(mapPrefab[1]); currentGO.transform.position = new Vector3(x, 0, y) + offset; UnityEngine.Debug.Log("����"); }
                else { Debug.LogError("ID" + id + "�Ԃ̐��������͖������ł��B"); continue; }
            }
        }
    }
}
