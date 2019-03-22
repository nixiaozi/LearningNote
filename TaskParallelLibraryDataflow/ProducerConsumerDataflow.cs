using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace TaskParallelLibraryDataflow
{
    public class ProducerConsumerDataflow
    {
        private static int CountBytes(string path)
        {
            byte[] buffer = new byte[1024];
            int totalZeroBytesRead = 0;
            using (var fileStream = File.OpenRead(path))
            {
                int bytesRead = 0;
                do
                {
                    bytesRead = fileStream.Read(buffer, 0, buffer.Length);
                    totalZeroBytesRead += buffer.Count(b => b == 0);
                } while (bytesRead > 0);
            }
            return totalZeroBytesRead;
        }
        public static void ProducerConsumerExample()
        {
            //定义一个bufferblock用来存储数据
            var buffer = new BufferBlock<byte[]>();

            //启用一个消费者，准备使用buffer中的数据
            var consumer = DataflowProducerConsumer.ConsumeAsync(buffer);

            //向块中提供数据
            DataflowProducerConsumer.Produce(buffer);

            //等待消费者获取所有数据
            //consumer.Wait();

            // Print the count of bytes processed to the console.
            Console.WriteLine("Processed {0} bytes.", consumer.Result); //由于使用了Result属性，系统会隐式的wait

        }

        public static void ActionReveivesDataExample()
        {
            // Create a temporary file on disk.
            string tempFile = Path.GetTempFileName();
            // Write random data to the temporary file.
            using (var fileStream = File.OpenWrite(tempFile))
            {
                Random rand = new Random();
                byte[] buffer = new byte[1024];
                for (int i = 0; i < 512; i++)
                {
                    rand.NextBytes(buffer);
                    fileStream.Write(buffer, 0, buffer.Length);
                }
            }

            // Create an ActionBlock< int > object that prints to the console
            // the number of bytes read.
            var printResult = new ActionBlock<int>(zeroBytesRead =>
            {
                Console.WriteLine("{0} contains {1} zero bytes.",
              Path.GetFileName(tempFile), zeroBytesRead);
            });

            // Create a TransformBlock<string, int> object that calls the
            // CountBytes function and returns its result.
            var countBytes = new TransformBlock<string, int>(
            new Func<string, int>(CountBytes));

            // Link the TransformBlock<string, int> object to the
            // ActionBlock<int> object.
            countBytes.LinkTo(printResult);
            // Create a continuation task that completes the ActionBlock<int>
            // object when the TransformBlock<string, int> finishes.
            countBytes.Completion.ContinueWith(delegate { printResult.Complete(); });
            // Post the path to the temporary file to the
            // TransformBlock<string, int> object.
            countBytes.Post(tempFile);
            // Requests completion of the TransformBlock<string, int> object.
            countBytes.Complete();
            // Wait for the ActionBlock<int> object to print the message.
            printResult.Completion.Wait();
            // Delete the temporary file.
            File.Delete(tempFile);

        }

    }
}
