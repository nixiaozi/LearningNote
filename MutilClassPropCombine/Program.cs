using System;
using System.Collections.Generic;
using System.Linq;

namespace MutilClassPropCombine
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Console.WriteLine("Begin Combine");

            var TestData = new List<List<string>>()
            {
                new List<string>(){ "白色","红色","黑色","黄色"},
                new List<string>(){"大号","小号","特小号"},
                new List<string>(){"液晶屏","等离子屏","LED屏","OLED屏"},
                new List<string>(){"钢质底座","木质底座"}
            };

            var RefTestData = TestData.ToList();


            var TestVoltageSupport = new List<string>() { "110V", "220V" };
            TestData.Add(new List<string>(TestVoltageSupport) { "380V" });

            Console.WriteLine("原始数据是:");
            foreach(var item in TestData)
            {
                Console.WriteLine(String.Join('、', item));
            }

            Console.WriteLine("新List引用原始数据是:");
            foreach (var item in RefTestData)
            {
                Console.WriteLine(String.Join('、', item));
            }



            var ResultData = CombineTest.DoCombine(TestData);

            Console.WriteLine("自由配对后数据是:");
            foreach (var item in ResultData)
            {
                Console.WriteLine(String.Join("+", item));
            }
            var PerListCount= CombineTest.PerListCount(TestData);
            Console.WriteLine("原数据应该可以生成自由配对后数据为："+String.Join(" * ", PerListCount) + " 结果为："+ CombineTest.AllNumberMultiply(PerListCount));
            
            Console.WriteLine("自由配对后数据条目共"+ ResultData.Count+ "条");

        }
    }

    class CombineTest
    {
        public static List<List<string>> DoCombine(List<List<string>> OrigionData)
        {
            List<List<string>> result = new List<List<string>>();

            var flag = true; 

            foreach(var item in OrigionData)
            {
                List<List<string>> cresult = result.ToList();   // ToList 可不可以不要？
                foreach (var pitem in item)
                {
                    if (flag)
                    {
                        result.Add(new List<string> { pitem });
                    }
                    else
                    {
                        foreach (var citem in cresult)          //ToList 删除后对导致这里迭代器异常，因为如果只是赋值就只是添加了引用并没有生成新的List
                        {
                            if (result.Contains(citem))
                            {
                                result.Remove(citem);
                            }
                            result.Add(new List<string>(citem) { pitem });
                        }
                    }
                }
                flag = false; // 保证只有第一个循环直接填充数组
            }

            return result;
        }


        public static List<int> PerListCount(List<List<string>> OrigionData)
        {
            List<int> Result = new List<int>();
            foreach(var item in OrigionData)
            {
                Result.Add(item.Count);
            }
            return Result;
        }

        public static int AllNumberMultiply(List<int> Data)
        {
            if (Data.Count == 0)
                return 0;

            int prod = 1;
            foreach (int i in Data)
            {
                prod = prod * i;
            }

            return prod;
        }

    }


}
