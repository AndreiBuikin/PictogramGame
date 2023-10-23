using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PictogramGame.Models
{
    /*
     * Используем паттерн MVC - Model-View-Controller
     * Данный файл является элементом Model и содержит необходимые поля одного вопроса:
     * 1) Ссылку на RTF файл с текстом закодированном пиктограммами
     * 2) Ответ на вопрос - расшифрованный текст
     * Метод CheckAnswer производит проверку на то, правильно ли дан ответ 
     */
    internal class Question
    {
        public string filename { get; set; }
        public string answer { get; set; }

        public Question(string filename, string answer) 
        { 
            this.filename = filename;
            this.answer = answer;
        }

        public bool CheckAnswer(string answer) 
        { 
            return String.Equals(this.answer, answer);
        }        
    }
}
