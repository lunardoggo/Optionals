using System.Linq;
using System.Net;
using System;

namespace LunarDoggo.Optionals.Examples
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Program.RunNumberInput();
            Console.WriteLine();
            Program.RunIPAddressParser();
        }

        /// <summary>
        /// Reads a string from the input stream and trys to convert the input to a number. If it is successful,
        /// the number is output, otherwise the error that occurred
        /// </summary>
        private static void RunNumberInput()
        {
            Console.Write("Please enter a valid 32 bit number: ");
            IOptional<int> value = Optional.OfValue(Console.ReadLine())
                .SafeMap<int, Exception>(_input => Int32.Parse(_input!));

            if(value.HasValue)
            {
                Console.WriteLine("Provided int value was: " + value.Value);
            }
            else
            {
                //Every optional that does not contain a value has to contain a message
                Console.WriteLine("An error ocurred: " + value.Message);
            }
        }

        /// <summary>
        /// Reads a string from the input stream and trys to parse the input to an IPv4 address 
        /// </summary>
        private static void RunIPAddressParser()
        {
            Console.Write("Please enter a valid IPv4 address: ");
            string ip = Optional.OfValue(Console.ReadLine())
                .Map(_input => _input!.Split(".", StringSplitOptions.RemoveEmptyEntries))
                .SafeFlatMap<byte[], Exception>(_parts =>
                {
                    if (_parts.Length != 4)
                    {
                        return Optional.OfMessage<byte[]>("The input contains " + (_parts.Length > 4 ? "more" : "less") + " segments than allowed");
                    }
                    return Optional.OfValue(_parts.Select(_part => Byte.Parse(_part)).ToArray());
                })
                .Map(_segments => new IPAddress(_segments))
                .ToString(_value => "Parsed IPv4 address: " + _value.ToString());

            Console.WriteLine(ip);
        }
    }
}