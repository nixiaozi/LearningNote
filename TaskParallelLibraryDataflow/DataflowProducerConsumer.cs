using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace TaskParallelLibraryDataflow
{
    public class DataflowProducerConsumer
    {
        public static void Produce(ITargetBlock<byte[]> target)
        {
            Random rand = new Random();

            for(int i = 0; i < 100; i++)
            {
                byte[] buffer = new byte[1024];
                rand.NextBytes(buffer);
                target.Post(buffer);
            }

            //调用Complete方法后，便是该目标块不再接受新数据
            target.Complete();

        }

        public static async Task<int> ConsumeAsync(ISourceBlock<byte[]> source)
        {
            //Initialize a counter to track the number of bytes that are processed.
            int bytesProcessed = 0;

            //Read from the source buffer until the source buffer has no
            //available output data
            while (await source.OutputAvailableAsync())
            {
                byte[] data = source.Receive();
                bytesProcessed += data.Length;
            }

            return bytesProcessed;
        }



    }
}
