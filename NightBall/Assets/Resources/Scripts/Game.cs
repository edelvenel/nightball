using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;

// Главный скрипт игры (компонент Main Camera)
[System.Serializable]
public class Game : MonoBehaviour {

    static int starsCount; // кол-во собранных звезд
    static int recordStars; // рекорд

    Dictionary<sbyte, Platform> platforms; // платформы на сцене
    Dictionary<sbyte, BGStar> bgStars; // фоны со звездами
    Dictionary<sbyte, Star> stars; // звезды для сбора
    
    bool updateScene; // была ли сцена с платформами обновлена
    bool updateBackground; // были ли обновлен задний план
    bool updateStars; // были ли обновлены звезды для сбора

    Platform bottom; // самая нижняя платформа
    BGStar bgBottom; // самый нижний фон со звездами
    Star starBottom; // самая нижняя звезда


    void Awake ()
    {
        platforms = new Dictionary<sbyte, Platform>();
        bgStars = new Dictionary<sbyte, BGStar>();
        stars = new Dictionary<sbyte, Star>();

        starsCount = 0;
        recordStars = LoadOldRecord();

        //Добавление начальных 10 платформ
        bottom = new Simple();
        platforms.Add(0, bottom);

        for (sbyte i = 1; i < 10; i++)
        {
            Platform previous;
            platforms.TryGetValue((sbyte)(i - 1), out previous);
            platforms.Add(i, RandomPlatform(previous));
        }

        //Добавление 3 фоновых спрайтов со звездами
        bgBottom = new BGStar(-11); //Первый фон со звездами находится на 11 ниже Игрока (11 - высота фонового спрайта)
        bgStars.Add(0, bgBottom);

        for (sbyte i = 1; i < 3; i++)
        {
            BGStar bgPrevious;
            bgStars.TryGetValue((sbyte)(i - 1), out bgPrevious);
            bgStars.Add(i, new BGStar(bgPrevious.Obj.transform.position.y + 11));
        }

        //Добавление звезд
        starBottom = RandomStar(new SimpleStar(Random.Range(-2.0f, 2.0f), Random.Range(1.0f, 3.0f)));
        stars.Add(0, starBottom);

        for (sbyte i = 1; i < 5; i++)
        {
            Star starPrevious;
            stars.TryGetValue((sbyte)(i - 1), out starPrevious);
            stars.Add(i, RandomStar(starPrevious));
        }
    }

    void FixedUpdate()
    {
        updateScene = GameObject.FindGameObjectWithTag("Player").transform.position.y - bottom.PosY <= 6.0f;
        updateBackground = GameObject.FindGameObjectWithTag("Player").transform.position.y - bgBottom.Obj.transform.position.y <= 10.0f;
        updateStars = GameObject.FindGameObjectWithTag("Player").transform.position.y - starBottom.PosY <= 6.0f;

        if (!updateScene)
        {
            // Удаление самой нижней платформы
            Destroy(bottom.Exemplar);
            platforms.Remove(0);

            // Переприсвоение индексов платформ
            for (sbyte i = 1; i < 10; i++)
            {
                Platform temp;
                platforms.TryGetValue(i, out temp);
                platforms.Add((sbyte)(i - 1), temp);
                platforms.Remove(i);
            }

            // Добавление новой платформы
            Platform previous;
            platforms.TryGetValue(8, out previous);
            platforms.Add(9, RandomPlatform(previous));
            platforms.TryGetValue(0, out bottom);

            if (!updateScene) return;
        }
        
        if (!updateBackground)
        {
            // Удаление нижнего фона со звездами
            Destroy(bgBottom.Obj);
            bgStars.Remove(0);

            // Переприсвоение индексов фонов
            for (sbyte i=1; i<3; i++)
            {
                BGStar bgTemp;
                bgStars.TryGetValue(i, out bgTemp);
                bgStars.Add((sbyte)(i - 1), bgTemp);
                bgStars.Remove(i);
            }

            // Добавление нового фона
            BGStar bgPrevious;
            bgStars.TryGetValue(1, out bgPrevious);
            bgStars.Add(2, new BGStar(bgPrevious.Obj.transform.position.y + 11));
            bgStars.TryGetValue(0, out bgBottom);

            if (!updateBackground) return;
        }

        if (!updateStars)
        {
            // Удаление нижней звезды
            Destroy(starBottom.Exemplar);
            stars.Remove(0);

            // Переприсваивание индексов звезд
            for (sbyte i=1; i<5; i++)
            {
                Star starTemp;
                stars.TryGetValue(i, out starTemp);
                stars.Add((sbyte)(i - 1), starTemp);
                stars.Remove(i);
            }

            // Добавление новой звезды
            Star starPrevious;
            stars.TryGetValue(3, out starPrevious);
            stars.Add(4, RandomStar(starPrevious));
            stars.TryGetValue(0, out starBottom);

            if (!updateStars) return;
        }
        
    }

