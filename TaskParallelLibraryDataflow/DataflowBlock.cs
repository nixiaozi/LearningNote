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

        public static void SimpleBatchBlockExample()
        {
            //Create a BatchBlock<int object that holds ten
            //elements per batch
            var batchBlock = new BatchBlock<int>(10);

            //Post several values to the block
            for(int i = 0; i < 13; i++)
            {
                batchBlock.Post(i);
            }
            // Set the block to the completed state. This causes
            // the block to propagate out any any remaining
            // values as a final batch.
            batchBlock.Complete();
            // Print the sum of both batches.
            Console.WriteLine("The sum of the elements in batch 1 is {0}.",
            batchBlock.Receive().Sum());
            Console.WriteLine("The sum of the elements in batch 2 is {0}.",
            batchBlock.Receive().Sum());
            //这个是把收到的数据按照批次发出的Block
        }

        public static void SimpleJoinBlockExample()
        {
            // Create a JoinBlock<int, int, char> object that requires
            // two numbers and an operator.
            var joinBlock = new JoinBlock<int, int, char>();
            // Post two values to each target of the join.
            joinBlock.Target1.Post(3);
            joinBlock.Target1.Post(6);
            joinBlock.Target2.Post(5);
            joinBlock.Target2.Post(4);
            joinBlock.Target3.Post('+');
            joinBlock.Target3.Post('-');
            // Receive each group of values and apply the operator part
            // to the number parts.
            for (int i = 0; i < 2; i++)
            {
                var data = joinBlock.Receive();
                switch (data.Item3)
                {
                    case '+':
                        Console.WriteLine("{0} + {1} = {2}",
                        data.Item1, data.Item2, data.Item1 + data.Item2);
                        break;
                    case '-':
                        Console.WriteLine("{0} - {1} = {2}",
                        data.Item1, data.Item2, data.Item1 - data.Item2);
                        break;
                    default:
                        Console.WriteLine("Unknown operator '{0}'.", data.Item3);
                        break;
                }
            }
        }

        public static void SimpleBatchedJoinBlockExample()
        {
            // For demonstration, create a Func<int, int> that
            // returns its argument, or throws ArgumentOutOfRangeException
            // if the argument is less than zero.
            Func<int, int> DoWork = n =>
            {
                if (n < 0)
                    throw new ArgumentOutOfRangeException();
                return n;
            };
            // Create a BatchedJoinBlock<int, Exception> object that holds
            // seven elements per batch.
            var batchedJoinBlock = new BatchedJoinBlock<int, Exception>(7);
            // Post several items to the block.
            foreach (int i in new int[] { 5, 6, -7, -22, 13, 55, 0 })
            {
                try
                {
                    // Post the result of the worker to the
                    // first target of the block.
                    batchedJoinBlock.Target1.Post(DoWork(i));
                }
                catch (ArgumentOutOfRangeException e)
                {
                    // If an error occurred, post the Exception to the
                    // second target of the block.
                    batchedJoinBlock.Target2.Post(e);
                }
            } 
            // Read the results from the block.
            var results = batchedJoinBlock.Receive();
            // Print the results to the console.
            // Print the results.
            foreach (int n in results.Item1)
            {
                Console.WriteLine(n);
            } 
            // Print failures.
            foreach (Exception e in results.Item2)
            {
                Console.WriteLine(e.Message);
            }
        }



    }
}
