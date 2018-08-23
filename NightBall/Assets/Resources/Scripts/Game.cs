using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;

public class Game : MonoBehaviour {

    int stars; //Кол-во собранных звезд
    //int recordStars; // Рекорд
    Dictionary<sbyte, Platform> platforms; //Платформы на сцене
    Dictionary<sbyte, BGStar> bgStars;
    Platform bottom; //Самая нижняя платформа (нулевая)
    bool updateScene; //Была ли сцена обновлена
    bool updateBackground;
    BGStar bgBottom; //Нижний фон со звездами


    void Awake () {
        platforms = new Dictionary<sbyte, Platform>();
        bgStars = new Dictionary<sbyte, BGStar>();
        stars = 0;
        //recordStars = 50;
        bottom = new Simple(0,0);
        platforms.Add(0, bottom); //Добавление нулевой платформы

        for (sbyte i = 1; i < 10; i++)
        {
            Platform previous;
            platforms.TryGetValue((sbyte)(i - 1), out previous);
            platforms.Add(i, RandomPlatform(previous));
        }

        //Добавление трех фоновых спрайтов со звездами
        bgBottom = new BGStar(-11);
        bgStars.Add(0, bgBottom);
        for (sbyte i = 1; i < 3; i++)
        {
            BGStar bgPrevious;
            bgStars.TryGetValue((sbyte)(i - 1), out bgPrevious);
            bgStars.Add(i, new BGStar(bgPrevious.Obj.transform.position.y + 11));
        }   

        updateScene = true;
    }

    void FixedUpdate()
    {
        updateScene = GameObject.FindGameObjectWithTag("Player").transform.position.y - bottom.PosY <= 6.0f;
        updateBackground = GameObject.FindGameObjectWithTag("Player").transform.position.y - bgBottom.Obj.transform.position.y <= 10.0f;

        if (!updateScene)
        {
            //Удаление самой нижней платформы
            Destroy(bottom.Exemplar);
            platforms.Remove(0);

            //Переприсвоение индексов платформ
            for (sbyte i = 1; i < 10; i++)
            {
                Platform temp;
                platforms.TryGetValue(i, out temp);
                platforms.Add((sbyte)(i - 1), temp);
                platforms.Remove(i);
            }

            //Добавление новой платформы
            Platform previous;
            platforms.TryGetValue(8, out previous);
            platforms.Add(9, RandomPlatform(previous));
            platforms.TryGetValue(0, out bottom);

            if (!updateScene) return;
        }
        
        if (!updateBackground)
        {
            //Удаление нижнего фона со звездами
            Destroy(bgBottom.Obj);
            bgStars.Remove(0);

            //Переприсвоение индексов фонов
            for (sbyte i=1; i<3; i++)
            {
                BGStar bgTemp;
                bgStars.TryGetValue(i, out bgTemp);
                bgStars.Add((sbyte)(i - 1), bgTemp);
                bgStars.Remove(i);
            }

            //Добавление нового фона
            BGStar bgPrevious;
            bgStars.TryGetValue(1, out bgPrevious);
            bgStars.Add(2, new BGStar(bgPrevious.Obj.transform.position.y + 11));
            bgStars.TryGetValue(0, out bgBottom);

            if (!updateBackground) return;
        }
        
    }

    void Update () {
		//Смерть
        if (bottom.PosY - GameObject.FindGameObjectWithTag("Player").transform.position.y > 4)
        {
            SaveNewRecord(stars);
            platforms.Clear();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
        }
	}

    //Создание случайной платформы
    Platform RandomPlatform (Platform previous)
    {
        float x = Random.Range(-2.0f, 2.0f);
        float y = CalculateY(previous.PosX, previous.PosY, x);
        Platform result;
        int random = Random.Range(0, 100);
        //Обычная платформа
        if (random < 40)
        {
            result = new Simple(x, y);
        }
        //Трамплин
        else if (random >= 40 && random <=45)
        {
            result = new Trampoline(x, y);
        }
        //Другое
        else
        {
            result = new Simple(x, y);
        }
        return result;
    }

    float CalculateY(float prevX, float prevY, float x)
    {
        float result;
        if (Mathf.Abs(prevX - x) < 1.2f)
        {
            result = Random.Range(2.0f, 3.0f) + prevY;
        } else
        {
            result = Random.Range(0.5f, 3.0f) + prevY;
        }
        
        return result;
    }

    //Сохранение рекорда в файл
    void SaveNewRecord(int count)
    {
        FileStream file;
        if (!File.Exists("Assets/Resources/Saves/record.dat"))
        {
           file = File.Create("Assets/Resources/Saves/record.dat");
        } else
        {
           file = File.OpenWrite("Assets/Resources/Saves/record.dat");
        }
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, count);
        file.Close();
    }

    //Чтение рекорда из файла
    int ReadOldRecord()
    {
        int count = 10;
        return count;
    }
}
