using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.IO;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{


    public GameObject m_facePoint;
    public List<GameObject> m_facePoints = new List<GameObject>();



    void Start()
    {
        StatusBarManager.BarAnim(2);
        StatusBarManager.Show(true);
        m_facePoint.GetComponent<MeshRenderer>().enabled = false;
        m_facePoint.transform.GetChild(0).gameObject.SetActive(false);
        m_facePoint.SetActive(false);
        for (int i = 0; i < 1220; i++) //1220 is count of vertices of arkit face mesh
        {
            GameObject g = Instantiate(m_facePoint);
            g.name = "Point_" + i;
            g.transform.GetChild(0).GetComponent<TextMesh>().text = i.ToString();
            g.SetActive(true);
            m_facePoints.Add(g);
        }


    }

    GameObject m_aRFace;

    public GameObject m_currentGlasses;
    public GameObject m_glassesParent;

    bool m_isFindingFace = true;

    Mesh m_mesh;

    void Update()
    {
        //return;
        if (m_isFindingFace)
        {
            GameObject[] allSceneGameobjects = FindObjectsOfType<GameObject>();

            for (int i = 0; i < allSceneGameobjects.Length; i++)
            {
                if (allSceneGameobjects[i].name.Contains("ARFace"))
                {
                    m_aRFace = allSceneGameobjects[i];
                    m_mesh = m_aRFace.GetComponent<MeshFilter>().mesh;
 
                    m_isFindingFace = false;

                }
                //print("Garik Scene Gameobject: " + allSceneGameobjects[i].name);
            }
        }


        if (!m_isFindingFace)
        {

            if(m_mesh.vertexCount == 0)
            {
                Debug.Log("Garik Face Mesh Vertex Count Is Zero");
                m_mesh = m_aRFace.GetComponent<MeshFilter>().mesh;
                /*for (int i = 0; i < m_mesh.vertices.Length; i++)
                {
                    m_facePoints[i].transform.localPosition = m_mesh.vertices[i];
                    m_facePoints[i].transform.SetParent(m_aRFace.transform);
                    //m_facePoints[i].transform.GetChild(0).transform.LookAt(Camera.main.transform.position);  
                }*/
            }


            for(int i = 0; i < m_mesh.vertices.Length; i++)
            {
                m_facePoints[i].transform.localPosition = m_mesh.vertices[i];
                m_facePoints[i].transform.SetParent(m_aRFace.transform);
                //m_facePoints[i].transform.GetChild(0).transform.LookAt(Camera.main.transform.position);  
            }



            fitGlasses();
        }

    }

    public void setGlasses(GameObject glasses)
    {
        Destroy(m_currentGlasses);
        m_currentGlasses = glasses;


        m_earpieceEndPointL = findByNameRecursively(glasses, Constants.s_earpieceStartPointLName);
        m_earpieceEndPointR = findByNameRecursively(glasses, Constants.s_earpieceStartPointRName);

        setAllChildMeshrenderersInactive(m_earpieceEndPointL);
        setAllChildMeshrenderersInactive(m_earpieceEndPointR);
    }

    /// ///////////////////////////////////////////////////////

    void fitGlasses()
    {

        //print("Garik fit glasses calling");
        /*
        m_facePoints[467].GetComponent<MeshRenderer>().material.color = Color.red; // Left Ear Point
        m_facePoints[888].GetComponent<MeshRenderer>().material.color = Color.red; // Right Ear Point


        m_facePoints[16].GetComponent<MeshRenderer>().material.color = Color.blue; // Nose Point
        m_facePoints[36].GetComponent<MeshRenderer>().material.color = Color.blue; // Under Nose Point

        m_facePoints[1132].GetComponent<MeshRenderer>().material.color = Color.yellow; // Right Eye Right Corner
        m_facePoints[1134].GetComponent<MeshRenderer>().material.color = Color.yellow; // Left Eye Left Corner
        */
        m_glassesParent.transform.position = Vector3.zero;
        m_glassesParent.transform.localScale = Vector3.one;
        m_glassesParent.transform.eulerAngles = Vector3.zero;

        if(m_currentGlasses == null)
        {
            return; 
        }

        m_currentGlasses.transform.SetParent(m_glassesParent.transform);
        m_currentGlasses.transform.position = new Vector3(0, 0, 0);
        m_currentGlasses.transform.eulerAngles = new Vector3(0, 0, 0);
        m_currentGlasses.SetActive(true);
        m_glassesParent.SetActive(true);

        Vector3 glassesPosition = getPositionOfGlassesObject(m_facePoints);
        float scaleVal = getScaleOfGlassesObject(m_facePoints, m_glassesParent);

        m_glassesParent.transform.position = new Vector3(glassesPosition.x, glassesPosition.y, glassesPosition.z);
        m_glassesParent.transform.rotation = m_aRFace.transform.rotation;

        m_glassesParent.transform.eulerAngles = new Vector3(-m_glassesParent.transform.eulerAngles.x, m_glassesParent.transform.eulerAngles.y - 180, -m_glassesParent.transform.eulerAngles.z);

        m_glassesParent.transform.localScale = new Vector3(scaleVal, scaleVal, scaleVal);

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        m_templeRotPointL = findByNameRecursively(m_glassesParent, Constants.s_templeRotPointLName);
        m_templeRotPointR = findByNameRecursively(m_glassesParent, Constants.s_templeRotPointRName);


        m_earpieceEndPointL = findByNameRecursively(m_glassesParent, Constants.s_earpieceStartPointLName);
        m_earpieceEndPointR = findByNameRecursively(m_glassesParent, Constants.s_earpieceStartPointRName);


        m_clmEarPointLeft = m_facePoints[888];
        m_clmEarPointRight = m_facePoints[467];

        deformGlassesToFitToPlane(m_templeRotPointL, m_earpieceEndPointL, m_clmEarPointRight.transform.position);
        deformGlassesToFitToPlane(m_templeRotPointR, m_earpieceEndPointR, m_clmEarPointLeft.transform.position);

    }
    GameObject m_templeRotPointL;
    GameObject m_templeRotPointR;
    GameObject m_earpieceEndPointL;
    GameObject m_earpieceEndPointR;
    GameObject m_clmEarPointLeft;
    GameObject m_clmEarPointRight;


    void deformGlassesToFitToPlane(GameObject templeRotPoint, GameObject earpieceEndPoint, Vector3 clmEarPoint)
    {

        float initialLength = Vector3.Distance(templeRotPoint.transform.position, earpieceEndPoint.transform.position);
        //print("Garik initialLength: " + initialLength);
        float expectedLength = Vector3.Distance(templeRotPoint.transform.position, clmEarPoint);
        //print("Garik expectedLength: " + expectedLength);
        float scale = expectedLength / initialLength;
        //print("Garik scale: " + scale);
        //print("Garik temple localscale z: " + templeRotPoint.transform.localScale);

        if (scale > 0.001)
        {
            templeRotPoint.transform.localScale = new Vector3(templeRotPoint.transform.localScale.x, templeRotPoint.transform.localScale.y, templeRotPoint.transform.localScale.z * scale);


            Vector3 i = earpieceEndPoint.transform.position - templeRotPoint.transform.position;

            Vector3 e = clmEarPoint - templeRotPoint.transform.position;

            if (i == e)
            {
                return;
            }

            Vector3 axisToRotate = Vector3.Cross(i, e);
            //UnityEngine.Debug.DrawLine(templeRotPoint.transform.position, templeRotPoint.transform.position + axisToRotate * 10, Color.blue);

            if (axisToRotate.magnitude < 0.0001)
            {
                return;
            }

            float rotationAngle = Mathf.Acos(Vector3.Dot(i, e) / (i.magnitude * e.magnitude));

            Vector3 upBefore = templeRotPoint.transform.up;

            templeRotPoint.transform.SetParent(null);

            templeRotPoint.transform.RotateAround(templeRotPoint.transform.position, axisToRotate, rotationAngle * 180 / Mathf.PI);

            templeRotPoint.transform.SetParent(m_currentGlasses.transform);
        }

    }

    public float getScaleOfGlassesObject(List<GameObject> points, GameObject originalObject)
    {
        float ds0 = Vector3.Distance(points[1132].transform.position, points[1134].transform.position);

        Vector3 a = originalObject.transform.GetChild(0).transform.Find(Constants.s_glassesPointLName).transform.localPosition;
        Vector3 b = originalObject.transform.GetChild(0).transform.Find(Constants.s_glassesPointRName).transform.localPosition;

        float ds1 = Vector3.Distance(a, b);

        float val = ds0 / ds1;

        return val * 1.3f;
    }

    public Vector3 getPositionOfGlassesObject(List<GameObject> points)
    {
        return (points[16].transform.position + points[36].transform.position) / 2;
    }

    void setAllChildMeshrenderersInactive(GameObject ob)
    {
        MeshRenderer[] a = ob.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer t in a)
        {
            if (t.gameObject != null)
            {
                t.enabled = false;
            }
        }
        return;
    }
    Transform[] m_componentsInChildren;

    GameObject findByNameRecursively(GameObject ob, string name)
    {
        m_componentsInChildren = ob.GetComponentsInChildren<Transform>();
        foreach (Transform t in m_componentsInChildren)
        {
            if (t.gameObject != null && t.gameObject.name == name)
            {
                return t.gameObject;
            }
        }

        return null;
    }

    /// ///////////////////////////////////////////////////////
    /// ///////////////////////////////////////////////////////

    void initializeBrandsCarousel()
    {
     
    }
}
