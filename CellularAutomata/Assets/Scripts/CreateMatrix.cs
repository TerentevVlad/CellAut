using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMatrix : MonoBehaviour
{
    GameObject[,,] matrix;
    [SerializeField]
    GameObject parrent;
    [SerializeField]
    GameObject Pref;

    [SerializeField]
    UnityEngine.UI.Slider slider;

    [SerializeField]
    GameObject CenterCams;

    Camera mainCamera;

    Light light;

    int[,,] Imatrix;
    int x, y, z;
    void Start()
    {
        x = PlayerPrefs.GetInt("X");
        y = PlayerPrefs.GetInt("Y");
        z = PlayerPrefs.GetInt("Z");

        CenterCams.transform.position = new Vector3(x / 2, y / 2, z / 2);
        mainCamera = Camera.main;
        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, -z * 2);
        light = mainCamera.transform.GetChild(0).GetComponent<Light>();
        light.range = z * 4;


        

        matrix = new GameObject[x, y, z];
        for(int i = 0; i < x; i++)
        {
            for(int j = 0; j < y; j++)
            {
                for(int k = 0; k < z; k++)
                {
                    matrix[i, j, k] = Instantiate(Pref, new Vector3(i, j, k), parrent.transform.rotation, parrent.transform);
                }
            }
        } 
        
        
        Imatrix = new int[x, y, z];
        for(int i = 0; i < x; i++)
        {
            for(int j = 0; j < y; j++)
            {
                for(int k = 0; k < z; k++)
                {
                    Imatrix[i,j,k] = Random.Range(0, 2);
                    Debug.Log(Imatrix[i, j, k]);
                }
            }
        }
        UpdateOMatrix();
        
    }

    int Searcneighbourhood(int x_, int y_, int z_)
    {
        int Ist = 1, Jst = 1, Kst = 1;
        int Ifn = 1, Jfn = 1, Kfn = 1;
        int neighbourhood = 0;
        //Для того чтобы не выходило за пределы массива
        if(x_ == 0)
        {
            Ist = 0;
        }
        if(y_ == 0)
        {
            Jst = 0;
        }
        if(z_ == 0)
        {
            Kst = 0;
        }
        if(x_ == x - 1)
        {
            Ifn = 0;
        }
        if (y_ == y - 1)
        {
            Jfn = 0;
        }
        if (z_ == z - 1)
        {
            Kfn = 0;
        }

        for (int i = x_- Ist; i <= x_+Ifn; i++)
        {
            for (int j = y_ - Jst; j <= y_ + Jfn; j++)
            {
                for (int k = z_ - Kst; k <= z_ + Kfn; k++)
                {
                    if (Imatrix[i, j, k] == 1)
                        neighbourhood++;
                }
            }
        }
        return neighbourhood;
    }
    void UpdateIMatrix()
    {
        int[,,] tempMatrix = new int[x, y, z];
        for(int i = 0; i < x; i++)
        {
            for(int j = 0; j < y; j++)
            {
                for(int k = 0; k < z; k++)
                {
                    int neighbour = Searcneighbourhood(i, j, k);
                    //Если жива и от 5, до 10 соседей, то остается жить
                    if (Imatrix[i, j, k] == 1 && neighbour >= 6 && neighbour <= 10)
                    {
                        tempMatrix[i, j, k] = 1;
                    }
                    //Если мертва и соседей от 8 до 11, то рождается
                    else if (Imatrix[i, j, k] == 0 && (neighbour >= 7 && neighbour <= 9))
                    {
                        tempMatrix[i, j, k] = 1;
                    }
                    //Если меньше 7 соседей или больше 10, то умирает
                    else if (neighbour < 7 || neighbour > 10)
                    {
                        tempMatrix[i, j, k] = 0;
                    }
                    else tempMatrix[i, j, k] = 0;

                }
            }
        }

        Imatrix = tempMatrix;

    }

    void UpdateOMatrix()
    {
        for(int i = 0; i < x; i++)
        {
            for(int j = 0; j < y; j++)
            {
                for(int k = 0; k < z; k++)
                {
                    if (Imatrix[i, j, k] == 1)
                        matrix[i, j, k].SetActive(true);
                    else
                        matrix[i, j, k].SetActive(false);
                }
            }
        }
    }
    float Next = 0;

    void RotorCam()
    {
        if(Input.GetKey(KeyCode.RightArrow))
        {
            CenterCams.transform.Rotate(new Vector3(0, -5, 0));
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            CenterCams.transform.Rotate(new Vector3(0, 5, 0));
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            CenterCams.transform.Rotate(new Vector3(5, 0, 0));
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            CenterCams.transform.Rotate(new Vector3(-5, 0, 0));
        }
        if(Input.GetKey(KeyCode.KeypadPlus) || Input.GetKey(KeyCode.Plus))
        {
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, CenterCams.transform.position, 5 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.KeypadMinus))
        {
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, CenterCams.transform.position, -5 * Time.deltaTime);
        }

    }
    void Update()
    {
        RotorCam();
        if (Next + 0.1f > slider.value)
        {

            Next = 0;
            UpdateIMatrix();
            UpdateOMatrix();
        }
        else
        {
            Next += Time.deltaTime;
            Debug.Log(Next);
        }
        
        
    }
}
