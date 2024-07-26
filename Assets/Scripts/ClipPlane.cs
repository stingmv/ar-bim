using UnityEngine;
using Debug = UnityEngine.Debug;
using Task = System.Threading.Tasks.Task;

public class ClipPlane : MonoBehaviour
{
    private Material[] mats;
    [Range(0,1)]
    [SerializeField] private float _alpha;
    private static readonly int PlaneNormal = Shader.PropertyToID("_PlaneNormal");
    private static readonly int PlanePosition = Shader.PropertyToID("_PlanePosition");
    private static readonly int Alpha = Shader.PropertyToID("_Alpha");

    private float _height;
    public float Height
    {
        get => _height;
        set
        {
            _height = value;
            UpdateHeight(_height);
        }
    }
    private void Start()
    {
        mats = ModelGLTF.Instance._materials.ToArray();
    }
    private void UpdateHeight(float height)
    {
        var temp = Vector3.up * height;
        for (int i = 0; i < mats.Length; i++)
        {
            mats[i].SetVector(PlanePosition, temp);
            mats[i].SetVector(PlaneNormal, transform.up);
        }
        // TaskRun(temp);
    }

    async void TaskRun(Vector3 temp)
    {
        Task task =Task.Run( () =>
        {
            Debug.Log("sd");
            for (int i = 0; i < mats.Length; i++)
            {
                Debug.Log("vv");
                mats[i].SetVector(PlanePosition, temp);
                mats[i].SetVector(PlaneNormal, transform.up);
            }
        });
        await task;
    }
}
