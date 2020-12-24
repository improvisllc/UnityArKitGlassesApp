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
                m_facePoints[i].transform.SetParent(m_aRFace.transform);
                Vector3 worldPt = m_aRFace.GetComponent<MeshFilter>().mesh.vertices[i];
                m_facePoints[i].transform.localPosition = new Vector3(worldPt.x, worldPt.y, worldPt.z);

                m_facePoints[i].transform.GetChild(0).transform.LookAt(Camera.main.transform.position);
            }

            //m_glasses.transform.position = m_aRFace.transform.position;
            //m_debugSphere.transform.position = m_aRFace.transform.position;

            fitGlasses();
        }
        //fitGlasses();
    }

    public void setGlasses(GameObject glasses)
    {
        Destroy(m_currentGlasses);
        glasses.SetActive(true);
        m_currentGlasses = glasses;

    }

    /// ///////////////////////////////////////////////////////

    void fitGlasses()
    {

        //print("Garik fit glasses calling");

        m_facePoints[467].GetComponent<MeshRenderer>().material.color = Color.red; // Left Ear Point
        m_facePoints[888].GetComponent<MeshRenderer>().material.color = Color.red; // Right Ear Point


        m_facePoints[16].GetComponent<MeshRenderer>().material.color = Color.blue; // Nose Point
        m_facePoints[36].GetComponent<MeshRenderer>().material.color = Color.blue; // Under Nose Point

        m_facePoints[1132].GetComponent<MeshRenderer>().material.color = Color.yellow; // Right Eye Right Corner
        m_facePoints[1134].GetComponent<MeshRenderer>().material.color = Color.yellow; // Left Eye Left Corner

        //GameObject.DestroyImmediate(m_glasses, true);
        //m_currentGlasses = Instantiate(m_glassesOriginal);

        //m_currentGlasses.name = "CurrentGlasses";
        m_glassesParent.transform.position = Vector3.zero;
        m_glassesParent.transform.localScale = Vector3.one;
        //m_glassesParent.transform.GetChild(0).transform.localScale = Vector3.one;
        m_glassesParent.transform.eulerAngles = Vector3.zero;

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
         

        GameObject templeRotPointL = findByNameRecursively(m_glassesParent, Constants.s_templeRotPointLName);
        GameObject templeRotPointR = findByNameRecursively(m_glassesParent, Constants.s_templeRotPointRName);


        GameObject earpieceEndPointL = findByNameRecursively(m_glassesParent, Constants.s_earpieceStartPointLName);
        GameObject earpieceEndPointR = findByNameRecursively(m_glassesParent, Constants.s_earpieceStartPointRName);


        GameObject clmEarPointLeft = m_facePoints[888];
        GameObject clmEarPointRight = m_facePoints[467];

        deformGlassesToFitToPlane(templeRotPointL, earpieceEndPointL, clmEarPointRight.transform.position);
        deformGlassesToFitToPlane(templeRotPointR, earpieceEndPointR, clmEarPointLeft.transform.position);
        //print("Garik After deform");
        setAllChildMeshrenderersInactive(earpieceEndPointL);
        setAllChildMeshrenderersInactive(earpieceEndPointR);
        //earpieceEndPointL.GetComponent<MeshRenderer>().enabled = false;
        //earpieceEndPointR.GetComponent<MeshRenderer>().enabled = false;

    }

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
    GameObject findByNameRecursively(GameObject ob, string name)
    {
        Transform[] a = ob.GetComponentsInChildren<Transform>();
        foreach (Transform t in a)
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
