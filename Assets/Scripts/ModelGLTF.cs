using System;
using System.Collections;
using System.Collections.Generic;
using AR.Constant;
using UnityEngine;

public class ModelGLTF : MonoBehaviour
{
    public static ModelGLTF Instance;
    [SerializeField] private GameObject _assetUnloader;
    [SerializeField] public DataUser userConfig; 
    public List<GameObject> meshObjects;
    [SerializeField] private float  _porcentage;
    public LoadBIMData  _loadBimData;
    public GameObject root;
    public Shader shaderMaterial;
    public Material shaderMaterial2;
    public List<Material> _materials;
    public Dictionary<string, Transform> objs = new Dictionary<string, Transform>();
    private float _initialScale;
    private Vector3 _initialPosition;
    public float minHeight;
    public float maxHeight;
    public Vector3 minBound;
    public Vector3 maxBound;
    public int batchSize;
    public Bounds bounds = new Bounds();

    public int numMeshObjects;
    public Model modelInfo;
    public bool _haveProcessRunning;
    public Constants.ActualProcess ActualProcess;
    public float InitialScale
    {
        get => _initialScale;
        set => _initialScale = value;
    }

    public Vector3 InitialPosition
    {
        get => _initialPosition;
        set => _initialPosition = value;
    }
    private void Awake()
    {
        if (Instance)
        {
            Destroy(this.gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(this);
    }

    public GameObject GetAssetUnloader()
    {
        return _assetUnloader;
    }
    public void SetAssetUnloader()
    {
        _assetUnloader = Instance.root;
    }
    public void Delete3DModel()
    {
        if (_assetUnloader)
        {
            Destroy(_assetUnloader.gameObject);
            _assetUnloader = null;
            meshObjects.Clear();
            _materials.Clear();
            objs.Clear();
            _loadBimData.IfcWall.Clear();
            _loadBimData.IfcDoor.Clear();
            _loadBimData.IfcWindow.Clear();
            _loadBimData.IfcSlab.Clear();
            _loadBimData.IfcBeam.Clear();
            _loadBimData.IfcColumn.Clear();
            _loadBimData.IfcStair.Clear();
            _loadBimData.IfcBuildingElementProxy.Clear();
            _loadBimData.IfcCurtainWall.Clear();
            _loadBimData.IfcSpace.Clear();
            _loadBimData.IfcGridAxis.Clear();
            _loadBimData.IfcGrid.Clear();
            _loadBimData.IfcFlowTerminal.Clear();
            _loadBimData.IfcRoof.Clear();
            _loadBimData.IfcPlate.Clear();
            _loadBimData.IfcMember.Clear();
            _loadBimData.IfcRailing.Clear();
            _loadBimData.IfcFooting.Clear();
            _loadBimData.IfcCovering.Clear();
            _loadBimData.IfcBuilding.Clear();
            _loadBimData.IfcFlowFitting.Clear();
            _loadBimData.IfcFlowSegment.Clear();
            _loadBimData.IfcBuildingStorey.Clear();
        }
    }

    public void DisableModelAsync()
    {
        if (_assetUnloader)
        {
            StartCoroutine(IDisableObjects());
        }
    }
    public void DisableModel()
    {
        if (_assetUnloader)
        {
            DisableObjects();
        }
    }

    public void EnableModelAsync()
    {
        if (_assetUnloader)
        {
            // _assetUnloader.gameObject.SetActive(true);
            StartCoroutine(IEnableObjects());
        }
    }
    public void EnableModel()
    {
        if (_assetUnloader)
        {
            EnableObjects();
        }
    }
    public void DisableObjects()
    {
        foreach (var obj in meshObjects)
        {
            obj.SetActive(false);
        }
    }
    public void EnableObjects()
    {
        foreach (var obj in meshObjects)
        {
            obj.SetActive(true);
        }
    }
    IEnumerator IDisableObjects()
    {
        var _batch = (int)(meshObjects.Count / _porcentage);
        Debug.Log(_batch);
        var count = 0;
        foreach (var obj in meshObjects)
        {
            obj.SetActive(false);
            count++;
            if (count >= _batch)
            {
                count = 0;
                yield return null;
            }
        }
    }
    IEnumerator IEnableObjects()
    {
        var _batch = (int)(meshObjects.Count / _porcentage);
        Debug.Log(_batch);
        var count = 0;
        foreach (var obj in meshObjects)
        {
            obj.SetActive(true);
            count++;
            if (count >= _batch)
            {
                count = 0;
                yield return null;
            }
        }
    }
    public void ResetScale()
    {
        if (_assetUnloader)
        {
            _assetUnloader.transform.localScale = Vector3.one ;
        }
    }
    public void ReduceScale1to20()
    {
        if (_assetUnloader)
        {
            _assetUnloader.transform.localScale = Vector3.one * (1f / 20f);

        }
    }
    public void ReduceScale1to50()
    {
        if (_assetUnloader)
        {
            _assetUnloader.transform.localScale = Vector3.one * (1f / 50f);

        }
    }

    public void SeparateModel()
    {
        _assetUnloader.transform.parent = null;
    }

    public void JoinModel()
    {
        _assetUnloader.transform.parent = this.transform;
    }
}
