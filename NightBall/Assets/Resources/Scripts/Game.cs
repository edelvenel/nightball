using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;

public class Game : MonoBehaviour {

    static int starsCount; //Кол-во собранных звезд
    static int recordStars; // Рекорд
    Dictionary<sbyte, Platform> platforms; //Платформы на сцене
    Dictionary<sbyte, BGStar> bgStars;
    Dictionary<sbyte, Star> stars;
    Platform bottom; //Самая нижняя платформа (нулевая)
    bool updateScene; //Была ли сцена обновлена
    bool updateBackground;
    bool updateStars;
    BGStar bgBottom; //Нижний фон со звездами
    Star starBottom; //Нижняя звезда


    void Awake () {
        platforms = new Dictionary<sbyte, Platform>();
        bgStars = new Dictionary<sbyte, BGStar>();
        stars = new Dictionary<sbyte, Star>();
        starsCount = 0;
        recordStars = ReadOldRecord();

        //Добавление платформ
        bottom = new Simple();
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

        //Добавление звезд
        starBottom = RandomStar(new SimpleStar());
        stars.Add(0, starBottom);
        for (sbyte i = 1; i < 5; i++)
        {
            Star starPrevious;
            stars.TryGetValue((sbyte)(i - 1), out starPrevious);
            stars.Add(i, RandomStar(starPrevious));
        }
        updateScene = true;
    }

    void FixedUpdate()
    {
        updateScene = GameObject.FindGameObjectWithTag("Player").transform.position.y - bottom.PosY <= 6.0f;
        updateBackground = GameObject.FindGameObjectWithTag("Player").transform.position.y - bgBottom.Obj.transform.position.y <= 10.0f;
        updateStars = GameObject.FindGameObjectWithTag("Player").transform.position.y - starBottom.PosY <= 6.0f;

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

        if (!updateStars)
        {
            //Удаление нижней звезды
            Destroy(starBottom.Exemplar);
            stars.Remove(0);

            //Переприсваивание индексов звезд
            for (sbyte i=1; i<5; i++)
            {
                Star starTemp;
                stars.TryGetValue(i, out starTemp);
                stars.Add((sbyte)(i - 1), starTemp);
                stars.Remove(i);
            }

            //Добавление новой звезды
            Star starPrevious;
            stars.TryGetValue(3, out starPrevious);
            stars.Add(4, RandomStar(starPrevious));
            stars.TryGetValue(0, out starBottom);

            if (!updateStars) return;
        }
        
    }

    void Update () {
		//Смерть
        if (bottom.PosY - GameObject.FindGameObjectWithTag("Player").transform.position.y > 4)
        {
            SaveNewRecord(starsCount);
            platforms.Clear();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
        }
        GameObject.FindGameObjectWithTag("Count").GetComponent<Text>().text = "Count: " + starsCount.ToString() + " / " + recordStars.ToString();
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

    //Создание случайной звезды!!!
    Star RandomStar (Star previous)
    {
        float x = Random.Range(-2.0f, 2.0f);
        float y = previous.PosY + Random.Range(2.5f, 6.0f);

        Star result;
        int random = Random.Range(0, 85);
        if (random < 50)
        {
            result = new SimpleStar(x, y);
        }
        else if (random >=50 && random < 70)
        {
            result = new YoungStar(x, y);
        }
        else if (random >= 70 && random < 80)
        {
            result = new OldStar(x, y);
        } else
        {
            result = new SuperStar(x, y);
        }
        return result;
    }


    //Вычисление координаты Y для новой платформы
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

    //Добавление очков
    public static void AddPoints (int points)
    {
        starsCount += points;
    }

    //Сохранение рекорда в файл
    void SaveNewRecord(int count)
    {
        if (ReadOldRecord() < starsCount)
        {
            FileStream file;
            if (!File.Exists("Assets/Resources/Saves/record.dat"))
            {
                file = File.Create("Assets/Resources/Saves/record.dat");
            }
            else
            {
                file = File.OpenWrite("Assets/Resources/Saves/record.dat");
            }
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, count);
            file.Close();
        }
    }

    //Чтение рекорда из файла
    int ReadOldRecord()
    {
        int count;
        if (!File.Exists("Assets/Resources/Saves/record.dat"))
        {
            count = 0;
        }
        else
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open("Assets/Resources/Saves/record.dat", FileMode.Open);
            count = (int)bf.Deserialize(file);
            file.Close();
        }
        return count;
    }
}
