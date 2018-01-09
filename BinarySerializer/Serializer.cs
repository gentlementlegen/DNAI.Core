using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializer
{
    public class Serializer
    {
        private static Type[] ScalarTypes = new Type[]
        {
            typeof(Char),
            typeof(Int16),
            typeof(Int32),
            typeof(Int64),
            typeof(UInt16),
            typeof(UInt32),
            typeof(UInt64),
            typeof(float),
            typeof(Double),
            typeof(String)
        };

        private static bool SerializeObject(object toSerialize, Stream output)//byte[] destination, ref int offset
        {
            PropertyInfo[] attrs = toSerialize.GetType().GetProperties();

            foreach (PropertyInfo attr in attrs)
            {
                if (attr.GetCustomAttribute(typeof(BinaryFormat)) != null)
                {
                    if (!ChooseSerializer(attr.GetValue(toSerialize), output))//destination, ref offset
                        throw new InvalidOperationException("Cannot serialize object attribute: " + attr.ToString());
                }
            }
            return true;
        }

        private static Dictionary<Type, Func<object, byte[]>> ScalarSerializers = new Dictionary<Type, Func<object, byte[]>>
        {
            {
                typeof(Char),
                (object data) =>
                {
                    return new byte[1] { Convert.ToByte((char)data) };
                }
            },
            {
                typeof(Int16),
                (object data) =>
                {
                    return BitConverter.GetBytes((Int16)data);
                }
            },
            {
                typeof(Int32),
                (object data) =>
                {
                    return BitConverter.GetBytes((Int32)data);
                }
            },
            {
                typeof(Int64),
                (object data) =>
                {
                    return BitConverter.GetBytes((Int64)data);
                }
            },
            {
                typeof(UInt16),
                (object data) =>
                {
                    return BitConverter.GetBytes((UInt16)data);
                }
            },
            {
                typeof(UInt32),
                (object data) =>
                {
                    return BitConverter.GetBytes((UInt32)data);
                }
            },
            {
                typeof(UInt64),
                (object data) =>
                {
                    return BitConverter.GetBytes((UInt64)data);
                }
            },
            {
                typeof(float),
                (object data) =>
                {
                    return BitConverter.GetBytes((float)data);
                }
            },
            {
                typeof(Double),
                (object data) =>
                {
                    return BitConverter.GetBytes((Double)data);
                }
            },
            {
                typeof(String),
                (object data) =>
                {
                    byte[] buff = System.Text.Encoding.UTF8.GetBytes((String)data);
                    byte[] size = BitConverter.GetBytes((UInt32)((String)data).Length);
                    byte[] final = new byte[buff.Length + size.Length];

                    Buffer.BlockCopy(size, 0, final, 0, size.Length);
                    Buffer.BlockCopy(buff, 0, final, size.Length, buff.Length);
                    return final;
                }
            }
        };

        private static bool SerializeScalar(object toSerialize, Stream output) //byte[] destination, ref int offset
        {
            byte[] dataBytes = ScalarSerializers[toSerialize.GetType()](toSerialize);

            output.Write(dataBytes, 0, dataBytes.Length);

            /*Buffer.BlockCopy(
                dataBytes, 0,
                destination, offset,
                dataBytes.Length);
            offset += dataBytes.Length;*/
            return true;
        }

        private static bool SerializeList(IEnumerable toSerialize, Stream output)//byte[] destination, ref int offset
        {
            UInt32 size = 0;
            //int savedOffset = offset;
            MemoryStream itemsbuff = new MemoryStream();

            //offset += sizeof(UInt32);
            foreach (var item in toSerialize)
            {
                if (!ChooseSerializer(item, itemsbuff))
                    throw new InvalidOperationException("Cannot serialize list item of type : " + toSerialize.ToString());
                ++size;
            }
            SerializeScalar(size, output);
            output.Write(itemsbuff.GetBuffer(), 0, (int)itemsbuff.Position);
            /*Buffer.BlockCopy(
                BitConverter.GetBytes(size), 0,
                destination, savedOffset,
                sizeof(UInt32));*/
            return true;
        }

        private static bool ChooseSerializer(object toSerialize, Stream output)// byte[] destination, ref int offset
        {
            if (toSerialize.GetType().IsEnum)
            {
                return SerializeScalar((Int32)toSerialize, output);
            }
            else if (ScalarTypes.Contains(toSerialize.GetType())) //serialize as scalar
            {
                return SerializeScalar(toSerialize, output);
            }
            else if ((toSerialize as IEnumerable) != null) //serialize as list
            {
                return SerializeList(toSerialize as IEnumerable, output);
            }
            else if (toSerialize.GetType().IsClass) //serialize as object
            {
                return SerializeObject(toSerialize, output);
            }
            throw new InvalidOperationException("Unrecognize serialization format for type : " + toSerialize.ToString());
        }

        public static int Serialize<T>(T to_serialize, byte[] destination, int offset = 0)
        {
            MemoryStream output = new MemoryStream(destination, offset, destination.Length);

            if (!ChooseSerializer(to_serialize, output))
                return 0;
            return (int)output.Position;
        }

        public static int Serialize<T>(T to_serialize, Stream output)
        {
            if (!ChooseSerializer(to_serialize, output))
                return 0;
            return (int)output.Position;
        }

        private static object DeserializeObject(Type todeserialize, Stream input)
        {
            PropertyInfo[] attrs = todeserialize.GetProperties();
            object data = Activator.CreateInstance(todeserialize);

            foreach (PropertyInfo attr in attrs)
            {
                if (attr.GetCustomAttribute(typeof(BinaryFormat)) != null)
                {
                    attr.SetValue(data, ChooseDeserializer(attr.PropertyType, input));
                }
            }
            return data;
        }

        private static Dictionary<Type, Func<Stream, object>> ScalarDeserializers = new Dictionary<Type, Func<Stream, object>>
        {
            {
                typeof(Char),
                (Stream input) =>
                {
                    byte[] dat = new byte[1];
                    input.Read(dat, 0, 1);
                    return Convert.ToChar(dat[0]);
                }
            },
            {
                typeof(Int16),
                (Stream input) =>
                {
                    byte[] dat = new byte[2];
                    input.Read(dat, 0, 2);
                    return BitConverter.ToInt16(dat, 0);
                }
            },
            {
                typeof(Int32),
                (Stream input) =>
                {
                    byte[] dat = new byte[4];
                    input.Read(dat, 0, 4);
                    return BitConverter.ToInt32(dat, 0);
                }
            },
            {
                typeof(Int64),
                (Stream input) =>
                {
                    byte[] dat = new byte[8];
                    input.Read(dat, 0, 8);
                    return BitConverter.ToInt64(dat, 0);
                }
            },
            {
                typeof(UInt16),
                (Stream input) =>
                {
                    byte[] dat = new byte[2];
                    input.Read(dat, 0, 2);
                    return BitConverter.ToUInt16(dat, 0);
                }
            },
            {
                typeof(UInt32),
                (Stream input) =>
                {
                    byte[] dat = new byte[4];
                    input.Read(dat, 0, 4);
                    return BitConverter.ToUInt32(dat, 0);
                }
            },
            {
                typeof(UInt64),
                (Stream input) =>
                {
                    byte[] dat = new byte[8];
                    input.Read(dat, 0, 8);
                    return BitConverter.ToUInt64(dat, 0);
                }
            },
            {
                typeof(float),
                (Stream input) =>
                {
                    byte[] dat = new byte[4];
                    input.Read(dat, 0, 4);
                    return BitConverter.ToSingle(dat, 0);
                }
            },
            {
                typeof(Double),
                (Stream input) =>
                {
                    byte[] dat = new byte[8];
                    input.Read(dat, 0, 8);
                    return BitConverter.ToDouble(dat, 0);
                }
            },
            {
                typeof(String),
                (Stream input) =>
                {
                    UInt32 size = Deserialize<UInt32>(input);
                    byte[] dat = new byte[size];
                    input.Read(dat, 0, (int)size);
                    return Encoding.UTF8.GetString(dat);
                }
            }
        };

        private static object DeserializeScalar(Type todeserialize, Stream input)
        {
            return ScalarDeserializers[todeserialize](input);
        }

        private static object DeserializeList(Type todeserialize, Stream input)
        {
            IList data = Activator.CreateInstance(typeof(List<>).MakeGenericType(todeserialize)) as IList;
            UInt32 size = Deserialize<UInt32>(input);

            for (int i = 0; i < size; i++)
            {
                data.Add(ChooseDeserializer(todeserialize, input));
            }
            return data;
        }

        private static object ChooseDeserializer(Type todeserialize, Stream input)//byte[] source, ref int offset
        {
            if (todeserialize.IsEnum)
            {
                return DeserializeScalar(typeof(Int32), input);
            }
            else if (ScalarTypes.Contains(todeserialize)) //serialize as scalar
            {
                return DeserializeScalar(todeserialize, input);
            }
            else if (typeof(IList).IsAssignableFrom(todeserialize)) //serialize as list
            {
                return DeserializeList(todeserialize.GetGenericArguments().Single(), input);
            }
            else if (todeserialize.IsClass) //serialize as object
            {
                return DeserializeObject(todeserialize, input);
            }
            return null;
        }

        public static T Deserialize<T>(byte[] source, int offset = 0)
        {
            return (T)ChooseDeserializer(typeof(T), new MemoryStream(source, offset, source.Length));
        }

        public static T Deserialize<T>(Stream input)
        {
            return (T)ChooseDeserializer(typeof(T), input);
        }
    }
}
