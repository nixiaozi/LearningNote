using ClassToSql;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using ClassToSql.Enums;
using EntityFrameworkTest.Model;
using EntityFrameworkTest.Context;
using EntityFrameworkTest;
using System.Diagnostics;
using StackExchangeRedisTest;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopWatch = new Stopwatch(); //用户获取执行耗时

            // EntityFrameworkTestHelper.InitLoadData();

            //需要测试RedisTTL时间
            RedisHelper.StringSet("TestTime", "123456", Convert.ToInt32((DateTime.Today.AddDays(1) - DateTime.Now).TotalMinutes));


            Member testMember = new Member();

            //Guid Test
            Console.WriteLine("Guid Test!");
            stopWatch.Restart();
            var guidTest = new SqlPatten<Member>(true)
                .AddSumSelect(nameof(testMember.Age), "TheAges")
                .AddSelect(nameof(testMember.Name))
                .WhereEqual(nameof(testMember.ID), new Guid("E15DE442-F6E7-4094-BFC3-012B5EE211B2"));

            var guidTestSql = SqlString.ToSqlString<Member>(guidTest);
            stopWatch.Stop();
            Console.WriteLine("耗时：" + stopWatch.Elapsed);
            Console.WriteLine(guidTestSql);


            Console.WriteLine("单表查询生成SQL");
            stopWatch.Start();
            var singleTable = new SqlPatten<Member>(true)
                .AddSelect(nameof(testMember.ID))
                .AddSelect(nameof(testMember.Name))
                .AddCountSelect(nameof(testMember.Age), "gdsa")
                .AddWhere(nameof(testMember.Name), "gd", WhereValueType.MatchLike)
                .WhereSmall(nameof(testMember.Age), "21", true)
                .WhereBig(nameof(testMember.Age), "21", false)
                .WhereLeftLike(nameof(testMember.Name), "gdet")
                .ToNotOrJoin()
                .AddTheSubWheres(
                    s => s
                        .WhereBig(nameof(testMember.Age), 84, false)
                        .ToOrJoin()
                        .WhereNotNull(nameof(testMember.ID))
                    )
                .ToAndJoin()
                .WhereIn(nameof(testMember.Age), new List<string> { "gdag", "gdhrre" })
                .WhereIn(nameof(testMember.Age), new List<int> { 52, 12 });

            var singleTableSql = SqlString.ToSqlString<Member>(singleTable);
            stopWatch.Stop();
            Console.WriteLine("耗时：" + stopWatch.Elapsed);
            Console.WriteLine(singleTableSql);



            Console.WriteLine("单表查询增加排序号字段");
            stopWatch.Restart();
            var singleTableWithOrder = new SqlPatten<Member>(true)
                .WhereBig(nameof(testMember.Age),20);

            var singleTableWithOrderSql = SqlString.ToSqlString<Member>(singleTableWithOrder, s => s.Add(nameof(testMember.CreateDate), OrderByType.Desc));
            stopWatch.Stop();
            Console.WriteLine("耗时：" + stopWatch.Elapsed);
            Console.WriteLine(singleTableWithOrderSql);



            Console.WriteLine("单表查询增加自定义分页查询");
            stopWatch.Restart();
            var singleTableWithOrderPaging = new SqlPatten<Member>(true)
                .AddSumSelect(nameof(testMember.Age), "TheAges")
                .AddSelect(nameof(testMember.Name));

            var singleTableWithOrderPagingSql = SqlString.ToSqlString<Member>(singleTableWithOrderPaging, s => s.Add("TheAges", OrderByType.Desc),
                1, 5);
            stopWatch.Stop();
            Console.WriteLine("耗时：" + stopWatch.Elapsed);
            Console.WriteLine(singleTableWithOrderPagingSql);


           

            

            //PerformDatabaseOperations.TestOperations().Wait(); //async 方法必须显式wait才能等待输出结果
            //PerformDatabaseOperations.TestOperations();  //主线程会等待异步操作完成，可能是因为知道这是异步方法,测试时

            //EntityFrameworkAsyncTest.TestMutiLineInsert();


            //同步多线程
            //ParallelIterateFileDirectories.MainAction();

            //TaskBase 任务
            //BaseTaskExample.MainAction();
            //BaseTaskExample.TaskRunAction();
            //BaseTaskExample.TaskFactoryStartAction();
            //BaseTaskExample.ComputeMutliTaskResultAction();
            //BaseTaskExample.LambdaCaptureOutExpectedExp();
            //BaseTaskExample.UseAsyncStateForTask();
            //BaseTaskExample.TaskCultureCustomization();
            //BaseTaskExample.ContinuationsTaskExp();
            //BaseTaskExample.ChainTypeContinuationsTask();
            //BaseTaskExample.DetachedChildTasks();

            //ChainingContinuationTask
            //ChainingContinuationTask.ContinuationSingleAntecedcent();
            //ChainingContinuationTask.ContinuationForMultipleAntecedents();
            //ChainingContinuationTask.PassingDataContinuation();
            //ChainingContinuationTask.PassingDataContinuationWithWait();
            //ChainingContinuationTask.CancelingContinuationExp();
            //ChainingContinuationTask.CancelContinuationFromAntecedent();
            //ChainingContinuationTask.ContinuationWithAntecedentChild();
            //ChainingContinuationTask.ContinuationWithAntDetachedChild();
            //ChainingContinuationTask.AssociateStateWithContinuations();
            //ChainingContinuationTask.HandingExceptionsFromContinuations();
            //ChainingContinuationTask.HandingAntecedentExceptionsFromContinuations();

            //AttachDetachedChildTask.TaskCancellation();

            //DataflowBlock.DataflowBlockExample();
            //DataflowBlock.SimpleBufferBlockExample();
            //DataflowBlock.SimpleBroadcastBlockExapmle();
            //DataflowBlock.SimpleWriteOnceBlockExample();
            //DataflowBlock.SimpleActionBlockExample();
            //DataflowBlock.SimpleTransformManyBlockExample();
            //DataflowBlock.SimpleBatchBlockExample();
            //DataflowBlock.SimpleJoinBlockExample();

            //ProducerConsumerDataflow.ProducerConsumerExample();
            //ProducerConsumerDataflow.ActionReveivesDataExample();

            //DataflowPipeline.BasicDataflowPipelineExample();

            

            var password = "123456";
            Console.WriteLine(password+"的hash值为："+ StringExtensions.ToMd5(password));

            Console.WriteLine("按任意键退出！");
            Console.ReadKey();
        }
    }

    public static class StringExtensions
    {
        public static string ToMd5(this string str)
        {
            byte[] data = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(str));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sb.AppendFormat("{0:x2}", data[i]);
            }
            return sb.ToString();
        }

    }
}