    void Update () {
		// Смерть
        if (bottom.PosY - GameObject.FindGameObjectWithTag("Player").transform.position.y > 4)
        {
            SaveNewRecord(starsCount);
            SceneManager.LoadScene("SimpleScene", LoadSceneMode.Single);
        }

        // Вывод счета на экран
        GameObject.FindGameObjectWithTag("Count").GetComponent<Text>().text = "Count: " + starsCount.ToString() + " / " + recordStars.ToString();

        // Выход из игры
        if (Input.GetKey(KeyCode.Escape))
        {
            SaveNewRecord(starsCount);
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
	}

    //Создание случайной платформы
    Platform RandomPlatform (Platform previous)
    {
        float x = Random.Range(-2.0f, 2.0f);
        // Высчитывается позиция по оси Y так, чтобы новая платформа не была слишком близко к предыдущей в случае одинаковой позиции по X
        float y = CalculateY(previous.PosX, previous.PosY, x);
        Platform result;
        int random = Random.Range(0, 100);

        // Низкая сложность
        if (starsCount < 100)
        {
            // Обычная платформа
            if (random < 40)
            {
                result = new Simple (x, y);
            }
            // Батут
            else if (random >= 40 && random <= 45)
            {
                result = new Trampoline (x, y);
            }
            // Движущаяся платформа
            else if (random >= 45 && random <= 50)
            {
                result = new MovingPlatform (x, y);
            }
            // Исчезающая платформа
            else if (random > 50 && random <=60)
            {
                result = new Fake(x, y);
            }
            // Другое
            else
            {
                result = new Simple (x, y);
            }
        }
        // Средняя сложность
        else if (starsCount >= 100 && starsCount <= 300)
        {
            // Обычная платформа
            if (random < 30)
            {
                result = new Simple (x, y);
            }
            // Батут
            else if (random >= 30 && random <= 45)
            {
                result = new Trampoline (x, y);
            }
            // Движущаяся платформа
            else if (random >= 45 && random <= 60)
            {
                result = new MovingPlatform (x, y);
            }
            // Исчезающая платформа
            else if (random > 60 && random <= 75)
            {
                result = new Fake(x, y);
            }
            //Другое
            else
            {
                result = new Simple (x, y);
            }
        }
        // Высокая сложность
        else
        {
            // Обычная платформа
            if (random < 20)
            {
                result = new Simple (x, y);
            }
            // Батут
            else if (random >= 20 && random <= 45)
            {
                result = new Trampoline (x, y);
            }
            // Движущаяся платформа
            else if (random >= 45 && random <= 60)
            {
                result = new MovingPlatform (x, y);
            }
            // Исчезающая платформа
            else if (random > 60 && random <= 80)
            {
                result = new Fake(x, y);
            }
            // Другое
            else
            {
                result = new Simple (x, y);
            }
        }

        return result;
    }

    // Создание случайной звезды
    Star RandomStar (Star previous)
    {
        float x = Random.Range(-2.0f, 2.0f);
        float y = previous.PosY + Random.Range(2.5f, 6.0f);

        Star result;
        float random = Random.Range(0.0f, 81.0f);

        if (random < 70)
        {
            result = new SimpleStar(x, y);
        }
        else if (random >=70 && random < 75)
        {
            result = new YoungStar(x, y);
        }
        else if (random >= 76 && random < 79)
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
        }
        else
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

    //Сохранение
    void SaveNewRecord (int count)
    {
        if (LoadOldRecord() < starsCount)
        {
            PlayerPrefs.SetInt("Record", count);
        }
    }

    // Загрузка сохранения
    int LoadOldRecord()
    {
        int count = PlayerPrefs.GetInt("Record");
        return count;
    }
}
