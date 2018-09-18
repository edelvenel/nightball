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

    static int chalksCount; // кол-во собранных мелков
    static int record; // рекорд
    static int height; // текущая высота

    Dictionary<sbyte, Platform> platforms; // платформы на сцене
    Dictionary<sbyte, Bg> bgs; // фоны
    Dictionary<sbyte, Chalk> chalks; // мелки
    Dictionary<sbyte, Picture> pictures; // фоновые картинки
    
    bool updateScene; // была ли сцена с платформами обновлена
    bool updateBackground; // были ли обновлен задний план
    bool updateChalks; // были ли обновлены мелки
    bool updatePictures; // были ли обновлены фоновые картинки

    Platform bottom; // самая нижняя платформа
    Bg bgBottom; // самый нижний фон
    Chalk chalkBottom; // самый нижний мелок
    Picture pictBottom; // самая нижеяя картинка

    void Awake ()
    {
        platforms = new Dictionary<sbyte, Platform>();
        bgs = new Dictionary<sbyte, Bg>();
        chalks = new Dictionary<sbyte, Chalk>();
        pictures = new Dictionary<sbyte, Picture>();

        chalksCount = 25;
        height = 0;
        record = LoadOldRecord();

        // Добавление начальных 10 платформ
        bottom = new Simple();
        platforms.Add(0, bottom);

        for (sbyte i = 1; i < 10; i++)
        {
            Platform previous;
            platforms.TryGetValue((sbyte)(i - 1), out previous);
            platforms.Add(i, RandomPlatform(previous));
        }

        // Добавление 3 фоновых спрайтов
        bgBottom = new Bg(-BGHeight()); //Первый фон находится на высоту фонового спрайта ниже игрока
        bgs.Add(0, bgBottom);

        for (sbyte i = 1; i < 3; i++)
        {
            Bg bgPrevious;
            bgs.TryGetValue((sbyte)(i - 1), out bgPrevious);
            bgs.Add(i, new Bg(bgPrevious.Obj.transform.position.y + BGHeight()));
            if (!bgPrevious.Obj.GetComponent<SpriteRenderer>().flipY)
            {
                Bg bgTemp;
                bgs.TryGetValue(i, out bgTemp);
                bgTemp.Obj.GetComponent<SpriteRenderer>().flipY = true;
            }
        }

        // Добавление мелков
        chalkBottom = RandomChalk(new SimpleChalk(Random.Range(-2.0f, 2.0f), Random.Range(1.0f, 3.0f)));
        chalks.Add(0, chalkBottom);

        for (sbyte i = 1; i < 5; i++)
        {
            Chalk chalkPrevious;
            chalks.TryGetValue((sbyte)(i - 1), out chalkPrevious);
            chalks.Add(i, RandomChalk(chalkPrevious));
        }

        // Добавление картинок
        pictBottom = new Picture(Random.Range(-2.0f, 2.0f), Random.Range(1.0f, 3.0f));
        pictures.Add(0, pictBottom);

        for (sbyte i=1; i < 17; i++)
        {
            Picture pictPrevious;
            pictures.TryGetValue((sbyte)(i - 1), out pictPrevious);
            pictures.Add(i, new Picture(Random.Range(-2.0f, 2.0f), pictPrevious.Exemplar.transform.position.y + Random.Range(1.0f,4.0f)));
        }
    }

    void FixedUpdate()
    {
        updateScene = GameObject.FindGameObjectWithTag("Player").transform.position.y - bottom.PosY <= 6.0f;
        updateBackground = GameObject.FindGameObjectWithTag("Player").transform.position.y - bgBottom.Obj.transform.position.y <= 20.0f;
        updateChalks = GameObject.FindGameObjectWithTag("Player").transform.position.y - chalkBottom.PosY <= 6.0f;
        updatePictures = GameObject.FindGameObjectWithTag("Player").transform.position.y - pictBottom.Exemplar.transform.position.y <= 20.0f;

        // ----- Обновление платформ -----
        if (!updateScene)
        {
            // Удаление самой нижней платформы
            Destroy(bottom.Exemplar);
            platforms.Remove(0);

            // Переприсвоение индексов платформ
            for (sbyte i = 1; i < platforms.Count; i++)
            {
                Platform temp;
                platforms.TryGetValue(i, out temp);
                platforms.Add((sbyte)(i - 1), temp);
                platforms.Remove(i);
            }

            // Добавление новой платформы
            if (chalksCount > 0)
            {
                Platform previous;
                platforms.TryGetValue((sbyte)(platforms.Count - 2), out previous);
                platforms.Add((sbyte)(platforms.Count - 1), RandomPlatform(previous));
                platforms.TryGetValue(0, out bottom);
                chalksCount--;
            }
            else
            {
                platforms.TryGetValue(0, out bottom);
            }

            if (!updateScene) return;
        }
        
        // ----- Обновление фонов -----
        if (!updateBackground)
        {
            // Удаление нижнего фона
            Destroy(bgBottom.Obj);
            bgs.Remove(0);

            // Переприсвоение индексов фонов
            for (sbyte i=1; i<3; i++)
            {
                Bg bgTemp;
                bgs.TryGetValue(i, out bgTemp);
                bgs.Add((sbyte)(i - 1), bgTemp);
                bgs.Remove(i);
            }

            // Добавление нового фона
            Bg bgPrevious;
            bgs.TryGetValue(1, out bgPrevious);
            bgs.Add(2, new Bg(bgPrevious.Obj.transform.position.y + BGHeight()));
            bgs.TryGetValue(0, out bgBottom);

            // Переворачивание фона
            if (bgBottom.Obj.GetComponent<SpriteRenderer>().flipY)
            { 
                Bg bgThird;
                bgs.TryGetValue(2, out bgThird);
                bgThird.Obj.GetComponent<SpriteRenderer>().flipY = true;
            }
            else
            {
                Bg bgSecond;
                bgs.TryGetValue(1, out bgSecond);
                bgSecond.Obj.GetComponent<SpriteRenderer>().flipY = true;
            }

            if (!updateBackground) return;
        }

        // ----- Обновление мелков -----
        if (!updateChalks)
        {
            // Удаление нижнего мелка
            Destroy(chalkBottom.Exemplar);
            chalks.Remove(0);

            // Переприсваивание индексов мелков
            for (sbyte i = 1; i < chalks.Count; i++)
            {
                Chalk chalkTemp;
                chalks.TryGetValue(i, out chalkTemp);
                chalks.Add((sbyte)(i - 1), chalkTemp);
                chalks.Remove(i);
            }

            // Добавление нового мелка
            Chalk chalkPrevious;
            chalks.TryGetValue((sbyte)(chalks.Count - 2), out chalkPrevious);
            chalks.Add((sbyte)(chalks.Count - 1), RandomChalk(chalkPrevious));
            chalks.TryGetValue(0, out chalkBottom);

            if (!updateChalks) return;
        }

        // ----- Обновление картинок -----
        if (!updatePictures)
        {
            // Удаление нижней картинки
            Destroy(pictBottom.Exemplar);
            pictures.Remove(0);

            // Переприсваивание индексов картинок
            for (sbyte i=1; i < pictures.Count; i++)
            {
                Picture pictureTemp;
                pictures.TryGetValue(i, out pictureTemp);
                pictures.Add((sbyte)(i - 1), pictureTemp);
                pictures.Remove(i);
            }

            // Добавление новой картинки
            Picture pictPrevious;
            pictures.TryGetValue((sbyte)(pictures.Count - 2), out pictPrevious);
            pictures.Add((sbyte)(pictures.Count - 1), new Picture(Random.Range(-2.0f, 2.0f), pictPrevious.Exemplar.transform.position.y + Random.Range(1.0f, 4.0f)));
            pictures.TryGetValue(0, out pictBottom);

            if (!updatePictures) return;
        }
        
    }

    void Update () {
        // Смерть
        if (bottom.PosY - GameObject.FindGameObjectWithTag("Player").transform.position.y > 2 || chalksCount < 1)  Death();

        // Подсчет высоты
        if (height < (int)(GameObject.FindGameObjectWithTag("Player").transform.position.y))
        {
            height = (int)(GameObject.FindGameObjectWithTag("Player").transform.position.y);
        }

        // Вывод счета на экран
        GameObject.FindGameObjectWithTag("Chalks").GetComponent<Text>().text = chalksCount.ToString();
        GameObject.FindGameObjectWithTag("Count").GetComponent<Text>().text = height.ToString() + " / " + record.ToString();

        // Выход из игры
        if (Input.GetKey(KeyCode.Escape))
        {
            SaveNewRecord(chalksCount);
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
        if (chalksCount < 100)
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
            else if (random >= 45 && random <= 70)
            {
                result = new MovingPlatform (x, y);
            }
            // Исчезающая платформа
            else if (random > 70 && random <=90)
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
        else if (chalksCount >= 100 && chalksCount <= 300)
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
            else if (random >= 45 && random <= 80)
            {
                result = new MovingPlatform (x, y);
            }
            // Исчезающая платформа
            else if (random > 80 && random <= 95)
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
            if (random < 10)
            {
                result = new Simple (x, y);
            }
            // Батут
            else if (random >= 10 && random <= 40)
            {
                result = new Trampoline (x, y);
            }
            // Движущаяся платформа
            else if (random >= 40 && random <= 80)
            {
                result = new MovingPlatform (x, y);
            }
            // Исчезающая платформа
            else if (random > 80 && random <= 95)
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

    // Создание случайного мелка
    Chalk RandomChalk (Chalk previous)
    {
        float x = Random.Range(-2.0f, 2.0f);
        float y = previous.PosY + Random.Range(2.5f, 5.0f);

        Chalk result;
        float random = Random.Range(0.0f, 81.0f);

        if (random < 70)
        {
            result = new SimpleChalk(x, y);
        }
        else if (random >=70 && random < 75)
        {
            result = new RedChalk(x, y);
        }
        else if (random >= 76 && random < 79)
        {
            result = new GreenChalk(x, y);
        } else
        {
            result = new VioletChalk(x, y);
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
        chalksCount += points;
    }

    //Сохранение
    void SaveNewRecord (int count)
    {
        if (LoadOldRecord() < height)
        {
            PlayerPrefs.SetInt("Record", count);
        }
    }

    // Загрузка сохранения
    int LoadOldRecord()
    {
        int record = PlayerPrefs.GetInt("Record");
        return record;
    }

    // Получение высоты блока
    float BGHeight()
    {
        float height = Resources.Load<GameObject>("Prefabs/Background").GetComponent<SpriteRenderer>().bounds.size.y;
        return height;
    }

    // Смерть
    void Death()
    {
        SaveNewRecord(height);
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
}
