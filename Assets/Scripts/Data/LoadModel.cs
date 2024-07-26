using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using GLTFast;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class LoadModel : MonoBehaviour
{
    [SerializeField] private string _extension;
    [SerializeField] private Image _loadImage;
    [SerializeField] private UnityEvent _initLoad;
    [SerializeField] private UnityEvent<string> _onError;
    [SerializeField] private UnityEvent<string> _onMessage;
    [SerializeField] private UnityEvent<string> _onMessageSuccess;
    [SerializeField] private UnityEvent _onLoad;

    private Stopwatch timeExecution;
    private int numElementsUsed;

    private float _porcentage;
    private const int zipBufferFile = 4096;
    public static XmlDocument xmlDoc = new XmlDocument();
    private int numChilds;
    private Transform childs;
    private Dictionary<string, Transform> objs = new Dictionary<string, Transform>();
    
    private static readonly int BaseColorFactor = Shader.PropertyToID("baseColorFactor");
    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
    private static readonly int Alpha = Shader.PropertyToID("_Alpha");
    private float progress;
    private Dictionary<string, XmlNode> nodeProperties = new Dictionary<string, XmlNode>();

    public float Porcentage
    {
        get => _porcentage;
        set
        {
            _porcentage = value;
            _loadImage.fillAmount = _porcentage;
        }
    }

    public void LoadModelURL(string url)
    {
        StartCoroutine(ILoadModelURL(url));
    }
    IEnumerator ILoadModelURL(string url)
    {
        _initLoad?.Invoke();
        
        //-------------------------------- Send Web Request ----------------------------------------
        _onMessage?.Invoke("Descargando modelo");

        UnityWebRequest request = UnityWebRequest.Get(url);
        

        UnityWebRequestAsyncOperation asyncOperation = request.SendWebRequest();
        while (!asyncOperation.isDone)
        {
            Porcentage = asyncOperation.progress;
            yield return null;
        }
        if (request.responseCode < 400)
        {
            var stream = new MemoryStream(request.downloadHandler.data);
            var zipFile = new ZipFile(stream);
            
            //---------------------------------- XML file storage ---------------------------------------
            _onMessage?.Invoke("Cargando datos xml");
            Porcentage = 0;
            
            Stream memoryStream = null;
            Debug.Log("Fase carga xml");
            foreach (ZipEntry zipEntry in zipFile)
            {
                var checkingFileExtension = GetFileExtension(zipEntry.Name, false);
                if (checkingFileExtension == "xml")
                {
                    memoryStream = ZipFileToStream(out checkingFileExtension, zipEntry, zipFile);
                    // _xmlDocument = XmlParser.Parse(memoryStream);
                    xmlDoc.Load(memoryStream);
                    Porcentage = 1;
                    break;
                }
            }
            
            //---------------------------------- GLB file storage ---------------------------------------
            _onMessage?.Invoke("Cargando de modelo 3D");
            Porcentage = 0;
            
            Debug.Log("Fase carga modelo 3D");
            string fileExtension = "glb";
            MemoryStream streamGLB = null;
            foreach (ZipEntry zipEntry in zipFile)
            {
                if (!zipEntry.IsFile)
                {
                    continue;
                }
                var checkingFileExtension = GetFileExtension(zipEntry.Name, false);
                if (checkingFileExtension == fileExtension)
                {
                    Debug.Log("glb exist");
                    streamGLB = ZipFileToStream(out fileExtension, zipEntry, zipFile);
                }
            }
            Porcentage = 1;
            byte[] buffer = new byte[zipBufferFile];
            if (streamGLB != null) buffer = streamGLB.ToArray();
            
            //---------------------------------- Load GLB file ---------------------------------------
            _onMessage?.Invoke("Lectura de geometría de modelo 3D");
            Porcentage = 0;
            LoadGltfBinaryFromMemory(buffer);
        }
    }
    
    private string GetFileExtension(string path, bool includeDot)
    {
        if (string.IsNullOrWhiteSpace(path))
            return (string)null;
        path = LimpiarRuta(path);
        int num = path.LastIndexOf('.');
        return num < 0 ? (string)null : path.Substring(includeDot ? num : num + 1).ToLowerInvariant();
    } 
    
    private string LimpiarRuta(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return (string) null;
        char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
        Remove<char>(ref invalidFileNameChars, Path.AltDirectorySeparatorChar);
        Remove<char>(ref invalidFileNameChars, Path.DirectorySeparatorChar);
        Remove<char>(ref invalidFileNameChars, Path.PathSeparator);
        Remove<char>(ref invalidFileNameChars, Path.VolumeSeparatorChar);
        path = string.Join("_", path.Split(invalidFileNameChars, StringSplitOptions.RemoveEmptyEntries));
        path = path.Replace('\\', '/');
        return path;
    }
    public static void Remove<T>(ref T[] arr, T item)
    {
        int num = Array.IndexOf<T>(arr, item);
        if (num < 0)
            return;
        for (int index = num; index < arr.Length - 1; ++index)
            arr[index] = arr[index + 1];
        Array.Resize<T>(ref arr, arr.Length - 1);
    }
    
    private MemoryStream ZipFileToStream(out string fileExtension, ZipEntry zipEntry, ZipFile zipFile )
    {
        var buffer = new byte[zipBufferFile];
        var zipFileStream = zipFile.GetInputStream(zipEntry);
        var memoryStream = new MemoryStream(zipBufferFile);
        StreamUtils.Copy(zipFileStream, memoryStream, buffer);
        memoryStream.Seek(0, SeekOrigin.Begin);
        zipFileStream.Close();
        fileExtension = GetFileExtension(zipEntry.Name, false);
        return memoryStream;
    }
    async void LoadGltfBinaryFromMemory(byte[] memoryStream)
    {
        GameObject root = new GameObject();
        ModelGLTF.Instance.root = root;
        var gltf = new GltfImport();
        var settings = new ImportSettings()
        {
            AnimationMethod = AnimationMethod.None,
            AnisotropicFilterLevel = 3,
            GenerateMipMaps = true,
            NodeNameMethod = NameImportMethod.OriginalUnique
        };
        bool success = await gltf.LoadGltfBinary(
            memoryStream,
            importSettings:settings
        );
        Debug.Log(success);

        //---------------------------------- Load GLB file ---------------------------------------
        _onMessage?.Invoke("Generando modelo 3D");
        Porcentage = 0;
        try
        {
            if (success)
            {
                success = await gltf.InstantiateMainSceneAsync(ModelGLTF.Instance.root.transform);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
            throw;
        }
        Debug.Log(success);
        
        Porcentage = 1;
        
        StartCoroutine(ProcesarElementos());
    }
    IEnumerator ProcesarElementos()
    {
        //---------------------------------- Update materials ---------------------------------------
        _onMessage?.Invoke("Actualizando materiales");
        Porcentage = 0;
        Debug.Log("Fase cambio materiales");
        numChilds = ModelGLTF.Instance.root.transform.GetChild(0).childCount;
        // childs = transform.GetComponentsInChildren<Transform>();
        childs = ModelGLTF.Instance.root.transform.GetChild(0);
        Transform trnTemp = ModelGLTF.Instance.root.transform;
        progress = 0;
        for (int i = 0; i < numChilds; i+= ModelGLTF.Instance.batchSize)
        {
            int startIndx = i;
            int endIndx = Mathf.Min(i + ModelGLTF.Instance.batchSize, numChilds);
            StartCoroutine(TaskEn(startIndx, endIndx, trnTemp));
            
        }
        yield return new WaitUntil(() => Porcentage >= 1f);
        //---------------------------------- charge BIM information to objects---------------------------------------
        // Debug.Log("Fase lectura xml");
        StartCoroutine(ReadXML());

    }
    IEnumerator TaskEn(int startIndx, int endIndx,Transform trnTemp)
    {
        Transform child = null;
        for (int j = startIndx; j < endIndx; j++)
        {
            child = childs.GetChild(j);
            if (child && child != trnTemp)
            {
                child.AddComponent<MeshCollider>();
                ModelGLTF.Instance.objs.Add(child.name, child);
                var render = child.GetComponentsInChildren<MeshRenderer>();
                ModelGLTF.Instance.bounds.Encapsulate(render[0].bounds);
                for (int i = 0; i < render.Length; i++)
                {
                    var material = render[i].material;
                    var baseColor = material.GetColor(BaseColorFactor);
                    // material.shader = ModelGLTF.Instance.shaderMaterial;
                    material = ModelGLTF.Instance.shaderMaterial2; 
                    material.SetColor(BaseColor, baseColor);
                    material.SetFloat(Alpha, 1);
                    render[i].material = Instantiate(material);
                    ModelGLTF.Instance._materials.Add(render[i].material );
                    ModelGLTF.Instance.meshObjects.Add(render[i].gameObject);
                }
                progress++;
                Porcentage = 1f * progress / numChilds;
            }
            yield return null;
        }
    }
    IEnumerator ReadXML()
    {
        // Get base path
        // string basePath = @"//ifc/decomposition";
        string basePath = @"//ifc";
        var childsTemp = xmlDoc.SelectSingleNode(basePath + "/properties")!;
        var nodeProject = xmlDoc.SelectSingleNode(basePath + "/decomposition/IfcProject");
        Debug.Log("Fase lectura propiedades BIM");
        progress = 0;
        int nodesCount = childsTemp.ChildNodes.Count;
        int batchSizeT = nodesCount / 200;
        int index = 0;
        //---------------------------------- Save proerties ---------------------------------------
        _onMessage?.Invoke("Lectura y almacenamiento de propiedades");
        Porcentage = 0;
        foreach (XmlNode xmlNode in childsTemp.ChildNodes)
        {
            var nod = xmlNode.Attributes.GetNamedItem("id").Value;
            nodeProperties.Add(nod, xmlNode) ;
            // Debug.Log(xmlNode.Attributes);
            progress++;
            index++;
            Porcentage = 1f * progress / nodesCount;
            if (index >= batchSizeT)
            {
                index = 0;
                yield return null;
            }
        }
        //---------------------------------- Save quantities ---------------------------------------
        _onMessage?.Invoke("Lectura y almacenamiento de quantities");
        Porcentage = 0;
        // Debug.Log("Fase lectura propiedades quiantities BIM");
        childsTemp = xmlDoc.SelectSingleNode(basePath + "/quantities");
        progress = 0;
        nodesCount = childsTemp.ChildNodes.Count;
        index = 0;
        foreach (XmlNode xmlNode in childsTemp.ChildNodes)
        {
            var nod = xmlNode.Attributes.GetNamedItem("id").Value;
            nodeProperties.Add(nod, xmlNode);
            // nodeProperties.Add(xmlNode.Attributes.First().Value.ToString(), xmlNode) ;
            progress++;
            index++;
            Porcentage = 1f * progress / nodesCount;
            if (index >= batchSizeT)
            {
                index = 0;
                yield return null;
            }
        }
        yield return null;
        ModelGLTF.Instance.root.name = "Model3D (IFC)";
        //---------------------------------- Write and charge BIM data ---------------------------------------
        _onMessage?.Invoke("Cargando datos BIM");
        progress = 0;
        Porcentage = 0;
        Debug.Log("Fase escritura informacion BIM");
        StartCoroutine(AddElement(nodeProject, ModelGLTF.Instance.root.transform));
        
        yield return new WaitUntil(() => Porcentage >= .99f);
        yield return new WaitForSeconds(.2f);
        Debug.Log("Proceso escritura BIM terminada");
        _onMessageSuccess?.Invoke("Modelo cargado exitosamente");
        yield return new WaitForSeconds(2f);
        ModelGLTF.Instance.root.transform.parent = ModelGLTF.Instance.transform;
        ModelGLTF.Instance.SetAssetUnloader();
        _onLoad?.Invoke();
    }
    IEnumerator  AddElement(XmlNode nodo, Transform parent)
    {
        XmlNode item;
        GameObject go = null;
    
        if (nodo.Attributes != null && (item = nodo.Attributes.GetNamedItem("id")) != null)
        {
            string idName = item.Value;
            var tr = ModelGLTF.Instance.objs.TryGetValue(idName, out var obj) ? obj : null ;
    
            if (!tr)
            {
                go = new GameObject();
                tr = go.transform;
            }
            if (tr)
            {
                go = tr.gameObject;
                go.name = nodo.Attributes.GetNamedItem("Name").Value;

                tr.parent = parent;
                //yield return StartCoroutine(AddProperties(nodo, tr));
                // Add IFC Data
                IFCData ifcData = tr.gameObject.AddComponent<IFCData>();
                //var attributes = nodo.Attributes;
                ifcData.IFCClass = nodo.Name;
                if (nodo.Attributes.GetNamedItem("ObjectType") != null)
                {
                    ifcData.ObjectType = nodo.Attributes.GetNamedItem("ObjectType").Value;

                }
                GetCategories(nodo.Name, go);
                ifcData.STEPId = nodo.Attributes.GetNamedItem("id").Value;
                ifcData.STEPName = nodo.Attributes.GetNamedItem("Name").Value;
                var numChildrenNode = nodo.ChildNodes.Count; 
                foreach (XmlNode childNode in nodo.ChildNodes)
                {
                    switch (childNode.Name)
                    {
                        case "IfcPropertySet":
                        case "IfcElementQuantity":
                            string link = childNode.Attributes.GetNamedItem("xlink:href").Value.Substring(1);
                            // string path = $"//ifc/properties/IfcPropertySet[@id='{link}']";
                            // if (nodeC.Name == "IfcElementQuantity")
                            //     path = $"//ifc/quantities/IfcPropertySet[@id='{link}']";
                            if (nodeProperties.TryGetValue(link, out var propertySet))
                            {              
                                
                                IFCPropertySet myPropertySet = new IFCPropertySet();
                                myPropertySet.propSetName = propertySet.Attributes.GetNamedItem("Name").Value;
                                myPropertySet.propSetId = propertySet.Attributes.GetNamedItem("id").Value;
                                
                                if (myPropertySet.properties == null)
                                    myPropertySet.properties = new List<IFCProperty>();

                                foreach (XmlNode propertieNode in propertySet.ChildNodes)
                                {
                                    string propName, propValue = "";
                                    IFCProperty myProp = new IFCProperty();
                                    var chilNode = propertieNode;
                                    var attribute = propertieNode.Attributes;
                                    if (attribute.Count > 0)
                                    {
                                        propName = propertieNode.Attributes.GetNamedItem("Name").Value.ToString();
                                        if (propertieNode.Name == "IfcPropertySingleValue")
                                        {
                                            if (propertieNode.Attributes.GetNamedItem("NominalValue") != null )
                                            {                        
                                                propValue = propertieNode.Attributes.GetNamedItem("NominalValue").Value;
                                            }
                                        }
                                        if (propertieNode.Name == "IfcQuantityLength")
                                            propValue = propertieNode.Attributes.GetNamedItem("LengthValue").Value.ToString();
                                        if (propertieNode.Name == "IfcQuantityArea")
                                            propValue = propertieNode.Attributes.GetNamedItem("AreaValue").Value.ToString();
                                        if (propertieNode.Name == "IfcQuantityVolume")
                                            propValue = propertieNode.Attributes.GetNamedItem("VolumeValue").Value.ToString();
                                        myProp.propName = propName;
                                        myProp.propValue = propValue;
                                        myPropertySet.properties.Add(myProp);
                                    }
                                    yield return null;
                                
                                }
                                
                                if (childNode.Name == "IfcPropertySet")
                                    ifcData.propertySets.Add(myPropertySet);
                                if (childNode.Name == "IfcElementQuantity")
                                    ifcData.quantitySets.Add(myPropertySet);
                            }
                            break;
                    }
                    if (obj)
                    {
                        progress += 1f / numChildrenNode;
                        Porcentage = 1f * progress / numChilds;
                    }
                    StartCoroutine(AddElement(childNode, tr));
                    yield return null;
                }    
            }
        }
        yield return null;
    }
    
    public bool GetAttribute(XmlNode node, out string key, string value)
    {
        if (node.Attributes.GetNamedItem(value) != null)
        { 
            key = node.Attributes.GetNamedItem(value).Value;
            return true;
        }

        key = (string)null;
        return false;
    }
    public void GetCategories(string classname, GameObject go)
    {
        switch (classname)
        {
            case "IfcWall":
            case "IfcWallStandardCase":
                ModelGLTF.Instance._loadBimData.IfcWall.Add(go);
                break;
            case "IfcDoor":
                ModelGLTF.Instance._loadBimData.IfcDoor.Add(go);
                break;
            case "IfcWindow":
                ModelGLTF.Instance._loadBimData.IfcWindow.Add(go);
                break;
            case "IfcSlab":
                ModelGLTF.Instance._loadBimData.IfcSlab.Add(go);
                break;
            case "IfcBeam":
                ModelGLTF.Instance._loadBimData.IfcBeam.Add(go);
                break;
            case "IfcColumn":
                ModelGLTF.Instance._loadBimData.IfcColumn.Add(go);
                break;
            case "IfcStair":
                ModelGLTF.Instance._loadBimData.IfcStair.Add(go);
                break;
            case "IfcBuildingElementProxy":
                ModelGLTF.Instance._loadBimData.IfcBuildingElementProxy.Add(go);
                break;
            case "IfcCurtainWall":
                ModelGLTF.Instance._loadBimData.IfcCurtainWall.Add(go);
                break;
            case "IfcSpace":
                ModelGLTF.Instance._loadBimData.IfcSpace.Add(go);
                break;
            case "IfcGridAxis":
                ModelGLTF.Instance._loadBimData.IfcGridAxis.Add(go);
                break;
            case "IfcGrid":
                ModelGLTF.Instance._loadBimData.IfcGrid.Add(go);
                break;
            case "IfcFlowTerminal":
                ModelGLTF.Instance._loadBimData.IfcFlowTerminal.Add(go);
                break;
            case "IfcRoof":
                ModelGLTF.Instance._loadBimData.IfcRoof.Add(go);
                break;
            case "IfcPlate":
                ModelGLTF.Instance._loadBimData.IfcPlate.Add(go);
                break;
            case "IfcMember":
                ModelGLTF.Instance._loadBimData.IfcMember.Add(go);
                break;
            case "IfcRailing":
                ModelGLTF.Instance._loadBimData.IfcRailing.Add(go);
                break;
            case "IfcFooting":
                ModelGLTF.Instance._loadBimData.IfcFooting.Add(go);
                break;
            case "IfcCovering":
                ModelGLTF.Instance._loadBimData.IfcCovering.Add(go);
                break;
            case "IfcBuildingStorey":
                ModelGLTF.Instance._loadBimData.IfcBuildingStorey.Add(go);
                break;
        }

    }
}

