using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace TaskParallelLibraryDataflow
{
    public static class DataflowPipeline
    {
        private static T ReceiveFromAny<T>(params ISourceBlock<T>[] sources)
        {
            // Create a WriteOnceBlock<T> object and link it to each source block.
            var writeOnceBlock = new WriteOnceBlock<T>(e => e);
            foreach (var source in sources)
            {
                // Setting MaxMessages to one instructs
                // the source block to unlink from the WriteOnceBlock<T> object
                // after offering the WriteOnceBlock<T> object one message.
                source.LinkTo(writeOnceBlock, new DataflowLinkOptions { MaxMessages = 1 });
            } // Return the first value that is offered to the WriteOnceBlock object.
            return writeOnceBlock.Receive();
        }

        private static int TrySolution(int n, CancellationToken ct)
        {
            // Simulate a lengthy operation that completes within three seconds
            // or when the provided CancellationToken object is cancelled.
            SpinWait.SpinUntil(() => ct.IsCancellationRequested,
            new Random().Next(3000));
            // Return a value.
            return n + 42;
        }
        public static void BasicDataflowPipelineExample()
        {
            //
            // Create the members of the pipeline.
            //

            // Downloads the requested resource as a string.
            var downloadString = new TransformBlock<string, string>(async uri =>
            {
                Console.WriteLine("Downloading '{0}'...", uri);
                return await new HttpClient().GetStringAsync(uri);
            });

            // Separates the specified text into an array of words.
            var createWordList = new TransformBlock<string, string[]>(text =>
            {
                Console.WriteLine("Creating word list...");
                // Remove common punctuation by replacing all non-letter characters
                // with a space character.
                char[] tokens = text.Select(c => char.IsLetter(c) ? c : ' ').ToArray();
                text = new string(tokens);
                // Separate the text into an array of words.
                return text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            });

            // Removes short words and duplicates.
            var filterWordList = new TransformBlock<string[], string[]>(words =>
            {
                Console.WriteLine("Filtering word list...");
                return words
                .Where(word => word.Length > 3)
                .Distinct()
                .ToArray();
            });

            // Finds all words in the specified collection whose reverse also
            // exists in the collection.
            var findReversedWords = new TransformManyBlock<string[], string>(words =>
            {
                Console.WriteLine("Finding reversed words...");
                var wordsSet = new HashSet<string>(words);
                return from word in words.AsParallel()
                       let reverse = new string(word.Reverse().ToArray())
                       where word != reverse && wordsSet.Contains(reverse)
                       select word;
            });

            // Prints the provided reversed words to the console.
            var printReversedWords = new ActionBlock<string>(reversedWord =>
            {
                Console.WriteLine("Found reversed words {0}/{1}",
                reversedWord, new string(reversedWord.Reverse().ToArray()));
            });

            //
            // Connect the dataflow blocks to form a pipeline.
            //
            var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };
            downloadString.LinkTo(createWordList, linkOptions);
            createWordList.LinkTo(filterWordList, linkOptions);
            filterWordList.LinkTo(findReversedWords, linkOptions);
            findReversedWords.LinkTo(printReversedWords, linkOptions);

            // Process "The Iliad of Homer" by Homer.
            downloadString.Post("http://www.gutenberg.org/files/6130/6130-0.txt");

            // Mark the head of the pipeline as complete.
            downloadString.Complete();
            // Wait for the last block in the pipeline to process all messages.
            printReversedWords.Completion.Wait();
        }

        public static void UnlinkDataflowBlockWithInPipe()
        {
            // Create a shared CancellationTokenSource object to enable the
            // TrySolution method to be cancelled.
            var cts = new CancellationTokenSource();
            // Create three TransformBlock<int, int> objects.
            // Each TransformBlock<int, int> object calls the TrySolution method.
            Func<int, int> action = n => TrySolution(n, cts.Token);
            var trySolution1 = new TransformBlock<int, int>(action);
            var trySolution2 = new TransformBlock<int, int>(action);
            var trySolution3 = new TransformBlock<int, int>(action);
            // Post data to each TransformBlock<int, int> object.
            trySolution1.Post(11);
            trySolution2.Post(21);
            trySolution3.Post(31);
            // Call the ReceiveFromAny<T> method to receive the result from the
            // first TransformBlock<int, int> object to finish.
            int result = ReceiveFromAny(trySolution1, trySolution2, trySolution3);
            // Cancel all calls to TrySolution that are still active.
            cts.Cancel();
            // Print the result to the console.
            Console.WriteLine("The solution is {0}.", result);
            cts.Dispose();
        }

    }
}
