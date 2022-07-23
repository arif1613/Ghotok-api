using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Buffers;

namespace MemoryManagement
{
    internal class Program
    {
        /*
         * System.Span<T>, a type that is used to ACCESS a contiguous region of memory.
         * System.Memory<T>, a WRAPPER over a contiguous region of memory. 
         */
        static void Main()
        {
            IMemoryOwner<TestObj> owner = MemoryPool<TestObj>.Shared.Rent();

            Console.Write("Enter id: ");
            try
            {
                var value = Int32.Parse(Console.ReadLine());
                Console.Write("Enter id: ");
                var name = Console.ReadLine();

                var p = new TestObj
                {
                    Id = value,
                    Name = name
                };
                var memory = owner.Memory;

                WriteInt32ToBuffer(p, memory);

                DisplayBufferToConsole(owner.Memory.Slice(0, value.ToString().Length));
            }
            catch (FormatException)
            {
                Console.WriteLine("You did not enter a valid number.");
            }
            catch (OverflowException)
            {
                Console.WriteLine($"You entered a number less than {Int32.MinValue:N0} or greater than {Int32.MaxValue:N0}.");
            }
            finally
            {
                owner?.Dispose();
            }
        }

        static void WriteInt32ToBuffer(TestObj value, Memory<TestObj> buffer)
        {
            var strValue = value.ToString();
            var span = buffer.Slice(0, strValue.Length).Span;
            //strValue.AsSpan().CopyTo(span);
        }

        static void DisplayBufferToConsole(Memory<TestObj> buffer) =>
            Console.WriteLine($"Contents of the buffer: '{buffer}'");
    }

    public class TestObj
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }
}
