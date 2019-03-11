using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace TaskParallelLibraryDataflow
{
    public class DataflowBlock
    {
        public static void DataflowBlockExample()
        {
            var throwIfNegative = new ActionBlock<int>(n =>
              {
                  Console.WriteLine("n={0}", n);
                  if (n < 0)
                  {
                      throw new ArgumentOutOfRangeException();
                  }
              });

            //Post values to the block
            throwIfNegative.Post(0);
            throwIfNegative.Post(-1);
            throwIfNegative.Post(1);
            throwIfNegative.Post(-2);
            throwIfNegative.Complete();

            //Wait for completion in a try/catch block.
            try
            {
                throwIfNegative.Completion.Wait();
            }
            catch(AggregateException ae)
            {
                ae.Handle(e =>
                {
                    Console.WriteLine("Encountered {0}: {1}",
                        e.GetType().Name, e.Message);
                    return true;
                });
            }
        }

        public static void SimpleBufferBlockExample()
        {
            var bufferBlock = new BufferBlock<int>();

            for(int i = 0; i < 3; i++)
            {
                bufferBlock.Post(i);
            }

            for(int i = 0; i < 3; i++)
            {
                Console.WriteLine(bufferBlock.Receive());
            }

            //BufferBlock 是一个先进先出的数据缓存块，输出时按照顺序
        }

        public static void SimpleBroadcastBlockExapmle()
        {
            // Create a BroadcastBlock<double> object.
            var broadcastBlock = new BroadcastBlock<double>(null);
            // Post a message to the block.
            broadcastBlock.Post(Math.PI);
            broadcastBlock.Post(Convert.ToDouble(4.543M));//新的对象会覆盖老的对象
            // Receive the messages back from the block several times.
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine(broadcastBlock.Receive());
            }

            //BufferBlock 只会存在一个对象，并且一直存在
        }

        public static void SimpleWriteOnceBlockExample()
        {
            // Create a WriteOnceBlock<string> object.
            var writeOnceBlock = new WriteOnceBlock<string>(null);
            // Post several messages to the block in parallel. The first
            // message to be received is written to the block.
            // Subsequent messages are discarded.
            Parallel.Invoke(
            () => writeOnceBlock.Post("Message 1"),
            () => writeOnceBlock.Post("Message 2"),
            () => writeOnceBlock.Post("Message 3"));
            // Receive the message from the block.
            Console.WriteLine(writeOnceBlock.Receive());

            //writeOnceBlock会一直保持第一次收到的值
        }

        public static void SimpleActionBlockExample()
        {
            // Create an ActionBlock<int> object that prints values
            // to the console.
            var actionBlock = new ActionBlock<int>(n => Console.WriteLine(n));
            // Post several messages to the block.
            for (int i = 0; i < 3; i++)
            {
                actionBlock.Post(i * 10);
            } 
            // Set the block to the completed state and wait for all
            // tasks to finish.
            actionBlock.Complete();
            actionBlock.Completion.Wait();
        }

        public static  void SimpleTransformBlockExample()
        {
            // Create a TransformBlock<int, double> object that
            // computes the square root of its input.
            var transformBlock = new TransformBlock<int, double>(n => Math.Sqrt(n));
            // Post several messages to the block.
            transformBlock.Post(10);
            transformBlock.Post(20);
            transformBlock.Post(30);
            // Read the output messages from the block.
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine(transformBlock.Receive());
            }

            //transformBlock可以创建链式调用
        }

        public static void SimpleTransformManyBlockExample()
        {
            // Create a TransformManyBlock<string, char> object that splits
            // a string into its individual characters.
            var transformManyBlock = new TransformManyBlock<string, char>(
            s => s.ToCharArray());
            // Post two messages to the first block.
            transformManyBlock.Post("Hello");
            transformManyBlock.Post("World"); //结果块会一直保存，就算是不同的Post
            //transformManyBlock.TryReceiveAll();
            // Receive all output values from the block.
            //transformManyBlock.Complete(); //表示不会再输出数据
            //transformManyBlock.Completion.Wait(); //
            Console.WriteLine("总共的输出对象为"+ transformManyBlock.OutputCount+"个");
            //如果把i的循环次数指定超过10，Reveive会挂起线程一直等待。
            for (int i = 0; i < 10/*("Hello" + "World").Length*/; i++)
            {
                
                var data = transformManyBlock.Receive();
                Console.WriteLine(data);
            }
        }


    }
}
