using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RestAPI : MonoBehaviour
{
    [SerializeField] private string URL = "https://apimocha.com/bsgarbim/posts";
    [SerializeField] private DataUser _dataUser;
    [SerializeField] private bool isLocal;
    private readonly string _id = "id";
    private readonly string _name = "name";
    private readonly string _email = "email";
    private readonly string _password = "password";
    private readonly string _courses = "courses";
    private readonly string _models = "models";
    private readonly string _description = "description";
    private readonly string _image = "image";
    private readonly string _link = "link";
    
    private void Start()
    {
        if (isLocal)
        {
            var jsonTEst = Resources.Load<TextAsset>("testJson");
            SimpleJSON.JSONNode stats = SimpleJSON.JSON.Parse(jsonTEst.text);
            SaveDataAllUsers(stats);
            return;
        }
        StartCoroutine(GetData());
    }

    private IEnumerator GetData()
    {
        
        using (UnityWebRequest request = UnityWebRequest.Get(URL))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                string json = request.downloadHandler.text;
                SimpleJSON.JSONNode stats = SimpleJSON.JSON.Parse(json);
                SaveDataAllUsers(stats);
            }
        }
    }

    

    private void SaveDataAllUsers(SimpleJSON.JSONNode stats)
    {
        _dataUser.Models.models.Clear();
        for (int i = 0; i < stats.Count; i++)
        {
            DataUser.DataUserConfig dataUserConfig = new DataUser.DataUserConfig();
            dataUserConfig.Id = stats[i][_id];
            dataUserConfig.Name = stats[i][_name];
            dataUserConfig.Email = stats[i][_email];
            dataUserConfig.Password = stats[i][_password];
            dataUserConfig.Courses = new List<DataUser.Course>();
                  
            for (int j = 0; j < stats[i][_courses].Count; j++)
            {
                DataUser.Course course = new DataUser.Course();
                course.id = stats[i][_courses][j][_id];
                course.name = stats[i][_courses][j][_name];
                course.image = stats[i][_courses][j][_image];
                //course.Models = new List<DataUser.Model>();
                
                        
                for (int k = 0; k < stats[i][_courses][j][_models].Count; k++)
                {
                    Model model = new Model();
                    model.id = stats[i][_courses][j][_models][k][_id];
                    model.name = stats[i][_courses][j][_models][k][_name];
                    model.description = stats[i][_courses][j][_models][k][_description];
                    model.image = stats[i][_courses][j][_models][k][_image];
                    model.link = stats[i][_courses][j][_models][k][_link];
                    model.courseId=course.id;
                    model.userId=dataUserConfig.Id;
                    
                    _dataUser.Models.models.Add(model);
                }
                dataUserConfig.Courses.Add(course);
            }
            _dataUser.Users.Add(dataUserConfig);
        }
    }
}
