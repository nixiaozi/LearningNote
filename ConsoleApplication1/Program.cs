using AsyncDemo;
using ClassToSql;
using DataParallel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TaskBasedAsyncProgramming;
using TaskParallelLibraryDataflow;
using ClassToSql;
using ClassToSql.Enums;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            MemberSql member = new MemberSql();
            var tableMember = new SqlPatten<MemberSql>()
                .AddSelect(nameof(member.ID))
                .AddSelect(nameof(member.Name)).AddCountSelect(nameof(member.Age), "gdsa")
                .AddWhere(nameof(member.Name),"gd", WhereValueType.MatchLike)
                .WhereBig(nameof(member.Age),"21",false)
                .WhereLeftLike(nameof(member.Name),"gdet")
                .AddTheSubWheres(s=>s.WhereBig(nameof(member.Age),"84",false).WhereNotNull(nameof(member.ID)));
            var sql = SqlString.ToSqlString<MemberSql>(tableMember, 
                s => s.Add(nameof(member.Name), OrderByType.Asc)
                    .Add(nameof(member.CreateDate), OrderByType.Desc),1,20);

            Console.WriteLine(sql);

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
