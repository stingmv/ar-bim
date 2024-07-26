using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu (fileName = "DataUserConfig", menuName = "ScriptableObjects/DataUserConfig")]
public class DataUser : ScriptableObject
{
    [Serializable]
    public struct DataUserConfig
    {
        public int Id;
        public string Name;
        public string Email;
        public string Password;
        public List<Course> Courses;
    }
    [Serializable]
    public struct Course
    {
        public int id;
        public string name;
        public string image;
        //public List<Model> Models;
    }

    /*[Serializable]
    public struct Model
    {
        public int id;
        public string name;
        public string description;
        public string image;
        public string link;
    }*/

    public List<DataUserConfig> Users;
    [Serializable]
    public struct CourseContainer
    {
        [SerializeField]
        public List<Course> courses;
        
    }

    [SerializeField]
    public CourseContainer Courses;
    [Serializable]
    public struct ModelContainer
    {
        
        [SerializeField] public List<Model> models;
        
    }
    [SerializeField]
    public ModelContainer Models;
    [SerializeField]
    public string courseName;
    private void OnEnable()
    {
        Users.Clear();
    }

    private void Reset()
    {
        Users.Clear();
    }
}
