using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InputData : MonoBehaviour
{
    //Делаем видимым для инспектора
    [SerializeField]
    InputField X, Y, Z;

    [SerializeField]
    Button BtnStart;

    //Для хранения размеров матрицы
    int x = 0, y = 0, z = 0;

    void Start()
    {
        X.text = Y.text = Z.text = "10";
        BtnStart.onClick.AddListener(delegate
        {
            try
            {
                x = System.Convert.ToInt32(X.text);
            }
            catch (System.FormatException)
            {
                x = 10;
                X.text = "10";
                throw;
            }
            try
            {
                y = System.Convert.ToInt32(Y.text);
            }
            catch (System.FormatException)
            {
                y = 10;
                Y.text = "10";
                throw;

            }
            try
            {
                z = System.Convert.ToInt32(Z.text); }
            catch (System.FormatException)
            {
                z = 10;
                Z.text = "10";
                throw;
            }
            if (x == 0)
            {
                x = 1;
                X.text = "1";
            }
            if (y == 0)
            {
                
                Y.text = "1";
                y = 1;
            }
            if (z == 0)
            {
                Z.text = "1";
                z = 1;
            }

            //Сохраняем дааные
            PlayerPrefs.SetInt("X", x);
            PlayerPrefs.SetInt("Y", y);
            PlayerPrefs.SetInt("Z", z);
            PlayerPrefs.Save();
            SceneManager.LoadScene("Automata", LoadSceneMode.Single);

        });
        
    }


}
