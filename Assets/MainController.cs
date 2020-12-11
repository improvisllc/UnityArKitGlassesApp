using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.IO;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{

    public GameObject m_debugSphere;
    public List<GameObject> m_debugSphereList = new List<GameObject>();

    public GameObject m_glasses;

    void Start()
    {

        for (int i = 0; i < 1220; i++) //1220 is count of arkit face mesh
        {
            GameObject g = Instantiate(m_debugSphere);
            g.name = "Point_" + i;
            m_debugSphereList.Add(g);
        }


    }

    GameObject m_aRFace;



    bool m_isFindingFace = true;
    void Update()
    {
        if (m_isFindingFace)
        {
            GameObject[] allSceneGameobjects = FindObjectsOfType<GameObject>();
            //ARFace E0487BD4233EA461-7A78B950B77D248A
            for (int i = 0; i < allSceneGameobjects.Length; i++)
            {
                if (allSceneGameobjects[i].name.Contains("ARFace "))
                {
                    m_aRFace = allSceneGameobjects[i];
                    m_isFindingFace = false;
 
                }
                //print("Garik Scene Gameobject: " + allSceneGameobjects[i].name);
            }
        }




        if (m_aRFace != null)
        {
            //print("Garik Vertex Count: " + m_aRFace.GetComponent<MeshFilter>().mesh.vertexCount);
            //print("Garik m_aRFace position: " + m_aRFace.transform.position);
            /*print("Garik m_aRFace scale: " + m_aRFace.transform.lossyScale);
            for (int i = 0; i < m_aRFace.GetComponent<MeshFilter>().mesh.vertexCount; i++)
            {
                print("Garik vertex positions: " + m_aRFace.GetComponent<MeshFilter>().mesh.vertices[i].x + "  "
                + m_aRFace.GetComponent<MeshFilter>().mesh.vertices[i].y + "  "
                    + m_aRFace.GetComponent<MeshFilter>().mesh.vertices[i].z);
            }*/




            for (int i = 0; i < m_aRFace.GetComponent<MeshFilter>().mesh.vertices.Length; i++)
            {
                m_debugSphereList[i].transform.SetParent(m_aRFace.transform);
                Vector3 worldPt = m_aRFace.GetComponent<MeshFilter>().mesh.vertices[i];
                m_debugSphereList[i].transform.localPosition = new Vector3(worldPt.x, worldPt.y, worldPt.z);
            }

            //m_glasses.transform.position = m_aRFace.transform.position;
            m_debugSphere.transform.position = m_aRFace.transform.position;
        }
    }
}
