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

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            MemberSql member = new MemberSql();
            var test = new SqlString<MemberSql>()
                .SqlSelect(nameof(member.ID))
                .SqlSelect(nameof(member.Name)).ToSqlString();

            Console.WriteLine(test);

            //PerformDatabaseOperations.TestOperations().Wait(); //async 方法必须显式wait才能等待输出结果
            PerformDatabaseOperations.TestOperations();  //主线程会等待异步操作完成，可能是因为知道这是异步方法

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
