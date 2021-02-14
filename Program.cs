using System;
using System.Collections.Generic;
using System.IO;

namespace bilateral_list
{
    class Program
    {
        static void Main()
        {
            ListNode d = new ListNode();
            d.Previous = new ListNode
            {
                Data = "d1",
                Next = new ListNode
                {
                    Data = "dkek1",
                    Next = new ListNode
                    {
                        Data = "TDO"
                    }
                },
                Previous = new ListNode
                {
                    Data = "d2",
                    Previous = new ListNode
                    {
                        Data = "d3"
                    }
                },
                Random = new ListNode
                {
                    Data = "r1",
                    Next = new ListNode
                    {
                        Data = "rN1",
                        Random = new ListNode
                        {
                            Data = "r1N1Previ1",
                            Random = new ListNode
                            {
                                Data = "DDD"
                            }
                        }
                    }
                }
            };
            d.Next = new ListNode
            {
                Data = "asc",
                Next = new ListNode
                {
                    Data = "des"
                }
            };

            ListNode NodeList = new ListNode();
            NodeList.Data = "NL";
            NodeList.Next = new ListNode { Data = "CL", Previous = new ListNode { Data = "rrr" }, Next = new ListNode { Data = "Xc" } };
            NodeList.Previous = new ListNode { Data = "zzz", Next = new ListNode { Data = "XYZ", Random = new ListNode { Data = "end" } } };

            ListRandom l = new ListRandom();
            l.Head = d;
            l.Tail = NodeList;
            l.Count = 125;

            StreamWriter stream = new StreamWriter("data.txt");
            l.Serialize(stream.BaseStream);


            StreamReader rstream = new StreamReader("data.txt");
            l.Deserialize(rstream.BaseStream);

            System.Console.WriteLine(l.Count);
        }
    }
}