using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using PictogramGame.Models;
using System.Security.Cryptography;
using System.Threading;

namespace PictogramGame.Controllers
{
    /*
     * Используем паттерн MVC - Model-View-Controller
     * Данный файл является элементом Controller и связывает View и Model:
     */
    internal class Quiz
    {
        // объявление переменных, View и Model
        private MainWindow _view;
        private List<Question> _questions;
        private int _questionsCount;
        private int _questionIndex;
        private int _score;
        private int _scorePercent;

        // Конструктор Контроллера
        public Quiz(MainWindow view)
        {
            _view = view;
            _view.submitButton.Click += SubmitButton_Click;
            _view.nextButton.Click += NextButton_Click;
            _view.startButton.Click += StartButton_Click;
            _questions = new List<Question>();
        }

        // Нажатие на кнопку Start
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            StartGame();
        }

        // Нажатие на кнопку Next Question
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            NextQuestion();
        }

        // Нажатие на кнопку Submit
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (_questionIndex < _questionsCount)
            {
                if (CheckAnswer())
                {
                    UpdateScore(1);
                }
            }
            NextQuestion();
        }

        // Начало игры
        private void StartGame()
        {
            // очистка переменных
            _questions.Clear();
            _questionsCount = 0;
            _score = 0;
            _scorePercent = 0;

            // скртытие и отображение необходимых элементов UI
            _view.submitButton.IsEnabled = true;
            _view.nextButton.IsEnabled = true;
            _view.startButton.Visibility = Visibility.Hidden;
            _view.difficultyComboBox.Visibility = Visibility.Hidden;
            _view.difficultyText.Visibility = Visibility.Hidden;
            _view.rulesButton.Visibility = Visibility.Hidden;
            _view.timer.Visibility = Visibility.Visible;

            // загрузка вопросов на основе выбранной сложности
            switch (_view.difficultyComboBox.SelectedIndex)
            {
                case 0:
                    _questions.Add(new Question("questions\\Easy\\1.rtf", "Держи язык за зубами"));
                    _questions.Add(new Question("questions\\Easy\\2.rtf", "Всякая мышь боится кошки"));
                    _questions.Add(new Question("questions\\Easy\\3.rtf", "Молчание - это золото"));
                    _questions.Add(new Question("questions\\Easy\\4.rtf", "Нет дыма без огня"));
                    Timer(180);
                    break;
                case 1:
                    _questions.Add(new Question("questions\\Medium\\1.rtf", "Готовь сани летом, а телегу - зимой"));
                    _questions.Add(new Question("questions\\Medium\\2.rtf", "Дареному коню в зубы не смотрят"));
                    _questions.Add(new Question("questions\\Medium\\3.rtf", "Один в поле - не воин"));
                    _questions.Add(new Question("questions\\Medium\\4.rtf", "Сколько волка не корми - все в лес смотрит"));
                    Timer(240);
                    break;
                case 2:
                    _questions.Add(new Question("questions\\Hard\\1.rtf", "Человек без друзей - что дерево без корней"));
                    _questions.Add(new Question("questions\\Hard\\2.rtf", "Лучше синица в руке, чем журавль в небе"));
                    _questions.Add(new Question("questions\\Hard\\3.rtf", "Пьяному море по колено, а лужа по уши"));
                    _questions.Add(new Question("questions\\Hard\\4.rtf", "Лучше один раз увидеть, чем сто раз услышать"));
                    Timer(300);
                    break;
            }
            _questionsCount = _questions.Count;
            _questionIndex = 0;
            LoadQuestion();
        }

        // загрузка следующего вопроса
        private void LoadQuestion()
        {
            TextRange doc = new TextRange(_view.questionText.Document.ContentStart, _view.questionText.Document.ContentEnd);
            using (FileStream fs = new FileStream(_questions[_questionIndex].filename, FileMode.Open))
            {
                doc.Load(fs, DataFormats.Rtf);
            }

        }

        // переход к следующему вопросу с проверкой на их количество
        private void NextQuestion()
        {
            _view.answerText.Text = "";
            if (++_questionIndex >= _questionsCount)
            {
                FinishGame();
            }
            else LoadQuestion();
        }

        // проверка ответа
        private bool CheckAnswer()
        {
            return _questions[_questionIndex].CheckAnswer(_view.answerText.Text);
        }

        // обновление результата
        private void UpdateScore(int res)
        {
            if (res == 1) _score++;
            double score = (double)_score / _questionsCount * 100;
            _scorePercent = (int)score;
            _view.scoreText.Text = _scorePercent.ToString() + "%";
        }

        // Завершение игры
        private void FinishGame()
        {
            // вывод результата и отображение окна
            string result = string.Empty;
            if (_scorePercent >= 50) result = "победили!";
            else result = "проиграли!";
            MessageBox.Show($"Игра окончена \n" +
                $"Вы {result}\n" +
                $"Ваш результат: {_scorePercent}%\n" +
                $"Нажмите ОК чтобы выйти");
            _view.Close();
        }
        public async Task Timer(int seconds)
        {
            int _secondsLeft = seconds;

            while (_secondsLeft > 0)
            {
                _view.timer.Text = _secondsLeft.ToString();
                await Task.Delay(1000);
                _secondsLeft--;
            }

            FinishGame();
        }

    }
}
