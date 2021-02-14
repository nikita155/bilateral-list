using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Unicode;
using System.Linq;

namespace bilateral_list
{
    class ListNode
    {
        public ListNode Previous;
        public ListNode Next;
        public ListNode Random; // произвольный элемент внутри списка
        public string Data;
    }

    class ListRandom
    {
        public ListNode Head;
        public ListNode Tail;
        public int Count;

        public void Serialize(Stream s)
        {
            List<(string, string, string, string)> listHead = new List<(string, string, string, string)>();//Кортеж для записи в файл
            List<(string, string, string, string)> listTail = new List<(string, string, string, string)>();//Кортеж для записи в файл

            //Рекурсия для получение потомков
            void DeepHead(ListNode node)
            {
                if (node != null)
                {
                    listHead.Add(new(node?.Data, node?.Next?.Data, node?.Previous?.Data, node?.Random?.Data));

                    DeepHead(node.Next);
                    DeepHead(node.Previous);
                    DeepHead(node.Random);
                }
            }

            //Рекурсия для получение потомков
            void DeepTail(ListNode node)
            {
                if (node != null)
                {
                    listTail.Add(new(node?.Data, node?.Next?.Data, node?.Previous?.Data, node?.Random?.Data));

                    DeepTail(node.Next);
                    DeepTail(node.Previous);
                    DeepTail(node.Random);
                }
            }

            DeepHead(Head);//Вызов локальной функции
            DeepTail(Tail);//Вызов локальной функции

            using (StreamWriter sw = new StreamWriter(s))
            {
                foreach (var item in listHead)
                {
                    if (string.IsNullOrEmpty(item.Item1) && string.IsNullOrWhiteSpace(item.Item1) && string.IsNullOrEmpty(item.Item2) && string.IsNullOrWhiteSpace(item.Item2)
                    && string.IsNullOrEmpty(item.Item3) && string.IsNullOrWhiteSpace(item.Item3) && string.IsNullOrEmpty(item.Item4) && string.IsNullOrWhiteSpace(item.Item4))
                    {

                    }
                    else
                    {
                        sw.WriteLine($"{item.Item1}$end{item.Item2}$end{item.Item3}$end{item.Item4}");
                    }
                }
                sw.WriteLine("ListTail");//Метка для нахождение свойства Tail
                foreach (var item in listTail)
                {
                    if (string.IsNullOrEmpty(item.Item1) && string.IsNullOrWhiteSpace(item.Item1) && string.IsNullOrEmpty(item.Item2) && string.IsNullOrWhiteSpace(item.Item2)
                    && string.IsNullOrEmpty(item.Item3) && string.IsNullOrWhiteSpace(item.Item3) && string.IsNullOrEmpty(item.Item4) && string.IsNullOrWhiteSpace(item.Item4))
                    {

                    }
                    else
                    {
                        sw.WriteLine($"{item.Item1}$end{item.Item2}$end{item.Item3}$end{item.Item4}");
                    }
                }
                sw.WriteLine("valCount");//Метка для нахождение свойства Count
                sw.WriteLine(Count);
                sw.Flush();
            }
        }

        public void Deserialize(Stream s)
        {
            List<(string, string, string, string)> listHead = new List<(string, string, string, string)>();
            List<(string, string, string, string)> listTail = new List<(string, string, string, string)>();

            using (StreamReader sr = new StreamReader(s))
            {
                bool IsTail = false;//Поле для нахождения метки Tail
                bool IsCount = false;//Поле для нахождения метки Count

                while (!sr.EndOfStream)
                {
                    string buffStr = sr.ReadLine();

                    if (buffStr != "ListTail" & IsTail == false & IsCount == false)
                    {
                        string[] subStr = buffStr.Split("$end");

                        //Пропуск пустых полей
                        if (string.IsNullOrEmpty(subStr[0]) && string.IsNullOrWhiteSpace(subStr[0]) && string.IsNullOrEmpty(subStr[1]) && string.IsNullOrWhiteSpace(subStr[1])
                        && string.IsNullOrEmpty(subStr[2]) && string.IsNullOrWhiteSpace(subStr[2]) && string.IsNullOrEmpty(subStr[3]) && string.IsNullOrWhiteSpace(subStr[3]))
                        {

                        }
                        else
                        {
                            listHead.Add(new(subStr[0], subStr[1], subStr[2], subStr[3]));
                        }
                    }
                    else if (buffStr == "ListTail" & IsTail == false)//Поиск метки Tail
                    {
                        IsTail = true;
                    }
                    else if (buffStr != "valCount" & IsTail == true & IsCount == false)
                    {
                        string[] subStr = buffStr.Split("$end");

                        //Пропуск пустых полей
                        if (string.IsNullOrEmpty(subStr[0]) && string.IsNullOrWhiteSpace(subStr[0]) && string.IsNullOrEmpty(subStr[1]) && string.IsNullOrWhiteSpace(subStr[1])
                        && string.IsNullOrEmpty(subStr[2]) && string.IsNullOrWhiteSpace(subStr[2]) && string.IsNullOrEmpty(subStr[3]) && string.IsNullOrWhiteSpace(subStr[3]))
                        {

                        }
                        else
                        {
                            listTail.Add(new(subStr[0], subStr[1], subStr[2], subStr[3]));
                        }
                    }
                    else if (buffStr == "valCount")//Поиск метки Count
                    {
                        IsCount = true;
                        IsTail = false;
                    }
                    else if (buffStr != "valCount" & IsCount == true)
                    {
                        Count = int.Parse(buffStr);
                    }
                }
                listHead = listHead.Distinct().ToList();
                listTail = listTail.Distinct().ToList();

                //Рекурсия восстановление данных из файла
                void Beep(ListNode outListNode, List<(string, string, string, string)> ArrSerData)
                {
                    if (ArrSerData.Count > 0)
                    {
                        outListNode.Data = ArrSerData[0].Item1;
                        outListNode.Next = new ListNode { Data = ArrSerData[0].Item2 };
                        outListNode.Previous = new ListNode { Data = ArrSerData[0].Item3 };
                        outListNode.Random = new ListNode { Data = ArrSerData[0].Item4 };
                        ArrSerData.RemoveAt(0);
                        Beep(outListNode.Next, ArrSerData);
                        Beep(outListNode.Previous, ArrSerData);
                        Beep(outListNode.Random, ArrSerData);
                    }
                }

                Beep(Head, listHead);//Заполнение полей класс из файла
                Beep(Tail, listTail);//Заполнение полей класс из файла
            }
        }
    }
}